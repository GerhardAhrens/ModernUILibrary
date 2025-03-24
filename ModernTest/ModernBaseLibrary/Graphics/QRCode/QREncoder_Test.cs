/*
 * <copyright file="QREncoder_Test.cs" company="Lifeprojects.de">
 *     Class: QREncoder_Test
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>24.03.2025 19:56:19</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernTest.ModernBaseLibrary.Graphics.QRCode
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using global::ModernBaseLibrary.Barcode;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QREncoder_Test : BaseTest
    {
        private string TestDirPath => TestContext.TestRunDirectory;
        private string TempDirPath => Path.Combine(TestDirPath, "Temp");
        private const string TEXT = "Das ist ein Text für den QRCode Encoder/Decoder. Plus: 12345678901;";


        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            if (Directory.Exists(TempDirPath) == false)
            {
                Directory.CreateDirectory(TempDirPath);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QREncoder_Test"/> class.
        /// </summary>
        public QREncoder_Test()
        {
        }

        [TestMethod]
        public void QREncoderToFile()
        {
            bool[,] QRCodeMatrix;
            int ModuleSize = 4;
            int QuietZone = 8;
            Bitmap QRCodeImage;

            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Graphics\\QRCode\\DemoData\\QRCodeDemo.png");

            using (QREncoder encoder = new QREncoder())
            {
                encoder.ErrorCorrection = ErrorCorrection.M;
                encoder.ECIAssignValue = -1;
                QRCodeMatrix = encoder.Encode(TEXT);
            }

            QRSaveBitmapImage BitmapImage = new(QRCodeMatrix);
            BitmapImage.ModuleSize = ModuleSize;
            BitmapImage.QuietZone = QuietZone;
            QRCodeImage = BitmapImage.CreateQRCodeBitmap();

            QRSavePngImage PngImage = new(QRCodeMatrix);
            PngImage.ModuleSize = ModuleSize;
            PngImage.QuietZone = QuietZone;
            PngImage.SaveQRCodeToPngFile(pathFileName);

            Assert.IsTrue(File.Exists(pathFileName) == true);
        }

        [TestMethod]
        public void QRDecoderFromFile()
        {
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
