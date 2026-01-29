namespace JLGA.Architecture.VisualNovel.Listener
{
    /// <summary>
    /// (Interface) Visual Novel Listeners. Represents all objects that are accessible to a visual novel.
    /// </summary>
    /// <typeparam name="A">The actor type.</typeparam>
    /// <typeparam name="D">The display type.</typeparam>
    /// <typeparam name="S">The sound type.</typeparam>
    public interface IVNListeners<A, D, S>
        where A : IVNActor
        where D : IVNDisplay
        where S : IVNSound
    {
        /// <summary>Used to select the current actor.</summary>
        public IVNListenerSelector<A> ActorSelector { get; }
        /// <summary>Used to select the current display.</summary>
        public IVNListenerSelector<D> DisplaySelector { get; }
        /// <summary>Used to select the current display targetted by controls.</summary>
        public IVNListenerSelector<D> ControlledDisplaySelector { get; }
        /// <summary>Used to select the current sound.</summary>
        public IVNListenerSelector<S> SoundSelector { get; }
        /// <summary>Used to set and get from a string-to-string mapping.</summary>
        public IVNFlags Flags { get; }
        /// <summary>Used to set and get from a string-to-float mapping.</summary>
        public IVNCounters Counters { get; }
        /// <summary>Used to update the current state of the visual novel.</summary>
        public IVNState State { get; }

        /// <summary>Initialize the visual novel listeners.</summary>
        public void Initialize();
        /// <summary>Clean up the visual novel listeners.</summary>
        public void Cleanup();
    }
}
