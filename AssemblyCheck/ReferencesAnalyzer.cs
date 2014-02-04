using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyCheck
{
    /// <summary>
    /// Analyzes references between a set of assemblies.
    /// </summary>
    public class ReferencesAnalyzer
    {
        private List<AssemblyInfo> _infos;

        /// <summary>
        /// Initializes the analyzer.
        /// </summary>
        /// <param name="assemblies">Assemblies to check.</param>
        public void Initialize(IEnumerable<AssemblyInfo> assemblies)
        {
            _infos = new List<AssemblyInfo>(assemblies);
        }

        //TODO Make the analysis method.
    }
}
