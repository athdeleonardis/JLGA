using UnityEngine;
using JLGA.Architecture.VisualNovel.VNScript.Listener;

namespace JLGA.Unity.VisualNovel.VNScript.Listener
{
    public abstract class AVNActorSelector : MonoBehaviour, IVNActorSelector<AVNActor>
    {
        public abstract AVNActor CurrentlySelected { get; }

        public abstract bool Select(string actorName);
        public abstract bool AddActor(AVNActor actor);
        public abstract void Cleanup();
    }
}
