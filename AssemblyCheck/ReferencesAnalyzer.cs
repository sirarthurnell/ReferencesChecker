using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;
using QuickGraph.Serialization;
using AssemblyCheck.References;
using System.Xml;
using System.IO;

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
        /// Saves the graph to file.
        /// </summary>
        /// <param name="graph">Graph containing the results
        /// of the analysis.</param>
        /// <param name="pathToFile">Path to the file.</param>
        /// <remarks>The export format is GML.</remarks>
        public void SaveGraph(AdjacencyGraph<AssemblyInfo, TaggedEdge<AssemblyInfo, string>> graph, string pathToFile)
        {
            try
            {
                using (XmlWriter xwr = XmlWriter.Create(pathToFile))
                {
                    graph.SerializeToGraphML<AssemblyInfo, TaggedEdge<AssemblyInfo, string>, AdjacencyGraph<AssemblyInfo, TaggedEdge<AssemblyInfo, string>>>(xwr);
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Unable to export the references graph.", ex);
            }            
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
                bool existsBetweenAssemblies = false;
                foreach (AssemblyInfo target in _infos)
                {
                    var state = reference.CheckReference(target);
                    switch (state)
                    {
                        case ReferenceState.None:
                            break;

                        case ReferenceState.Ok:
                            var okEdge = new TaggedEdge<AssemblyInfo, string>(source, target, ReferenceState.Ok.ToString());
                            graph.AddEdge(okEdge);
                            existsBetweenAssemblies = true;
                            break;

                        case ReferenceState.Broken:
                            var brokenEdge = new TaggedEdge<AssemblyInfo, string>(source, target, ReferenceState.Broken.ToString());
                            graph.AddEdge(brokenEdge);
                            existsBetweenAssemblies = true;
                            break;

                        default:
                            throw new InvalidOperationException("Unrecognized reference state.");
                    }
                }

                if (!existsBetweenAssemblies)
                {
                    graph.AddVertex(reference);
                    var brokenEdge = new TaggedEdge<AssemblyInfo, string>(source, reference, ReferenceState.NotExistent.ToString());
                    graph.AddEdge(brokenEdge);
                }
            }
        }
    }
}
