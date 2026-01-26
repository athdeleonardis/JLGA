namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNListener
    {
        string VNListenerName { get; }
        void SetVNListenerName(string name);
        void Cleanup();
    }
}
