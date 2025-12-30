namespace JLGA.Architecture.VisualNovel.VNScript.Data
{
    public readonly struct VNIndex
    {
        public VNIndex(int line, int character)
        {
            Line = line;
            Character = character;
        }

        public int Line { get; }
        public int Character { get; }
    }
}
