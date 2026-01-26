using System.Collections.Generic;
using JLGA.Architecture.VisualNovel.Data;

namespace JLGA.Architecture.VisualNovel.Parser
{
    public class VNActionParser<C, R>
    {
        private readonly char _argumentSeparationCharacter;
        private readonly SortedSet<char> _charactersToIgnore;
        private readonly SortedDictionary<string, VNAction<C, R>> _actions;
        private readonly VNBracketPair _argumentsBracketPair;

        public readonly struct Result
        {
            public VNAction<C, R> Action { get; }
            public List<VNString> Arguments { get; }

            public Result(VNAction<C, R> action, List<VNString> arguments)
            {
                Action = action;
                Arguments = arguments;
            }
        }

        private class IndexRange
        {
            private VNScript _script;
            public VNIndex Index { get; private set; }
            public VNIndex End { get; }
            public bool Ended => Index == End;

            public IndexRange(VNScript script, VNIndex start, VNIndex end)
            {
                _script = script;
                Index = start;
                End = end;
            }

            public void Next()
            {
                Index = _script.Next(Index).Value;
            }
        }

        private readonly struct ResultActionName
        {
            public VNIndex FirstNameCharacter { get; }
            public VNIndex LastNameCharacter { get; }
            public bool LeftArgumentBracketParsed { get; }
            public VNIndex? FirstUnallowedCharacter { get; }
            public VNIndex? LastUnallowedCharacter { get; }

            public ResultActionName(VNIndex firstNameCharacter, VNIndex lastNameCharacter, bool leftArgumentBracketParsed, VNIndex? firstUnallowedCharacter, VNIndex? lastUnallowedCharacter)
            {
                FirstNameCharacter = firstNameCharacter;
                LastNameCharacter = lastNameCharacter;
                LeftArgumentBracketParsed = leftArgumentBracketParsed;
                FirstUnallowedCharacter = firstUnallowedCharacter;
                LastUnallowedCharacter = lastUnallowedCharacter;
            }
        }

        #region VNActionParser Public

        public VNActionParser(char argumentSeparationCharacter, string charactersToIgnore, VNBracketPair argumentsBracketPair)
        {
            _argumentSeparationCharacter = argumentSeparationCharacter;
            _charactersToIgnore = new SortedSet<char>(charactersToIgnore);
            _actions = new SortedDictionary<string, VNAction<C, R>>();
            _argumentsBracketPair = argumentsBracketPair;
        }

        public void AddAction(VNAction<C, R> action)
        {
            _actions.Add(action.Name, action);
        }

        public List<Result> Parse(VNScript script, VNErrorAccumulator errors, VNString indexRange)
        {
            List<Result> results = new List<Result>();
            IndexRange range = new IndexRange(script, indexRange.Start, indexRange.End);

            while (!range.Ended)
            {
                Result? result = _ParseAction(script, errors, range);
                if (result is Result resultValue)
                {
                    results.Add(resultValue);
                }
            }

            return results;
        }

        #endregion

        #region VNActionParser Private

        private void _ParseCharactersToIgnore(VNScript script, IndexRange range)
        {
            while (!range.Ended && _charactersToIgnore.Contains(script.ToCharacter(range.Index)))
            {
                range.Next();
            }
        }

        private Result? _ParseAction(VNScript script, VNErrorAccumulator errors, IndexRange range)
        {
            _ParseCharactersToIgnore(script, range);
            if (range.Ended)
            {
                return null;
            }

            VNAction<C, R>? action = _ParseActionName(script, errors, range);
            if (action == null)
            {
                return null;
            }

            List<VNString> arguments = _ParseActionArguments(script, errors, range, action.Value.ArgumentBracketPairs);
            if (arguments == null)
            {
                return null;
            }

            return new Result(action.Value, arguments);
        }

        private VNAction<C, R>? _ParseActionName(VNScript script, VNErrorAccumulator errors, IndexRange range)
        {
            ResultActionName parseResult = _ParseActionNameInternal(script, range);
            return _ParseActionNameHandleResult(script, errors, range, parseResult);
        }

        private ResultActionName _ParseActionNameInternal(VNScript script, IndexRange range)
        {
            VNIndex firstNameIndex = range.Index;
            VNIndex lastNameIndex = range.Index;
            bool leftArgumentBracketFound = false;
            VNIndex? firstUnallowedCharacter = null;
            VNIndex? lastUnallowedCharacter = null;

            while (!range.Ended)
            {
                char currentCharacter = script.ToCharacter(range.Index);

                if (currentCharacter == _argumentsBracketPair.Left)
                {
                    leftArgumentBracketFound = true;
                    range.Next();
                    break;
                }

                if (_charactersToIgnore.Contains(currentCharacter))
                {
                    break;
                }

                if (!_IsAllowedActionNameCharacter(currentCharacter))
                {
                    (firstUnallowedCharacter, lastUnallowedCharacter) = _UpdateUnallowedCharacterRange(firstUnallowedCharacter, range.Index);
                }

                lastNameIndex = range.Index;
                range.Next();
            }

            return new ResultActionName(firstNameIndex, lastNameIndex, leftArgumentBracketFound, firstUnallowedCharacter, lastUnallowedCharacter);
        }

