using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLGA.Architecture.VisualNovel.VNScript.Listener
{
    public interface IVNState
    {
        void Initialize();
        void ShowDisplay();
        void HideDisplay();
        void EnterDialogueChoice();
        void ExitDialogueChoice();
        void Cleanup();
    }
}
