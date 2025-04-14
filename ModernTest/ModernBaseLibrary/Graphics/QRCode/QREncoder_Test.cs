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
    using System.Text;
    using System.Threading;

    using global::ModernBaseLibrary.Barcode;
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

    [TestClass]
    public class QREncoder_Test : BaseTest
    {
        private string TestDirPath => TestContext.TestRunDirectory;
        private string TempDirPath => Path.Combine(TestDirPath, "Temp");
        private const string TEXT = "Das ist ein Text für den QRCode Encoder/Decoder. Plus: 12345678901;";
        private const string TEXT2 = "ModernUI Library Version 1.1.2025.15. Test zum Lesen und Erstellen eines QR-Code";
        private const string TEXT3 = "www.google.de";


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
            // QR decoder variables
            QRDecoder QRCodeDecoder;
            Bitmap QRCodeInputImage = null;

            QRCodeTrace.Open("QRCodeDecoderTrace.txt");
            QRCodeTrace.Write("QRCodeDecoder");

            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Graphics\\QRCode\\DemoData\\QRCodeImageForRead.png");

            if (QRCodeInputImage != null)
            {
                QRCodeInputImage.Dispose();
            }

            if (File.Exists(pathFileName) == true)
            {
                QRCodeInputImage = new Bitmap(pathFileName);

                // create decoder
                using (QRCodeDecoder = new QRDecoder())
                {
                    QRCodeResult[] QRCodeResultArray = QRCodeDecoder.ImageDecoder(QRCodeInputImage);
                    if (QRCodeResultArray != null)
                    {
                        string qrCodeDimensionLabel = QRCodeResultArray[0].QRCodeDimension.ToString();
                        string errorCodeLabel = QRCodeResultArray[0].ErrorCorrection.ToString();
                        string eciValueLabel = QRCodeResultArray[0].ECIAssignValue >= 0 ? QRCodeResultArray[0].ECIAssignValue.ToString() : null;
                        string dataTextBox = this.ConvertResultToDisplayString(QRCodeResultArray);
                        if (IsValidUrl(dataTextBox))
                        {

                        }

                        Assert.AreEqual(qrCodeDimensionLabel, "37");
                        Assert.AreEqual(errorCodeLabel, "M");
                        Assert.AreEqual(TEXT2, dataTextBox);
                    }
                }
            }
        }

        [TestMethod]
        public void QRDecoderFromFileDataSegment()
        {
            // QR decoder variables
            QRDecoder QRCodeDecoder;
            Bitmap QRCodeInputImage = null;

            QRCodeTrace.Open("QRCodeDecoderTrace.txt");
            QRCodeTrace.Write("QRCodeDecoder");

            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Graphics\\QRCode\\DemoData\\QRCodeImageForReadDS.png");

            if (QRCodeInputImage != null)
            {
                QRCodeInputImage.Dispose();
            }

            if (File.Exists(pathFileName) == true)
            {
                QRCodeInputImage = new Bitmap(pathFileName);

                // create decoder
                using (QRCodeDecoder = new QRDecoder())
                {
                    QRCodeResult[] QRCodeResultArray = QRCodeDecoder.ImageDecoder(QRCodeInputImage);
                    if (QRCodeResultArray != null)
                    {
                        string qrCodeDimensionLabel = QRCodeResultArray[0].QRCodeDimension.ToString();
                        string errorCodeLabel = QRCodeResultArray[0].ErrorCorrection.ToString();
                        string eciValueLabel = QRCodeResultArray[0].ECIAssignValue >= 0 ? QRCodeResultArray[0].ECIAssignValue.ToString() : null;
                        string dataTextBox = this.ConvertResultToDisplayString(QRCodeResultArray);
                        if (IsValidUrl(dataTextBox))
                        {

                        }

                        Assert.AreEqual(qrCodeDimensionLabel, "37");
                        Assert.AreEqual(errorCodeLabel, "M");
                        Assert.AreEqual(TEXT2, dataTextBox);
                    }
                }
            }
        }

        [TestMethod]
        public void QRDecoderFromFileURL()
        {
            // QR decoder variables
            QRDecoder QRCodeDecoder;
            Bitmap QRCodeInputImage = null;

            QRCodeTrace.Open("QRCodeDecoderTrace.txt");
            QRCodeTrace.Write("QRCodeDecoder");

            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Graphics\\QRCode\\DemoData\\QRCodeImageForReadURL.png");

            if (QRCodeInputImage != null)
            {
                QRCodeInputImage.Dispose();
            }

            if (File.Exists(pathFileName) == true)
            {
                QRCodeInputImage = new Bitmap(pathFileName);

                // create decoder
                using (QRCodeDecoder = new QRDecoder())
                {
                    QRCodeResult[] QRCodeResultArray = QRCodeDecoder.ImageDecoder(QRCodeInputImage);
                    if (QRCodeResultArray != null)
                    {
                        string qrCodeDimensionLabel = QRCodeResultArray[0].QRCodeDimension.ToString();
                        string errorCodeLabel = QRCodeResultArray[0].ErrorCorrection.ToString();
                        string eciValueLabel = QRCodeResultArray[0].ECIAssignValue >= 0 ? QRCodeResultArray[0].ECIAssignValue.ToString() : null;
                        string dataTextBox = this.ConvertResultToDisplayString(QRCodeResultArray);

                        Assert.AreEqual(qrCodeDimensionLabel, "21");
                        Assert.AreEqual(errorCodeLabel, "M");
                        Assert.AreEqual(TEXT3, dataTextBox);
                        Assert.AreEqual(dataTextBox.IsUrl() == true, true);
                    }
                }
            }
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

        private string ConvertResultToDisplayString(QRCodeResult[] DataByteArray)
        {
            // no QR code
            if (DataByteArray == null)
            {
                return string.Empty;
            }

            // image has one QR code
            if (DataByteArray.Length == 1) return SingleQRCodeResult(QRDecoder.ByteArrayToStr(DataByteArray[0].DataArray));

            // image has more than one QR code
            StringBuilder Str = new();
            for (int Index = 0; Index < DataByteArray.Length; Index++)
            {
                if (Index != 0) Str.Append("\r\n");
                Str.AppendFormat("QR Code {0}\r\n", Index + 1);
                Str.Append(SingleQRCodeResult(QRDecoder.ByteArrayToStr(DataByteArray[Index].DataArray)));
            }
            return Str.ToString();
        }

        private string SingleQRCodeResult(string result)
        {
            int Index;
            for (Index = 0; Index < result.Length && (result[Index] >= ' ' && result[Index] <= '~' || result[Index] >= 160); Index++) ;
            if (Index == result.Length) return result;

            StringBuilder Display = new(result[..Index]);
            for (; Index < result.Length; Index++)
            {
                char OneChar = result[Index];
                if (OneChar >= ' ' && OneChar <= '~' || OneChar >= 160)
                {
                    Display.Append(OneChar);
                    continue;
                }

                if (OneChar == '\r')
                {
                    Display.Append("\r\n");
                    if (Index + 1 < result.Length && result[Index + 1] == '\n') Index++;
                    continue;
                }

                if (OneChar == '\n')
                {
                    Display.Append("\r\n");
                    continue;
                }

                Display.Append('¿');
            }

            return Display.ToString();
        }

        private bool IsValidUrl(string Url)
        {
            if (System.Uri.IsWellFormedUriString(Url, UriKind.Absolute) && System.Uri.TryCreate(Url, UriKind.Absolute, out Uri TempUrl))
            {
                return TempUrl.Scheme == System.Uri.UriSchemeHttp || TempUrl.Scheme == System.Uri.UriSchemeHttps;
            }

            return false;
        }
    }
}
