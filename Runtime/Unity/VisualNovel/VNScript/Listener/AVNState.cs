using UnityEngine;
using JLGA.Architecture.VisualNovel.VNScript.Listener;

namespace JLGA.Unity.VisualNovel.VNScript.Listener
{
    public abstract class AVNState : MonoBehaviour, IVNState
    {
        public abstract void Initialize();
        public abstract void EnterDialogueChoice();
        public abstract void ExitDialogueChoice();
        public abstract void ShowDisplay();
        public abstract void HideDisplay();
        public abstract void Cleanup();
    }
}
