using System;
using System.Collections;
using System.Collections.Generic;

namespace JLGA.Architecture.VisualNovel.VNScript.Data
{
    public readonly struct VNAction
    {
        public string Name { get; }
        public List<VNBracketPair> ArgumentBracketPairs { get; }
        public Action<List<VNString>> Callback { get; }

        public VNAction(string name, VNBracketPair[] argumentBracketPairs, Action<List<VNString>> callback)
        {
            Name = name;
            ArgumentBracketPairs = new List<VNBracketPair>(argumentBracketPairs);
            Callback = callback;
        }
    }
}
