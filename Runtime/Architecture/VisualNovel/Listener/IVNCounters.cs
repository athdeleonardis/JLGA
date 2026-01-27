using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNCounters
    {
        void Set(string counterName, float value);
        void Add(string counterName, float value);
        float? Get(string counterName);
    }
}
