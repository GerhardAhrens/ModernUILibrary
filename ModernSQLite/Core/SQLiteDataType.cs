//-----------------------------------------------------------------------
// <copyright file="SQLiteDataType.cs" company="Lifeprojects.de">
//     Class: SQLiteDataType
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>16.06.2017</date>
//
// <summary>Class for SQLiteDataType (Enum)</summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    public enum SQLiteDataType
    {
        None = 0,
        Text,
        VarChar,
        DateTime,
        Integer,
        Decimal,
        Real,
        BLOB,
        Guid,
        Boolean
    }
}