        private VNAction<C, R>? _ParseActionNameHandleResult(VNScript script, VNErrorAccumulator errors, IndexRange range, ResultActionName result)
        {
            // Immediately parsed an argument left bracket
            if (range.Index == result.LastNameCharacter)
            {
                errors.Add(_ErrorNoActionNameForArguments(range.Index));
                _ParseUntilRightArgumentsBracket(script, errors, range);
                return null;
            }

            // Parsed an action name with illegal characters
            if (result.FirstUnallowedCharacter != null)
            {
                errors.Add(_ErrorActionNameContainsUnallowedCharacters(result.FirstUnallowedCharacter.Value, result.LastUnallowedCharacter.Value));
                if (result.LeftArgumentBracketParsed)
                {
                    _ParseUntilRightArgumentsBracket(script, errors, range);
                }
                else
                {
                    errors.Add(_ErrorNoArgumentsBrackets(result.FirstNameCharacter, result.LastNameCharacter));
                }
                return null;
            }

            // Parsed a valid action name with no arguments left bracket
            if (!result.LeftArgumentBracketParsed)
            {
                errors.Add(_ErrorNoArgumentsBrackets(result.FirstNameCharacter, result.LastNameCharacter));
                return null;
            }

            // Everything was okay
            string actionName = script.ToString(new VNString(result.FirstNameCharacter, result.LastNameCharacter), includeLastCharacter: true);

            if (!_actions.ContainsKey(actionName))
            {
                errors.Add(_ErrorUnregisteredActionName(result.FirstNameCharacter, result.LastNameCharacter));
                if (result.LeftArgumentBracketParsed)
                {
                    _ParseUntilRightArgumentsBracket(script, errors, range);
                }
                else
                {
                    errors.Add(_ErrorNoArgumentsBrackets(result.FirstNameCharacter, result.LastNameCharacter));
                }
                return null;
            }

            return _actions[actionName];
        }

        private bool _ParseUntilRightArgumentsBracket(VNScript script, VNErrorAccumulator errors, IndexRange range)
        {
            VNIndex start = range.Index;
            while (!range.Ended && script.ToCharacter(range.Index) != _argumentsBracketPair.Right)
            {
                range.Next();
            }
            if (range.Ended) // Didn't find the right arguments bracket
            {
                errors.Add(_ErrorNoArgumentRightBracket(start, range.Index));
                return false;
            }

            range.Next();
            return true;
        }

        private List<VNString> _ParseActionArguments(VNScript script, VNErrorAccumulator errors, IndexRange range, List<VNBracketPair> argumentBrackets)
        {
            List<VNString> arguments = new List<VNString>();

            bool isFirstArgument = true;
            foreach (VNBracketPair bracketPair in argumentBrackets)
            {
                if (!_ParseBetweenArguments(script, errors, range, isFirstArgument))
                {
                    return null;
                }
                isFirstArgument = false;

                VNString? argument = _ParseArgument(script, errors, range, bracketPair);
                if (argument == null)
                {
                    return null;
                }

                arguments.Add(argument.Value);
            }

            // Parse until right arguments bracket, ensuring empty space
            _ParseCharactersToIgnore(script, range);
            if (range.Ended)
            {
                _ParseUntilRightArgumentsBracket(script, errors, range);
                return null;
            }
            char characterFound = script.ToCharacter(range.Index);
            if (characterFound != _argumentsBracketPair.Right)
            {
                errors.Add(_ErrorExpectedArgumentsRightBracket(range.Index, characterFound));
                _ParseUntilRightArgumentsBracket(script, errors, range);
                return null;
            }
            // Means right bracket found
            range.Next();

            return arguments;
        }

        private bool _ParseBetweenArguments(VNScript script, VNErrorAccumulator errors, IndexRange range, bool isFirstArgument)
        {
            _ParseCharactersToIgnore(script, range);
            if (range.Ended)
            {
                errors.Add(_ErrorExpectedMoreArguments(range.Index));
                return false;
            }

            if (isFirstArgument) // Don't need to parse the argument separation character
            {
                return true;
            }

            char currentCharacter = script.ToCharacter(range.Index);
            if (currentCharacter != _argumentSeparationCharacter)
            {
                errors.Add(_ErrorExpectedArgumentSeparationCharacter(range.Index));
                _ParseUntilRightArgumentsBracket(script, errors, range);
                return false;
            }
            range.Next();

            _ParseCharactersToIgnore(script, range);
            if (range.Ended)
            {
                errors.Add(_ErrorExpectedMoreArguments(range.Index));
                return false;
            }

            return true;
        }

