namespace JLGA.Architecture.VisualNovel.VNScript.Listener
{
    public interface IVNActorSelector<T> where T : IVNActor
    {
        T CurrentlySelected { get; }

        bool Select(string actorName);
        bool AddActor(T actor);
        void Cleanup();
    }
}
