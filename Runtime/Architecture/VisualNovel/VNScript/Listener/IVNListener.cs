namespace JLGA.Architecture.VisualNovel.VNScript.Listener
{
    public interface IVNListener
    {
        string VNListenerName { get; }
        void SetVNListenerName(string name);
        void Cleanup();
    }
}