        private VNString? _ParseArgument(VNScript script, VNErrorAccumulator errors, IndexRange range, VNBracketPair bracketPair)
        {
            VNIndex leftbracketIndex = range.Index;
            char leftBracket = script.ToCharacter(range.Index);
            if (leftBracket != bracketPair.Left)
            {
                errors.Add(_ErrorExpectedArgumentBracketPair(bracketPair, range.Index, script.ToCharacter(range.Index)));
                _ParseUntilRightArgumentsBracket(script, errors, range);
                return null;
            }
            range.Next();
            VNIndex argumentStart = range.Index;

            while (!range.Ended && script.ToCharacter(range.Index) != bracketPair.Right)
            {
                range.Next();
            }
            if (range.Ended)
            {
                errors.Add(_ErrorArgumentBracketPairUnfinished(leftbracketIndex, range.Index, bracketPair));
                return null;
            }

            // Reaching here means the right bracket was found
            VNIndex argumentEnd = range.Index;
            range.Next();
            return new VNString(argumentStart, argumentEnd);
        }

        private bool _IsAllowedActionNameCharacter(char nameCharacter)
        {
            return nameCharacter == '.' || char.IsLetterOrDigit(nameCharacter);
        }

        private (VNIndex, VNIndex) _UpdateUnallowedCharacterRange(VNIndex? start, VNIndex newEnd)
        {
            return start == null ? (newEnd, newEnd) : (start.Value, newEnd);
        }

        #endregion

        #region VNActionParser Errors

        private VNError _ErrorUnregisteredActionName(VNIndex firstActionNameCharacter, VNIndex lastActionNameCharacter)
        {
            return new VNError(
                $"[VisualNovel][Parser][Actions] Found unregistered action name {firstActionNameCharacter}-{lastActionNameCharacter}.",
                VNError.EStatus.NonFatal,
                firstActionNameCharacter,
                lastActionNameCharacter
            );
        }

        private VNError _ErrorNoActionNameForArguments(VNIndex index)
        {
            return new VNError(
                $"[VisualNovel][Parser][Actions] Found arguments with no action name {index}.",
                VNError.EStatus.NonFatal,
                index,
                index
            );
        }

        private VNError _ErrorNoArgumentsBrackets(VNIndex firstActionNameCharacter, VNIndex lastActionNameCharacter)
        {
            return new VNError(
                $"[VisualNovel][Parser][Actions] Found action name with no arguments {firstActionNameCharacter}-{lastActionNameCharacter}.",
                VNError.EStatus.NonFatal,
                firstActionNameCharacter,
                lastActionNameCharacter
            );
        }

        private VNError _ErrorNoArgumentRightBracket(VNIndex leftBracket, VNIndex end)
        {
            return new VNError(
                $"[VisualNovel][Parser][Actions] Found arguments with no right bracket {leftBracket}-{end}.",
                VNError.EStatus.NonFatal,
                leftBracket,
                end
            );
        }

        private VNError _ErrorActionNameContainsUnallowedCharacters(VNIndex leftUnallowedCharacter, VNIndex rightUnallowedCharacter)
        {
            return new VNError(
                $"[VisualNovel][Parser][Actions] Action name contains unallowed characters {leftUnallowedCharacter}-{rightUnallowedCharacter}.",
                VNError.EStatus.NonFatal,
                leftUnallowedCharacter,
                rightUnallowedCharacter
            );
        }

        private VNError _ErrorExpectedArgumentSeparationCharacter(VNIndex index)
        {
            return new VNError(
                $"[VisualNovel][Parser][Actions] Expected '{_argumentSeparationCharacter}' between arguments {index}.",
                VNError.EStatus.NonFatal,
                index,
                index
            );
        }

        private VNError _ErrorExpectedMoreArguments(VNIndex index)
        {
            return new VNError(
                $"[VisualNovel][Parser][Actions] Expected more arguments {index}.",
                VNError.EStatus.NonFatal,
                index,
                index
            );
        }

        private VNError _ErrorExpectedArgumentBracketPair(VNBracketPair bracketPair, VNIndex index, char foundCharacter)
        {
            return new VNError(
                $"[VisualNovel][Parser][Actions] Expected argument to have left bracket '{bracketPair.Left}' and right bracket '{bracketPair.Right}', but found '{foundCharacter}' {index}.",
                VNError.EStatus.NonFatal,
                index,
                index
            );
        }

        private VNError _ErrorArgumentBracketPairUnfinished(VNIndex leftBracketIndex, VNIndex end, VNBracketPair bracketPair)
        {
            return new VNError(
                $"[VisualNovel][Parser][Actions] Expected argument to end with right bracket '{bracketPair.Right}' {leftBracketIndex}-{end}.",
                VNError.EStatus.NonFatal,
                leftBracketIndex,
                end
            );
        }

        private VNError _ErrorExpectedArgumentsRightBracket(VNIndex index, char foundCharacter)
        {
            return new VNError(
                $"[VisualNovel][Parser][Actions] Expected arguments to end with right bracket '{_argumentsBracketPair.Right}' but found '{foundCharacter}' {index}.",
                VNError.EStatus.NonFatal,
                index,
                index
            );
        }

        #endregion
    }
}
