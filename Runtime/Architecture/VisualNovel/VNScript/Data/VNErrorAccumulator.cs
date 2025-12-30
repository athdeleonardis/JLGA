using System.Collections;
using System.Collections.Generic;

namespace JLGA.Architecture.VisualNovel.VNScript.Data
{
    public class VNErrorAccumulator
    {
        public List<VNError> Errors { get; }
        public bool IsFatal { get; private set;  }

        public VNErrorAccumulator()
        {
            Errors = new List<VNError>();
        }

        public void Add(VNError error)
        {
            Errors.Add(error);
            IsFatal |= error.IsFatal;
        }

        public void Clear()
        {
            Errors.Clear();
            IsFatal = false;
        }
    }
}
