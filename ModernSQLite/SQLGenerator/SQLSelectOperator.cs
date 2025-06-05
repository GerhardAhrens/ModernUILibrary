//-----------------------------------------------------------------------
// <copyright file="SelectOperator.cs" company="Lifeprojects.de">
//     Class: SQLGenerator
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>07.05.2025 14:27:39</date>
//
// <summary>
// Basisstruktur eines SQLGenerator
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    using System.ComponentModel;

    public enum SQLSelectOperator : int
    {
        [Description("Alle Datensätze")]
        All = 0,
        [Description("Count Anzahl")]
        Count = 1,
        [Description("SQL Anweisung")]
        Direct = 3,
    }
}
