using UnityEngine;
using JLGA.Architecture.VisualNovel.Listener;

namespace JLGA.Unity.VisualNovel.Listener
{
    public abstract class AVNListenerSelector<T> : MonoBehaviour, IVNListenerSelector<T> where T : IVNListener
    {
        public abstract T CurrentlySelected { get; }

        public abstract bool AddListener(T listener);
        public abstract bool Select(string name);
        public abstract void Unselect();
        public abstract void Cleanup();
    }
}
