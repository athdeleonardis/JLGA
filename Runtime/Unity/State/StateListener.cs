using UnityEngine;

namespace JLGA.Unity.State
{
    public abstract class StateListener : MonoBehaviour, Architecture.State.IStateListener
    {
        [SerializeField] private string _stateName;

        #region Unity

        private void Start()
        {
            StateMachine.Instance().AddListener(this);
        }

        private void OnDestroy()
        {
            StateMachine.Instance().RemoveListener(this);
        }

        #endregion

        #region IStateListener

        public string StateName()
        {
            return _stateName;
        }

        public abstract void OnStateAdded();

        public abstract void OnStateRemoved();

        #endregion
    }
}
