using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLGA.Architecture.VisualNovel.Listener
{
    public interface IVNListenerSelector<T> where T : IVNListener
    {
        public T CurrentlySelected { get; }

        public bool AddListener(T listener);
        public bool Select(string name);
        public void Unselect();
        public void Cleanup();
    }
}
