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

namespace ModernSqlServer.Generator
{
    public enum SqlDataType
    {
        None = 0,
        uniqueidentifier,
        nvarchar,
        varchar,
        DateTime,
        Integer,
        Decimal,
        binary,
        bit
    }
}
