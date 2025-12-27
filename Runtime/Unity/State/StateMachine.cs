using System.Collections;
using System.Collections.Generic;

namespace JLGA.Unity.State
{
    public class StateMachine : Architecture.State.IStateMachine<StateStack, StateListener>
    {
        private static StateMachine _instance;

        private StateStack _base;
        private List<string> _formerlyActiveStates;
        private SortedDictionary<string, List<StateListener>> _listeners;

        #region StateMachine

        private StateMachine()
        {
            _formerlyActiveStates = new List<string>();
            _listeners = new SortedDictionary<string, List<StateListener>>();
        }

        public static StateMachine Instance()
        {
            if (_instance == null)
            {
                _instance = new StateMachine();
            }
            return _instance;
        }

        public void SetBase(StateStack baseStack)
        {
            _base = baseStack;
        }

        public void RemoveBase(StateStack baseStack)
        {
            if (_base == baseStack)
            {
                _base = null;
            }
        }

        #endregion

        #region IStateMachine

        public StateStack Base()
        {
            return _base;
        }

        public List<string> CurrentlyActiveStates()
        {
            List<string> currentlyActiveStates = new List<string>();

            StateStack currentStack = Base();
            while (currentStack != null)
            {
                currentlyActiveStates.Add(currentStack.StateName());
                currentStack = currentStack.Top();
            }

            return currentlyActiveStates;
        }

        public List<string> FormerlyActiveStates()
        {
            return _formerlyActiveStates;
        }

        public SortedDictionary<string, List<StateListener>> Listeners()
        {
            return _listeners;
        }

        public void AddListener(StateListener listener)
        {
            if (!_listeners.ContainsKey(listener.StateName()))
            {
                _listeners.Add(listener.StateName(), new List<StateListener>());
            }

            List<StateListener> listenersList = _listeners[listener.StateName()];
            listenersList.Add(listener);

            if (_formerlyActiveStates.Contains(listener.StateName())) {
                listener.OnStateAdded();
            }
        }

        public void RemoveListener(StateListener listener)
        {
            if (!_listeners.ContainsKey(listener.StateName()))
            {
                return;
            }

            _listeners[listener.StateName()].Remove(listener);
        }

        public void UpdateListeners()
        {
            List<string> currentlyActiveStates = CurrentlyActiveStates();

            SortedSet<string> addedStates = new SortedSet<string>(currentlyActiveStates);
            addedStates.ExceptWith(_formerlyActiveStates);
            SortedSet<string> removedStates = new SortedSet<string>(_formerlyActiveStates);
            removedStates.ExceptWith(currentlyActiveStates);

            foreach (string stateName in removedStates)
            {
                _DisableListeners(stateName);
            }

            foreach (string stateName in addedStates)
            {
                _EnableListeners(stateName);
            }

            _formerlyActiveStates = currentlyActiveStates;
        }

        #endregion

        private void _EnableListeners(string stateName)
        {
            if (!_listeners.ContainsKey(stateName))
            {
                return;
            }

            foreach (StateListener listener in _listeners[stateName])
            {
                listener.OnStateAdded();
            }
        }

        private void _DisableListeners(string stateName)
        {
            if (!_listeners.ContainsKey(stateName))
            {
                return;
            }

            foreach (StateListener listener in _listeners[stateName])
            {
                listener.OnStateRemoved();
            }
        }
    }
}
