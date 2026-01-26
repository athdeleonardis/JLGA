namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNSound : IVNListener
    {
        public bool Load(string soundName);
        public void Play();
        public void Loop();
        public void Pause();
        public void Unpause();
    }
}
