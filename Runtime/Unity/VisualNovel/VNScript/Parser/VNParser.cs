using UnityEngine;
using System;
using System.Collections.Generic;
using JLGA.Architecture.VisualNovel.VNScript.Data;
using JLGA.Architecture.VisualNovel.VNScript.Parser;
using JLGA.Unity.VisualNovel.VNScript.Listener;

namespace JLGA.Unity.VisualNovel.VNScript.Parser
{
    public class VNParser : MonoBehaviour
    {
        [SerializeField] private VNListeners _listeners;

        private VNIndex? parseLocation;
        private string _scriptFileLocation;
        private Architecture.VisualNovel.VNScript.Data.VNScript _script;
        private SortedDictionary<string, VNIndex> _markerLocations;
        private Action _endCallback;

        private static VNBracketsParser _s_bracketsParser;
        private static VNActionParser<Context, Result> _s_actionParser;
        private static SortedDictionary<char, Func<Context, VNString, VNErrorAccumulator, Result>> _s_bracketsCallbacks; // Map from left bracket to callback function

        private static readonly char _s_endParseCharacter = ';';
        private static readonly VNBracketPair _s_nameBracketPair = new VNBracketPair('{', '}');
        private static readonly VNBracketPair _s_dialogueBracketPair = new VNBracketPair('[', ']');
        private static readonly VNBracketPair _s_actionBracketPair = new VNBracketPair('<', '>');
        private static readonly VNBracketPair _s_actionArgumentsBracketPair = new VNBracketPair('(', ')');
        private static readonly char _s_actionArgumentsSeparationCharacter = ',';
        private static readonly VNBracketPair _s_markerBracketPair = new VNBracketPair('!', '!');
        private static readonly VNBracketPair _s_commentBracketPair = new VNBracketPair('"', '"');
        private static readonly VNBracketPair _s_fileBracketPair = new VNBracketPair('\'', '\'');
        private static readonly VNBracketPair _s_flagBracketPair = new VNBracketPair('?', '?');
        private static readonly string _s_charactersToIgnore = " \t\r\n";
        
        private readonly struct Context
        {
            public VNParser Parser { get; }
            public Architecture.VisualNovel.VNScript.Data.VNScript Script { get; }
            public VNListeners Listeners { get; }
            public SortedDictionary<string, VNIndex> MarkerLocations { get; }
            public Action EndCallback { get; }

            public Context(
                VNParser parser,
                Architecture.VisualNovel.VNScript.Data.VNScript script,
                VNListeners listeners,
                SortedDictionary<string, VNIndex> markerLocations,
                Action endCallback
            )
            {
                Parser = parser;
                Script = script;
                Listeners = listeners;
                MarkerLocations = markerLocations;
                EndCallback = endCallback;
            }
        }

        private readonly struct Result
        {
            public bool EndVisualNovel { get; }
            public bool EndActionParse { get; }
            public bool RestartParse { get; }
            public bool Any { get; }

            public Result(bool endVisualNovel, bool endActionParse, bool restartParse)
            {
                EndVisualNovel = endVisualNovel;
                EndActionParse = endActionParse;
                RestartParse = restartParse;
                Any = EndActionParse | EndVisualNovel | RestartParse;
            }

            public static Result Continue => new Result(false, false, false);
            public static Result End => new Result(true, false, false);
            public static Result EndAction => new Result(false, true, false);
            public static Result Restart => new Result(false, false, true);
        }

        #region Unity

        private void Awake()
        {
            _s_Initialize();
        }

        #endregion

        #region VNParser Public

        public void SetVisualNovel(string fileLocation)
        {
            if (_scriptFileLocation != null && _scriptFileLocation.Equals(fileLocation))
            {
                return;
            }

            TextAsset scriptText = Resources.Load<TextAsset>(fileLocation);
            if (scriptText == null)
            {
                Debug.LogError($"[VisualNovel][Load] Script '{fileLocation}' does not exist.");
                return;
            }

            _scriptFileLocation = fileLocation;
            _script = new Architecture.VisualNovel.VNScript.Data.VNScript(scriptText.text);
            _FindMarkerLocations();
            _listeners.Initialize();
        }

