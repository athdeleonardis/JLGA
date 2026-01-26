namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNListeners<A, D>
        where A : IVNActor
        where D : IVNDisplay
    {
        public IVNListenerSelector<A> ActorSelector { get; }
        public IVNListenerSelector<D> DisplaySelector { get; }
        public IVNListenerSelector<D> ControlledDisplaySelector { get; }
        public IVNFlags Flags { get; }
        public IVNState State { get; }

        public void Initialize();
        public void Cleanup();
    }
}
