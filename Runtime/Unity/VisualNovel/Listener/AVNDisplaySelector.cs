using UnityEngine;
using JLGA.Architecture.VisualNovel.Listener;

namespace JLGA.Unity.VisualNovel.Listener
{
    public abstract class AVNDisplaySelector : MonoBehaviour, IVNDisplaySelector<AVNDisplay>
    {
        public abstract AVNDisplay CurrentlySelected { get; }

        public abstract bool AddDisplay(AVNDisplay display);
        public abstract bool Select(string displayName);
        public abstract void Cleanup();
    }
}
