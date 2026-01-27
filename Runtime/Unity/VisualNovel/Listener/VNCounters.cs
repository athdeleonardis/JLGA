using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLGA.Unity.VisualNovel.Listener
{
    public class VNCounters : AVNCounters
    {
        private SortedDictionary<string, float> _counters;

        private void Awake()
        {
            _counters = new SortedDictionary<string, float>();
        }

        public override void Set(string counterName, float value)
        {
            _counters[counterName] = value;
        }

        public override void Add(string counterName, float value)
        {
            if (_counters.TryGetValue(counterName, out float counterValue))
            {
                _counters[counterName] = counterValue + value;
                return;
            }
            Set(counterName, value);
        }

        public override float? Get(string counterName)
        {
            if (_counters.TryGetValue(counterName, out float counterValue))
            {
                return counterValue;
            }
            return null;
        }
    }
}
