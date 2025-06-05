//-----------------------------------------------------------------------
// <copyright file="SQLCommandExtensions.cs" company="Lifeprojects.de">
//     Class: SQLCommandExtensions
//     Copyright © PTA GmbH 2016
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>2.7.2018</date>
//
// <summary>SQLCommandExtensions Class for SQL Server Database</summary>
//-----------------------------------------------------------------------

namespace System.Data.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public static class SQLCommandExtensions
    {
        #region run CmdExecuteNonQuery

        public static int CmdExecuteNonQuery(this SQLiteConnection @this, string sql, CommandType commandType = CommandType.Text)
        {
            int resultCommand = -1;

            try
            {
                using (SQLiteCommand cmd = @this.CreateCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    resultCommand = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                throw;
            }

            return resultCommand;
        }

        public static int CmdExecuteNonQuery(this SQLiteConnection @this, SQLiteTransaction transaction, string sql, CommandType commandType = CommandType.Text)
        {
            int resultCommand = -1;

            try
            {
                using (SQLiteCommand cmd = @this.CreateCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    cmd.Transaction = transaction;
                    resultCommand = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                throw;
            }

            return resultCommand;
        }

        public static int CmdExecuteNonQuery(this SQLiteConnection @this,
                                             string sql,
                                             Dictionary<string, object> parameterCollection,
                                             CommandType commandType = CommandType.Text)
        {
            int resultCommand = -1;

            try
            {
                using (SQLiteCommand cmd = @this.CreateCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    if (parameterCollection.Count > 0)
                    {
                        cmd.Parameters.Clear();
                        foreach (KeyValuePair<string, object> item in parameterCollection)
                        {
                            cmd.Parameters.AddWithValue(item.Key, item.Value);
                        }

                        foreach (SQLiteParameter parameter in cmd.Parameters)
                        {
                            if (parameter.IsNullable == false)
                            {
                                if (parameter.DbType.ToString() == typeof(DateTime).Name)
                                {
                                    if ((DateTime)parameter.Value == DateTime.MinValue)
                                    {
                                        parameter.Value = new DateTime(1900, 1, 1);
                                    }
                                }
                                else
                                {
                                    if (parameter.Value == DBNull.Value || parameter.Value == null)
                                    {
                                        parameter.Value = DBNull.Value;
                                    }
                                }
                            }
                        }
                    }

                    resultCommand = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resultCommand;
        }

        public static int CmdExecuteNonQuery(this SQLiteConnection @this,
                                    SQLiteTransaction transaction,
                                     string sql,
                                     Dictionary<string, object> parameterCollection,
                                     CommandType commandType = CommandType.Text)
        {
            int resultCommand = -1;

            try
            {
                using (SQLiteCommand cmd = @this.CreateCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    cmd.Transaction = transaction;
                    if (parameterCollection.Count > 0)
                    {
                        cmd.Parameters.Clear();
                        foreach (KeyValuePair<string, object> item in parameterCollection)
                        {
                            cmd.Parameters.AddWithValue(item.Key, item.Value);
                        }

                        foreach (SQLiteParameter parameter in cmd.Parameters)
                        {
                            if (parameter.IsNullable == false)
                            {
                                if (parameter.DbType.ToString() == typeof(DateTime).Name)
                                {
                                    if ((DateTime)parameter.Value == DateTime.MinValue)
                                    {
                                        parameter.Value = new DateTime(1900, 1, 1);
                                    }
                                }
                                else
                                {
                                    if (parameter.Value == DBNull.Value || parameter.Value == null)
                                    {
                                        parameter.Value = DBNull.Value;
                                    }
                                }
                            }
                        }
                    }

                    resultCommand = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resultCommand;
        }

        public static void ExecuteNonQuery(this SQLiteConnection @this, Action<SQLiteCommand> commandFactory)
        {
            using (SQLiteCommand command = @this.CreateCommand())
            {
                commandFactory(command);

                command.ExecuteNonQuery();
            }
        }

        #endregion run CmdExecuteNonQuery

        #region run CmdExecuteScalar

        public static T CmdExecuteScalar<T>(this SQLiteConnection connection, string sql, CommandType commandType = CommandType.Text)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection is missing");
            }

            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("sql is missing");
            }

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;

                    var result = cmd.ExecuteScalar();
                    return result == DBNull.Value ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T CmdExecuteScalar<T>(this SQLiteConnection connection,
                                            string sql,
                                            Dictionary<string, object> parameterCollection,
                                            CommandType commandType = CommandType.Text)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection is missing");
            }

            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("sql is missing");
            }

            if (parameterCollection == null)
            {
                throw new ArgumentNullException("parameterCollection is missing");
            }

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    if (parameterCollection.Count > 0)
                    {
                        cmd.Parameters.Clear();
                        foreach (KeyValuePair<string, object> item in parameterCollection)
                        {
                            cmd.Parameters.AddWithValue(item.Key, item.Value);
                        }
                    }

                    var result = cmd.ExecuteScalar();
                    return result == DBNull.Value || result == null ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion run CmdExecuteScalar

        #region run CmdReaderToDataTable

        public static DataTable CmdReaderToDataTable(this SQLiteConnection connection, string sql, CommandType commandType = CommandType.Text)
        {
            DataTable resultCommand = null;

            if (connection == null)
            {
                throw new ArgumentNullException("connection is missing");
            }

            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("sql is missing");
            }

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    using (SQLiteDataReader dr = cmd.ExecuteReader(CommandBehavior.Default))
                    {
                        resultCommand = new DataTable();
                        resultCommand.Load(dr);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resultCommand;
        }

        public static DataTable CmdReaderToDataTable(this SQLiteConnection connection,
                                string sql,
                                Dictionary<string, object> parameterCollection,
                                CommandType commandType = CommandType.Text)
        {
            DataTable resultCommand = null;

            if (connection == null)
            {
                throw new ArgumentNullException("connection is missing");
            }

            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("sql is missing");
            }

            if (parameterCollection == null)
            {
                throw new ArgumentNullException("parameterCollection is missing");
            }

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    if (parameterCollection.Count > 0)
                    {
                        cmd.Parameters.Clear();
                        foreach (KeyValuePair<string, object> item in parameterCollection)
                        {
                            cmd.Parameters.AddWithValue(item.Key, item.Value);
                        }

                        foreach (SQLiteParameter parameter in cmd.Parameters)
                        {
                            if (parameter.IsNullable == false)
                            {
                                if (parameter.DbType.ToString() == typeof(DateTime).Name)
                                {
                                    if ((DateTime)parameter.Value == DateTime.MinValue)
                                    {
                                        parameter.Value = new DateTime(1900, 1, 1);
                                    }
                                }
                                else
                                {
                                    if (parameter.Value == DBNull.Value || parameter.Value == null)
                                    {
                                        parameter.Value = DBNull.Value;
                                    }
                                }
                            }
                        }
                    }

                    using (SQLiteDataReader dr = cmd.ExecuteReader(CommandBehavior.Default))
                    {
                        resultCommand = new DataTable();
                        resultCommand.Load(dr);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resultCommand;
        }

        #endregion run CmdReaderToDataTable

        #region CmdReaderToMapOfT
        public static IEnumerable<TModel> CmdReaderToMap<TModel>(this SQLiteConnection connection, string sql, CommandType commandType = CommandType.Text)
        {
            IEnumerable<TModel> resultCommand = null;

            if (connection == null)
            {
                throw new ArgumentNullException("connection is missing");
            }

            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("sql is missing");
            }

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = commandType;
                    cmd.CommandText = sql;
                    using (SQLiteDataReader dr = cmd.ExecuteReader(CommandBehavior.Default))
                    {
                        if (dr.HasRows == true && dr.VisibleFieldCount > 0)
                        {
                            resultCommand = dr.MapToList<TModel>();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resultCommand;
        }

        #endregion CmdReaderToMapOfT

        #region SQL Helper Methodes

        public static string ConvertSqlToString(this SQLiteCommand pSqlCommand)
        {
            return string.Empty;
        }

        #endregion SQL Helper Methodes
    }
}