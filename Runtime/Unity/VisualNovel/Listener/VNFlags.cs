using UnityEngine;
using JLGA.Architecture.VisualNovel.Listener;
using System.Collections.Generic;

namespace JLGA.Unity.VisualNovel.Listener
{
    public class VNFlags : MonoBehaviour, IVNFlags
    {
        private SortedDictionary<string, string> _flags;

        private void Awake()
        {
            _flags = new SortedDictionary<string, string>();
        }

        public IDictionary<string, string> Flags => _flags;

        public string GetFlag(string flag)
        {
            if (_flags.TryGetValue(flag, out string value))
            {
                return value;
            }
            return null;
        }

        public void SetFlag(string flag, string value)
        {
            if (value == null)
            {
                if (_flags.ContainsKey(flag))
                {
                    _flags.Remove(flag);
                }
                return;
            }
            _flags[flag] = value;
        }
    }
}
