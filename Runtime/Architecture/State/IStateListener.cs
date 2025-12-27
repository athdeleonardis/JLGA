namespace JLGA.Architecture.State
{
    public interface IStateListener
    {
        string StateName();
        void OnStateAdded();
        void OnStateRemoved();
    }
}
