using System;
using System.Collections;
using System.Collections.Generic;

namespace JLGA.Architecture.VisualNovel.Data
{
    /// <summary>
    /// Visual Novel Action. Represents a named visual novel action whose arguments are surrounded by bracket pairs.
    /// </summary>
    /// <typeparam name="C">The context type provided to the callback function.</typeparam>
    /// <typeparam name="R">The return type of the callback function.</typeparam>
    public readonly struct VNAction<C, R>
    {
        /// <summary>The name of the VNAction.</summary>
        public string Name { get; }
        /// <summary>The list of bracket pairs respective arguments to the VNAction are expected to be surrounded by.</summary>
        public List<VNBracketPair> ArgumentBracketPairs { get; }
        /// <summary>
        /// The function to be called when the action is parsed.
        /// First argument is the context available.
        /// Second argument is the arguments parsed from a VNScript.
        /// Returns the result of the action.
        /// </summary>
        public Func<C, List<VNString>, VNErrorAccumulator, R> Callback { get; }

        /// <summary>
        /// Create a VNAction.
        /// </summary>
        /// <param name="name">The name of the VNAction.</param>
        /// <param name="argumentBracketPairs">The list of bracket pairs respective arguments to the VNAction are expected to be surrounded by.</param>
        /// <param name="callback">
        /// The function to be called when the action is parsed.
        /// First argument is the context available.
        /// Second argument is the arguments parsed from a VNScript.
        /// Returns the result of the action.
        /// </param>
        public VNAction(string name, VNBracketPair[] argumentBracketPairs, Func<C, List<VNString>, VNErrorAccumulator, R> callback)
        {
            Name = name;
            ArgumentBracketPairs = new List<VNBracketPair>(argumentBracketPairs);
            Callback = callback;
        }
    }
}
