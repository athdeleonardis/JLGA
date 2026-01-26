namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNDisplaySelector<T> where T : IVNDisplay
    {
        T CurrentlySelected { get; }

        bool Select(string displayName);
        bool AddDisplay(T display);
        void Cleanup();
    }
}
