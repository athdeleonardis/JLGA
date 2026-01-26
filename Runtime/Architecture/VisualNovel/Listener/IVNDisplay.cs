using System;

namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNDisplay : IVNListener
    {
        bool IsDialogueTextFinished { get; }
        bool IsDialogueOption { get; }
        bool IsShowing { get; }
        void Initialize();
        void SetNameText(string name);
        void SetDialogueText(string dialogue);
        void EndDialogueText();
        void AddDialogueOption(string dialogue, Action callback);
        void FinalizeDialogueOptions();
        void ChooseDialogueOption(int optionIndex);
        void Show();
        void Hide();
    }
}
