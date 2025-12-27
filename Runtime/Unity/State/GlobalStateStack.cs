using UnityEngine;

namespace JLGA.Unity.State
{
    public class GlobalStateStack : MonoBehaviour
    {
        [SerializeField] private StateStack _globalStack;

        void Start()
        {
            StateMachine stateMachine = StateMachine.Instance();
            stateMachine.SetBase(_globalStack);
            stateMachine.UpdateListeners();
        }

        private void OnDestroy()
        {
            StateMachine stateMachine = StateMachine.Instance();
            stateMachine.RemoveBase(_globalStack);
            stateMachine.UpdateListeners();
        }
    }
}
