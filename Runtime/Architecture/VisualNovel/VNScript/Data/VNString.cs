namespace JLGA.Architecture.VisualNovel.VNScript.Data
{
    public readonly struct VNString
    {
        public VNString(VNIndex start, VNIndex end)
        {
            Start = start;
            End = end;
        }

        public VNIndex Start { get; }
        public VNIndex End { get; }

        public VNString StartFrom(VNIndex start)
        {
            return new VNString(start, End);
        }
    }
}
