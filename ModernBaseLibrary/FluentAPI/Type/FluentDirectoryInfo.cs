//-----------------------------------------------------------------------
// <copyright file="FluentDirectoryInfo.cs" company="Lifeprojects.de">
//     Class: FluentDirectoryInfo
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@pta.de</email>
// <date>20.06.2021</date>
//
// <summary>
// Die Klasse stellt Methoden zur DirectoryInfo Behandlung auf Basis einer
// FluentAPI (FluentDirectoryInfo Extension) zur Verfügung
// https://www.c-sharpcorner.com/blogs/systemio-directoryinfo-extensions1
// https://gist.github.com/jlgager24/82f21a0b508fdfbe0c4f6092a186073e
// https://stackoverflow.com/questions/7039580/multiple-file-extensions-searchpattern-for-system-io-directory-getfiles
// </summary>
//-----------------------------------------------------------------------

namespace System.IO
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;

    using ModernBaseLibrary.Extension;
    using ModernBaseLibrary.FluentAPI;

    [SupportedOSPlatform("windows")]
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public class FluentDirectoryInfo : FluentDirectoryInfo<FluentDirectoryInfo>
    {
        // <summary>
        /// Initializes a new instance of the <see cref="FluentDirectoryInfo"/> class.
        /// </summary>
        public FluentDirectoryInfo(DirectoryInfo value) : base(value)
        {
        }
    }

    public class FluentDirectoryInfo<TAssertions> : ReferenceTypeAssertions<DirectoryInfo, TAssertions> where TAssertions : FluentDirectoryInfo<TAssertions>
    {
        public FluentDirectoryInfo(DirectoryInfo value) : base(value)
        {
            this.DirectoryInfoValue = value;
        }

        private DirectoryInfo DirectoryInfoValue { get; set; }

        public IEnumerable<FileInfo> EnumerateFiles()
        {
            return Directory.EnumerateFiles(this.DirectoryInfoValue.FullName).Select(x => new FileInfo(x));
        }

        public IEnumerable<FileInfo> EnumerateFiles(string searchPattern)
        {
            return Directory.EnumerateFiles(this.DirectoryInfoValue.FullName, searchPattern).Select(x => new FileInfo(x));
        }

        public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(this.DirectoryInfoValue.FullName, searchPattern, searchOption).Select(x => new FileInfo(x));
        }

        public IEnumerable<FileInfo> EnumerateFiles(string[] searchPatterns)
        {
            return searchPatterns.SelectMany(x => this.DirectoryInfoValue.GetFiles(x)).Distinct();
        }

        /// <summary>
        /// The method returns all files that match the search pattern.
        /// </summary>
        /// <param name="DirectoryInfo">Typ</param>
        /// <param name="searchPatternExpression">search pattern '\.docx|\.txt'</param>
        /// <param name="searchOption">SearchOption.AllDirectories, SearchOption.TopDirectoryOnly</param>
        /// <returns>IEnumerable<FileInfo></returns>
        public IEnumerable<FileInfo> GetFiles(string searchPatternExpression = "", SearchOption searchOption = SearchOption.AllDirectories)
        {
            Regex reSearchPattern = new Regex(searchPatternExpression, RegexOptions.IgnoreCase);
            return this.DirectoryInfoValue.EnumerateFiles("*", searchOption).Where(file => reSearchPattern.IsMatch(Path.GetExtension(file.FullName)));
        }

        /// <summary>
        /// The method returns all files. 
        /// </summary>
        /// <param name="DirectoryInfo">Typ</param>
        /// <param name="searchOption">SearchOption.AllDirectories, SearchOption.TopDirectoryOnly</param>
        /// <returns>IEnumerable<FileInfo></returns>
        public IEnumerable<FileInfo> GetFiles(SearchOption searchOption = SearchOption.AllDirectories)
        {
            return this.DirectoryInfoValue.EnumerateFiles("*", searchOption);
        }

        /// <summary>
        ///     A DirectoryInfo extension method that clears all files and directories in this directory.
        /// </summary>
        /// <param name="this">The obj to act on.</param>
        public void DeleteAll()
        {
            try
            {
                Array.ForEach(this.DirectoryInfoValue.GetFiles(), x => x.Delete());
                Array.ForEach(this.DirectoryInfoValue.GetDirectories(), x => x.Delete(true));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Die Methode kopiert Dateien unter berücksichtigung des Patterns is das angegebene Verzeichnis.
        /// </summary>
        /// <param name="destDirName">Name of the dest dir.</param>
        /// <param name="pattern">The pattern.</param>
        [SupportedOSPlatform("windows")]
        public void CopyTo(DirectoryInfo destDirName, string pattern = "*.*")
        {
            if (string.IsNullOrEmpty(pattern) == true)
            {
                pattern = "*.*";
            }

            if (destDirName != null)
            {
                if (destDirName.Exists == false)
                {
                    destDirName.Create();
                }

                this.DirectoryInfoValue.CopyTo(destDirName.FullName, "*.*", SearchOption.TopDirectoryOnly);
            }
        }

        public void CopyFilesTo(string destDirName, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var files = this.DirectoryInfoValue.GetFiles(searchPattern, searchOption);
            foreach (var file in files)
            {
                var outputFile = destDirName + file.FullName.Substring(this.DirectoryInfoValue.FullName.Length);
                var directory = new FileInfo(outputFile).Directory;

                if (directory == null)
                {
                    throw new Exception("The directory cannot be null.");
                }

                if (!directory.Exists)
                {
                    directory.Create();
                }

                file.CopyTo(outputFile);
            }

            // Ensure empty dir are also copied
            var directories = this.DirectoryInfoValue.GetDirectories(searchPattern, searchOption);
            foreach (var directory in directories)
            {
                var outputDirectory = destDirName + directory.FullName.Substring(this.DirectoryInfoValue.FullName.Length);
                var directoryInfo = new DirectoryInfo(outputDirectory);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
            }
        }
    }

    public static partial class DirectoryInfoExtension
    {
        /// <summary>
        /// Gibt die Summe der Größe aller Dateien in der Liste zurück
        /// </summary>
        /// <param name="this">The this.</param>
        /// <returns></returns>
        public static long Length(this IEnumerable<FileInfo> @this)
        {
            long sumLength = 0;
            if (@this != null && @this.Any() == true)
            {
                sumLength = @this.Sum(f => f.Length);
            }

            return sumLength;
        }

        /// <summary>
        /// Gibt die Summe der Größe aller Dateien in der Liste as formatierten Text zurück
        /// </summary>
        /// <param name="this">The this.</param>
        /// <returns></returns>
        public static string LengthAsText(this IEnumerable<FileInfo> @this)
        {
            long sumBytes = 0;
            if (@this != null && @this.Any() == true)
            {
                sumBytes = @this.Sum(f => f.Length);
            }

            if (sumBytes >= 1073741824)
            {
                decimal size = decimal.Divide(sumBytes, 1073741824);
                return string.Format("{0:##.##} GB", size);
            }
            else if (sumBytes >= 1048576)
            {
                decimal size = decimal.Divide(sumBytes, 1048576);
                return string.Format("{0:##.##} MB", size);
            }
            else if (sumBytes >= 1024)
            {
                decimal size = decimal.Divide(sumBytes, 1024);
                return string.Format("{0:##.##} KB", size);
            }
            else if (sumBytes > 0 & sumBytes < 1024)
            {
                decimal size = sumBytes;
                return string.Format("{0:##.##} Bytes", size);
            }
            else
            {
                return "0 Bytes";
            }
        }

        /// <summary>
        /// Gibt die Anzahl der dateien in der Liste zurück
        /// </summary>
        /// <param name="this">The this.</param>
        /// <returns></returns>
        public static int CountFiles(this IEnumerable<FileInfo> @this)
        {
            int sumLength = 0;
            if (@this != null && @this.Count() > 0)
            {
                sumLength = @this.Count();
            }

            return sumLength;
        }
    }
}