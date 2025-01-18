namespace ModernTest.ModernBaseLibrary.Core
{
    using System.IO;

    using global::ModernBaseLibrary.Core.IO;
    using global::ModernBaseLibrary.Cryptography;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SecureDelete_Test : BaseTest
    {
        public string TestDirPath => TestContext.TestRunDirectory;

        public string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void SetUp()
        {
            Directory.CreateDirectory(TempDirPath);
        }

        [TestCleanup]
        public void Clean()
        {
            if (Directory.Exists(TempDirPath))
            {
                Directory.Delete(TempDirPath, true);
            }
        }

        [TestMethod]
        public void SecureDelete_File()
        {
            const string FILE = @"c:\temp\testT_1m.tmp";
            RandomContentFile f = new RandomContentFile();
            f.CreateRandomTextFile(FILE, 1, FileSizeUnit.Mb);

            SecureDelete wipe = new SecureDelete();
            wipe.SecureDeleteFile(FILE,2);
        }
    }
}