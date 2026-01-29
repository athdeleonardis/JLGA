using System;

namespace JLGA.Architecture.VisualNovel.Data
{
    /// <summary>
    /// Visual Novel Bracket Pair. Represents a left bracket and a right bracket in a VNScript.
    /// </summary>
    public readonly struct VNBracketPair : IEquatable<VNBracketPair>
    {
        /// <summary>The bracket pair's left bracket.</summary>
        public char Left { get; }
        /// <summary>The bracket pair's right bracket.</summary>
        public char Right { get; }

        /// <summary>
        /// Create a VNBracket pair with a left and right bracket.
        /// </summary>
        /// <param name="leftBracket">The bracket pair's left bracket.</param>
        /// <param name="rightBracket">The bracket pair's right bracket.</param>
        public VNBracketPair(char leftBracket, char rightBracket)
        {
            Left = leftBracket;
            Right = rightBracket;
        }

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
