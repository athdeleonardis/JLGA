namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNListeners<A, D, S>
        where A : IVNActor
        where D : IVNDisplay
        where S : IVNSound
    {
        public IVNListenerSelector<A> ActorSelector { get; }
        public IVNListenerSelector<D> DisplaySelector { get; }
        public IVNListenerSelector<D> ControlledDisplaySelector { get; }
        public IVNListenerSelector<S> SoundSelector { get; }
        public IVNFlags Flags { get; }
        public IVNCounters Counters { get; }
        public IVNState State { get; }

        public void Initialize();
        public void Cleanup();
    }
}
