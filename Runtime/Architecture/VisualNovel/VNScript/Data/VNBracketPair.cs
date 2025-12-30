using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLGA.Architecture.VisualNovel.VNScript.Data
{
    public readonly struct VNBracketPair
    {
        public VNBracketPair(char leftBracket, char rightBracket)
        {
            Left = leftBracket;
            Right = rightBracket;
        }

        public char Left { get; }
        public char Right { get; }
    }
}
