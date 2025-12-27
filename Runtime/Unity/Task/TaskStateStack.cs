using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLGA.Unity.Task
{
    public class TaskStateStack : Task
    {
        [SerializeField] private bool _doActivate;
        [SerializeField] private bool _doListenerUpdate;
        [SerializeField] private State.StateStack _stateStack;

        #region TaskStateStack

        public override void BeginTask()
        {
            if (_doActivate)
            {
                _stateStack.ActivateState();
            }
            else
            {
                _stateStack.DeactivateState();
            }

            if (_doListenerUpdate)
            {
                State.StateMachine.Instance().UpdateListeners();
            }

            EndTask();
        }

        protected override void _EndTaskInternal() { }
    }

    #endregion
}
