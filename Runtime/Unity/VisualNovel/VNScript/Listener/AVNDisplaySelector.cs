using UnityEngine;
using JLGA.Architecture.VisualNovel.VNScript.Listener;

namespace JLGA.Unity.VisualNovel.VNScript.Listener
{
    public abstract class AVNDisplaySelector : MonoBehaviour, IVNDisplaySelector<AVNDisplay>
    {
        public abstract AVNDisplay CurrentlySelected { get; }

        public abstract bool AddDisplay(AVNDisplay display);
        public abstract void Cleanup();
        public abstract bool Select(string displayName);
    }
}
