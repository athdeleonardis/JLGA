using System.Collections.Generic;

namespace JLGA.Architecture.VisualNovel.VNScript.Listener
{
    public class VNGlobalFlags : IVNFlags
    {
        private static IVNFlags _instance;

        private readonly SortedDictionary<string, string> _flags;

        public IDictionary<string, string> Flags => _flags;

        private VNGlobalFlags()
        {
            _flags = new SortedDictionary<string, string>();
        }

        public static IVNFlags Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new VNGlobalFlags();
                }
                return _instance;
            }
        }

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
            _flags[flag] = value;
        }
    }
}
