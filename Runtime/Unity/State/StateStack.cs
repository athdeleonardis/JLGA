using System.Collections.Generic;
using UnityEngine;

namespace JLGA.Unity.State
{
    public class StateStack : MonoBehaviour, Architecture.State.IStateStack<StateStack>
    {
        [SerializeField] private string _stateName;
        [SerializeField] private StateStack _parent;
        [SerializeField] private List<StateStack> _children;

        #region IStateStack

        public string StateName()
        {
            return _stateName;
        }

        public StateStack Parent()
        {
            return _parent;
        }

        public List<StateStack> Children()
        {
            return _children;
        }

        public void SetParent(StateStack parent)
        {
            _parent = parent;
        }

        public void ActivateState()
        {
            StateStack parent = Parent();
            if (parent == null)
            {
                return;
            }
            parent.Push(this);
        }

        public void DeactivateState()
        {
            StateStack parent = Parent();
            if (parent == null)
            {
                return;
            }
            parent.PopChild(this);
        }

        public void PopChild(StateStack child)
        {
            Children().Remove(child);
        }

        public void PopFromParent()
        {
            StateStack parent = Parent();
            if (parent != null)
            {
                parent.PopChild(this);
            }
        }

        public void Push(StateStack child)
        {
            Children().Add(child);
        }

        public StateStack Top()
        {
            List<StateStack> children = Children();
            if (children.Count == 0)
            {
                return null;
            }

            return children[children.Count - 1];
        }

        #endregion
    }
}
