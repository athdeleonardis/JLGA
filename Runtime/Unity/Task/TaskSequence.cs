using UnityEngine;

namespace JLGA.Unity.Task
{
    public class TaskSequence : Task
    {
        [SerializeField] private Task[] _tasks;

        #region Task

        public override void BeginTask()
        {
            if (_tasks.Length == 0)
            {
                EndTask();
                return;
            }

            _SequenceTasks();
            _tasks[0].BeginTask();
        }

        protected override void _EndTaskInternal() {}

        #endregion

        #region TaskSequence

        private void _SequenceTasks()
        {
            for (int i = 0; i < _tasks.Length - 1; i++)
            {
                _tasks[i].SetNextTask(_tasks[i + 1]);
            }

            _tasks[_tasks.Length - 1].SetNextTask(GetNextTask());
        }

        #endregion
    }
}