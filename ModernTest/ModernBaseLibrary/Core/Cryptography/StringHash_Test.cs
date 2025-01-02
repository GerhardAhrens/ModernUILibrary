namespace ModernTest.ModernBaseLibrary.Cryptography
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Text;

    using global::ModernBaseLibrary.Cryptography;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringHash_Test : BaseHashAlgorithmTests
    {
        private readonly string beforeHashText = "Hallo, hier ist Gerhard";

        [TestMethod]
        public void StringHashMD5()
        {
            string result = string.Empty;

            using (StringHash sh = new StringHash(this.beforeHashText))
            {
                result = sh.ComputeHash(HashTyp.MD5);
            }

            Assert.IsTrue(result == "93f825872bc71b4e7c52884289dad1b4");
        }


        #region Crc32

        [TestMethod]
        public void Crc32_StaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            uint actual = Crc32.Compute(this.SimpleBytesAscii);

            Assert.AreEqual(actual, (uint)1233476865);
        }

        [TestMethod]
        public void Crc32_StaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Crc32.Compute(this.SimpleBytes2Ascii);

            Assert.AreEqual(actual, (uint)4175310668);
        }

        [TestMethod]
        public void Crc32_InstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Crc32());
            Assert.AreEqual((uint)2556801136, GetBigEndianUInt32(hash));
        }

        [TestMethod]
        public void Crc32_InstanceDefaultSeedAndPolynomialWith1MBinaryFile()
        {
            var hash = GetTestFileHash(Binary1MFileName, new Crc32());

            Assert.AreEqual((uint)2952719775, GetBigEndianUInt32(hash));
        }
        #endregion Crc32

        #region Crc64Iso
        [TestMethod]
        public void Crc64_StaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            ulong actual = Crc64Iso.Compute(this.SimpleBytesAscii);

            Assert.AreEqual(actual, (ulong)17006011519361926887);
        }
        #endregion Crc64Iso

        #region Create Random Files
        [TestMethod]
        public void CreateRandomFileWith1M()
        {
            CreateRandomFile(Binary1MFileName);
        }

        [TestMethod]
        public void CreateRandomFileWith12K()
        {
            CreateRandomFile(Binary12KFileName);
        }

        [TestMethod]
        public void CreateRandomFileWithJpg()
        {
            CreateRandomPictureFile(PictureFileNameJpg);
        }

        [TestMethod]
        public void CreateRandomFileWithPng()
        {
            CreateRandomPictureFile(PictureFileNamePng);
        }
        #endregion Create Random Files
    }

    public abstract class BaseHashAlgorithmTests
    {
        protected const string SimpleString = @"Gerhard, Beate und Budy.";
        protected readonly byte[] SimpleBytesAscii = Encoding.ASCII.GetBytes(SimpleString);
        protected const string SimpleString2 = @"Es gibt Programme die funktionieren, und es gibt gute Programms.";
        protected readonly byte[] SimpleBytes2Ascii = Encoding.ASCII.GetBytes(SimpleString2);
        protected readonly string RunFolder = AppDomain.CurrentDomain.BaseDirectory;
        protected readonly string PictureFileNamePng = "randomPictureFile.png";
        protected readonly string PictureFileNameJpg = "randomPictureFile.jpg";
        protected readonly string Binary12KFileName = "binary12K.bin";
        protected readonly string Binary1MFileName = "binary1M.bin";

        public DirectoryInfo GetAssemblyPath
        {
            get
            {
                string assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                return new DirectoryInfo(assemblyPath);
            }
        }

        protected byte[] GetTestFileHash(string name, HashAlgorithm hashAlgorithm)
        {
            var pathToTests = $"{this.GetAssemblyPath}\\Resources\\File";
            using (var stream = File.Open(Path.Combine(pathToTests, name), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return hashAlgorithm.ComputeHash(stream);
            }
        }

        protected void CreateRandomFile(string name)
        {
            var pathToTests = $"{this.GetAssemblyPath}\\Resources\\File\\{name}";
            if (Directory.Exists(Path.GetDirectoryName(pathToTests)) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(pathToTests));
            }

            CreateRandomFile(pathToTests, 1);
        }

        protected void CreateRandomPictureFile(string name)
        {
            var pathToTests = $"{this.GetAssemblyPath}\\Resources\\File\\{name}";
            if (Directory.Exists(Path.GetDirectoryName(pathToTests)) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(pathToTests));
            }

            CreateRandomFileJpeg(pathToTests, "Hallo PTA",200,200, ImageFormat.Png);
        }

        string CombinePaths(string baseFolder, params string[] folders)
        {
            var result = baseFolder;
            foreach (var folder in folders)
            {
                result = Path.Combine(result, folder);
            }

            return result;
        }

        protected static UInt32 GetBigEndianUInt32(byte[] bytes)
        {
            if (bytes.Length != 4)
            {
                throw new ArgumentOutOfRangeException("bytes", "Must be 4 bytes in length");
            }

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt32(bytes, 0);
        }

        protected static UInt64 GetBigEndianUInt64(byte[] bytes)
        {
            if (bytes.Length != 8)
            {
                throw new ArgumentOutOfRangeException("bytes", "Must be 8 bytes in length");
            }

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt64(bytes, 0);
        }

        protected static void CreateRandomFile(string filePath, double sizeInMb)
        {
            const int blockSize = 1024 * 8;
            const int blocksPerMb = (1024 * 1024) / blockSize;
            byte[] data = new byte[blockSize];
            Random rng = new Random();
            using (FileStream stream = File.OpenWrite(filePath))
            {
                for (int i = 0; i < sizeInMb * blocksPerMb; i++)
                {
                    rng.NextBytes(data);
                    stream.Write(data, 0, data.Length);
                }
            }
        }

        private static void CreateRandomFileJpeg(string outputPath, string nameToEmbed, int width, int height, ImageFormat pictureTyp)
        {
            using (var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb))
            {
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
                byte[] noise = new byte[data.Width * data.Height * 3];
                new Random().NextBytes(noise); // note that if you do that in a loop or from multiple threads - you may want to store this random in outside variable
                Marshal.Copy(noise, 0, data.Scan0, noise.Length);
                bmp.UnlockBits(data);
                using (var g = Graphics.FromImage(bmp))
                {
                    // draw white rectangle in the middle
                    g.FillRectangle(Brushes.White, 0, height / 2 - 20, width, 40);
                    var fmt = new StringFormat();
                    fmt.Alignment = StringAlignment.Center;
                    fmt.LineAlignment = StringAlignment.Center;
                    // draw text inside that rectangle
                    g.DrawString(nameToEmbed, SystemFonts.DefaultFont, Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), fmt);
                }
                using (var fs = File.Create(outputPath))
                {
                    if (pictureTyp == ImageFormat.Jpeg)
                    {
                        bmp.Save(fs, ImageFormat.Jpeg);
                    }
                    else if (pictureTyp == ImageFormat.Png)
                    {
                        bmp.Save(fs, ImageFormat.Png);
                    }
                }
            }
        }
    }
}