//-----------------------------------------------------------------------
// <copyright file="SqlLogicOperator.cs" company="Lifeprojects.de">
//     Class: SqlLogicOperator
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//   Definition of SqlLogicOperator Class
//   Represents logic operators for chaining WHERE and HAVING clauses together in a statement
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    public enum SqlLogicOperator
    {
        None = 0,
        And = 1,
        Or = 2
    }
}
