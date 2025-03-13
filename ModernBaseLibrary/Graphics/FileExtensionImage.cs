//-----------------------------------------------------------------------
// <copyright file="FileExtensionImage.cs" company="Lifeprojects.de">
//     Class: FileExtensionImage
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>13.03.2025 08:22:01</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Graphics
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media.Imaging;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Extension;

    public static class FileExtensionImage
    {
        public static byte[] Get(string fileName, Assembly assembly = null)
        {
            byte[] result = null;
            BitmapSource tempIcon = null;

            if (assembly == null)
            {
                if (UnitTestDetector.IsInUnitTest == true)
                {
                    assembly = Assembly.GetExecutingAssembly();
                }
                else
                {
                    assembly = Assembly.GetExecutingAssembly();
                }
            }

            try
            {
                if (Path.GetExtension(fileName).ToLower() == ".bin" || fileName.Contains("ExtensionBin") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionBin");
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
                    }
                    else
                    {
                        result = assembly.GetByteFromResource(fileName);
                    }
                }
                else if (Path.GetExtension(fileName).ToLower() == ".xls" || fileName.Contains("ExtensionEpub") == true)
                {
                    if (assembly == null)
                    {
                        tempIcon = (BitmapSource)Application.Current.TryFindResource("imgExtensionExcel");
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
                        result = GraphicsConverter.BitmapSourcePngToByteArray(tempIcon);
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
