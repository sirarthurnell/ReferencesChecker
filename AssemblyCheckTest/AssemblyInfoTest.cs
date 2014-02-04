using AssemblyCheck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AssemblyCheckTest
{    
    [TestClass]
    public class AssemblyInfoTest
    {
        private string _pathToAssembly;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _pathToAssembly = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(new char[] { '\\' }) +
                "\\Mono.Cecil.dll";
        }

        /// <summary>
        ///Una prueba de ReadAssembly
        ///</summary>
        [TestMethod]
        public void ReadAssemblyTest()
        {
            AssemblyInfo info = AssemblyInfo.ReadAssembly(_pathToAssembly);
            Assert.AreEqual("Mono.Cecil", info.Name);
            Assert.AreEqual("0738eb9f132ed756", info.PublicKeyToken);
            Assert.AreEqual("", info.Culture);
            Assert.AreEqual(info.Version.ToString(), "0.9.5.0");
            Assert.IsTrue(info.References.Count > 0);
            Assert.AreEqual(info.Path, _pathToAssembly);
        }
    }
}
