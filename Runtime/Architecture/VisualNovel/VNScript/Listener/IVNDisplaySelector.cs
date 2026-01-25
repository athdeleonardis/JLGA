using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLGA.Architecture.VisualNovel.VNScript.Listener
{
    public interface IVNDisplaySelector<T> where T : IVNDisplay
    {
        T CurrentlySelected { get; }

        bool Select(string displayName);
        bool AddDisplay(T display);
        void Cleanup();
    }
}
