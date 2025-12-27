using System.Collections.Generic;

namespace JLGA.Architecture.State
{
    public interface IStateMachine<S,L>
        where S : class, IStateStack<S>
        where L : IStateListener
    {
        S Base();
        List<string> CurrentlyActiveStates();
        List<string> FormerlyActiveStates();
        SortedDictionary<string, List<L>> Listeners();
        void AddListener(L listener);
        void RemoveListener(L listener);
        void UpdateListeners();
    }
}
