using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCheck.References
{
    public static class ReferenceExtensions
    {
        /// <summary>
        /// Checks the reference against an assembly to see
        /// the state of that reference.
        /// </summary>
        /// <param name="reference">Reference to check.</param>
        /// <param name="target">Target to compare against.</param>
        /// <returns>State of the reference.</returns>
        public static ReferenceState CheckReference(this AssemblyInfo reference, AssemblyInfo target)
        {
            if (String.Compare(reference.GetFullNameWithoutVersion(), target.GetFullNameWithoutVersion(), StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                return ReferenceState.None;
            }

            if (target.Version >= reference.Version)
            {
                return ReferenceState.Ok;
            }

            return ReferenceState.Broken;
        }
    }
}
