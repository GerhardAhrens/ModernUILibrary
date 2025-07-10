//-----------------------------------------------------------------------
// <copyright file="LimitClause.cs" company="Lifeprojects.de">
//     Class: LimitClause
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//      Definition of LimitClause Struct Class
//      Represents a LIMIT clause for SELECT statements
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    public struct LimitClause
    {
        public LimitClause(int nr)
        {
            this.Quantity = nr;
            this.Unit = SqlTopUnit.Records;
        }

        public int Quantity { get; set; }

        public SqlTopUnit Unit { get; set; }
    }
}
