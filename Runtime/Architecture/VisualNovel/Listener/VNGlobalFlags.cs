using System.Collections.Generic;

namespace JLGA.Architecture.VisualNovel.Listener
{
    /// <summary>
    /// (Interface) Visual Novel Global Flags. Represents a globally accessible string-to-string mapping.
    /// </summary>
    public class VNGlobalFlags : IVNFlags
    {
        private static IVNFlags _instance;

        private readonly SortedDictionary<string, string> _flags;

        public IDictionary<string, string> Flags => _flags;
        
        /// <summary>The globally accessible instance.</summary>
        public static IVNFlags Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VNGlobalFlags();
                }
                return _instance;
            }
        }

        private VNGlobalFlags()
        {
            _flags = new SortedDictionary<string, string>();
        }

        public void SetFlag(string flag, string value)
        {
            _flags[flag] = value;
        }

        public string GetFlag(string flag)
        {
            if (_flags.TryGetValue(flag, out string value))
            {
                return value;
            }
            return null;
        }
    }
}
