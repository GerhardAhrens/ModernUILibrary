//-----------------------------------------------------------------------
// <copyright file="DirectoryHelper.cs" company="Lifeprojects.de">
//     Class: DirectoryHelper
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>19.12.2022</date>
//
// <summary>
// Helper Klasse für Directory Operationen (Copy, Move, Delete)
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.IO;

    /// <summary>
    /// Directory Helper for Copy, Move, Delete).
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// Moves the directory.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        /// <param name="deleteSource">if set to <c>true</c> [delete source].</param>
        public static void MoveDirectory(string sourceDirectory, string destinationDirectory, bool deleteSource)
        {
            if (Directory.Exists(sourceDirectory))
            {
                if (Directory.GetDirectoryRoot(sourceDirectory) == Directory.GetDirectoryRoot(destinationDirectory))
                {
                    Directory.Move(sourceDirectory, destinationDirectory);
                }
                else
                {
                    try
                    {
                        CopyDirectory(new DirectoryInfo(sourceDirectory), new DirectoryInfo(destinationDirectory));
                        if (deleteSource == true)
                        {
                            Directory.Delete(sourceDirectory, true);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="destinationDirectory">The destination directory.</param>
        public static void CopyDirectory(DirectoryInfo sourceDirectory, DirectoryInfo destinationDirectory)
        {
            try
            {
                if (destinationDirectory.Exists == false)
                {
                    destinationDirectory.Create();
                }

                FileInfo[] fiSrcFiles = sourceDirectory.GetFiles();
                foreach (FileInfo fiSrcFile in fiSrcFiles)
                {
                    fiSrcFile.CopyTo(Path.Combine(destinationDirectory.FullName, fiSrcFile.Name));
                }

                DirectoryInfo[] diSrcDirectories = sourceDirectory.GetDirectories();

                foreach (DirectoryInfo diSrcDirectory in diSrcDirectories)
                {
                    CopyDirectory(diSrcDirectory, new DirectoryInfo(Path.Combine(destinationDirectory.FullName, diSrcDirectory.Name)));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DeleteDirectory(DirectoryInfo sourceDirectory)
        {
            try
            {
                if (sourceDirectory.Exists == true)
                {
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}