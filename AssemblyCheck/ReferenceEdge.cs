using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;

namespace AssemblyCheck
{
    /// <summary>
    /// State of a reference between two assemblies.
    /// </summary>
    public enum ReferenceState
    {
        /// <summary>
        /// No reference between them.
        /// </summary>
        None,

        /// <summary>
        /// Reference can work.
        /// </summary>
        Ok,

        /// <summary>
        /// Reference is set, but unsatisfied.
        /// </summary>
        Broken,

        /// <summary>
        /// Reference is set, but the corresponding
        /// assembly doesn't exist.
        /// </summary>
        NotExistent
    }

    /// <summary>
    /// Represents a reference between two assemblies.
    /// </summary>
    public class ReferenceEdge : Edge<AssemblyInfo>
    {
        public ReferenceState State { get; set; }

        public ReferenceEdge(AssemblyInfo source, AssemblyInfo target)
            : base(source, target)
        {
            State = ReferenceState.None;
        }

        public ReferenceEdge(AssemblyInfo source, AssemblyInfo target, ReferenceState state) : base(source, target)
        {
            State = state;
        }
    }
}
