namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNListeners<A, D>
        where A : IVNActor
        where D : IVNDisplay
    {
        public IVNActorSelector<A> ActorSelector { get; }
        public IVNDisplaySelector<D> DisplaySelector { get; }
        public IVNControlledDisplaySelector<D> ControlledDisplaySelector { get; }
        public IVNFlags Flags { get; }
        public IVNState State { get; }

        public void Initialize();
        public void Cleanup();
    }
}
