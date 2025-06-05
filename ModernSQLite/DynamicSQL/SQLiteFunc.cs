//-----------------------------------------------------------------------
// <copyright file="SQLiteFunc.cs" company="Lifeprojects.de">
//     Class: SQLiteFunc
//     Copyright © PTA GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//   Definition of SQLiteFunc Enum Class
//   Represents a ORDER BY clause to be used with SELECT statements
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    public enum SQLiteFunc
    {
        Date,
        Time,
        DateTime
    }
}
