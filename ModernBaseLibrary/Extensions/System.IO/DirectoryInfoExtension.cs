//-----------------------------------------------------------------------
// <copyright file="DirectoryInfoExtension.cs" company="Lifeprojects.de">
//     Class: DirectoryInfoExtension
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.07.2017</date>
//
// <summary>
// Extensions Class for DirectoryInfo Types
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;

    [SupportedOSPlatform("windows")]
    public static partial class DirectoryInfoExtension
    {
        /// <summary>
        /// The method returns all files that match the search pattern.
        /// </summary>
        /// <param name="DirectoryInfo">Typ</param>
        /// <param name="searchPatternExpression">search pattern '\.docx|\.txt'</param>
        /// <param name="searchOption">SearchOption.AllDirectories, SearchOption.TopDirectoryOnly</param>
        /// <returns>IEnumerable<FileInfo></returns>
        public static IEnumerable<FileInfo> GetFilesEx(this DirectoryInfo directoryInfo,
                                                       string searchPatternExpression = "",
                                                       SearchOption searchOption = SearchOption.AllDirectories)
        {
            Regex reSearchPattern = new Regex(searchPatternExpression, RegexOptions.IgnoreCase);
            return directoryInfo.EnumerateFiles("*", searchOption).Where(file => reSearchPattern.IsMatch(Path.GetExtension(file.FullName)));
        }

        /// <summary>
        /// The method returns all files. 
        /// </summary>
        /// <param name="DirectoryInfo">Typ</param>
        /// <param name="searchOption">SearchOption.AllDirectories, SearchOption.TopDirectoryOnly</param>
        /// <returns>IEnumerable<FileInfo></returns>
        public static IEnumerable<FileInfo> GetFilesEx(this DirectoryInfo directoryInfo,
                                                       SearchOption searchOption = SearchOption.AllDirectories)
        {
            return directoryInfo.EnumerateFiles("*", searchOption);
        }

        /// <summary>
        /// 	Searches the provided directory recursively and returns the first file matching the provided pattern.
        /// </summary>
        /// <param name = "directory">The directory.</param>
        /// <param name = "pattern">The pattern.</param>
        /// <returns>The found file</returns>
        /// <example>
        /// 	<code>
        /// 		var directory = new DirectoryInfo(@"c:\");
        /// 		var file = directory.FindFileRecursive("win.ini");
        /// 	</code>
        /// </example>
        public static FileInfo FindFileRecursive(this DirectoryInfo directory, string pattern)
        {
            var files = directory.GetFiles(pattern);
            if (files.Length > 0)
                return files[0];

            foreach (var subDirectory in directory.GetDirectories())
            {
                var foundFile = subDirectory.FindFileRecursive(pattern);
                if (foundFile != null)
                    return foundFile;
            }
            return null;
        }

        /// <summary>
        /// 	Searches the provided directory recursively and returns the first file matching to the provided predicate.
        /// </summary>
        /// <param name = "directory">The directory.</param>
        /// <param name = "predicate">The predicate.</param>
        /// <returns>The found file</returns>
        /// <example>
        /// 	<code>
        /// 		var directory = new DirectoryInfo(@"c:\");
        /// 		var file = directory.FindFileRecursive(f => f.Extension == ".ini");
        /// 	</code>
        /// </example>
        public static FileInfo FindFileRecursive(this DirectoryInfo directory, Func<FileInfo, bool> predicate)
        {
            foreach (var file in directory.GetFiles())
            {
                if (predicate(file))
                    return file;
            }

            foreach (var subDirectory in directory.GetDirectories())
            {
                var foundFile = subDirectory.FindFileRecursive(predicate);
                if (foundFile != null)
                    return foundFile;
            }
            return null;
        }

        /// <summary>
        /// 	Searches the provided directory recursively and returns the all files matching the provided pattern.
        /// </summary>
        /// <param name = "directory">The directory.</param>
        /// <param name = "pattern">The pattern.</param>
        /// <remarks>
        /// 	This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.
        /// </remarks>
        /// <returns>The found files</returns>
        /// <example>
        /// 	<code>
        /// 		var directory = new DirectoryInfo(@"c:\");
        /// 		var files = directory.FindFilesRecursive("*.ini");
        /// 	</code>
        /// </example>
        public static FileInfo[] FindFilesRecursive(this DirectoryInfo directory, string pattern)
        {
            var foundFiles = new List<FileInfo>();
            FindFilesRecursive(directory, pattern, foundFiles);
            return foundFiles.ToArray();
        }

        private static void FindFilesRecursive(DirectoryInfo directory, string pattern, List<FileInfo> foundFiles)
        {
            foundFiles.AddRange(directory.GetFiles(pattern));
            directory.GetDirectories().ForEach(d => FindFilesRecursive(d, pattern, foundFiles));
        }

        /// <summary>
        /// 	Searches the provided directory recursively and returns the all files matching to the provided predicate.
        /// </summary>
        /// <param name = "directory">The directory.</param>
        /// <param name = "predicate">The predicate.</param>
        /// <returns>The found files</returns>
        /// <remarks>
        /// 	This methods is quite perfect to be used in conjunction with the newly created FileInfo-Array extension methods.
        /// </remarks>
        /// <example>
        /// 	<code>
        /// 		var directory = new DirectoryInfo(@"c:\");
        /// 		var files = directory.FindFilesRecursive(f => f.Extension == ".ini");
        /// 	</code>
        /// </example>
        public static FileInfo[] FindFilesRecursive(this DirectoryInfo directory, Func<FileInfo, bool> predicate)
        {
            var foundFiles = new List<FileInfo>();
            FindFilesRecursive(directory, predicate, foundFiles);
            return foundFiles.ToArray();
        }

        private static void FindFilesRecursive(DirectoryInfo directory, Func<FileInfo, bool> predicate, List<FileInfo> foundFiles)
        {
            foundFiles.AddRange(directory.GetFiles().Where(predicate));
            directory.GetDirectories().ForEach(d => FindFilesRecursive(d, predicate, foundFiles));
        }

        /// <summary>
        ///     A DirectoryInfo extension method that clears all files and directories in this directory.
        /// </summary>
        /// <param name="this">The obj to act on.</param>
        public static void DeleteAll(this DirectoryInfo @this)
        {
            Array.ForEach(@this.GetFiles(), x => x.Delete());
            Array.ForEach(@this.GetDirectories(), x => x.Delete(true));
        }

        /// <summary>A DirectoryInfo extension method that copies to.</summary>
        /// <param name="this">The obj to act on.</param>
        /// <param name="destDirName">Pathname of the destination directory.</param>
        public static void CopyTo(this DirectoryInfo @this, string destDirName)
        {
            @this.CopyTo(destDirName, "*.*", SearchOption.TopDirectoryOnly);
        }

        /// <summary>A DirectoryInfo extension method that copies to.</summary>
        /// <param name="this">The obj to act on.</param>
        /// <param name="destDirName">Pathname of the destination directory.</param>
        /// <param name="searchPattern">A pattern specifying the search.</param>
        public static void CopyTo(this DirectoryInfo @this, string destDirName, string searchPattern)
        {
            @this.CopyTo(destDirName, searchPattern, SearchOption.TopDirectoryOnly);
        }

        /// <summary>A DirectoryInfo extension method that copies to.</summary>
        /// <param name="this">The obj to act on.</param>
        /// <param name="destDirName">Pathname of the destination directory.</param>
        /// <param name="searchOption">The search option.</param>
        public static void CopyTo(this DirectoryInfo @this, string destDirName, SearchOption searchOption)
        {
            @this.CopyTo(destDirName, "*.*", searchOption);
        }

        /// <summary>A DirectoryInfo extension method that copies to.</summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The obj to act on.</param>
        /// <param name="destDirName">Pathname of the destination directory.</param>
        /// <param name="searchPattern">A pattern specifying the search.</param>
        /// <param name="searchOption">The search option.</param>
        public static void CopyTo(this DirectoryInfo @this, string destDirName, string searchPattern, SearchOption searchOption)
        {
            var files = @this.GetFiles(searchPattern, searchOption);
            foreach (var file in files)
            {
                var outputFile = destDirName + file.FullName.Substring(@this.FullName.Length);
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
            var directories = @this.GetDirectories(searchPattern, searchOption);
            foreach (var directory in directories)
            {
                var outputDirectory = destDirName + directory.FullName.Substring(@this.FullName.Length);
                var directoryInfo = new DirectoryInfo(outputDirectory);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
            }
        }

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the directories where.
        /// </summary>
        /// <param name="this">The obj to act on.</param>
        /// <param name="predicate">The predicate.</param>
        public static void DeleteDirectoriesWhere(this DirectoryInfo @this, Func<DirectoryInfo, bool> predicate)
        {
            @this.GetDirectories().Where(predicate).ForEach(x => x.Delete());
        }

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the directories where.
        /// </summary>
        /// <param name="this">The obj to act on.</param>
        /// <param name="searchOption">The search option.</param>
        /// <param name="predicate">The predicate.</param>
        public static void DeleteDirectoriesWhere(this DirectoryInfo @this, SearchOption searchOption, Func<DirectoryInfo, bool> predicate)
        {
            @this.GetDirectories("*.*", searchOption).Where(predicate).ForEach(x => x.Delete());
        }

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the files where.
        /// </summary>
        /// <param name="this">The obj to act on.</param>
        /// <param name="predicate">The predicate.</param>
        public static void DeleteFilesWhere(this DirectoryInfo @this, Func<FileInfo, bool> predicate)
        {
            @this.GetFiles().Where(predicate).ForEach(x => x.Delete());
        }

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the files where.
        /// </summary>
        /// <param name="this">The obj to act on.</param>
        /// <param name="searchOption">The search option.</param>
        /// <param name="predicate">The predicate.</param>
        public static void DeleteFilesWhere(this DirectoryInfo @this, SearchOption searchOption, Func<FileInfo, bool> predicate)
        {
            @this.GetFiles("*.*", searchOption).Where(predicate).ForEach(x => x.Delete());
        }

        // <summary>
        ///     A DirectoryInfo extension method that deletes the older than.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        /// <param name="timeSpan">The time span.</param>
        public static void DeleteOlderThan(this DirectoryInfo @this, TimeSpan timeSpan)
        {
            DateTime minDate = DateTime.Now.Subtract(timeSpan);
            @this.GetFiles().Where(x => x.LastWriteTime < minDate).ToList().ForEach(x => x.Delete());
            @this.GetDirectories().Where(x => x.LastWriteTime < minDate).ToList().ForEach(x => x.Delete());
        }

        /// <summary>
        ///     A DirectoryInfo extension method that deletes the older than.
        /// </summary>
        /// <param name="this">The obj to act on.</param>
        /// <param name="searchOption">The search option.</param>
        /// <param name="timeSpan">The time span.</param>
        public static void DeleteOlderThan(this DirectoryInfo @this, SearchOption searchOption, TimeSpan timeSpan)
        {
            DateTime minDate = DateTime.Now.Subtract(timeSpan);
            @this.GetFiles("*.*", searchOption).Where(x => x.LastWriteTime < minDate).ToList().ForEach(x => x.Delete());
            @this.GetDirectories("*.*", searchOption).Where(x => x.LastWriteTime < minDate).ToList().ForEach(x => x.Delete());
        }

        public static long GetSize(this DirectoryInfo @this)
        {
            return @this.GetFiles("*.*", SearchOption.AllDirectories).Sum(x => x.Length);
        }

        public static string PathCombine(this DirectoryInfo @this, params string[] paths)
        {
            List<string> list = paths.ToList();
            list.Insert(0, @this.FullName);
            return Path.Combine(list.ToArray());
        }

        /// <summary>
        ///     Combines multiples string into a path.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="paths">A variable-length parameters list containing paths.</param>
        /// <returns>
        ///     The combined paths as a DirectoryInfo. If one of the specified paths is a zero-length string, this method
        ///     returns the other path.
        /// </returns>
        public static DirectoryInfo PathCombineDirectory(this DirectoryInfo @this, params string[] paths)
        {
            List<string> list = paths.ToList();
            list.Insert(0, @this.FullName);
            return new DirectoryInfo(Path.Combine(list.ToArray()));
        }

        /// <summary>
        ///     Combines multiples string into a path.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="paths">A variable-length parameters list containing paths.</param>
        /// <returns>
        ///     The combined paths as a FileInfo. If one of the specified paths is a zero-length string, this method returns
        ///     the other path.
        /// </returns>
        public static FileInfo PathCombineFile(this DirectoryInfo @this, params string[] paths)
        {
            List<string> list = paths.ToList();
            list.Insert(0, @this.FullName);
            return new FileInfo(Path.Combine(list.ToArray()));
        }

        public static bool IsDirectory(this DirectoryInfo @this)
        {
            return File.GetAttributes(@this.FullName).HasFlag(FileAttributes.Directory);
        }
    }
}
