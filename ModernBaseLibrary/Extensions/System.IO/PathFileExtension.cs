//-----------------------------------------------------------------------
// <copyright file="PathFileExtension.cs" company="Lifeprojects.de">
//     Class: PathFileExtension
//     Copyright © Lifeprojects.de 2016
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>Extensions Class for Path and File Functions</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class PathFileExtension
    {
        /// <summary>
        ///     An IEnumerable&lt;FileInfo&gt; extension method that deletes the given @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        public static void Delete(this IEnumerable<FileInfo> @this)
        {
            foreach (FileInfo t in @this)
            {
                t.Delete();
            }
        }

        /// <summary>
        ///     Enumerates for each in this collection.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="action">The action.</param>
        /// <returns>An enumerator that allows foreach to be used to process for each in this collection.</returns>
        public static IEnumerable<FileInfo> ForEach(this IEnumerable<FileInfo> @this, Action<FileInfo> action)
        {
            foreach (FileInfo t in @this)
            {
                action(t);
            }
            return @this;
        }
    }
}