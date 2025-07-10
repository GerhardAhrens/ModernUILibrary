//-----------------------------------------------------------------------
// <copyright file="WhereClause.cs" company="Lifeprojects.de">
//     Class: WhereClause
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//      Definition of WhereClause Struct Class
//      Represents a ORDER BY clause to be used with SELECT statements
//      Represents a WHERE clause on 1 database column, containing 1 or more comparisons on 
//      that column, chained together by logic operators: eg (UserID=1 or UserID=2 or UserID>100)
//      This can be achieved by doing this:
//      WhereClause myWhereClause = new WhereClause("UserID", Comparison.Equals, 1);
//      myWhereClause.AddClause(LogicOperator.Or, Comparison.Equals, 2);
//      myWhereClause.AddClause(LogicOperator.Or, Comparison.GreaterThan, 100);
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    using System.Collections.Generic;

    public struct WhereClause
    {
        internal List<SubClause> SubClauses;

        public WhereClause(string field, SqlComparison firstCompareOperator, object firstCompareValue)
        {
            this.FieldName = field;
            this.ComparisonOperator = firstCompareOperator;
            this.Value = firstCompareValue;
            this.LogicOperator = SqlLogicOperator.None;
            this.SubClauses = new List<SubClause>();
            this.AddBracket = AddBracket.None;
        }

        public WhereClause(AddBracket bracket, string field, SqlComparison firstCompareOperator, object firstCompareValue)
        {
            this.FieldName = field;
            this.ComparisonOperator = firstCompareOperator;
            this.Value = firstCompareValue;
            this.LogicOperator = SqlLogicOperator.None;
            this.SubClauses = new List<SubClause>();
            this.AddBracket = bracket;
        }

        public WhereClause(SqlLogicOperator logicOperator, string field, SqlComparison firstCompareOperator, object firstCompareValue)
        {
            this.FieldName = field;
            this.ComparisonOperator = firstCompareOperator;
            this.Value = firstCompareValue;
            this.LogicOperator = logicOperator;
            this.SubClauses = new List<SubClause>();
            this.AddBracket = AddBracket.None;
        }

        public WhereClause(AddBracket bracket, SqlLogicOperator logicOperator, string field, SqlComparison firstCompareOperator, object firstCompareValue)
        {
            this.FieldName = field;
            this.ComparisonOperator = firstCompareOperator;
            this.Value = firstCompareValue;
            this.LogicOperator = logicOperator;
            this.SubClauses = new List<SubClause>();
            this.AddBracket = bracket;
        }

        public string FieldName { get; set; }

        public SqlComparison ComparisonOperator { get; set; }

        public object Value { get; set; }

        public SqlLogicOperator LogicOperator { get; set; }

        public AddBracket AddBracket { get; set; }

        public void AddClause(SqlLogicOperator logic, SqlComparison compareOperator, object compareValue)
        {
            SubClause newSubClause = new SubClause(logic, compareOperator, compareValue);
            this.SubClauses.Add(newSubClause);
        }

        internal struct SubClause
        {
            public SqlLogicOperator LogicOperator;

            public SqlComparison ComparisonOperator;

            public object Value;

            public SubClause(SqlLogicOperator logic, SqlComparison compareOperator, object compareValue)
            {
                this.LogicOperator = logic;
                this.ComparisonOperator = compareOperator;
                this.Value = compareValue;
            }
        }
    }
}
