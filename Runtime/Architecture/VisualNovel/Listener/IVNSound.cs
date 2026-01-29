namespace JLGA.Architecture.VisualNovel.Listener
{
    /// <summary>
    /// (Interface) Visual Novel Sound. Represents a sound accessible by a visual novel.
    /// </summary>
    public interface IVNSound : IVNListener
    {
        /// <summary>
        /// Load a sound. Returns whether the load was successful.
        /// </summary>
        /// <param name="soundName">The identifier to load the sound.</param>
        /// <returns>True if the load was successful. False otherwise.</returns>
        public bool Load(string soundName);

        /// <summary>Play the currently loaded sound.</summary>
        public void Play();

        /// <summary>Loop the currently loaded sound.</summary>
        public void Loop();

        /// <summary>Pause the currently playing/looping sound.</summary>
        public void Pause();

        /// <summary>Unpause the currently paused sound.</summary>
        public void Unpause();
    }
}
