namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNActor : IVNListener
    {
        void Initialize();
        void SetPosition(float x, float y, float z, float t);
        void SetRotation(float rx, float ry, float rz, float t);
        void SetScale(float sx, float sy, float sz, float t);
        bool SetAppearance(string appearance);
        bool PlayAnimation(string animation);
    }
}
