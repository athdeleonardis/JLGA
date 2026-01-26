using System;

namespace JLGA.Architecture.VisualNovel.Data
{
    public readonly struct VNBracketPair : IEquatable<VNBracketPair>
    {
        public VNBracketPair(char leftBracket, char rightBracket)
        {
            Left = leftBracket;
            Right = rightBracket;
        }

        public char Left { get; }
        public char Right { get; }

        public static bool operator ==(VNBracketPair left, VNBracketPair right)
        {
            return left.Left == right.Left && left.Right == right.Right;
        }

        public static bool operator !=(VNBracketPair left, VNBracketPair right)
        {
            return left.Left != right.Left || left.Right != right.Right;
        }

        public bool Equals(VNBracketPair other)
        {
            return this == other;
        }

        public override bool Equals(object other)
        {
            return other is VNBracketPair otherPair && this == otherPair;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Right);
        }
    }
}
