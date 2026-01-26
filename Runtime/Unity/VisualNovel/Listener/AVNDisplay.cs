using UnityEngine;
using JLGA.Architecture.VisualNovel.Listener;
using System;

namespace JLGA.Unity.VisualNovel.Listener
{
    public abstract class AVNDisplay : MonoBehaviour, IVNDisplay
    {
        public abstract string VNListenerName { get; }
        public abstract bool IsShowing { get; }
        public abstract bool IsDialogueTextFinished { get; }
        public abstract bool IsDialogueOption { get; }

        public abstract void SetVNListenerName(string name);
        public abstract void Initialize();
        public abstract void SetNameText(string name);
        public abstract void SetDialogueText(string dialogue);
        public abstract void EndDialogueText();
        public abstract void AddDialogueOption(string dialogue, Action callback);
        public abstract void FinalizeDialogueOptions();
        public abstract void ChooseDialogueOption(int optionIndex);
        public abstract void Show();
        public abstract void Hide();
        public abstract void Cleanup();
    }
}
