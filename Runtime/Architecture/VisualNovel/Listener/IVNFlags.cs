using System.Collections.Generic;

namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNFlags
    {
        public void SetFlag(string flag, string value);
        public string GetFlag(string flag);
        public IDictionary<string, string> Flags { get; }
    }
}
