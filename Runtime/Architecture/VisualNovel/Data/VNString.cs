namespace JLGA.Architecture.VisualNovel.Data
{
    /// <summary>
    /// Represents a portion of text in a VNScript.
    /// </summary>
    public readonly struct VNString
    {
        /// <summary>The start VNIndex of the VNString.</summary>
        public VNIndex Start { get; }
        /// <summary>The end VNIndex of the VNString</summary>
        public VNIndex End { get; }

        /// <summary>
        /// Create a VNString with a start VNIndex and an end VNIndex.
        /// </summary>
        /// <param name="start">The start VNIndex of the VNString.</param>
        /// <param name="end">The end VNIndex of the VNString.</param>
        public VNString(VNIndex start, VNIndex end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Create a new VNString with the inputted start VNIndex, and this VNString's end VNIndex.
        /// </summary>
        /// <param name="start">The start VNIndex of the new VNString.</param>
        /// <returns>A VNString with the inputted start VNIndex, and this VNString's end VNIndex.</returns>
        public VNString StartFrom(VNIndex start)
        {
            return new VNString(start, End);
        }

        public override string ToString()
        {
            return $"{Start}-{End}";
        }
    }
}
