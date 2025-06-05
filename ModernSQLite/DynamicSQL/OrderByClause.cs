//-----------------------------------------------------------------------
// <copyright file="OrderByClause.cs" company="Lifeprojects.de">
//     Class: OrderByClause
//     Copyright © PTA GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//      Definition of OrderByClause Struct Class
//      Represents a ORDER BY clause to be used with SELECT statements
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    public struct OrderByClause
    {
        public OrderByClause(string field)
        {
            this.FieldName = field;
            this.SortOrder = SqlSorting.Ascending;
        }

        public OrderByClause(string field, SqlSorting order)
        {
            this.FieldName = field;
            this.SortOrder = order;
        }

        public string FieldName { get; set; }

        public SqlSorting SortOrder { get; set; }
    }
}
