namespace JLGA.Architecture.VisualNovel.Listener
{
    /// <summary>
    /// (Interface) Visual Novel State. Represents the current state of a visual novel.
    /// </summary>
    public interface IVNState
    {
        /// <summary>Initialize the visual novel state.</summary>
        void Initialize();
        /// <summary>Enter the state where the display is shown.</summary>
        void ShowDisplay();
        /// <summary>Enter the state where the display is hidden.</summary>
        void HideDisplay();
        /// <summary>Enter the state where dialogue choices are chosen.</summary>
        void EnterDialogueChoice();
        /// <summary>Exit from the state where dialogue choices are chosen.</summary>
        void ExitDialogueChoice();
        /// <summary>Cleanup the visual novel state.</summary>
        void Cleanup();
    }
}
