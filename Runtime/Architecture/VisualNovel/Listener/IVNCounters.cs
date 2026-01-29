using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLGA.Architecture.VisualNovel.Listener
{
    /// <summary>
    /// (Interface) Visual Novel Counters. Represents a string-to-float mapping accessible by a visual novel.
    /// </summary>
    public interface IVNCounters
    {
        /// <summary>
        /// Set the value of a counter.
        /// </summary>
        /// <param name="counterName">The name of the counter to have it's value set.</param>
        /// <param name="value">The value to set the counter to.</param>
        void Set(string counterName, float value);

        /// <summary>
        /// Add a value to a counter's current value.
        /// </summary>
        /// <param name="counterName">The name of the counter to have it's value added to.</param>
        /// <param name="value">The value to add to the counter's current value.</param>
        void Add(string counterName, float value);

        /// <summary>
        /// Get the value of a counter.
        /// </summary>
        /// <param name="counterName">The name of the counter.</param>
        /// <returns>The value of the counter. If the counter doesn't exist, returns null.</returns>
        float? Get(string counterName);
    }
}
