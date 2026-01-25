using UnityEngine;
using JLGA.Architecture.VisualNovel.VNScript.Listener;

namespace JLGA.Unity.VisualNovel.VNScript.Listener
{
    public abstract class AVNActor : MonoBehaviour, IVNActor
    {
        public abstract string VNListenerName { get; }

        public abstract void SetVNListenerName(string name);
        public abstract void Initialize();
        public abstract void SetPosition(float x, float y, float z, float t);
        public abstract void SetRotation(float rx, float ry, float rz, float t);
        public abstract void SetScale(float sx, float sy, float sz, float t);
        public abstract bool SetAppearance(string appearance);
        public abstract bool PlayAnimation(string animation);
        public abstract void Cleanup();
    }
}
