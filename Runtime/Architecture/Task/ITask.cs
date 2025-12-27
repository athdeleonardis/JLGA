namespace JLGA.Architecture.Task
{
    public interface ITask<T> where T : ITask<T>
    {
        void BeginTask();
        void SetNextTask(T task);
        T GetNextTask();
        void NextTask();
        void EndTask();
    }
}
