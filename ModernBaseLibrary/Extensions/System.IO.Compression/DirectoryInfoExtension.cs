//-----------------------------------------------------------------------
// <copyright file="DirectoryInfoExtension.cs" company="Lifeprojects.de">
//     Class: DirectoryInfoExtension
//     Copyright � Lifeprojects.de 2017
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
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    public static partial class DirectoryInfoExtension
    {
        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFileName">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFileName);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory, uses the specified compression level, and optionally includes the base directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFileName">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to
        ///     emphasize speed or compression effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from
        ///     sourceDirectoryName at the root of the archive; false to include only the contents of the
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFileName, compressionLevel, includeBaseDirectory);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified directory, uses the specified
        ///     compression level and character encoding for entry names, and optionally includes the base directory.
        /// </summary>
        /// <param name="this">
        ///     The path to the directory to be archived, specified as a relative or absolute path. A relative path
        ///     is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="destinationArchiveFileName">
        ///     The path of the archive to be created, specified as a relative or absolute
        ///     path. A relative path is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to emphasize speed or compression
        ///     effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from sourceDirectoryName at the root of the
        ///     archive; false to include only the contents of the directory.
        /// </param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in this archive. Specify a
        ///     value for this parameter only when an encoding is required for interoperability with zip archive tools and
        ///     libraries that do not support UTF-8 encoding for entry names.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, string destinationArchiveFileName, CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFileName, compressionLevel, includeBaseDirectory, entryNameEncoding);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFile">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFile.FullName);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory, uses the specified compression level, and optionally includes the base directory.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="destinationArchiveFile">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to
        ///     emphasize speed or compression effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from
        ///     sourceDirectoryName at the root of the archive; false to include only the contents of the
        ///     directory.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile, CompressionLevel compressionLevel, bool includeBaseDirectory)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFile.FullName, compressionLevel, includeBaseDirectory);
        }

        /// <summary>
        ///     Creates a zip archive that contains the files and directories from the specified
        ///     directory, uses the specified compression level and character encoding for entry names, and
        ///     optionally includes the base directory.
        /// </summary>
        /// <param name="this">
        ///     The path to the directory to be archived, specified as a relative or
        ///     absolute path. A relative path is interpreted as relative to the current working directory.
        /// </param>
        /// <param name="destinationArchiveFile">
        ///     The path of the archive to be created, specified as a
        ///     relative or absolute path. A relative path is interpreted as relative to the current working
        ///     directory.
        /// </param>
        /// <param name="compressionLevel">
        ///     One of the enumeration values that indicates whether to
        ///     emphasize speed or compression effectiveness when creating the entry.
        /// </param>
        /// <param name="includeBaseDirectory">
        ///     true to include the directory name from
        ///     sourceDirectoryName at the root of the archive; false to include only the contents of the
        ///     directory.
        /// </param>
        /// <param name="entryNameEncoding">
        ///     The encoding to use when reading or writing entry names in
        ///     this archive. Specify a value for this parameter only when an encoding is required for
        ///     interoperability with zip archive tools and libraries that do not support UTF-8 encoding for
        ///     entry names.
        /// </param>
        public static void CreateZipFile(this DirectoryInfo @this, FileInfo destinationArchiveFile, CompressionLevel compressionLevel, bool includeBaseDirectory, Encoding entryNameEncoding)
        {
            ZipFile.CreateFromDirectory(@this.FullName, destinationArchiveFile.FullName, compressionLevel, includeBaseDirectory, entryNameEncoding);
        }
    }
}
