using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickGraph;
using AssemblyCheck;
using System.IO;

namespace AssemblyCheckTest
{
    [TestClass]
    public class ReferencesAnalyzerTest
    {
        [TestMethod]
        public void TestIntegrity()
        {
            AssemblyInfo source = new AssemblyInfo("Mono.Cecil, Version=0.9.5.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
            AssemblyInfo target = new AssemblyInfo("mscorlib, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            source.References.Add(target);

            ReferencesAnalyzer analyzer = new ReferencesAnalyzer();
            analyzer.Initialize(new AssemblyInfo[] { source, target });
            var graph = analyzer.Analyze();

            Assert.IsTrue(graph.ContainsVertex(source));
            Assert.IsTrue(graph.ContainsVertex(target));
            Assert.IsTrue(graph.VertexCount == 2);
            Assert.IsTrue(graph.EdgeCount == 1);

            ReferenceEdge edge;
            Assert.IsTrue(graph.TryGetEdge(source, target, out edge));
            Assert.IsNotNull(edge);
        }

        [TestMethod]
        public void TestCheckOkTargetReference()
        {
            AssemblyInfo source = new AssemblyInfo("Mono.Cecil, Version=0.9.5.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
            AssemblyInfo target = new AssemblyInfo("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            source.References.Add(target);

            ReferencesAnalyzer analyzer = new ReferencesAnalyzer();
            analyzer.Initialize(new AssemblyInfo[]{source, target});
            var graph = analyzer.Analyze();

            ReferenceEdge edge;
            graph.TryGetEdge(source, target, out edge);
            Assert.IsTrue(edge.State == ReferenceState.Ok);
        }

        [TestMethod]
        public void TestCheckBrokenTargetReference()
        {
            AssemblyInfo source = new AssemblyInfo("Mono.Cecil, Version=0.9.5.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
            AssemblyInfo badReference = new AssemblyInfo("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            AssemblyInfo target = new AssemblyInfo("mscorlib, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            source.References.Add(badReference);

            ReferencesAnalyzer analyzer = new ReferencesAnalyzer();
            analyzer.Initialize(new AssemblyInfo[] { source, target });
            var graph = analyzer.Analyze();

            ReferenceEdge edge;
            graph.TryGetEdge(source, target, out edge);
            Assert.IsTrue(edge.State == ReferenceState.Broken);
        }

        [TestMethod]
        public void TestCheckNotExistentTargetReference()
        {
            AssemblyInfo source = new AssemblyInfo("Mono.Cecil, Version=0.9.5.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
            AssemblyInfo notExistentReference = new AssemblyInfo("notexistent, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            AssemblyInfo target = new AssemblyInfo("mscorlib, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            source.References.Add(notExistentReference);

            ReferencesAnalyzer analyzer = new ReferencesAnalyzer();
            analyzer.Initialize(new AssemblyInfo[] { source, target });
            var graph = analyzer.Analyze();

            ReferenceEdge edge;
            graph.TryGetEdge(source, notExistentReference, out edge);
            Assert.IsTrue(edge.State == ReferenceState.NotExistent);
        }

        [TestMethod]
        public void TestSaveGraph()
        {
            AssemblyInfo source = new AssemblyInfo("Mono.Cecil, Version=0.9.5.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
            AssemblyInfo badReference = new AssemblyInfo("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            AssemblyInfo target = new AssemblyInfo("mscorlib, Version=3.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            source.References.Add(badReference);

            ReferencesAnalyzer analyzer = new ReferencesAnalyzer();
            analyzer.Initialize(new AssemblyInfo[] { source, target });
            var graph = analyzer.Analyze();

            string pathToFile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }) + "\\referencesGraph.gml";
            analyzer.SaveAnalysis(graph, pathToFile);

            Assert.IsTrue(File.Exists(pathToFile));
        }
    }
}
