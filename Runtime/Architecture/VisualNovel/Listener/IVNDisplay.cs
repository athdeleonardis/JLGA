using System;

namespace JLGA.Architecture.VisualNovel.Listener
{
    /// <summary>
    /// (Interface) Visual Novel Display. Represents a visual novel's display, updated as a visual novel script is read.
    /// </summary>
    public interface IVNDisplay : IVNListener
    {
        /// <summary>Whether the currently displayed line of dialogue text has finished displaying.</summary>
        bool IsDialogueTextFinished { get; }

        /// <summary>Whether the currently displayed content is dialogue options or not.</summary>
        bool IsDialogueOption { get; }
        /// <summary>Whether the display is showing or hidden.</summary>
        bool IsShowing { get; }

        /// <summary>
        /// Initialize the visual novel display.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Set the currently displayed character name.
        /// </summary>
        /// <param name="name">The character name to display.</param>
        void SetNameText(string name);

        /// <summary>
        /// Set the currently displayed line of dialogue text.
        /// </summary>
        /// <param name="dialogue">The line of dialogue text to display.</param>
        void SetDialogueText(string dialogue);

        /// <summary>
        /// If the current line of dialogue text has not finished displaying, finishes displaying it.
        /// </summary>
        void EndDialogueText();

        /// <summary>
        /// Add a dialogue option with a displayed line of text, and a callback upon choosing.
        /// </summary>
        /// <param name="dialogue">The line of dialogue to display for the dialogue option.</param>
        /// <param name="callback">The callback to be done if the dialogue option is chosen.</param>
        void AddDialogueOption(string dialogue, Action callback);

        /// <summary>
        /// Display the currently added dialogue options.
        /// </summary>
        void FinalizeDialogueOptions();

        /// <summary>
        /// Choose a dialogue option, indexed by the order it was added.
        /// </summary>
        /// <param name="optionIndex">The index of the dialogue option to choose.</param>
        void ChooseDialogueOption(int optionIndex);

        /// <summary>
        /// Show the visual novel display.
        /// </summary>
        void Show();

        /// <summary>
        /// Hide the visual novel display.
        /// </summary>
        void Hide();
    }
}
