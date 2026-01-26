namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNControlledDisplaySelector<T> where T : IVNDisplay
    {
        T CurrentlySelected { get; }

        bool Select(string displayName);
        bool AddDisplay(T display);
        void Unselect();
        void Cleanup();
    }
}
