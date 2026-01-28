using System;

namespace JLGA.Architecture.VisualNovel.Data
{
    /// <summary>
    /// Represents a line and character of a VNScript.
    /// </summary>
    public readonly struct VNIndex : IEquatable<VNIndex>
    {
        /// <summary>The zero-indexed line of a VNScript.</summary>
        public int Line { get; }
        /// <summary>The zero-indexed character of a line of a VNScript.</summary>
        public int Character { get; }

        /// <summary>
        /// Create a VNIndex with a particular line and character of the line.
        /// </summary>
        /// <param name="line">The zero-indexed line of a VNScript.</param>
        /// <param name="character">The zero-indexed character of the line of a VNScript.</param>
        public VNIndex(int line, int character)
        {
            Line = line;
            Character = character;
        }

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
