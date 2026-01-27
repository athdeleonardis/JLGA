using UnityEngine;
using JLGA.Architecture.VisualNovel.Listener;

namespace JLGA.Unity.VisualNovel.Listener
{
    public abstract class AVNCounters : MonoBehaviour, IVNCounters
    {
        public abstract void Set(string counterName, float value);
        public abstract void Add(string counterName, float value);
        public abstract float? Get(string counterName);
    }
}
