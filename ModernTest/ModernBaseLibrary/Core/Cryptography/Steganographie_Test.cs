namespace ModernBaseLibrary.Cryptography
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernBaseLibrary.Extension;

    using ModernTest.ModernBaseLibrary;

    /// <summary>
    ///     Test to find out if BmpPwd works
    /// </summary>
    [TestClass]
    public class Steganographie_Test : BaseTest
    {
        private const string Key = "Passwort.2023";
        private const string Text = "Das ist ein Text der in einem Bitmap verschüsselt ist.12345678901";

        [TestMethod]
        public void TestBmpPwdLineSaveLoad()
        {
            string saveImage = $"{Path.GetDirectoryName(this.GetAssemblyPath.FullName)}\\SveImage.png";
            Image encrypted = ImagePassword.Encrypt(Key, Text, new Cipher(), DrawingScheme.Circular, ColorScheme.Rainbow);

            using (var memory = new MemoryStream())
            {
                encrypted.Save(memory, ImageFormat.Png);
                var bytes = memory.ToArray();
                BitmapSource imageSource = (BitmapSource)ByteToImage(bytes);

                using (var fileStream = new FileStream(saveImage, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(imageSource as BitmapSource));
                    encoder.Save(fileStream);
                }
            }

            string decrypted = string.Empty;
            var bytesIn = File.ReadAllBytes(saveImage);
            var biImg = new BitmapImage();
            using (var ms = new MemoryStream(bytesIn))
            {
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                using (var outStream = new MemoryStream())
                {
                    BitmapEncoder enc = new BmpBitmapEncoder();
                    enc.Frames.Add(BitmapFrame.Create(biImg));
                    enc.Save(outStream);
                    Bitmap bitmap = new Bitmap(outStream);

                    decrypted = ImagePassword.Decrypt(Key, bitmap, new Cipher(), DrawingScheme.Circular, ColorScheme.Rainbow);
                }
            }

            Assert.AreEqual(Text, decrypted);
        }

        private ImageSource ByteToImage(byte[] imageData)
        {
            var biImg = new BitmapImage();
            var ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg;

            return imgSrc;
        }


        [TestMethod]
        public void TestBmpPwdLine()
        {
            Image encrypted = ImagePassword.Encrypt(Key, Text, new Cipher(), DrawingScheme.Line, ColorScheme.Rainbow);
            string decrypted = ImagePassword.Decrypt(Key, encrypted, new Cipher(), DrawingScheme.Line, ColorScheme.Rainbow);

            Assert.AreEqual(Text, decrypted);
        }

        [TestMethod]
        public void TestBmpPwdCircle()
        {
            var encrypted = ImagePassword.Encrypt(Key, Text, new Cipher(), DrawingScheme.Circular, ColorScheme.Rainbow);
            string decrypted = ImagePassword.Decrypt(Key, encrypted, new Cipher(), DrawingScheme.Circular, ColorScheme.Rainbow);

            Assert.AreEqual(Text, decrypted);
        }

        [TestMethod]
        public void TestBmpPwdSquare()
        {
            var encrypted = ImagePassword.Encrypt(Key, Text, new Cipher(), DrawingScheme.Square, ColorScheme.Rainbow);
            string decrypted = ImagePassword.Decrypt(Key, encrypted, new Cipher(), DrawingScheme.Square, ColorScheme.Rainbow);

            Assert.AreEqual(Text, decrypted);
        }


        [TestMethod]
        public void TestNormalEncryption()
        {
            var cipher = new Cipher();
            string encrypted = cipher.Encrypt(Key, Text);
            string decrypted = cipher.Decrypt(Key, encrypted);

            Assert.AreEqual(Text, decrypted);
        }
    }
}