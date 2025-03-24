namespace ModernBaseLibrary.Cryptography
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernBaseLibrary.Extension;

    using ModernTest.ModernBaseLibrary;

    /// <summary>
    ///     Test to find out if BmpPwd works
    /// </summary>
    [TestClass]
    public class SteganographieV2_Test : BaseTest
    {
        private string TestDirPath => TestContext.TestRunDirectory;
        private string TempDirPath => Path.Combine(TestDirPath, "Temp");
        private const string KEY = "Passwort.2025";
        private const string TEXT = "Das ist ein Text der in einem Bitmap verschüsselt ist.12345678901";

        [TestInitialize]
        public void SetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            if (Directory.Exists(TempDirPath) == false)
            {
                Directory.CreateDirectory(TempDirPath);
            }
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void BildImageSaveAndLoad()
        {
            string decryptedText = string.Empty;
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Core\\Cryptography\\DemoData\\TestImage.png");
            string testFile = Path.Combine(Path.GetDirectoryName(pathFileName), $"{Path.GetFileNameWithoutExtension(pathFileName)}.Test.png");
            string testFileNew = Path.Combine(Path.GetDirectoryName(pathFileName), $"{Path.GetFileNameWithoutExtension(pathFileName)}.Neu.png");

            if (File.Exists(pathFileName) == true)
            {
                File.Copy(pathFileName, testFile, true );
                Image img = Image.FromFile(testFile);
                Bitmap newBitmap = new Bitmap(img.Width, img.Height,System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics graphics = Graphics.FromImage(newBitmap);
                graphics.DrawImage(img, 0, 0);
                graphics.Dispose();
                img.Dispose();

                Bitmap bmpResult = SteganographyHelper.EmbedText(TEXT.EncryptRSA(KEY), newBitmap);
                if (bmpResult != null)
                {
                    bmpResult.Save(testFileNew, ImageFormat.Png);
                }

                if (File.Exists(testFile) == true)
                {
                    File.Delete(testFile);
                }
            }

            if (File.Exists(testFileNew) == true)
            {
                Image img = Image.FromFile(testFileNew);
                Bitmap newBitmap = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics graphics = Graphics.FromImage(newBitmap);
                graphics.DrawImage(img, 0, 0);
                graphics.Dispose();
                img.Dispose();

                if (newBitmap != null)
                {
                    decryptedText = SteganographyHelper.ExtractText(newBitmap).DecryptRSA(KEY);
                }
            }

            Assert.AreEqual(TEXT, decryptedText);
        }
    }
}