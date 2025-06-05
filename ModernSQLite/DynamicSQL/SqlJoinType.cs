//-----------------------------------------------------------------------
// <copyright file="SqlJoinType.cs" company="Lifeprojects.de">
//     Class: SqlJoinType
//     Copyright © PTA GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//   Definition of SqlJoinType Class
//   Represents operators for JOIN clauses
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    /// <summary>
    /// Represents operators for JOIN clauses
    /// </summary>
    public enum SqlJoinType
    {
        InnerJoin,
        OuterJoin,
        LeftJoin,
        RightJoin,
        RightOuterJoin,
        LeftOuterJoin
    }
}
