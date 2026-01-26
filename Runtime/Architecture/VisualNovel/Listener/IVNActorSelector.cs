namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNActorSelector<T> where T : IVNActor
    {
        T CurrentlySelected { get; }

        bool Select(string actorName);
        bool AddActor(T actor);
        void Cleanup();
    }
}
