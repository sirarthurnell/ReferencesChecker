using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph;

namespace AssemblyCheckTest
{
    [TestClass]
    public class QuickGraphExplorationTest
    {
        [TestMethod]
        public void TestCreateBasicGraph()
        {
            int value1 = 1;
            int value2 = 2;

            var graph = new AdjacencyGraph<int, TaggedEdge<int, string>>();
            graph.AddVertex(value1);
            graph.AddVertex(value2);

            var edge1 = new TaggedEdge<int, string>(value1, value2, "simple connection");            
            graph.AddEdge(edge1);

            Assert.AreEqual(graph.VertexCount, 2);
            Assert.AreEqual(graph.EdgeCount, 1);
        }
    }
}
