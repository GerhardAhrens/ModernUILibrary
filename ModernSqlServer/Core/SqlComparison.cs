//-----------------------------------------------------------------------
// <copyright file="SqlComparison.cs" company="Lifeprojects.de">
//     Class: SqlComparison
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//   Definition of SqlComparison Class
//   Represents comparison operators for WHERE, HAVING and JOIN clauses
// </summary>
//-----------------------------------------------------------------------

namespace ModernSqlServer.Generator
{
    public enum SqlComparison
    {
        None = 0,
        Equals,
        NotEquals,
        Like,
        NotLike,
        GreaterThan,
        GreaterOrEquals,
        LessThan,
        LessOrEquals,
        In,
        NotIn,
        Glob,
        IsNull,
        IsNotNull,
        Sql,
        SqlNative,
        SubSelect
    }
}
