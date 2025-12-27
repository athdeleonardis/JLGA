using UnityEngine;

namespace JLGA.Unity.Task
{
    public abstract class Task : MonoBehaviour, Architecture.Task.ITask<Task>
    {
        [SerializeField] private Task _nextTask;

        #region Task

        public abstract void BeginTask();
        protected abstract void _EndTaskInternal();

        #endregion

        #region ITask

        public void EndTask()
        {
            _EndTaskInternal();
            NextTask();
        }

        public void NextTask()
        {
            if (_nextTask != null)
            {
                _nextTask.BeginTask();
            }
        }

        public void SetNextTask(Task task)
        {
            _nextTask = task;
        }

        public Task GetNextTask()
        {
            return _nextTask;
        }

        #endregion
    }
}
