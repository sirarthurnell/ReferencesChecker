using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph;
using QuickGraph.Serialization;
using System.Xml;
using System.IO;

namespace AssemblyCheckTest
{
    [TestClass]
    public class QuickGraphExplorationTest
    {
        [TestMethod]
        public void TestCreateBasicGraph()
        {
            var graph = CreateGraph();
            Assert.AreEqual(graph.VertexCount, 2);
            Assert.AreEqual(graph.EdgeCount, 1);
        }

        [TestMethod]
        public void TestExportGraph()
        {
            string pathToFile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }) + "\\simpleGraph.gml";
            var graph = CreateGraph();
            using (XmlWriter xwr = XmlWriter.Create(pathToFile))
            {
                graph.SerializeToGraphML<int, TaggedEdge<int, string>, BidirectionalGraph<int, TaggedEdge<int, string>>>(xwr);
            }

            Assert.IsTrue(File.Exists(pathToFile));
        }

        private BidirectionalGraph<int, TaggedEdge<int, string>> CreateGraph()
        {
            int value1 = 1;
            int value2 = 2;

            var graph = new BidirectionalGraph<int, TaggedEdge<int, string>>();
            graph.AddVertex(value1);
            graph.AddVertex(value2);

            var edge1 = new TaggedEdge<int, string>(value1, value2, "simple connection");
            graph.AddEdge(edge1);

            return graph;
        }
    }
}
