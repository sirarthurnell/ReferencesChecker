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
        public AnalysisResult Analyze()
        {
            var graph = new AnalysisResult();
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
        public void SaveAnalysis(AnalysisResult graph, string pathToFile)
        {
            try
            {
                using (XmlWriter xwr = XmlWriter.Create(pathToFile))
                {
                    graph.SerializeToGraphML<AssemblyInfo, ReferenceEdge, AnalysisResult>(xwr);
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
        private void UpdateGraph(AnalysisResult graph)
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
        private void AddReferences(AssemblyInfo source, AnalysisResult graph)
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
                            var okEdge = new ReferenceEdge(source, target, ReferenceState.Ok);
                            graph.AddEdge(okEdge);
                            existsBetweenAssemblies = true;
                            break;

                        case ReferenceState.Broken:
                            var brokenEdge = new ReferenceEdge(source, target, ReferenceState.Broken);
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
                    var brokenEdge = new ReferenceEdge(source, reference, ReferenceState.NotExistent);
                    graph.AddEdge(brokenEdge);
                }
            }
        }
    }
}
