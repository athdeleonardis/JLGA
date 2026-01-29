namespace JLGA.Architecture.VisualNovel.Data
{
    /// <summary>
    /// Visual Novel Error. Represents an error in a VNScript.
    /// </summary>
    public readonly struct VNError
    {
        /// <summary>The description of the error.</summary>
        public string Description { get; }
        /// <summary>The outcome of the error.</summary>
        public EStatus Status { get; }
        /// <summary>Whether the outcome of the error is fatal.</summary>
        public bool IsFatal { get; }
        /// <summary>The possibly unspecified start index of the error.</summary>
        public VNIndex? Start { get; }
        /// <summary>The possibly unspecified end index of the error.</summary>
        public VNIndex? End { get; }

        /// <summary>
        /// An enum representing the outcome of a VNError.
        /// </summary>
        public enum EStatus
        {
            NonFatal,
            Fatal
        }

        /// <summary>
        /// Create a VNError with an unspecified position in a VNScript.
        /// </summary>
        /// <param name="description">The description of the error.</param>
        /// <param name="status">The outcome of the error.</param>
        public VNError(string description, EStatus status)
        {
            Description = description;
            Status = status;
            IsFatal = status == EStatus.Fatal;
            Start = null;
            End = null;
        }

        /// <summary>
        /// Create a VNError with a specified start index and possibly unspecified end index.
        /// </summary>
        /// <param name="description">The description of the error.</param>
        /// <param name="status">The outcome of the error.</param>
        /// <param name="start">The start index of the error.</param>
        /// <param name="end">The possibly unspecified end index of the error.</param>
        public VNError(string description, EStatus status, VNIndex start, VNIndex? end)
        {
            Description = description;
            Status = status;
            IsFatal = status == EStatus.Fatal;
            Start = start;
            End = end;
        }

        /// <summary>
        /// Create a VNError with specified start and end indices.
        /// </summary>
        /// <param name="description">The description of the error.</param>
        /// <param name="status">The outcome of the error.</param>
        /// <param name="errorString">The VNString representing the start and end indices of the error.</param>
        public VNError(string description, EStatus status, VNString errorString)
        {
            Description = description;
            Status = status;
            IsFatal = status == EStatus.Fatal;
            Start = errorString.Start;
            End = errorString.End;
        }
    }
}
