using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLGA.Architecture.VisualNovel.VNScript.Listener
{
    public interface IVNControlledDisplaySelector<T> where T : IVNDisplay
    {
        T CurrentlySelected { get; }

        bool Select(string displayName);
        bool AddDisplay(T display);
        void Unselect();
        void Cleanup();
    }
}
