namespace JLGA.Architecture.VisualNovel.VNScript.Data
{
    public readonly struct VNError
    {
        public enum EStatus
        {
            NonFatal,
            Fatal
        }

        public VNError(string description, EStatus status)
        {
            Description = description;
            Status = status;
            IsFatal = status == EStatus.Fatal;
            Start = null;
            End = null;
        }

        public VNError(string description, EStatus status, VNIndex start, VNIndex? end)
        {
            Description = description;
            Status = status;
            IsFatal = status == EStatus.Fatal;
            Start = start;
            End = end;
        }

        public VNError(string description, EStatus status, VNString errorString)
        {
            Description = description;
            Status = status;
            IsFatal = status == EStatus.Fatal;
            Start = errorString.Start;
            End = errorString.End;
        }

        public string Description { get; }
        public EStatus Status { get; }
        public bool IsFatal { get; }
        public VNIndex? Start { get; }
        public VNIndex? End { get; }
    }
}
