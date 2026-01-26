using UnityEngine;
using JLGA.Architecture.VisualNovel.Listener;

namespace JLGA.Unity.VisualNovel.Listener
{
    public abstract class AVNSoundSelector : MonoBehaviour, IVNListenerSelector<AVNSound>;
    {
        public abstract AVNSound CurrentlySelected { get; }

        public abstract bool AddListener(AVNSound listener);
        public abstract bool Select(string name);
        public abstract void Unselect();
        public abstract void Cleanup();
    }
}
