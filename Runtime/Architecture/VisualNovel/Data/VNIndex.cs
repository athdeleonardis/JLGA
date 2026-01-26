using System;

namespace JLGA.Architecture.VisualNovel.Data
{
    public readonly struct VNIndex : IEquatable<VNIndex>
    {
        public VNIndex(int line, int character)
        {
            Line = line;
            Character = character;
        }

        public int Line { get; }
        public int Character { get; }

        public static bool operator ==(VNIndex left, VNIndex right)
        {
            return left.Character == right.Character && left.Line == right.Line;
        }

        public static bool operator !=(VNIndex left, VNIndex right)
        {
            return left.Character != right.Character || left.Line != right.Line;
        }

        public bool Equals(VNIndex other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj is VNIndex index && this == index;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Line, Character);
        }

        public override string ToString()
        {
            return $"({Line},{Character})";
        }
    }
}
