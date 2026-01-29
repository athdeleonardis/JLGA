using System.Collections.Generic;

namespace JLGA.Architecture.VisualNovel.Listener
{
    /// <summary>
    /// (Interface) Visual Novel Flags. Represents a string-to-string mapping accessible by a visual novel.
    /// </summary>
    public interface IVNFlags
    {
        /// <summary>The mapping from flag name to flag value.</summary>
        public IDictionary<string, string> Flags { get; }

        /// <summary>
        /// Set a flag to have a value.
        /// </summary>
        /// <param name="flag">The name of the flag to be set.</param>
        /// <param name="value">The value of the flag to be set.</param>
        public void SetFlag(string flag, string value);

        /// <summary>
        /// Get the value of a flag. Returns null if the flag does not exist.
        /// </summary>
        /// <param name="flag"></param>
        /// <returns>The value of the flag. Returns null if the flag does not exist.</returns>
        public string GetFlag(string flag);
    }
}
