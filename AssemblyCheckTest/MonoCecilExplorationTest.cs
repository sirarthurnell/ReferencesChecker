using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mono.Cecil;

namespace AssemblyCheckTest
{
    [TestClass]
    public class MonoCecilExplorationTest
    {
        private string _pathToAssembly;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _pathToAssembly = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[]{'\\'}) + 
                "\\AssemblyCheck.dll";
        }

        [TestMethod]
        public void LoadAssemblyReferencesTest()
        {
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(_pathToAssembly);
            List<AssemblyNameReference> references = new List<AssemblyNameReference>();

            foreach (ModuleDefinition module in assembly.Modules)
            {
                foreach (AssemblyNameReference reference in module.AssemblyReferences)
                {
                    references.Add(reference);
                }
            }

            Assert.IsTrue(references.Count > 0);
        }
    }
}
