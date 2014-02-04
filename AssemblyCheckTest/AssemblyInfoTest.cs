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

        [TestMethod]
        public void ReadAssemblyTest()
        {
            AssemblyInfo info = AssemblyInfo.ReadAssembly(_pathToAssembly);
            Assert.AreEqual("Mono.Cecil", info.Name);
            Assert.AreEqual("Mono.Cecil, Version=0.9.5.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756", info.FullName);
            Assert.AreEqual("0738eb9f132ed756", info.PublicKeyToken);
            Assert.AreEqual("", info.Culture);
            Assert.AreEqual(info.Version.ToString(), "0.9.5.0");
            Assert.IsTrue(info.References.Count > 0);
        }

        [TestMethod]
        public void ReadReferenceTest()
        {
            AssemblyInfo info = AssemblyInfo.ReadAssembly(_pathToAssembly);
            AssemblyInfo reference = info.References[0];

            Assert.AreEqual("mscorlib", reference.Name);
            Assert.AreEqual("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", reference.FullName);
            Assert.AreEqual("b77a5c561934e089", reference.PublicKeyToken);
            Assert.AreEqual("", reference.Culture);
            Assert.AreEqual(reference.Version.ToString(), "4.0.0.0");
            Assert.IsTrue(reference.References.Count == 0);
        }

        [TestMethod]
        public void ToStringEqualsFullName()
        {
            AssemblyInfo info = AssemblyInfo.ReadAssembly(_pathToAssembly);
            Assert.AreEqual(info.ToString(), info.FullName);
        }
    }
}
