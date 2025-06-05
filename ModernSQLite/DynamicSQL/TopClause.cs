//-----------------------------------------------------------------------
// <copyright file="TopClause.cs" company="Lifeprojects.de">
//     Class: TopClause
//     Copyright © PTA GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//      Definition of TopClause Struct Class
//      Represents a TOP clause for SELECT statements
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    public struct TopClause
    {
        public TopClause(int nr)
        {
            this.Quantity = nr;
            this.Unit = SqlTopUnit.Records;
        }

        public TopClause(int nr, SqlTopUnit unit)
        {
            this.Quantity = nr;
            this.Unit = unit;
        }

        public int Quantity { get; set; }

        public SqlTopUnit Unit { get; set; }
    }
}
