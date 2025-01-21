namespace ModernTest.ModernBaseLibrary.Core
{
    using System.IO;

    using global::ModernBaseLibrary.Core.IO;
    using global::ModernBaseLibrary.Cryptography;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FileFilter_Test : BaseTest
    {
        public string TestDirPath => TestContext.TestRunDirectory;

        public string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void SetUp()
        {
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void BuildFileFilter_AlleDateien()
        {
            using (FileFilter fileFilter = new FileFilter())
            {
                fileFilter.AddFilter("Alle Dateien", "*", true);
                string filter = fileFilter.GetFileFilter();
                Assert.AreEqual(filter, "Alle Dateien (*.*)|*.*");

                int filterIndex = fileFilter.DefaultFilterIndex;
                Assert.AreEqual(filterIndex, 1);
            }
        }

        [TestMethod]
        public void BuildFileFilter_MehrereDateien()
        {
            using (FileFilter fileFilter = new FileFilter())
            {
                fileFilter.AddFilter("Alle Dateien", "*", true);
                fileFilter.AddFilter("Images", "*.png;*.jpg", false);
                string filter = fileFilter.GetFileFilter();
                Assert.AreEqual(filter, "Alle Dateien (*.*)|*.*|Images (*.png;*.jpg;)|*.png;*.jpg;");

                int filterIndex = fileFilter.DefaultFilterIndex;
                Assert.AreEqual(filterIndex, 1);
            }
        }

        [TestMethod]
        public void BuildFileFilter_SetDefault()
        {
            using (FileFilter fileFilter = new FileFilter())
            {
                fileFilter.AddFilter("Alle Dateien", "*", false);
                fileFilter.AddFilter("Images", "*.png;*.jpg", false);

                fileFilter.SetDefaultFilter("Images");
                string defaultExt = fileFilter.DefaultExtension;
                Assert.AreEqual(defaultExt, "*.png;*.jpg");
            }
        }
    }
}