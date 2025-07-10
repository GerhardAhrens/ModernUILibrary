//-----------------------------------------------------------------------
// <copyright file="DynamicSQLBuilder.cs" company="Lifeprojects.de">
//     Class: DynamicSQLBuilder
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//      Definition of DynamicSQLBuilder Class
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text;

    public class DynamicSQLBuilder : IQueryBuilder, IDisposable
    {
        private bool classIsDisposed = false;
        private DbProviderFactory dbproviderFactory;
        private StatementTyp queryTyp = StatementTyp.None;
        private bool distinct = false;
        private TopClause topClause = new TopClause(100, SqlTopUnit.Percent);
        private LimitClause limitClause = new LimitClause(0);
        private List<string> selectedColumns = new List<string>();
        private Dictionary<string, object> fieldAdnValues = null;
        private List<string> selectedTables = new List<string>();
        private List<JoinClause> joins = new List<JoinClause>();
        private WhereStatement _whereStatement = new WhereStatement();
        private List<OrderByClause> orderByStatement = new List<OrderByClause>();
        private List<string> groupByColumns = new List<string>();
        private WhereStatement havingStatement = new WhereStatement();

        public DynamicSQLBuilder()
        {
            if (this.selectedColumns != null)
            {
                this.selectedColumns.Clear();
            }
        }

        public DynamicSQLBuilder(DbProviderFactory factory)
        {
            if (this.selectedColumns != null)
            {
                this.selectedColumns.Clear();
            }

            this.dbproviderFactory = factory;
        }

        public enum StatementTyp
        {
            None = 0,
            Insert = 1,
            Update = 2,
            Delete = 3,
            Select = 4
        }

        public int CountField
        {
            get
            {
                int count = -1;

                if (this.fieldAdnValues != null || this.fieldAdnValues.Count > 0)
                {
                    count = this.fieldAdnValues.Count;
                }

                if (this.selectedColumns != null || this.selectedColumns.Count > 0)
                {
                    count = this.selectedColumns.Count;
                }

                return count;
            }
        }

        public bool Distinct
        {
            get { return this.distinct; }
            set { this.distinct = value; }
        }

        public int TopRecords
        {
            get { return this.topClause.Quantity; }
            set
            {
                this.topClause.Quantity = value;
                this.topClause.Unit = SqlTopUnit.Records;
            }
        }

        public int LimitRecords
        {
            get { return this.limitClause.Quantity; }
            set
            {
                this.limitClause.Quantity = value;
                this.limitClause.Unit = SqlTopUnit.Records;
            }
        }

        public TopClause TopClause
        {
            get { return this.topClause; }
            set { this.topClause = value; }
        }

        public LimitClause LimitClause
        {
            get { return this.limitClause; }
            set { this.limitClause = value; }
        }

        public WhereStatement Having
        {
            get { return this.havingStatement; }
            set { this.havingStatement = value; }
        }

        public string[] SelectedColumns
        {
            get
            {
                if (this.selectedColumns.Count > 0)
                {
                    return this.selectedColumns.ToArray();
                }
                else
                {
                    return new string[1] { "*" };
                }
            }
        }

        public string[] SelectedTables
        {
            get { return this.selectedTables.ToArray(); }
        }

        public WhereStatement Where
        {
            get { return this._whereStatement; }
            set { this._whereStatement = value; }
        }

        internal WhereStatement WhereStatement
        {
            get { return this._whereStatement; }
            set { this._whereStatement = value; }
        }

        public void SetDbProviderFactory(DbProviderFactory factory)
        {
            this.dbproviderFactory = factory;
        }

        public void SelectAllColumns()
        {
            this.selectedColumns.Clear();
        }

        public void SelectCount()
        {
            this.SelectColumn("count(*)");
        }

        public void SelectColumn(string column)
        {
            this.selectedColumns.Add(column);
        }

        public void SelectColumns(params string[] columns)
        {
            foreach (string column in columns)
            {
                this.selectedColumns.Add(column);
            }
        }

        public void SelectColumns(IEnumerable<string> columns)
        {
            foreach (string column in columns)
            {
                this.selectedColumns.Add(column);
            }
        }

        public void SelectFromTable(string table)
        {
            this.selectedTables.Clear();
            this.queryTyp = StatementTyp.Select;
            this.selectedTables.Add(table);
            this.selectedColumns.Clear();
        }

        public void InsertIntoTable(string table)
        {
            this.selectedTables.Clear();
            this.queryTyp = StatementTyp.Insert;
            this.selectedTables.Add(table);
        }

        public void UpdateTable(string table)
        {
            this.selectedTables.Clear();
            this.queryTyp = StatementTyp.Update;
            this.selectedTables.Add(table);
        }

        public void DeleteTable(string table)
        {
            this.selectedTables.Clear();
            this.queryTyp = StatementTyp.Delete;
            this.selectedTables.Add(table);
        }

        public void SelectFromTables(params string[] tables)
        {
            this.selectedTables.Clear();
            this.queryTyp = StatementTyp.Select;
            foreach (string table in tables)
            {
                this.selectedTables.Add(table);
            }
        }

        public void AddColumn(string field)
        {
            try
            {
                if (this.fieldAdnValues == null)
                {
                    this.fieldAdnValues = new Dictionary<string, object>();
                }

                if (this.fieldAdnValues.ContainsKey(field) == false)
                {
                    this.fieldAdnValues.Add(field, $"@{field}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddColumn(string field, string fieldParameter)
        {
            try
            {
                if (this.fieldAdnValues == null)
                {
                    this.fieldAdnValues = new Dictionary<string, object>();
                }

                if (this.fieldAdnValues.ContainsKey(field) == false)
                {
                    this.fieldAdnValues.Add(field, fieldParameter);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddColumn(string field, object fieldParameter)
        {
            try
            {
                if (this.fieldAdnValues == null)
                {
                    this.fieldAdnValues = new Dictionary<string, object>();
                }

                if (this.fieldAdnValues.ContainsKey(field) == false)
                {
                    this.fieldAdnValues.Add(field, fieldParameter);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddColumns(params string[] fields)
        {
            try
            {
                if (this.fieldAdnValues == null)
                {
                    this.fieldAdnValues = new Dictionary<string, object>();
                }

                foreach (var itemField in fields)
                {
                    if (this.fieldAdnValues.ContainsKey(itemField) == false)
                    {
                        this.fieldAdnValues.Add(itemField, itemField);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddColumns(IEnumerable<string> fields)
        {
            try
            {
                if (this.fieldAdnValues == null)
                {
                    this.fieldAdnValues = new Dictionary<string, object>();
                }

                foreach (var itemField in fields)
                {
                    if (this.fieldAdnValues.ContainsKey(itemField) == false)
                    {
                        this.fieldAdnValues.Add(itemField, itemField);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddJoin(JoinClause newJoin)
        {
            this.joins.Add(newJoin);
        }

        public void AddJoin(SqlJoinType join, string toTableName, string toColumnName, SqlComparison @operator, string fromTableName, string fromColumnName)
        {
            JoinClause newJoin = new JoinClause(join, toTableName, toColumnName, @operator, fromTableName, fromColumnName);
            this.joins.Add(newJoin);
        }

        public void AddWhere(WhereClause clause)
        {
            this.AddWhere(clause, 1);
        }

        public WhereClause AddWhere(string field)
        {
            return this.AddWhere(field, SqlComparison.SqlNative, null, 1);
        }

        public void AddWhere(WhereClause clause, int level)
        {
            this._whereStatement.Add(clause, level);
        }

        public WhereClause AddWhere(string field, SqlComparison @operator)
        {
            return this.AddWhere(field, @operator, null, 1);
        }

        public WhereClause AddWhere(string field, SqlComparison @operator, object compareValue)
        {
            return this.AddWhere(field, @operator, compareValue, 1);
        }

        public WhereClause AddWhere(Enum field, SqlComparison @operator, object compareValue)
        {
            return this.AddWhere(field.ToString(), @operator, compareValue, 1);
        }

        public WhereClause AddWhere(string field, SqlComparison @operator, object compareValue, int level)
        {
            WhereClause newWhereClause = new WhereClause(field, @operator, compareValue);
            this._whereStatement.Add(newWhereClause, level);
            return newWhereClause;
        }

        public void AddOrderBy(OrderByClause clause)
        {
            this.orderByStatement.Add(clause);
        }

        public void AddOrderBy(Enum field, SqlSorting order)
        {
            this.AddOrderBy(field.ToString(), order);
        }

        public void AddOrderBy(string field, SqlSorting order)
        {
            OrderByClause newOrderByClause = new OrderByClause(field, order);
            this.orderByStatement.Add(newOrderByClause);
        }

        public void GroupBy(params string[] columns)
        {
            foreach (string column in columns)
            {
                this.groupByColumns.Add(column);
            }
        }

        public void AddHaving(WhereClause clause)
        {
            this.AddHaving(clause, 1);
        }

        public void AddHaving(WhereClause clause, int level)
        {
            this.havingStatement.Add(clause, level);
        }

        public WhereClause AddHaving(string field, SqlComparison @operator, object compareValue)
        {
            return this.AddHaving(field, @operator, compareValue, 1);
        }

        public WhereClause AddHaving(Enum field, SqlComparison @operator, object compareValue)
        {
            return this.AddHaving(field.ToString(), @operator, compareValue, 1);
        }

        public WhereClause AddHaving(string field, SqlComparison @operator, object compareValue, int level)
        {
            WhereClause newWhereClause = new WhereClause(field, @operator, compareValue);
            this.havingStatement.Add(newWhereClause, level);
            return newWhereClause;
        }

        public DbCommand BuildCommand()
        {
            return (DbCommand)this.BuildQuery(true);
        }

        public string BuildQuery()
        {
            string result = string.Empty;


            if (this.queryTyp == StatementTyp.Insert)
            {
                result = this.BuildInsertStatement();
            }
            else if (this.queryTyp == StatementTyp.Update)
            {
                result = this.BuildUpdateStatement();
            }
            else if (this.queryTyp == StatementTyp.Delete)
            {
                result = this.BuildDeleteStatement();
            }
            else
            {
                result = ((string)this.BuildQuery(false)).Trim();
            }

            return result; //.Replace("''", "'");
        }

        #region Dispose Function
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing)
                {
                    this.fieldAdnValues = null;
                    this.selectedColumns = null;
                    this.selectedTables = null;
                    this.queryTyp = StatementTyp.None;
                    this.joins = null;
                    this.orderByStatement = null;
                    this.groupByColumns = null;
                }
            }

            this.classIsDisposed = true;
        }
        #endregion Dispose Function

        private string BuildUpdateStatement()
        {
            string result = string.Empty;
            string lastField = string.Empty;
            StringBuilder sqlText = new StringBuilder();

            if (this.selectedTables == null || this.selectedTables.Count == 0)
            {
                return result;
            }

            try
            {
                string currentTable = this.selectedTables.First();
                sqlText.Append($"UPDATE {currentTable} SET");

                if (this.fieldAdnValues.Any() == true)
                {
                    lastField = this.fieldAdnValues.Last().Key;

                    sqlText.Append(" ");
                    foreach (KeyValuePair<string, object> itemColums in this.fieldAdnValues)
                    {
                        sqlText.Append($"{itemColums.Key} = {itemColums.Value}");
                        if (itemColums.Key != lastField)
                        {
                            sqlText.Append(", ");
                        }
                    }

                    if (this._whereStatement.ClauseLevels > 0)
                    {
                        sqlText.Append(" WHERE " + this._whereStatement.BuildWhereStatement());
                    }

                    sqlText.Length--;
                    sqlText.Length--;
                    sqlText.Append(";");
                    result = sqlText.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }


        private string BuildInsertStatement()
        {
            string result = string.Empty;
            string lastField = string.Empty;
            StringBuilder sqlText = new StringBuilder();

            if (this.selectedTables == null || this.selectedTables.Count == 0)
            {
                return result;
            }

            try
            {
                string currentTable = this.selectedTables.First();
                sqlText.Append($"INSERT INTO {currentTable}");
                if (this.fieldAdnValues.Any() == true)
                {
                    lastField = this.fieldAdnValues.Last().Key;

                    sqlText.Append(" ");
                    sqlText.Append("(");
                    foreach (KeyValuePair<string, object> itemColums in this.fieldAdnValues)
                    {
                        sqlText.Append(itemColums.Key);
                        if (itemColums.Key != lastField)
                        {
                            sqlText.Append(", ");
                        }
                    }

                    sqlText.Append(")");
                    sqlText.Append(" VALUES ");
                    sqlText.Append("(");
                    foreach (KeyValuePair<string, object> itemColums in this.fieldAdnValues)
                    {
                        if (itemColums.Value.ToString().StartsWith("@") == true)
                        {
                            sqlText.Append($"{itemColums.Value}");
                        }
                        else
                        {
                            sqlText.Append($"@{itemColums.Value}");
                        }

                        if (itemColums.Key != lastField)
                        {
                            sqlText.Append(", ");
                        }
                    }

                    sqlText.Append(")");
                    sqlText.Append(";");

                    result = sqlText.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        private string BuildDeleteStatement()
        {
            string result = string.Empty;
            string lastField = string.Empty;
            StringBuilder sqlText = new StringBuilder();

            if (this.selectedTables == null || this.selectedTables.Count == 0)
            {
                return result;
            }

            try
            {
                string currentTable = this.selectedTables.First();
                sqlText.Append($"DELETE FROM {currentTable}");

                if (this._whereStatement.ClauseLevels > 0)
                {
                    sqlText.Append(" WHERE " + this._whereStatement.BuildWhereStatement());
                }

                sqlText.Length--;
                sqlText.Length--;
                sqlText.Append(";");

                result = sqlText.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        private object BuildQuery(bool buildCommand)
        {
            if (buildCommand && this.dbproviderFactory == null)
            {
                throw new Exception("Cannot build a command when the Db Factory hasn't been specified. Call SetDbProviderFactory first.");
            }

            DbCommand command = null;
            if (buildCommand == true)
            {
                command = this.dbproviderFactory.CreateCommand();
            }

            string sqlQuery = "SELECT ";

            if (this.distinct == true)
            {
                sqlQuery += "DISTINCT ";
            }

            if ((this.topClause.Quantity == 100 & this.topClause.Unit == SqlTopUnit.Percent) == false)
            {
                sqlQuery += "TOP " + this.topClause.Quantity;
                if (this.topClause.Unit == SqlTopUnit.Percent)
                {
                    sqlQuery += " PERCENT";
                }

                sqlQuery += " ";
            }

            if (this.selectedColumns.Count == 0)
            {
                if (this.selectedTables.Count == 1)
                {
                    sqlQuery += this.selectedTables[0] + ".";
                }

                sqlQuery += "*";
            }
            else
            {
                foreach (string columnName in this.selectedColumns)
                {
                    sqlQuery += columnName + ',';
                }

                sqlQuery = sqlQuery.TrimEnd(',');
                sqlQuery += ' ';
            }

            if (this.selectedTables.Count > 0)
            {
                sqlQuery += " FROM ";
                foreach (string tableName in this.selectedTables)
                {
                    sqlQuery += tableName + ',';
                }

                sqlQuery = sqlQuery.TrimEnd(',');
                sqlQuery += ' ';
            }

            if (this.joins.Count > 0)
            {
                foreach (JoinClause sqlClause in this.joins)
                {
                    string joinSqlString = string.Empty;
                    switch (sqlClause.JoinType)
                    {
                        case SqlJoinType.InnerJoin:
                            joinSqlString = "INNER JOIN";
                            break;

                        case SqlJoinType.OuterJoin:
                            joinSqlString = "OUTER JOIN";
                            break;

                        case SqlJoinType.LeftOuterJoin:
                            joinSqlString = "LEFT OUTER JOIN";
                            break;

                        case SqlJoinType.LeftJoin:
                            joinSqlString = "LEFT JOIN";
                            break;

                        case SqlJoinType.RightJoin:
                            joinSqlString = "RIGHT JOIN";
                            break;
                        case SqlJoinType.RightOuterJoin:
                            joinSqlString = "RIGHT OUTER JOIN";
                            break;
                    }

                    joinSqlString += " " + sqlClause.ToTable + " ON ";
                    joinSqlString += WhereStatement.CreateComparisonClause(sqlClause.FromTable + '.' + sqlClause.FromColumn, sqlClause.ComparisonOperator, new SqlLiteral(sqlClause.ToTable + '.' + sqlClause.ToColumn));
                    sqlQuery += joinSqlString + ' ';
                }
            }

            if (this._whereStatement.ClauseLevels > 0)
            {
                if (buildCommand)
                {
                    sqlQuery += " WHERE " + this._whereStatement.BuildWhereStatement(true, ref command);
                }
                else
                {
                    sqlQuery += " WHERE " + this._whereStatement.BuildWhereStatement();
                }
            }

            if (this.groupByColumns.Count > 0)
            {
                sqlQuery += " GROUP BY ";
                foreach (string column in this.groupByColumns)
                {
                    sqlQuery += column + ',';
                }

                sqlQuery = sqlQuery.TrimEnd(',');
                sqlQuery += ' ';
            }

            if (this.havingStatement.ClauseLevels > 0)
            {
                if (this.groupByColumns.Count == 0)
                {
                    throw new Exception("Having statement was set without Group By");
                }

                if (buildCommand)
                {
                    sqlQuery += " HAVING " + this.havingStatement.BuildWhereStatement(true, ref command);
                }
                else
                {
                    sqlQuery += " HAVING " + this.havingStatement.BuildWhereStatement();
                }
            }

            if (this.orderByStatement.Count > 0)
            {
                sqlQuery += " ORDER BY ";
                foreach (OrderByClause sqlClause in this.orderByStatement)
                {
                    string orderByClause = string.Empty;
                    switch (sqlClause.SortOrder)
                    {
                        case SqlSorting.Ascending:
                            orderByClause = sqlClause.FieldName + " ASC";
                            break;

                        case SqlSorting.Descending:
                            orderByClause = sqlClause.FieldName + " DESC";
                            break;
                    }

                    sqlQuery += orderByClause + ',';
                }

                sqlQuery = sqlQuery.TrimEnd(',');
                sqlQuery += ' ';
            }

            if (this.limitClause.Quantity > 0)
            {
                sqlQuery += "LIMIT " + this.limitClause.Quantity;

                sqlQuery += " ";
            }

            if (buildCommand)
            {
                command.CommandText = sqlQuery;
                return command;
            }
            else
            {
                return sqlQuery;
            }
        }

        private string WhereOperatorAsText(SqlComparison sqlCompare)
        {
            string result = string.Empty;

            if (sqlCompare == SqlComparison.None)
            {
                result = string.Empty;
            }
            else if (sqlCompare == SqlComparison.Equals)
            {
                result = " = ";
            }
            else if (sqlCompare == SqlComparison.NotEquals)
            {
                result = " <> ";
            }
            else if (sqlCompare == SqlComparison.GreaterOrEquals)
            {
                result = " >= ";
            }
            else if (sqlCompare == SqlComparison.GreaterThan)
            {
                result = " > ";
            }
            else if (sqlCompare == SqlComparison.LessOrEquals)
            {
                result = " <= ";
            }
            else if (sqlCompare == SqlComparison.GreaterThan)
            {
                result = " < ";
            }
            else if (sqlCompare == SqlComparison.In)
            {
                result = " IN(@) ";
            }
            else if (sqlCompare == SqlComparison.Like)
            {
                result = " LIKE ";
            }
            else if (sqlCompare == SqlComparison.NotLike)
            {
                result = " NOT LIKE ";
            }
            else if (sqlCompare == SqlComparison.IsNull)
            {
                result = " IS NULL ";
            }
            else if (sqlCompare == SqlComparison.IsNotNull)
            {
                result = " IS NOT NULL";
            }

            return result;
        }

        private string ValueAsText(object compareValue)
        {
            string result = string.Empty;

            if (compareValue.GetType() == typeof(string))
            {
                result = $"'{compareValue.ToString()}'";
            }
            else if (compareValue.GetType() == typeof(Guid))
            {
                result = $"'{compareValue.ToString()}'";
            }
            else if (compareValue.GetType() == typeof(bool))
            {
                result = $"{compareValue.ToString()}";
            }
            else if (compareValue.GetType() == typeof(int))
            {
                result = $"{compareValue.ToString()}";
            }
            else if (compareValue.GetType() == typeof(long))
            {
                result = $"{compareValue.ToString()}";
            }
            else if (compareValue.GetType() == typeof(double))
            {
                result = $"{compareValue.ToString()}";
            }
            else if (compareValue.GetType() == typeof(decimal))
            {
                result = $"{compareValue.ToString()}";
            }

            return result;
        }
    }
}