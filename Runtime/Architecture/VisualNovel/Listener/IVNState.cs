namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNState
    {
        void Initialize();
        void ShowDisplay();
        void HideDisplay();
        void EnterDialogueChoice();
        void ExitDialogueChoice();
        void Cleanup();
    }
}
