namespace JLGA.Architecture.VisualNovel.Listener
{
    /// <summary>
    /// (Interface) Visual Novel Listener. Represents an object accessible by a visual novel.
    /// </summary>
    public interface IVNListener
    {
        /// <summary>The name used by a visual novel to identify the listener.</summary>
        string VNListenerName { get; }

        /// <summary>Set the name used by a visual novel to identify the listener.</summary>
        /// <param name="name">The new name to identify the listener.</param>
        void SetVNListenerName(string name);

        /// <summary>
        /// Cleanup the visual novel listener.
        /// </summary>
        void Cleanup();
    }
}
