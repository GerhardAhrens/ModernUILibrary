namespace ModernTest.ModernBaseLibrary.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::ModernBaseLibrary.Comparer;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Comparer_Test
    {
        [TestMethod]
        public void OrdinalStringComparer_Test()
        {
            List<string> files = new List<string>();
            files.Add("File3");
            files.Add("File01");
            files.Add("File1");
            files.Add("File40");
            files.Add("File2");
            files.Add("File10");
            files.Add("File0");

            IList<string> lastFileNonSort = files.OrderBy(x => x).ToList();
            Assert.IsNotNull(lastFileNonSort);
            Assert.IsTrue(lastFileNonSort[0] == "File0");
            Assert.IsTrue(lastFileNonSort[2] == "File1");
            Assert.IsTrue(lastFileNonSort[3] == "File10");
            Assert.IsTrue(lastFileNonSort[4] == "File2");
            Assert.IsTrue(lastFileNonSort[6] == "File40");

            List<string> lastFileSort = files.OrderBy(x => x, new System.OrdinalStringComparer()).ToList();
            Assert.IsNotNull(lastFileSort);
            Assert.IsTrue(lastFileSort[0] == "File0");
            Assert.IsTrue(lastFileSort[2] == "File1");
            Assert.IsTrue(lastFileSort[3] == "File2");
            Assert.IsTrue(lastFileSort[4] == "File3");
            Assert.IsTrue(lastFileSort[6] == "File40");
        }

        [TestMethod]
        public void VersionStringComparer()
        {
            List<string> versionList = new List<string>();
            versionList.Add("1.0.0.0");
            versionList.Add("1.0.2020.0");
            versionList.Add("2.1.2020.12");
            versionList.Add("1.0.2020.1");
            versionList.Add("1.1.2020.10");
            versionList.Add("1.0.2020.9");
            versionList.Add("1.10.2020.19");
            versionList.Add("2.11.2020.21");
            versionList.Add("2.0.2020.11");

            IList<string> versionNonSort = versionList.OrderBy(x => x).ToList();
            Assert.IsNotNull(versionNonSort);
            Assert.IsTrue(versionNonSort[0] == "1.0.0.0");
            Assert.IsTrue(versionNonSort[8] == "2.11.2020.21");

            List<string> versionListSort = versionList.OrderBy(x => x, new VersionStringComparer()).ToList();
            Assert.IsNotNull(versionListSort);
            Assert.IsTrue(versionListSort[0] == "1.0.0.0");
            Assert.IsTrue(versionListSort[8] == "2.11.2020.21");
        }
    }
}