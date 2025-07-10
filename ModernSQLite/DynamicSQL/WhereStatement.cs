//-----------------------------------------------------------------------
// <copyright file="WhereStatement.cs" company="Lifeprojects.de">
//     Class: WhereStatement
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.08.2017</date>
//
// <summary>
//      Definition of WhereStatement Struct Class
//      Represents a JOIN clause to be used with SELECT statements
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Globalization;

    public class WhereStatement : List<List<WhereClause>>
    {
        public int ClauseLevels
        {
            get { return this.Count; }
        }

        public static WhereStatement Copy(WhereStatement statement)
        {
            WhereStatement result = new WhereStatement();
            int currentLevel = 0;
            foreach (List<WhereClause> level in statement)
            {
                currentLevel++;
                result.Add(new List<WhereClause>());
                foreach (WhereClause clause in statement[currentLevel - 1])
                {
                    WhereClause clauseCopy = new WhereClause(clause.FieldName, clause.ComparisonOperator, clause.Value);
                    foreach (WhereClause.SubClause subClause in clause.SubClauses)
                    {
                        WhereClause.SubClause subClauseCopy = new WhereClause.SubClause(subClause.LogicOperator, subClause.ComparisonOperator, subClause.Value);
                        clauseCopy.SubClauses.Add(subClauseCopy);
                    }

                    result[currentLevel - 1].Add(clauseCopy);
                }
            }

            return result;
        }

        public static WhereStatement CombineStatements(WhereStatement statement1, WhereStatement statement2)
        {
            /* statement1: {Level1}((Age<15 OR Age>=20) AND (strEmail LIKE 'e%') OR {Level2}(Age BETWEEN 15 AND 20))
               Statement2: {Level1}((Name = 'Peter'))
               Return statement: {Level1}((Age<15 or Age>=20) AND (strEmail like 'e%') AND (Name = 'Peter'))
            */

            WhereStatement result = WhereStatement.Copy(statement1);

            for (int i = 0; i < statement2.ClauseLevels; i++) 
            {
                List<WhereClause> level = statement2[i];
                foreach (WhereClause clause in level) 
                {
                    for (int j = 0; j < result.ClauseLevels; j++) 
                    {
                        result.AddWhereClauseToLevel(clause, j);
                    }
                }
            }

            return result;
        }

        public void Add(WhereClause clause)
        {
            this.Add(clause, 1);
        }

        public void Add(WhereClause clause, int level)
        {
            this.AddWhereClauseToLevel(clause, level);
        }

        public WhereClause Add(string field, SqlComparison @operator, object compareValue)
        {
            return this.Add(field, @operator, compareValue, 1);
        }

        public WhereClause Add(Enum field, SqlComparison @operator, object compareValue)
        {
            return this.Add(field.ToString(), @operator, compareValue, 1);
        }

        public WhereClause Add(string field, SqlComparison @operator, object compareValue, int level)
        {
            WhereClause newWhereClause = new WhereClause(field, @operator, compareValue);
            this.AddWhereClauseToLevel(newWhereClause, level);

            return newWhereClause;
        }

        public string BuildWhereStatement()
        {
            DbCommand dummyCommand = null; /* = DataAccess.UsedDbProviderFactory.CreateCommand(); */
            return this.BuildWhereStatement(false, ref dummyCommand);
        }

        public string BuildWhereStatement(bool useCommandObject, ref DbCommand usedDbCommand)
        {
            string result = string.Empty;

            foreach (List<WhereClause> sqlWhereStatement in this) 
            {
                string levelWhere = string.Empty;
                foreach (WhereClause sqlClause in sqlWhereStatement) 
                {
                    string whereClause = string.Empty;

                    if (useCommandObject)
                    {
                        string parameterName = $"@p{usedDbCommand.Parameters.Count + 1}_{sqlClause.FieldName.Replace('.', '_')}";

                        DbParameter parameter = usedDbCommand.CreateParameter();
                        parameter.ParameterName = parameterName;
                        parameter.Value = sqlClause.Value;
                        usedDbCommand.Parameters.Add(parameter);

                        whereClause += CreateComparisonClause(sqlClause.FieldName, sqlClause.ComparisonOperator, new SqlLiteral(parameterName));
                    }
                    else
                    {
                        if (sqlClause.ComparisonOperator == SqlComparison.SqlNative)
                        {
                            whereClause = CreateComparisonClause(sqlClause.FieldName, SqlComparison.SqlNative, null);
                        }
                        else
                        {
                            whereClause = CreateComparisonClause(sqlClause.FieldName, sqlClause.ComparisonOperator, sqlClause.Value);
                        }
                    }

                    foreach (WhereClause.SubClause sqlSubWhereClause in sqlClause.SubClauses)	
                    {
                        switch (sqlSubWhereClause.LogicOperator)
                        {
                            case SqlLogicOperator.And:
                                whereClause += " AND ";
                                break;

                            case SqlLogicOperator.Or:
                                whereClause += " OR ";
                                break;
                        }

                        if (useCommandObject)
                        {
                            string parameterName = $"@p{usedDbCommand.Parameters.Count + 1}_{sqlClause.FieldName.Replace('.', '_')}";

                            DbParameter parameter = usedDbCommand.CreateParameter();
                            parameter.ParameterName = parameterName;
                            parameter.Value = sqlSubWhereClause.Value;
                            usedDbCommand.Parameters.Add(parameter);

                            whereClause += CreateComparisonClause(sqlClause.FieldName, sqlSubWhereClause.ComparisonOperator, new SqlLiteral(parameterName));
                        }
                        else
                        {
                            whereClause += CreateComparisonClause(sqlClause.FieldName, sqlSubWhereClause.ComparisonOperator, sqlSubWhereClause.Value);
                        }
                    }

                    levelWhere += "(" + whereClause + ") AND ";
                }

                levelWhere = levelWhere.Substring(0, levelWhere.Length - 5); 

                if (sqlWhereStatement.Count > 1)
                {
                    result += " (" + levelWhere + ") ";
                }
                else
                {
                    result += " " + levelWhere + " ";
                }

                result += " OR";
            }

            result = result.Substring(0, result.Length - 2); 

            return result;
        }

        internal static string CreateComparisonClause(string fieldName, SqlComparison comparisonOperator, object value)
        {
            string output = string.Empty;
            if (value != null && value != System.DBNull.Value)
            {
                switch (comparisonOperator)
                {
                    case SqlComparison.Equals:
                        output = fieldName + " = " + FormatSQLValue(value, fieldName);
                        break;

                    case SqlComparison.NotEquals:
                        output = fieldName + " <> " + FormatSQLValue(value, fieldName);
                        break;

                    case SqlComparison.GreaterThan:
                        output = fieldName + " > " + FormatSQLValue(value, fieldName);
                        break;

                    case SqlComparison.GreaterOrEquals:
                        output = fieldName + " >= " + FormatSQLValue(value, fieldName);
                        break;

                    case SqlComparison.LessThan:
                        output = fieldName + " < " + FormatSQLValue(value, fieldName);
                        break;

                    case SqlComparison.LessOrEquals:
                        output = fieldName + " <= " + FormatSQLValue(value, fieldName);
                        break;

                    case SqlComparison.Like:
                        output = fieldName + " LIKE " + FormatSQLValue(value, fieldName);
                        break;

                    case SqlComparison.NotLike:
                        output = "NOT " + fieldName + " LIKE " + FormatSQLValue(value, fieldName);
                        break;

                    case SqlComparison.In:
                        output = fieldName + " IN (" + FormatSQLValue(value) + ")";
                        break;

                    case SqlComparison.Sql:
                        output = $"{fieldName} = ({value})";
                        break;

                    case SqlComparison.SqlNative:
                        output = $"{fieldName}";
                        break;

                    case SqlComparison.SubSelect:
                        output = $"{fieldName} IN ({value})";
                        break;
                }
            }
            else
            {
                /* value==null	|| value==DBNull.Value */
                if ((comparisonOperator != SqlComparison.Equals) && (comparisonOperator != SqlComparison.NotEquals))
                {
                    if (comparisonOperator == SqlComparison.IsNotNull)
                    {
                        output = fieldName + " IS NOT NULL";
                    }
                    else if (comparisonOperator == SqlComparison.IsNull)
                    {
                        output = fieldName + " IS NULL";
                    }
                    else if (comparisonOperator == SqlComparison.SqlNative)
                    {
                        output = fieldName;
                    }
                }
                else
                {
                    switch (comparisonOperator)
                    {
                        case SqlComparison.Equals:
                            output = fieldName + " IS NULL";
                            break;

                        case SqlComparison.NotEquals:
                            output = "NOT " + fieldName + " IS NULL";
                            break;

                        case SqlComparison.SqlNative:
                            output = fieldName;
                            break;
                    }
                }
            }

            return output;
        }

        internal static string FormatSQLValue(object someValue, string fieldName = "")
        {
            string formattedValue = string.Empty;

            if (someValue == null)
            {
                formattedValue = "NULL";
            }
            else
            {
                switch (someValue.GetType().Name)
                {
                    case "String":
                        if (someValue.ToString() == "@")
                        {
                            formattedValue = $"@{fieldName}";
                        }
                        else
                        {
                            formattedValue = $"'{someValue}'";
                        }

                        break;

                    case "Guid":
                        formattedValue = "'" + ((Guid)someValue).ToString() + "'";
                        break;

                    case "DateTime":
                        CultureInfo ci = new CultureInfo("en-US");
                        if (((DateTime)someValue).ToLongTimeString() == "00:00:00")
                        {
                            formattedValue = "'" + ((DateTime)someValue).ToString("yyyy-MM-dd", ci) + "'";
                        }
                        else
                        {
                            formattedValue = "'" + ((DateTime)someValue).ToString("yyyy-MM-dd HH:mm:ssZ", ci) + "'";
                        }

                        break;

                    case "DBNull":
                        formattedValue = "NULL";
                        break;

                    case "Boolean":
                        formattedValue = (bool)someValue ? "1" : "0";
                        break;

                    case "SqlLiteral":
                        formattedValue = ((SqlLiteral)someValue).Value;
                        break;

                    default:
                        formattedValue = someValue.ToString();
                        break;
                }
            }

            return formattedValue;
        }

        private void AssertLevelExistance(int level)
        {
            if (this.Count < (level - 1))
            {
                throw new Exception("Level " + level + " not allowed because level " + (level - 1) + " does not exist.");
            }
            else if (this.Count < level)
            {
                this.Add(new List<WhereClause>());
            }
        }

        private void AddWhereClause(WhereClause clause)
        {
            this.AddWhereClauseToLevel(clause, 1);
        }

        private void AddWhereClauseToLevel(WhereClause clause, int level)
        {
            this.AssertLevelExistance(level);
            this[level - 1].Add(clause);
        }
    }
}