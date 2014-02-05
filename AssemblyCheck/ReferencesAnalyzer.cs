using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;
using AssemblyCheck.References;

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
        Broken
    }

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

        /// <summary>
        /// Composes the references graph.
        /// </summary>
        /// <returns>Graph of references between assemblies.</returns>
        public AdjacencyGraph<AssemblyInfo, TaggedEdge<AssemblyInfo, string>> ComposeGraph()
        {
            var graph = new AdjacencyGraph<AssemblyInfo, TaggedEdge<AssemblyInfo, string>>();
            graph.AddVertexRange(_infos);

            UpdateGraph(graph);

            return graph;
        }

        /// <summary>
        /// Updates the graph with references existent
        /// between assemblies.
        /// </summary>
        /// <param name="graph">Graph to update.</param>
        private void UpdateGraph(AdjacencyGraph<AssemblyInfo, TaggedEdge<AssemblyInfo, string>> graph)
        {
            foreach (AssemblyInfo info in _infos)
            {
                AddReferences(info, graph);
            }
        }

        /// <summary>
        /// Adds the assembly's references to the graph.
        /// </summary>
        /// <param name="source">Info of the assembly.</param>
        /// <param name="graph">References graph.</param>
        private void AddReferences(AssemblyInfo source, AdjacencyGraph<AssemblyInfo, TaggedEdge<AssemblyInfo, string>> graph)
        {
            foreach (AssemblyInfo reference in source.References)
            {
                foreach (AssemblyInfo target in _infos)
                {
                    var state = reference.CheckReference(target);
                    switch (state)
                    {
                        case ReferenceState.None:
                            break;

                        case ReferenceState.Ok:
                            var okEdge = new TaggedEdge<AssemblyInfo, string>(source, target, "Ok");
                            graph.AddEdge(okEdge);
                            break;

                        case ReferenceState.Broken:
                            var brokenEdge = new TaggedEdge<AssemblyInfo, string>(source, target, "Broken");
                            graph.AddEdge(brokenEdge);
                            break;                        

                        default:
                            throw new InvalidOperationException("Unrecognized reference state.");
                    }
                }
            }
        }
    }
}
