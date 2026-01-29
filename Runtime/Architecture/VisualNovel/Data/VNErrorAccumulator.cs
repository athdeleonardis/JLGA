using System.Collections;
using System.Collections.Generic;

namespace JLGA.Architecture.VisualNovel.Data
{
    /// <summary>
    /// Visual Novel Error Accumulator. Represents a list of visual novel errors, keeping track of whether any errors added are fatal.
    /// </summary>
    public class VNErrorAccumulator
    {
        /// <summary>The list of visual novel errors.</summary>
        public List<VNError> Errors { get; }
        /// <summary>Whether any of the errors added are fatal.</summary>
        public bool IsFatal { get; private set;  }

        /// <summary>
        /// Create a visual novel error accumulator with no errors.
        /// </summary>
        public VNErrorAccumulator()
        {
            Errors = new List<VNError>();
        }

        /// <summary>
        /// Add a visual novel error to the list of errors, updating the overall fatality of the error list based on the error.
        /// </summary>
        /// <param name="error">The error to be added. If the error is fatal, the error accumulator is fatal.</param>
        public void Add(VNError error)
        {
            Errors.Add(error);
            IsFatal |= error.IsFatal;
        }

        /// <summary>
        /// Clears the list of visual novel errors, and becomes non-fatal.
        /// </summary>
        public void Clear()
        {
            Errors.Clear();
            IsFatal = false;
        }
    }
}
