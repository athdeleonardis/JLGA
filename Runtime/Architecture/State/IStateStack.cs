using System.Collections.Generic;

namespace JLGA.Architecture.State
{
    public interface IStateStack<S>
        where S : class, IStateStack<S>
    {
        string StateName();
        S Parent();
        List<S> Children();
        void SetParent(S parent);
        void ActivateState();
        void DeactivateState();
        void Push(S child);
        void PopChild(S child);
        void PopAll();
        S Top();
    }
}
