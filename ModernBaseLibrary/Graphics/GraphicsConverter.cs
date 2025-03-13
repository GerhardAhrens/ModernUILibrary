//-----------------------------------------------------------------------
// <copyright file="ToolsGraphics.cs" company="Lifeprojects.de">
//     Class: ToolsGraphics
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.08.2017</date>
//
// <summary>
// Klasse mit Methoden zur Bearbeitung und Konvertierung von Grafiken/Images
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Graphics
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Versioning;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    [SupportedOSPlatform("windows")]
    public static class GraphicsConverter
    {
        public static byte[] ImageToByteArray(Image pBitmapSource, System.Drawing.Imaging.ImageFormat pImageFormat)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                pBitmapSource.Save(memoryStream, pImageFormat);
                memoryStream.Flush();
                return memoryStream.ToArray();
            }
        }

        public static byte[] BitmapSourceJpegToByteArray(BitmapSource pBitmapSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(pBitmapSource));
            encoder.QualityLevel = 100;
            byte[] bit = Array.Empty<byte>();
            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Frames.Add(BitmapFrame.Create(pBitmapSource));
                encoder.Save(stream);
                bit = stream.ToArray();
                stream.Close();
            }

            return bit;
        }

        public static byte[] BitmapSourcePngToByteArray(BitmapSource pBitmapSource)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(pBitmapSource));
            byte[] bit = Array.Empty<byte>();
            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Frames.Add(BitmapFrame.Create(pBitmapSource));
                encoder.Save(stream);
                bit = stream.ToArray();
                stream.Close();
            }

            return bit;
        }

        public static Bitmap ByteArrayToBitmap(byte[] pSource)
        {
            ImageConverter ic = new ImageConverter();
            System.Drawing.Image img = (System.Drawing.Image)ic.ConvertFrom(pSource);
            Bitmap bitmap = new Bitmap(img);
            return bitmap;
        }

        public static BitmapSource ByteArrayToBitmapSource(byte[] pSource)
        {
            var stream = new MemoryStream(pSource);
            return System.Windows.Media.Imaging.BitmapFrame.Create(stream);
        }

        public static System.Drawing.Image LoadImageNoLock(string path)
        {
            var ms = new MemoryStream(File.ReadAllBytes(path));
            return System.Drawing.Image.FromStream(ms);
        }

        public static BitmapImage ImageSourceToBitmapImage(ImageSource pImageSource)
        {
            BitmapImage retVal = null;

            retVal = new BitmapImage(new Uri(pImageSource.ToString()));

            return retVal;
        }

        public static byte[] ImageSourceToBytes(BitmapEncoder encoder, ImageSource imageSource)
        {
            byte[] bytes = null;

            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        public static BitmapImage ConvertSourceToImage(BitmapSource source, string extension)
        {
            BitmapEncoder encoder;
            string lowerExtension = extension.ToLower();
            if (lowerExtension.Contains("jpeg") || lowerExtension.Contains("jpg"))
            {
                encoder = new JpegBitmapEncoder();
            }
            else if (lowerExtension.Contains("png"))
            {
                encoder = new PngBitmapEncoder();
            }
            else
            {
                return null;
            }

            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(memoryStream);

            memoryStream.Position = 0;
            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
            bImg.EndInit();
            bImg.Freeze();
            //memoryStream.Close();
            return bImg;
        }
    }
}