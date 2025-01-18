namespace ModernTest.ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.FluentAPI;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FluentDirectoryInfoExtension_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        public FluentDirectoryInfoExtension_Test()
        {
        }

        [TestMethod]
        public void ToDirectoryInfo()
        {
            string folder = "c:\\Temp";
            DirectoryInfo di = folder.That().ToDirectoryInfo();
            Assert.AreEqual(di.GetType(), typeof(DirectoryInfo));
        }

        [TestMethod]
        public void GetFiles()
        {
            string folder = this.GetAssemblyPath.FullName;
            DirectoryInfo di = folder.That().ToDirectoryInfo();
            IEnumerable<FileInfo> files = di.That().GetFiles(@"\.dll|\.pdb");
            Assert.IsNotNull(files);
            Assert.IsTrue(files.Count() > 0);
        }

        [TestMethod]
        public void GetFiles_Length()
        {
            string folder = this.GetAssemblyPath.FullName;
            DirectoryInfo di = folder.That().ToDirectoryInfo();
            long filesLength = di.That().GetFiles(@"\.dll|\.pdb").Length();
            Assert.IsTrue(filesLength > 0);
        }

        [TestMethod]
        public void GetFiles_LengthAsText()
        {
            string folder = this.GetAssemblyPath.FullName;
            DirectoryInfo di = folder.That().ToDirectoryInfo();
            string filesLength = di.That().GetFiles(@"\.dll|\.pdb").LengthAsText();
        }

        [TestMethod]
        public void GetFiles_Count()
        {
            string folder = this.GetAssemblyPath.FullName;
            DirectoryInfo di = folder.That().ToDirectoryInfo();
            int filesCount = di.That().GetFiles(@"\.dll|\.pdb").CountFiles();
            Assert.IsTrue(filesCount > 0);
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }
    }
}
