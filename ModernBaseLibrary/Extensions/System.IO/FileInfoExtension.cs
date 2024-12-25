//-----------------------------------------------------------------------
// <copyright file="FileInfoExtension.cs" company="Lifeprojects.de">
//     Class: DirectoryInfoExtension
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>21.09.2020</date>
//
// <summary>Extensions Class for FileInfo Types</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    [SupportedOSPlatform("windows")]
    public static partial class FileInfoExtension
    {
        private static readonly Dictionary<string, string> resourcesIcon;

        static FileInfoExtension()
        {
            resourcesIcon = AddResourcesIcon();
        }

        public static int Count(this FileInfo @this, string token, bool ignorCase = false)
        {
            int result = 0;
            RegexOptions opt = RegexOptions.None;

            if (ignorCase == false)
            {
                opt = RegexOptions.IgnoreCase;
            }
            else
            {
                opt = RegexOptions.None;
            }

            result = Regex.Matches(@this.ReadAllText(), token, opt).Count;

            return result;
        }

        /// <summary>Gets the total number of lines in a file. </summary>
        /// <param name="this">The file to perform the count on.</param>
        /// <returns>The total number of lines in a file. </returns>
        public static int CountLines(this FileInfo @this)
        {
            return File.ReadAllLines(@this.FullName).Length;
        }

        /// <summary>Gets the total number of lines in a file that satisfy the condition in the predicate function.</summary>
        /// <param name="this">The file to perform the count on.</param>
        /// <param name="predicate">A function to test each line for a condition.</param>
        /// <returns>The total number of lines in a file that satisfy the condition in the predicate function.</returns>
        public static int CountLines(this FileInfo @this, Func<string, bool> predicate)
        {
            return File.ReadAllLines(@this.FullName).Count(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static byte[] FileExtensionIcon(this FileInfo @this)
        {
            byte[] result = null;
            Assembly assebly = Assembly.GetExecutingAssembly();

            try
            {
                string fileExtesion = Path.GetExtension(@this.FullName).ToLower();
                if (string.IsNullOrEmpty(fileExtesion) == false)
                {
                    string resName = GetResourceNameFromExtension(fileExtesion);
                    bool resFound = assebly.GetManifestResourceNames().Any(a => a == resName);
                    if (resFound == true)
                    {
                        Stream resourceStream = assebly.GetManifestResourceStream(resName);
                        result = new byte[resourceStream.Length];
                        if (result.Length > 0)
                        {
                            resourceStream.Read(result, 0, result.Length);
                        }
                    }
                }
                else
                {
                    string extensionDefault = GetResourceNameFromExtension(".bin");
                    Stream resourceStream = assebly.GetManifestResourceStream(extensionDefault);
                    result = new byte[resourceStream.Length];
                    if (result.Length > 0)
                    {
                        resourceStream.Read(result, 0, result.Length);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static IEnumerable<string> ReadLines(this FileInfo @this)
        {
            using (var fs = new FileStream(@this.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static byte[] ImageToByte(this FileInfo @this)
        {
            byte[] result = new byte[] { 0x20 };

            try
            {
                long imageFileLength = @this.Length;
                using (FileStream fs = new FileStream(@this.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        result = br.ReadBytes((int)imageFileLength);
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return result;
        }

        public static ImageSource ByteToImageSource(this byte[] @this)
        {
            if (@this == null)
            {
                return null;
            }

            ImageSource imgSrc = null;
            BitmapImage biImg = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(@this))
            {
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                imgSrc = biImg as ImageSource;
            }

            return imgSrc;
        }

        public static byte[] FileToByte(this FileInfo @this)
        {
            byte[] result = new byte[] { 0x20 };

            try
            {
                long imageFileLength = @this.Length;
                using (FileStream fs = new FileStream(@this.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        result = br.ReadBytes((int)imageFileLength);
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return result;
        }

        public static byte[] ReadAllBytes(this FileInfo @this)
        {
            byte[] bytes = new byte[0];

            if (File.Exists(@this.FullName) == true)
            {
                using (FileStream fs = new FileStream(@this.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    int index = 0;
                    long fileLength = fs.Length;
                    if (fileLength > Int32.MaxValue)
                    {
                        throw new IOException("File too long");
                    }

                    int count = (int)fileLength;
                    bytes = new byte[count];
                    while (count > 0)
                    {
                        int n = fs.Read(bytes, index, count);
                        if (n == 0)
                        {
                            throw new InvalidOperationException("End of file reached before expected");
                        }

                        index += n;
                        count -= n;
                    }
                }
            }
            return bytes;
        }

        public static string ReadAllText(this FileInfo @this)
        {
            string allText = string.Empty;
            if (File.Exists(@this.FullName) == true)
            {
                using (FileStream fs = new FileStream(@this.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (sr.Peek() >= 0)
                        {
                            allText = sr.ReadToEnd();
                        }
                    }
                }
            }
            return allText;
        }

        public static string HashMD5(this FileInfo @this)
        {
            if (File.Exists(@this.FullName) == true)
            {
                using (var md5 = MD5.Create())
                {
                    byte[] byteFromFile = File.ReadAllBytes(@this.FullName);
                    return BitConverter.ToString(md5.ComputeHash(byteFromFile)).Replace("-", string.Empty);
                }
            }

            return string.Empty;
        }

        public static string FileSizeAsText(this FileInfo @this)
        {
            long Bytes = @this.Length;

            if (Bytes >= 1073741824)
            {
                Decimal size = Decimal.Divide(Bytes, 1073741824);
                return String.Format("{0:##.##} GB", size);
            }
            else if (Bytes >= 1048576)
            {
                Decimal size = Decimal.Divide(Bytes, 1048576);
                return String.Format("{0:##.##} MB", size);
            }
            else if (Bytes >= 1024)
            {
                Decimal size = Decimal.Divide(Bytes, 1024);
                return String.Format("{0:##.##} KB", size);
            }
            else if (Bytes > 0 & Bytes < 1024)
            {
                Decimal size = Bytes;
                return String.Format("{0:##.##} Bytes", size);
            }
            else
            {
                return "0 Bytes";
            }
        }

        public static Encoding GetFileEncoding(this FileInfo @this)
        {
            var encodings = Encoding.GetEncodings()
                .Select(e => e.GetEncoding())
                .Select(e => new { Encoding = e, Preamble = e.GetPreamble() })
                .Where(e => e.Preamble.Any())
                .ToArray();

            var maxPrembleLength = encodings.Max(e => e.Preamble.Length);
            byte[] buffer = new byte[maxPrembleLength];

            using (var stream = File.OpenRead(@this.FullName))
            {
                stream.Read(buffer, 0, (int)Math.Min(maxPrembleLength, stream.Length));
            }

            return encodings
                .Where(enc => enc.Preamble.SequenceEqual(buffer.Take(enc.Preamble.Length)))
                .Select(enc => enc.Encoding)
                .FirstOrDefault() ?? Encoding.Default;
        }

        public static bool IsFileLocked(this FileInfo @this)
        {
            bool result = false;
            FileStream fs = null;

            try
            {
                if (File.Exists(@this.FullName) == true)
                {
                    fs = new FileStream(@this.FullName, FileMode.Open, FileAccess.Read, FileShare.None);
                    if (fs != null)
                    {
                        result = false;
                    }
                }
            }
            catch (Exception)
            {
                result = true;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return result;
        }

        public static bool IsFile(this FileInfo @this)
        {
            return !File.GetAttributes(@this.FullName).HasFlag(FileAttributes.Directory);
        }

        public static string ToShortPath(this FileInfo @this, int maxLength)
        {
            if (maxLength == -1)
            {
                return @this.FullName;
            }

            if (@this.Length < maxLength)
            {
                return @this.FullName;
            }

            List<string> result = new List<string>();
            string[] tItem = @this.FullName.Split('\\');
            if (tItem.Count() < 2)
            {
                return @this.FullName;
            }

            result.Add(tItem[0]);
            result.Add("...");
            result.Add(tItem.Last());

            //Minimum Requirement
            int totalLength = 0;
            foreach (string item in result) totalLength += item.Length;
            totalLength += result.Count - 1; //Including [\] between each item

            for (int x = tItem.Count() - 2; x > 0; x--)
            {
                if (totalLength + tItem[x].Length < maxLength)
                {
                    result.Insert(2, tItem[x]);
                    totalLength += tItem[x].Length + 1; //Including [\] symbol
                }

                else break;
            }

            return string.Join("\\", result.ToArray());
        }

        /// <summary>
        /// Die Methode ändert den dateityp einer Datei.
        /// </summary>
        /// <param name="this">FileInfo</param>
        /// <param name="extension">Neuer Dateityp</param>
        /// <returns>Vollständiger Pfad und Dateiname</returns>
        public static string ChangeExtension(this FileInfo @this, string extension)
        {
            return Path.ChangeExtension(@this.FullName, extension);
        }

        private static Dictionary<string, string> AddResourcesIcon()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string resNameSpace = "EasyPrototyping.Resources.Picture.FileExtension.";

            try
            {
                result.Add(".txt", $"{resNameSpace}ExtensionTxt.png");
                result.Add(".doc", $"{resNameSpace}ExtensionWord.png");
                result.Add(".docx", $"{resNameSpace}ExtensionWord.png");
                result.Add(".xls", $"{resNameSpace}ExtensionExcel.png");
                result.Add(".xlsx", $"{resNameSpace}ExtensionExcel.png");
                result.Add(".bin", $"{resNameSpace}ExtensionBin.png");
                result.Add(".csv", $"{resNameSpace}ExtensionCsv.png");
                result.Add(".dll", $"{resNameSpace}ExtensionDll.png");
                result.Add(".epup", $"{resNameSpace}ExtensionEpub.png");
                result.Add(".gif", $"{resNameSpace}ExtensionGif.png");
                result.Add(".html", $"{resNameSpace}ExtensionHtml.png");
                result.Add(".ico", $"{resNameSpace}ExtensionIco.png");
                result.Add(".jpg", $"{resNameSpace}ExtensionJpeg.png");
                result.Add(".jpeg", $"{resNameSpace}ExtensionJpeg.png");
                result.Add(".mov", $"{resNameSpace}ExtensionMov.png");
                result.Add(".mp3", $"{resNameSpace}ExtensionMp3.png");
                result.Add(".mp4", $"{resNameSpace}ExtensionMp4.png");
                result.Add(".mpeg", $"{resNameSpace}ExtensionMpeg.png");
                result.Add(".pdf", $"{resNameSpace}ExtensionPDFX.png");
                result.Add(".png", $"{resNameSpace}ExtensionPng.png");
                result.Add(".ppt", $"{resNameSpace}ExtensionPPT.png");
                result.Add(".pptx", $"{resNameSpace}ExtensionPPT.png");
                result.Add(".rtf", $"{resNameSpace}ExtensionRtf.png");
                result.Add(".zip", $"{resNameSpace}ExtensionZip.png");
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        private static string GetResourceNameFromExtension(string extension)
        {
            string result = string.Empty;

            if (resourcesIcon.Any(p => p.Key == extension))
            {
                result = resourcesIcon.First(p => p.Key == extension).Value;
            }

            return result;
        }

    }
}