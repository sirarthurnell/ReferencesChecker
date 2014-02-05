using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph;
using AssemblyCheck;

namespace AssemblyCheckTest
{
    [TestClass]
    public class ReferencesAnalyzerTest
    {
        [TestMethod]
        public void TestCheckGoodTargetReference()
        {
            AssemblyInfo source = new AssemblyInfo("Mono.Cecil, Version=0.9.5.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
            AssemblyInfo target = new AssemblyInfo("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            source.References.Add(target);

            ReferencesAnalyzer analyzer = new ReferencesAnalyzer();
            analyzer.Initialize(new AssemblyInfo[]{source, target});
            var graph = analyzer.ComposeGraph();

            CheckIntegrity(source, target, graph);

            TaggedEdge<AssemblyInfo, string> edge;
            Assert.IsTrue(graph.TryGetEdge(source, target, out edge));
            Assert.IsNotNull(edge);
            Assert.IsTrue(edge.Tag == "Ok");
        }

        [TestMethod]
        public void TestCheckBadTargetReference()
        {
            AssemblyInfo source = new AssemblyInfo("Mono.Cecil, Version=0.9.5.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
            AssemblyInfo badReference = new AssemblyInfo("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            AssemblyInfo target = new AssemblyInfo("mscorlib, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            source.References.Add(badReference);

            ReferencesAnalyzer analyzer = new ReferencesAnalyzer();
            analyzer.Initialize(new AssemblyInfo[] { source, target });
            var graph = analyzer.ComposeGraph();

            CheckIntegrity(source, target, graph);

            TaggedEdge<AssemblyInfo, string> edge;
            Assert.IsTrue(graph.TryGetEdge(source, target, out edge));
            Assert.IsNotNull(edge);
            Assert.IsTrue(edge.Tag == "Broken");
        }

        private void CheckIntegrity(AssemblyInfo source, AssemblyInfo target, AdjacencyGraph<AssemblyInfo, TaggedEdge<AssemblyInfo, string>> graph)
        {
            Assert.IsTrue(graph.ContainsVertex(source));
            Assert.IsTrue(graph.ContainsVertex(target));
            Assert.IsTrue(graph.VertexCount == 2);
            Assert.IsTrue(graph.EdgeCount == 1);
        }
    }
}
