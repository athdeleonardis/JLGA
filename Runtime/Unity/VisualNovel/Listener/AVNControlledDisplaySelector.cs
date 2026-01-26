using UnityEngine;
using JLGA.Architecture.VisualNovel.Listener;

namespace JLGA.Unity.VisualNovel.Listener
{
    public abstract class AVNControlledDisplaySelector : MonoBehaviour, IVNControlledDisplaySelector<AVNDisplay>
    {
        public abstract AVNDisplay CurrentlySelected { get; }

        public abstract bool Select(string displayName);
        public abstract bool AddDisplay(AVNDisplay display);
        public abstract void Unselect();
        public abstract void Cleanup();
    }
}