        public void SetMarker(string marker)
        {
            if (!_markerLocations.ContainsKey(marker))
            {
                Debug.LogError($"[VisualNovel][Marker] Visual novel '{_scriptFileLocation}' does not contain marker '{marker}'.");
                return;
            }
            VNIndex markerIndex = _markerLocations[marker];
            parseLocation = _script.Next(markerIndex);
        }

        public void SetEndCallback(Action callback)
        {
            _endCallback = callback;
        }

        public void Parse()
        {
            bool endVisualNovel = false;
            VNErrorAccumulator errors = new VNErrorAccumulator();
            if (parseLocation == null)
            {
                errors.Add(new VNError("[VisualNovel][Parse] Attempting to parse from invalid index.", VNError.EStatus.Fatal));
                goto end;
            }

        start:
            while (parseLocation != null)
            {
                VNIndex index = parseLocation.Value;
                VNBracketsParser.Result bracketsResult = _s_bracketsParser.Parse(_script, errors, index);
                parseLocation = bracketsResult.EndIndex;

                switch (bracketsResult.Type)
                {
                    case VNBracketsParser.ResultType.EndParse:
                    case VNBracketsParser.ResultType.Error:
                        goto end;
                    case VNBracketsParser.ResultType.BracketPair:
                        VNBracketPair bracketPair = bracketsResult.BracketPair.Value;
                        Func<Context, VNString, VNErrorAccumulator, Result> bracketsCallback = _s_bracketsCallbacks[bracketPair.Left];
                        Context context = _CreateContext();
                        Result result = bracketsCallback(context, bracketsResult.BracketPairContents.Value, errors);
                        if (result.RestartParse)
                        {
                            goto start;
                        }
                        if (result.EndVisualNovel)
                        {
                            endVisualNovel = true;
                            goto end;
                        }
                        break;
                }
            }

        end:
            if (errors.Errors.Count > 0)
            {
                Debug.LogError(_script.ToString(errors));
            }
            if (endVisualNovel)
            {
                _listeners.Cleanup();
                _endCallback?.Invoke();
            }
        }

        #endregion

        #region VNParser Private

        private void _FindMarkerLocations()
        {
            _markerLocations = new SortedDictionary<string, VNIndex>();
            VNIndex? index = _script.Start();

            VNErrorAccumulator errors = new VNErrorAccumulator();
            while (index != null)
            {
                VNIndex currentIndex = index.Value;
                VNBracketsParser.Result bracketResult = _s_bracketsParser.Parse(_script, errors, currentIndex);
                index = bracketResult.EndIndex;

                switch (bracketResult.Type)
                {
                    case VNBracketsParser.ResultType.Error:
                        return;
                    case VNBracketsParser.ResultType.EndParse:
                        continue;
                    case VNBracketsParser.ResultType.BracketPair:
                        char leftBracket = bracketResult.BracketPair.Value.Left;
                        if (leftBracket != _s_markerBracketPair.Left)
                        {
                            continue;
                        }
                        string marker = _script.ToString(bracketResult.BracketPairContents.Value);
                        _markerLocations.Add(marker, bracketResult.BracketPairContents.Value.End);
                        break;
                }
            }
        }

        private Context _CreateContext()
        {
            return new Context(
                this,
                _script,
                _listeners,
                _markerLocations,
                _endCallback
            );
        }

        #endregion

        #region VNParser Private Static Initialization

        private static void _s_Initialize()
        {
            _s_InitializeBracketsParser();
            _s_InitializeBracketsCallbacks();
            _s_InitializeActionsParser();
        }

