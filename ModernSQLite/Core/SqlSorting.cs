//-----------------------------------------------------------------------
// <copyright file="SqlSorting.cs" company="Lifeprojects.de">
//     Class: SqlSorting
//     Copyright © PTA GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//   Definition of SqlSorting Class
//   Represents sorting operators for SELECT statements
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    using System.ComponentModel;

    public enum SqlSorting
    {
        [Description("ASC")]
        Ascending,
        [Description("DESC")]
        Descending
    }
}
