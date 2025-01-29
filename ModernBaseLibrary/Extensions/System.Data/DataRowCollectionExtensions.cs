//-----------------------------------------------------------------------
// <copyright file="DataRowCollectionExtensions.cs" company="Lifeprojects.de">
//     Class: DataRowCollectionExtensions
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.09.2020</date>
//
// <summary>Extension Class für DataRow</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Data;
    using System.Linq;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public static class DataRowCollectionExtensions
    {
        /// <summary>
        /// A DataTable extension method that return the first row.
        /// </summary>
        /// <param name="this">The table to act on.</param>
        /// <returns>The first row of the table.</returns>
        public static DataRow FirstRow(this DataRowCollection @this)
        {
            return @this[0];
        }

        /// <summary>A DataTable extension method that last row.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DataRow.</returns>
        public static DataRow LastRow(this DataRowCollection @this)
        {
            return @this[@this.Count - 1];
        }

        /// <summary>returns the datarow with the passed index.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DataRow.</returns>
        public static DataRow RowByIndex(this DataRowCollection @this, int index)
        {
            return @this[index];
        }
    }
}