        private static void _s_InitializeBracketsParser()
        {
            if (_s_bracketsParser != null)
            {
                return;
            }
            _s_bracketsParser = new VNBracketsParser(_s_endParseCharacter, _s_charactersToIgnore);
            _s_bracketsParser.AddBracketPair(_s_nameBracketPair);
            _s_bracketsParser.AddBracketPair(_s_dialogueBracketPair);
            _s_bracketsParser.AddBracketPair(_s_actionBracketPair);
            _s_bracketsParser.AddBracketPair(_s_markerBracketPair);
            _s_bracketsParser.AddBracketPair(_s_commentBracketPair);
        }

        private static void _s_InitializeActionsParser()
        {
            if (_s_actionParser != null)
            {
                return;
            }
            _s_actionParser = new VNActionParser<Context, Result>(_s_actionArgumentsSeparationCharacter, _s_charactersToIgnore, _s_actionArgumentsBracketPair);
            _s_InitializeActionParser_ActorActions();
            _s_InitializeActionParser_DisplayActions();
            _s_InitializeActionParser_Miscellaneous();
        }

        private static void _s_InitializeActionParser_ActorActions()
        {
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Actor.LoadPreset",
                new VNBracketPair[] { _s_nameBracketPair, _s_fileBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    string newActorName = context.Script.ToString(args[0]);
                    string presetFileLocation = context.Script.ToString(args[1]);
                    GameObject preset = Resources.Load<GameObject>(presetFileLocation);
                    if (preset == null)
                    {
                        errors.Add(new VNError($"Failed to find preset {args[1]}", VNError.EStatus.NonFatal, args[1]));
                        return Result.EndAction;
                    }
                    AVNActor actor = preset.GetComponent<AVNActor>();
                    if (actor == null)
                    {
                        errors.Add(new VNError($"Preset does not have AVNActor {args[1]}", VNError.EStatus.NonFatal, args[1]));
                        return Result.EndAction;
                    }
                    actor.SetVNListenerName(newActorName);
                    if (!context.Listeners.ActorSelector.AddActor(actor))
                    {
                        errors.Add(new VNError($"Actor name already in use {args[1]}.", VNError.EStatus.NonFatal, args[1]));
                        return Result.EndAction;
                    }
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Actor.Select",
                new VNBracketPair[] { _s_nameBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    string actorName = context.Script.ToString(args[0]);
                    if (!context.Listeners.ActorSelector.Select(actorName))
                    {
                        errors.Add(new VNError($"Actor does not exist {args[0]}.", VNError.EStatus.NonFatal, args[0]));
                        return Result.EndAction;
                    }
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Actor.Move",
                new VNBracketPair[] { _s_fileBracketPair, _s_fileBracketPair, _s_fileBracketPair, _s_fileBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    (bool success, List<float> values) = _ParseFloats(context, args, errors);
                    if (!success)
                    {
                        return Result.EndAction;
                    }
                    if (context.Listeners.ActorSelector.CurrentlySelected == null)
                    {
                        VNString errorRange = new VNString(args[0].Start, args[args.Count - 1].End);
                        errors.Add(new VNError($"Actor not selected {errorRange}.", VNError.EStatus.NonFatal, errorRange));
                        return Result.EndAction;
                    }
                    context.Listeners.ActorSelector.CurrentlySelected.SetPosition(values[0], values[1], values[2], values[3]);
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Actor.Rotate",
                new VNBracketPair[] { _s_fileBracketPair, _s_fileBracketPair, _s_fileBracketPair, _s_fileBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    (bool success, List<float> values) = _ParseFloats(context, args, errors);
                    if (!success)
                    {
                        return Result.EndAction;
                    }
                    if (context.Listeners.ActorSelector.CurrentlySelected == null)
                    {
                        VNString errorRange = new VNString(args[0].Start, args[args.Count - 1].End);
                        errors.Add(new VNError($"Actor not selected {errorRange}.", VNError.EStatus.NonFatal, errorRange));
                        return Result.EndAction;
                    }
                    context.Listeners.ActorSelector.CurrentlySelected.SetRotation(values[0], values[1], values[2], values[3]);
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Actor.Scale",
                new VNBracketPair[] { _s_fileBracketPair, _s_fileBracketPair, _s_fileBracketPair, _s_fileBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    (bool success, List<float> values) = _ParseFloats(context, args, errors);
                    if (!success)
                    {
                        return Result.EndAction;
                    }
                    if (context.Listeners.ActorSelector.CurrentlySelected == null)
                    {
                        VNString errorRange = new VNString(args[0].Start, args[args.Count - 1].End);
                        errors.Add(new VNError($"Actor not selected {errorRange}.", VNError.EStatus.NonFatal, errorRange));
                        return Result.EndAction;
                    }
                    context.Listeners.ActorSelector.CurrentlySelected.SetScale(values[0], values[1], values[2], values[3]);
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Actor.Appearance",
                new VNBracketPair[] { _s_fileBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    string appearance = context.Script.ToString(args[0]);
                    if (context.Listeners.ActorSelector.CurrentlySelected == null)
                    {
                        errors.Add(new VNError($"Actor not selected {args[0]}", VNError.EStatus.NonFatal, args[0]));
                        return Result.EndAction;
                    }
                    if (!context.Listeners.ActorSelector.CurrentlySelected.SetAppearance(appearance))
                    {
                        errors.Add(new VNError($"Actor does not have appearance '{appearance}' {args[0]}.", VNError.EStatus.NonFatal, args[0]));
                        return Result.EndAction;
                    }
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Actor.Animation",
                new VNBracketPair[] { _s_fileBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    string animation = context.Script.ToString(args[0]);
                    if (context.Listeners.ActorSelector.CurrentlySelected == null)
                    {
                        errors.Add(new VNError($"Actor not selected {args[0]}", VNError.EStatus.NonFatal, args[0]));
                        return Result.EndAction;
                    }
                    if (!context.Listeners.ActorSelector.CurrentlySelected.PlayAnimation(animation))
                    {
                        errors.Add(new VNError($"Actor does not have animation '{animation}' {args[0]}.", VNError.EStatus.NonFatal, args[0]));
                        return Result.EndAction;
                    }
                    return Result.Continue;
                }
            ));
        }

        private static void _s_InitializeActionParser_DisplayActions()
        {
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Display.LoadPreset",
                new VNBracketPair[] { _s_nameBracketPair, _s_fileBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    string displayName = context.Script.ToString(args[0]);
                    string presetFileLocation = context.Script.ToString(args[1]);
                    GameObject displayObject = Resources.Load<GameObject>(presetFileLocation);
                    if (displayObject == null)
                    {
                        errors.Add(new VNError($"Failed to find preset {args[1]}.", VNError.EStatus.NonFatal, args[1]));
                        return Result.EndAction;
                    }
                    AVNDisplay display = displayObject.GetComponent<AVNDisplay>();
                    if (display == null)
                    {
                        errors.Add(new VNError($"Preset does not contain AVNDisplay {args[1]}.", VNError.EStatus.NonFatal, args[1]));
                        return Result.EndAction;
                    }
                    display.SetVNListenerName(displayName);
                    if (!context.Listeners.DisplaySelector.AddDisplay(display))
                    {
                        errors.Add(new VNError($"Display name already in use {args[0]}.", VNError.EStatus.NonFatal, args[0]));
                        return Result.EndAction;
                    }
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Display.Select",
                new VNBracketPair[] { _s_nameBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    string displayName = context.Script.ToString(args[0]);
                    if (!context.Listeners.DisplaySelector.Select(displayName))
                    {
                        errors.Add(new VNError($"Display does not exist {args[0]}.", VNError.EStatus.NonFatal, args[0]));
                        return Result.EndAction;
                    }
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Display.Initialize",
                new VNBracketPair[] { },
                (Context context, List<VNString> _, VNErrorAccumulator errors) =>
                {
                    AVNDisplay currentDisplay = context.Listeners.DisplaySelector.CurrentlySelected;
                    if (currentDisplay == null)
                    {
                        return Result.EndAction;
                    }
                    currentDisplay.Initialize();
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Display.DialogueOption",
                new VNBracketPair[] { _s_flagBracketPair, _s_dialogueBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    string[] flag_value = context.Script.ToString(args[0]).Split(":");
                    string flag = flag_value[0];
                    string value = (flag_value.Length > 1) ? flag_value[1] : "true";
                    if (value.Equals("$null"))
                    {
                        value = null;
                    }
                    string dialogue = context.Script.ToString(args[1]);
                    if (context.Listeners.DisplaySelector.CurrentlySelected == null)
                    {
                        VNString errorRange = new VNString(args[0].Start, args[args.Count - 1].End);
                        errors.Add(new VNError($"Display not selected {errorRange}.", VNError.EStatus.NonFatal, errorRange));
                        return Result.EndAction;
                    }
                    context.Listeners.DisplaySelector.CurrentlySelected.AddDialogueOption(dialogue, () =>
                    {
                        context.Listeners.Flags.SetFlag(flag, value);
                        context.Listeners.State.ExitDialogueChoice();
                        context.Parser.Parse();
                    });
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Display.FinalizeDialogueOptions",
                new VNBracketPair[] { },
                (Context context, List<VNString> _, VNErrorAccumulator __) =>
                {
                    if (context.Listeners.DisplaySelector.CurrentlySelected == null)
                    {
                        return Result.EndAction;
                    }
                    context.Listeners.DisplaySelector.CurrentlySelected.FinalizeDialogueOptions();
                    context.Listeners.State.EnterDialogueChoice();
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Display.Hide",
                new VNBracketPair[] { },
                (Context context, List<VNString> _, VNErrorAccumulator __) =>
                {
                    if (context.Listeners.DisplaySelector.CurrentlySelected == null)
                    {
                        return Result.EndAction;
                    }
                    context.Listeners.State.HideDisplay();
                    context.Listeners.DisplaySelector.CurrentlySelected.Hide();
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Display.Show",
                new VNBracketPair[] { },
                (Context context, List<VNString> _, VNErrorAccumulator __) =>
                {
                    if (context.Listeners.DisplaySelector.CurrentlySelected == null)
                    {
                        return Result.EndAction;
                    }
                    context.Listeners.State.ShowDisplay();
                    context.Listeners.DisplaySelector.CurrentlySelected.Show();
                    return Result.Continue;
                }
            ));
        }

        private static void _s_InitializeActionParser_Miscellaneous()
        {
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Controls.Target",
                new VNBracketPair[] { _s_nameBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    string displayName = context.Script.ToString(args[0]);
                    if (!context.Listeners.ControlledDisplaySelector.Select(displayName))
                    {
                        errors.Add(new VNError($"[VisualNovel][VNParser][Controls.Target]: Display '{displayName}' does not exist {args[0]}.", VNError.EStatus.NonFatal, args[0]));
                        return Result.EndAction;
                    }
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Controls.Untarget",
                new VNBracketPair[] { },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    context.Listeners.ControlledDisplaySelector.Unselect();
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Marker.Jump",
                new VNBracketPair[] { _s_markerBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    string marker = context.Script.ToString(args[0]);
                    context.Parser.SetMarker(marker);
                    return Result.Restart;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Flag.Set",
                new VNBracketPair[] { _s_flagBracketPair, _s_commentBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    string flag = context.Script.ToString(args[0]);
                    string flagValue = context.Script.ToString(args[1]);
                    if (flagValue.Equals("$null"))
                    {
                        flagValue = null;
                    }
                    context.Listeners.Flags.SetFlag(flag, flagValue);
                    return Result.Continue;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "Flag.If",
                new VNBracketPair[] { _s_flagBracketPair, _s_commentBracketPair },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    string flag = context.Script.ToString(args[0]);
                    string expectedFlagValue = context.Script.ToString(args[1]);
                    if (expectedFlagValue.Equals("$null"))
                    {
                        expectedFlagValue = null;
                    }
                    string actualFlag = context.Listeners.Flags.GetFlag(flag);
                    if (expectedFlagValue == null)
                    {
                        return (actualFlag == null) ? Result.Continue : Result.EndAction;
                    }
                    return (expectedFlagValue.Equals(actualFlag)) ? Result.Continue : Result.EndAction;
                }
            ));
            _s_actionParser.AddAction(new VNAction<Context, Result>(
                "End",
                new VNBracketPair[] { },
                (Context context, List<VNString> args, VNErrorAccumulator errors) =>
                {
                    return Result.End;
                }
            ));
        }

        private static void _s_InitializeBracketsCallbacks()
        {
            if (_s_bracketsCallbacks != null)
            {
                return;
            }
            _s_bracketsCallbacks = new SortedDictionary<char, Func<Context, VNString, VNErrorAccumulator, Result>>();

            _s_bracketsCallbacks.Add(_s_nameBracketPair.Left, (Context context, VNString arg, VNErrorAccumulator errors) =>
            {
                if (context.Listeners.DisplaySelector.CurrentlySelected == null)
                {
                    errors.Add(new VNError($"Display not selected {arg}.", VNError.EStatus.NonFatal, arg));
                    return Result.Continue;
                }
                string nameText = context.Script.ToString(arg);
                context.Listeners.DisplaySelector.CurrentlySelected.SetNameText(nameText);
                return Result.Continue;
            });
            _s_bracketsCallbacks.Add(_s_dialogueBracketPair.Left, (Context context, VNString arg, VNErrorAccumulator errors) =>
            {
                if (context.Listeners.DisplaySelector.CurrentlySelected == null)
                {
                    errors.Add(new VNError($"Display not selected {arg}.", VNError.EStatus.NonFatal, arg));
                    return Result.Continue;
                }
                string dialogueText = context.Script.ToString(arg);
                context.Listeners.DisplaySelector.CurrentlySelected.SetDialogueText(dialogueText);
                return Result.Continue;
            });
            _s_bracketsCallbacks.Add(_s_actionBracketPair.Left, _s_ActionBracketsCallback);
            _s_bracketsCallbacks.Add(_s_markerBracketPair.Left, (Context context, VNString _, VNErrorAccumulator __) => Result.Continue);
            _s_bracketsCallbacks.Add(_s_commentBracketPair.Left, (Context context, VNString _, VNErrorAccumulator __) => Result.Continue);
        }

        #endregion

        #region VNParser Private Static Miscellaneous

        private static (bool, List<float>) _ParseFloats(Context context, List<VNString> args, VNErrorAccumulator errors)
        {
            List<float> values = new List<float>();
            bool overallSuccess = true;
            foreach (VNString arg in args)
            {
                (bool valueSuccess, float value) = _ParseFloat(context, arg, errors);
                values.Add(value);
                overallSuccess &= valueSuccess;
            }
            return (overallSuccess, values);
        }

        private static (bool, float) _ParseFloat(Context context, VNString arg, VNErrorAccumulator errors)
        {
            float v;
            if (!float.TryParse(context.Script.ToString(arg), out v)) {
                errors.Add(new VNError($"Expected argument to be a float {arg}", VNError.EStatus.NonFatal, arg));
                return (false, 0);
            }
            return (true, v);
        }

        private static Result _s_ActionBracketsCallback(Context context, VNString arg, VNErrorAccumulator errors)
        {
            List<VNActionParser<Context, Result>.Result> actionResults = _s_actionParser.Parse(context.Script, errors, arg);
            foreach (VNActionParser<Context, Result>.Result result in actionResults)
            {
                Result actionCallbackResult = result.Action.Callback(context, result.Arguments, errors);
                if (actionCallbackResult.Any)
                {
                    return actionCallbackResult;
                }
            }
            return Result.Continue;
        }

        #endregion

    }
}
