using System;
using System.Collections;
using System.Collections.Generic;

namespace JLGA.Architecture.VisualNovel.Data
{
    /// <summary>
    /// Represents a named visual novel action whose arguments are surrounded by bracket pairs.
    /// </summary>
    /// <typeparam name="C">The context type provided to the callback function.</typeparam>
    /// <typeparam name="R">The return type of the callback function.</typeparam>
    public readonly struct VNAction<C, R>
    {
        public string Name { get; }
        public List<VNBracketPair> ArgumentBracketPairs { get; }
        public Func<C, List<VNString>, VNErrorAccumulator, R> Callback { get; }

        public VNAction(string name, VNBracketPair[] argumentBracketPairs, Func<C, List<VNString>, VNErrorAccumulator, R> callback)
        {
            Name = name;
            ArgumentBracketPairs = new List<VNBracketPair>(argumentBracketPairs);
            Callback = callback;
        }
    }
}
