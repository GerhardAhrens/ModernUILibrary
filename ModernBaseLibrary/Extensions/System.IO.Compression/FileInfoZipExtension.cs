//-----------------------------------------------------------------------
// <copyright file="FileInfoZipExtension.cs" company="Lifeprojects.de">
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
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    public static partial class FileInfoZipExtension
    {
        /// <summary>
        ///     Extracts all the files in the specified zip archive to a directory on the file system
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationDirectoryName">
        ///     The path to the directory in which to place the
        ///     extracted files, specified as a relative or absolute path. A relative path is interpreted as
        ///     relative to the current working directory.
        /// </param>
        public static void ExtractZipFileToDirectory(this FileInfo @this, string destinationDirectoryName)
        {
            ZipFile.ExtractToDirectory(@this.FullName, destinationDirectoryName);
        }

        /// <summary>
        ///     Extracts all the files in the specified zip archive to a directory on the file system and uses the specified
        ///     character encoding for entry names.
        /// </summary>
        /// <param name="this">The path to the archive that is to be extracted.</param>
        /// <param name="destinationDirectoryName">
        ///     The path to the directory in which to place the extracted files, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in this archive. Specify a
        ///     value for this parameter only when an encoding is required for interoperability with zip archive tools and
        ///     libraries that do not support UTF-8 encoding for entry names.
        /// </param>
        public static void ExtractZipFileToDirectory(this FileInfo @this, string destinationDirectoryName, Encoding entryNameEncoding)
        {
            ZipFile.ExtractToDirectory(@this.FullName, destinationDirectoryName, entryNameEncoding);
        }

        /// <summary>Extracts all the files in the specified zip archive to a directory on the file system.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationDirectory">Pathname of the destination directory.</param>
        public static void ExtractZipFileToDirectory(this FileInfo @this, DirectoryInfo destinationDirectory)
        {
            ZipFile.ExtractToDirectory(@this.FullName, destinationDirectory.FullName);
        }

        /// <summary>
        ///     Extracts all the files in the specified zip archive to a directory on the file system
        ///     and uses the specified character encoding for entry names.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationDirectory">Pathname of the destination directory.</param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in
        ///     this archive. Specify a value for this parameter only when an encoding is required for
        ///     interoperability with zip archive tools and libraries that do not support UTF-8 encoding for
        ///     entry names.
        /// </param>
        public static void ExtractZipFileToDirectory(this FileInfo @this, DirectoryInfo destinationDirectory, Encoding entryNameEncoding)
        {
            ZipFile.ExtractToDirectory(@this.FullName, destinationDirectory.FullName, entryNameEncoding);
        }

        /// <summary>
        ///     The path to the archive to open, specified as a relative or absolute path. A relative path is interpreted as
        ///     relative to the current working directory.
        /// </summary>
        /// <param name="this">
        ///     The path to the archive to open, specified as a relative or absolute path. A relative path is
        ///     interpreted as relative to the current working directory.
        /// </param>
        /// <returns>The opened zip archive.</returns>
        public static ZipArchive OpenReadZipFile(this FileInfo @this)
        {
            return ZipFile.OpenRead(@this.FullName);
        }

        /// <summary>Opens a zip archive at the specified path and in the specified mode.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="mode">
        ///     One of the enumeration values that specifies the actions that are allowed
        ///     on the entries in the opened archive.
        /// </param>
        /// <returns>A ZipArchive.</returns>
        public static ZipArchive OpenZipFile(this FileInfo @this, ZipArchiveMode mode)
        {
            return ZipFile.Open(@this.FullName, mode);
        }

        /// <summary>Opens a zip archive at the specified path and in the specified mode.</summary>
        /// <param name="this">
        ///     The path to the archive to open, specified as a relative or absolute
        ///     path. A relative path is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="mode">
        ///     One of the enumeration values that specifies the actions that are allowed
        ///     on the entries in the opened archive.
        /// </param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in
        ///     this archive. Specify a value for this parameter only when an encoding is required for
        ///     interoperability with zip archive tools and libraries that do not support UTF-8 encoding for
        ///     entry names.
        /// </param>
        /// <returns>A ZipArchive.</returns>
        public static ZipArchive OpenZipFile(this FileInfo @this, ZipArchiveMode mode, Encoding entryNameEncoding)
        {
            return ZipFile.Open(@this.FullName, mode, entryNameEncoding);
        }
    }
}