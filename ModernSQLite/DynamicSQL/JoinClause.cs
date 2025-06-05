//-----------------------------------------------------------------------
// <copyright file="JoinClause.cs" company="Lifeprojects.de">
//     Class: JoinClause
//     Copyright © PTA GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//      Definition of JoinClause Struct Class
//      Represents a JOIN clause to be used with SELECT statements
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    public struct JoinClause
    {
        public JoinClause(SqlJoinType join, string toTableName, string toColumnName, SqlComparison @operator, string fromTableName, string fromColumnName)
        {
            this.JoinType = join;
            this.FromTable = fromTableName;
            this.FromColumn = fromColumnName;
            this.ComparisonOperator = @operator;
            this.ToTable = toTableName;
            this.ToColumn = toColumnName;
        }

        public SqlJoinType JoinType { get; set; }

        public string FromTable { get; set; }

        public string FromColumn { get; set; }

        public SqlComparison ComparisonOperator { get; set; }

        public string ToTable { get; set; }

        public string ToColumn { get; set; }
    }
}