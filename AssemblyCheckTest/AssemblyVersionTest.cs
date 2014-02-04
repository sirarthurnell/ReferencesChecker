using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssemblyCheck;

namespace AssemblyCheckTest
{
    /// <summary>
    /// Unit tests for AssemblyVersion class.
    /// </summary>
    [TestClass]
    public class AssemblyVersionTest
    {
        [TestMethod]
        public void TestStringConstructor()
        {
            AssemblyVersion version1 = new AssemblyVersion("1.2.3.4");
            List<int> parts = new List<int>(version1.Parts);

            for(int i = 0; i < parts.Count; i++)
            {
                Assert.AreEqual(i + 1, parts[i]);
            }
        }

        [TestMethod]
        public void TestToString()
        {
            AssemblyVersion version1 = new AssemblyVersion(new int[] {1,1,1,1});
            Assert.AreEqual(version1.ToString(), "1.1.1.1");
        }

        [TestMethod]
        public void TestComparation()
        {
            AssemblyVersion version1 = new AssemblyVersion("65535.65535.65535.65535");
            AssemblyVersion version2 = new AssemblyVersion("1.1.1.1");
            AssemblyVersion version3 = new AssemblyVersion("1.1.1.1");

            Assert.IsTrue(version1 > version2);
            Assert.IsTrue(version1 >= version2);
            Assert.IsTrue(version2 < version1);
            Assert.IsTrue(version2 <= version1);
            Assert.IsTrue(version2 == version3);
        }
    }
}
