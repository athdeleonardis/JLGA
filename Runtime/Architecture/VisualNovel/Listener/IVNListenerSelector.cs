namespace JLGA.Architecture.VisualNovel.Listener
{
    /// <summary>
    /// (Interface) Visual Novel Listener Selector. Represents one visual novel listener selected from a group of listeners.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IVNListenerSelector<T> where T : IVNListener
    {
        /// <summary>The currently selected visual novel listener.</summary>
        public T CurrentlySelected { get; }

        /// <summary>
        /// Add a listener to the selector. Returns whether the listener was successfully added.
        /// </summary>
        /// <param name="listener">The listener to be added to the listener selector.</param>
        /// <returns>True if the listener was successfully added. False otherwise.</returns>
        public bool AddListener(T listener);

        /// <summary>
        /// Select a listener, using the inputted argument as the listener's identifier. Returns whether the listener was successfully selected.
        /// </summary>
        /// <param name="name">The name of the listener to be selected.</param>
        /// <returns>True if the listener was successfully selected. False otherwise.</returns>
        public bool Select(string name);

        /// <summary>
        /// Unselect the currently selected listener.
        /// </summary>
        public void Unselect();

        /// <summary>
        /// Cleanup the listener selector.
        /// </summary>
        public void Cleanup();
    }
}
