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
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using ModernBaseLibrary.Extension;

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

        public static byte[] BitmapSorceJpegToByteArray(BitmapSource pBitmapSource)
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

        public static byte[] BitmapSorcePngToByteArray(BitmapSource pBitmapSource)
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

        public static byte[] FileExtensionIcon(string fileName, Assembly assembly = null)
        {
            byte[] result = null;
            BitmapSource tempIcon = null;

            try
            {
                if (Path.GetExtension(fileName).ToLower() == ".bin" || fileName.Contains("ExtensionBin") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionBin");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".csv" || fileName.Contains("ExtensionCsv") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionCsv");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".dll" || fileName.Contains("ExtensionDll") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionDll");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".epub" || fileName.Contains("ExtensionEpub") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionEpub");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".xlsx" || fileName.Contains("ExtensionExcel") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionExcel");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".xls" || fileName.Contains("ExtensionEpub") == true )
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionExcel");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".gif" || fileName.Contains("ExtensionGif") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionGif");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".html" || fileName.Contains("ExtensionHtml") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionHtml");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".xml" || fileName.Contains("ExtensionHtml") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionHtml");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".ico" || fileName.Contains("ExtensionIco") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionIco");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".jpg" || fileName.Contains("ExtensionJpeg") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionJpeg");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".jpeg" || fileName.Contains("ExtensionJpeg") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionJpeg");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".mov" || fileName.Contains("ExtensionMov") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionMov");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".mp3" || fileName.Contains("ExtensionMp3") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionMp3");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".mp3" || fileName.Contains("ExtensionMp3") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionMp3");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".mp4" || fileName.Contains("ExtensionEpub") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionMp4");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".mpeg" || fileName.Contains("ExtensionMpeg") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionMpeg");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".pdf" || fileName.Contains("ExtensionPDF") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionPDF");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".m3u" || fileName.Contains("ExtensionPlaylist") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionPlaylist");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".png" || fileName.Contains("ExtensionPng") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionPng");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".ppt" || fileName.Contains("ExtensionPPT") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionPPT");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".pptx" || fileName.Contains("ExtensionPPT") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionPPT");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".pptx" || fileName.Contains("ExtensionPPT") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionPPT");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".rtf" || fileName.Contains("ExtensionRtf") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionRtf");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".cs" || fileName.Contains("ExtensionSource") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionSource");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".vb" || fileName.Contains("ExtensionSource") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionSource");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".css" || fileName.Contains("ExtensionSource") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionSource");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".xaml" || fileName.Contains("ExtensionSource") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionSource");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".tiff" || fileName.Contains("ExtensionTiff") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionTiff");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".txt" || fileName.Contains("ExtensionTxt") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionTxt");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".wav" || fileName.Contains("ExtensionWav") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionWav");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".wmv" || fileName.Contains("ExtensionEpub") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionWmv");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".doc" || fileName.Contains("ExtensionWord") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionWord");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".docx" || fileName.Contains("ExtensionWord") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionWord");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".zip" || fileName.Contains("ExtensionZip") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionZip");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionBin");
                        result = BitmapSorcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}