//-----------------------------------------------------------------------
// <copyright file="SQLRecordSetExtension.cs" company="Lifeprojects.de">
//     Class: SQLRecordSetExtension
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.06.2025</date>
//
// <summary>SQLRecordSetExtension Class for SQLite Database, Analoge RecordSet Anweisung</summary>
//-----------------------------------------------------------------------

namespace System.Data.SQLite
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Transactions;
    using System.Windows.Data;

    using ModernBaseLibrary.Extension;

    using ModernSQLite.Generator;

    public static class SQLRecordSetExtension
    {
        /// <summary>
        /// Führt eine SQL Anweisung für eine offen Datenbank-Connection aus.
        /// </summary>
        /// <typeparam name="T">Erwarteter Datentyp</typeparam>
        /// <param name="this">Connection Objekt der Datenbankverbindung</param>
        /// <param name="sql">SQL Anweisung</param>
        /// <returns>Erwarteterer Wert der SQL Anweisung, werden keine Daten gefunden, wird entweder null oder der Default-Wert des Datentyp zurückgegeben.</returns>
        public static RecordSetResult<T> RecordSet<T>(this SQLiteConnection @this, string sql)
        {
            return new RecordSetResult<T>(@this, default, sql);
        }

        public static RecordSetResult<T> RecordSet<T>(this SQLiteConnection @this, string sql, SQLiteTransaction transaction)
        {
            return new RecordSetResult<T>(@this, default, sql);
        }

        /// <summary>
        /// Führt eine SQL Anweisung für eine offen Datenbank-Connection aus.
        /// </summary>
        /// <typeparam name="T">Erwarteter Datentyp</typeparam>
        /// <param name="this">Connection Objekt der Datenbankverbindung</param>
        /// <param name="sql">SQL Anweisung</param>
        /// <param name="parameterCollection">Dictionary mit einer Liste von Parametern als String (Parametername) und Object (Parametervalue)</param>
        /// <returns>Erwarteterer Wert der SQL Anweisung, werden keine Daten gefunden, wird entweder null oder der Default-Wert des Datentyp zurückgegeben.</returns>
        public static RecordSetResult<T> RecordSet<T>(this SQLiteConnection @this, string sql, Dictionary<string, object> parameterCollection)
        {
            return new RecordSetResult<T>(@this, default, sql, parameterCollection);
        }

        public static RecordSetResult<T> RecordSet<T>(this SQLiteConnection @this, string sql, Dictionary<string, object> parameterCollection, SQLiteTransaction transaction)
        {
            return new RecordSetResult<T>(@this, default, sql, parameterCollection);
        }

        /// <summary>
        /// Führt eine SQL Anweisung für eine offen Datenbank-Connection aus.
        /// </summary>
        /// <typeparam name="T">Erwarteter Datentyp</typeparam>
        /// <param name="this">Connection Objekt der Datenbankverbindung</param>
        /// <param name="sql">SQL Anweisung</param>
        /// <param name="parameterCollection">SQLiteParameter Array mit einer Liste von Parametern als String (Parametername) und Object (Parametervalue)</param>
        /// <returns>Erwarteterer Wert der SQL Anweisung, werden keine Daten gefunden, wird entweder null oder der Default-Wert des Datentyp zurückgegeben.</returns>
        public static RecordSetResult<T> RecordSet<T>(this SQLiteConnection @this, string sql, SQLiteParameter[] parameterCollection)
        {
            return new RecordSetResult<T>(@this, default, sql, parameterCollection);
        }

        public static RecordSetResult<T> RecordSet<T>(this SQLiteConnection @this, string sql, SQLiteParameter[] parameterCollection, SQLiteTransaction transaction)
        {
            return new RecordSetResult<T>(@this, default, sql, parameterCollection);
        }

        #region SET, SQL Anweisungen (Update, Delete) ausführen
        public static RecordSetResult<T> Set<T>(this RecordSetResult<T> @this)
        {
            if (@this.Connection == null)
            {
                throw new ArgumentException($"Das Connection-Object ist null. Daher kann das RecordSet nicht ausgeführt werden");
            }

            if (@this.Connection.State != ConnectionState.Open)
            {
                throw new ArgumentException($"Damit das RecordSet ausgeführt werden kann, muß die Connection offen sein.");
            }

            if (CheckSetResultParameter(typeof(T)) == false)
            {
                throw new ArgumentException($"Der Typ '{typeof(T).Name}' ist für das Schreiben des RecordSet nicht gültig.");
            }

            T resultValue = default;

            try
            {
                if (typeof(T).IsGenericType == false && typeof(T).IsPrimitive == true && typeof(T).Namespace == "System")
                {
                    resultValue = SetExecuteNonQuery<T>(@this.Connection, @this.SQL, @this.ParameterCollection, @this.SQLiteParameter);
                }
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return new RecordSetResult<T>(@this.Connection, resultValue, @this.SQL);
        }

        private static T SetExecuteNonQuery<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection, SQLiteParameter[] sqliteParameter)
        {
            object getAs = null;

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    int? result = cmd.ExecuteNonQuery();
                    getAs = result == null ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (SQLiteException ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)getAs;
        }

        #endregion SET, SQL Anweisungen (Update, Delete) ausführen

        #region GET, Lesen von Daten in verschiedene Typen
        public static RecordSetResult<T> Get<T>(this RecordSetResult<T> @this)
        {
            if (@this.Connection == null)
            {
                throw new ArgumentException($"Das Connection-Object ist null. Daher kann das RecordSet nicht ausgeführt werden");
            }

            if (@this.Connection.State != ConnectionState.Open)
            {
                throw new ArgumentException($"Damit das RecordSet ausgeführt werden kann, muß die Connection offen sein.");
            }

            if (CheckGetResultParameter(typeof(T)) == false)
            {
                throw new ArgumentException($"Der Typ '{typeof(T).Name}' ist für die Rückgabe des RecordSet Result nicht gültig.");
            }

            T resultValue = default;

            try
            {
                if (typeof(T).IsGenericType == false && typeof(T).IsPrimitive == true && typeof(T).Namespace == "System")
                {
                    resultValue = GetScalar<T>(@this.Connection, @this.SQL, @this.ParameterCollection, @this.SQLiteParameter);
                }
                else if (typeof(T) == typeof(byte[]) && typeof(T).IsGenericType == false && typeof(T).Namespace == "System")
                {
                    resultValue = GetScalar<T>(@this.Connection, @this.SQL, @this.ParameterCollection, @this.SQLiteParameter);
                }
                else if (typeof(T) == typeof(DataRow))
                {
                    resultValue = GetDataRow<T>(@this.Connection, @this.SQL, @this.ParameterCollection, @this.SQLiteParameter);
                }
                else if (typeof(T) == typeof(ICollectionView))
                {
                    resultValue = GetCollectionView<T>(@this.Connection, @this.SQL, @this.ParameterCollection, @this.SQLiteParameter);
                }
                else if (typeof(T) == typeof(DataTable))
                {
                    resultValue = GetDataTable<T>(@this.Connection, @this.SQL, @this.ParameterCollection, @this.SQLiteParameter);
                }
                else if (typeof(T).IsGenericType == true && typeof(T).GetGenericTypeDefinition() == typeof(List<>) && typeof(T).GetGenericArguments()[0].Namespace != "System")
                {
                    resultValue = GetListOfTGeneric<T>(@this.Connection, @this.SQL, @this.ParameterCollection, @this.SQLiteParameter);
                }
                else if (typeof(T).IsGenericType == true && typeof(T).GetGenericTypeDefinition() == typeof(List<>) && typeof(T).GetGenericArguments()[0].Namespace == "System")
                {
                    resultValue = GetListOfTNonGeneric<T>(@this.Connection, @this.SQL, @this.ParameterCollection, @this.SQLiteParameter);
                }
                else if (typeof(T).IsGenericType == true && typeof(T).GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    resultValue = GetDictionary<T>(@this.Connection, @this.SQL, @this.ParameterCollection, @this.SQLiteParameter);
                }
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return new RecordSetResult<T>(@this.Connection, resultValue, @this.SQL);
        }

        private static T GetScalar<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection, SQLiteParameter[] sqliteParameter)
        {
            object getAs = null;

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        getAs = result == null ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                    }
                    else
                    {
                        getAs = (T)Convert.ChangeType(0, typeof(T));
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)getAs;
        }

        private static T GetDataRow<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection, SQLiteParameter[] sqliteParameter)
        {
            object result = null;

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows == true && dr.VisibleFieldCount > 0)
                        {
                            DataTable dt = new DataTable();
                            dt.TableName = ExtractTablename(sql);
                            dt.Load(dr);
                            result = dt.Rows[0];
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)result;
        }

        private static T GetCollectionView<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection, SQLiteParameter[] sqliteParameter)
        {
            ICollectionView result;

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    DataTable dt = null;
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows == true && dr.VisibleFieldCount > 0)
                        {
                            dt = new DataTable();
                            dt.Load(dr);
                            if (dt.HasColumn("CreatedOn") == true && dt.HasColumn("CreatedBy") == true && dt.HasColumn("ModifiedOn") == true && dt.HasColumn("ModifiedBy") == true)
                            {
                                if (dt.HasColumn("Timestamp") == false)
                                {
                                    dt.Columns.Add("Timestamp", typeof(string));
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        TimeStamp ts = new TimeStamp();
                                        DateTime createdOn;
                                        DateTime modifiedOn;
                                        if (row["CreatedOn"] == DBNull.Value)
                                        {
                                            createdOn = new DateTime(1900, 1, 1);
                                        }
                                        else
                                        {
                                            createdOn = (DateTime)row["CreatedOn"];
                                        }

                                        if (row["ModifiedOn"] == DBNull.Value)
                                        {
                                            modifiedOn = new DateTime(1900, 1, 1);
                                        }
                                        else
                                        {
                                            modifiedOn = (DateTime)row["ModifiedOn"];
                                        }

                                        string timestamp = ts.MaxEntry(createdOn, row["CreatedBy"].ToString(), modifiedOn, row["ModifiedBy"].ToString());
                                        row.BeginEdit();
                                        row["Timestamp"] = timestamp;
                                        row.EndEdit();
                                    }
                                }
                            }
                        }
                    }

                    if (dt != null)
                    {
                        result = CollectionViewSource.GetDefaultView(dt.Rows) as CollectionView;
                    }
                    else
                    {
                        result = null;
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)result;
        }

        private static T GetDataTable<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection, SQLiteParameter[] sqliteParameter)
        {
            object result = null;

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows == true)
                        {
                            result = new DataTable();
                            ((DataTable)result).TableName = ExtractTablename(sql);
                            ((DataTable)result).Load(dr);
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)result;
        }

        private static T GetListOfTGeneric<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection, SQLiteParameter[] sqliteParameter)
        {
            T result = default;

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows == true)
                        {
                            /* Typ für List<T> erstellen */
                            Type typeCollection = typeof(T);
                            result = (T)Activator.CreateInstance(typeCollection);

                            while (dr.Read())
                            {
                                int columnCount = dr.FieldCount;

                                /* Typ für List-Content erstellen */
                                Type genericType = typeCollection.GetGenericArguments()[0];
                                var instance = Activator.CreateInstance(genericType);
                                if (instance != null)
                                {
                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        string columnName = dr.GetName(i);
                                        object columnValue = dr[i];
                                        PropertyInfo itemProperty = instance.GetType().GetProperty(columnName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                                        if (itemProperty == null || itemProperty.CanWrite == false)
                                        {
                                            throw new ArgumentException($"Property '{columnName}' kann nicht beschrieben werden. Prüfen Sie die Klasse '{genericType.Name}'");
                                        }

                                        if (itemProperty.PropertyType == typeof(Guid))
                                        {
                                            if (columnValue != DBNull.Value)
                                            {
                                                itemProperty.SetValue(instance, new Guid(columnValue.ToString()), null);
                                            }
                                        }
                                        else if (itemProperty.PropertyType == typeof(int))
                                        {
                                            if (columnValue != DBNull.Value)
                                            {
                                                itemProperty.SetValue(instance, dr.GetInt32(i), null);
                                            }
                                        }
                                        else if (itemProperty.PropertyType == typeof(long))
                                        {
                                            if (columnValue != DBNull.Value)
                                            {
                                                itemProperty.SetValue(instance, Convert.ToInt64(columnValue), null);
                                            }
                                        }
                                        else if (itemProperty.PropertyType == typeof(decimal))
                                        {
                                            if (columnValue != DBNull.Value)
                                            {
                                                itemProperty.SetValue(instance, Convert.ToDecimal(columnValue), null);
                                            }
                                        }
                                        else if (itemProperty.PropertyType == typeof(double))
                                        {
                                            if (columnValue != DBNull.Value)
                                            {
                                                itemProperty.SetValue(instance, Convert.ToDouble(columnValue), null);
                                            }
                                        }
                                        else if (itemProperty.PropertyType == typeof(DateTime))
                                        {
                                            if (columnValue != DBNull.Value)
                                            {
                                                itemProperty.SetValue(instance, dr.GetDateTime(i), null);
                                            }
                                            else
                                            {
                                                itemProperty.SetValue(instance, new DateTime(1900, 1, 1), null);
                                            }
                                        }
                                        else if (itemProperty.PropertyType == typeof(bool))
                                        {
                                            if (columnValue != DBNull.Value)
                                            {
                                                itemProperty.SetValue(instance, dr.GetBoolean(i), null);
                                            }
                                        }
                                        else if (itemProperty.PropertyType == typeof(byte[]))
                                        {
                                            if (columnValue != DBNull.Value)
                                            {
                                                byte[] byteArray = (byte[])dr.GetValue(i);
                                                itemProperty.SetValue(instance, byteArray, null);
                                            }
                                        }
                                        else
                                        {
                                            if (columnValue != DBNull.Value)
                                            {
                                                itemProperty.SetValue(instance, columnValue, null);
                                            }
                                        }
                                    }
                                }

                                /* Add Methode mit Content per Invoke erstellen */
                                MethodInfo method = typeCollection.GetMethod("Add");
                                method.Invoke(result, new object[] { instance });
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)Convert.ChangeType(result, typeof(T));
        }

        private static T GetListOfTNonGeneric<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection, SQLiteParameter[] sqliteParameter)
        {
            IList result = default;

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows == true)
                        {
                            /* Typ für List<T> erstellen */
                            Type genericListType = typeof(List<>);
                            Type genericType = typeof(T).GetGenericArguments()[0];
                            Type concreteListType = genericListType.MakeGenericType(genericType);
                            result = Activator.CreateInstance(concreteListType) as IList;

                            while (dr.Read())
                            {
                                int columnCount = dr.FieldCount;

                                if (result != null)
                                {
                                    string valueName = dr.GetName(0);
                                    if (genericType == typeof(Guid))
                                    {
                                        result.Add(Convert.ChangeType(new Guid(dr[0].ToString()), genericType));
                                    }
                                    else
                                    {
                                        result.Add(Convert.ChangeType(dr[0], genericType));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)Convert.ChangeType(result, typeof(T));
        }

        private static T GetDictionary<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection, SQLiteParameter[] sqliteParameter)
        {
            T result = default;

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows == true)
                        {
                            /* Typ für List<T> erstellen */
                            Type typeCollection = typeof(T);
                            result = (T)Activator.CreateInstance(typeCollection);

                            while (dr.Read())
                            {
                                int columnCount = dr.FieldCount;
                                string keyName = dr.GetName(0);
                                string valueName = dr.GetName(1);
                                MethodInfo method = typeCollection.GetMethod("Add");

                                Type[] genericTyp = typeCollection.GenericTypeArguments;
                                if (genericTyp[0].Name == typeof(string).Name && genericTyp[1].Name == typeof(string).Name)
                                {
                                    method.Invoke(result, new object[] { dr.GetString(keyName), dr.GetString(valueName) });
                                }
                                else if (genericTyp[0].Name == typeof(Int32).Name && genericTyp[1].Name == typeof(string).Name)
                                {
                                    method.Invoke(result, new object[] { dr.GetInt32(keyName), dr.GetString(valueName) });
                                }
                                else if (genericTyp[0].Name == typeof(Int64).Name && genericTyp[1].Name == typeof(string).Name)
                                {
                                    method.Invoke(result, new object[] { dr.GetInt64(keyName), dr.GetString(valueName) });
                                }
                                else if (genericTyp[0].Name == typeof(Guid).Name && genericTyp[1].Name == typeof(string).Name)
                                {
                                    method.Invoke(result, new object[] { dr.GetGuid(keyName), dr.GetString(valueName) });
                                }
                                else if (genericTyp[0].Name == typeof(string).Name && genericTyp[1].Name == typeof(object).Name)
                                {
                                    method.Invoke(result, new object[] { dr.GetString(keyName), dr.GetString(valueName) });
                                }
                                else
                                {
                                    throw new ArgumentException($"Die Parameter für Key: '{genericTyp[0].Name}' und Value: '{genericTyp[1].Name}' dürfen in diese Kombination nicht verwendet werden.");
                                }
                            }
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)Convert.ChangeType(result, typeof(T));
        }
        #endregion GET, Lesen von Daten in verschiedene Typen

        #region Neues DataRow
        public static RecordSetResult<T> New<T>(this RecordSetResult<T> @this)
        {
            if (@this.Connection == null)
            {
                throw new ArgumentException($"Das Connection-Object ist null. Daher kann das RecordSet nicht ausgeführt werden");
            }

            if (@this.Connection.State != ConnectionState.Open)
            {
                throw new ArgumentException($"Damit das RecordSet ausgeführt werden kann, muß die Connection offen sein.");
            }

            if (CheckNewResultParameter(typeof(T)) == false)
            {
                throw new ArgumentException($"Der Typ '{typeof(T).Name}' ist für das Erstellen eines Typ über das RecordSet nicht gültig.");
            }

            T resultValue = default;

            try
            {
                if (typeof(T) == typeof(DataRow))
                {
                    string sql = $"SELECT {@this.SQL}.* FROM {@this.SQL} ORDER BY rowid DESC LIMIT 1";
                    resultValue = NewDataRow<T>(@this.Connection, sql);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return new RecordSetResult<T>(@this.Connection, resultValue, @this.SQL);
        }

        private static T NewDataRow<T>(SQLiteConnection connection, string sql)
        {
            object result = null;

            try
            {
                using (SQLiteCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;

                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.VisibleFieldCount > 0)
                        {
                            DataTable dt = new DataTable();
                            dt.TableName = ExtractTablename(sql);
                            dt.Load(dr);
                            result = dt.NewRow();
                        }
                    }
                }
            }
            catch (SQLiteException ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)result;
        }

        #endregion Neues DataRow

        #region Execute SQL Anweisung
        public static RecordSetResult<T> Execute<T>(this RecordSetResult<T> @this)
        {
            if (@this.Connection == null)
            {
                throw new ArgumentException($"Das Connection-Object ist null. Daher kann das RecordSet nicht ausgeführt werden");
            }

            if (@this.Connection.State != ConnectionState.Open)
            {
                throw new ArgumentException($"Damit das RecordSet ausgeführt werden kann, muß die Connection offen sein.");
            }

            if (CheckExecuteResultParameter(typeof(T)) == false)
            {
                throw new ArgumentException($"Der Typ '{typeof(T).Name}' ist für eine Execute Anweisung nicht gültig. Versuchen Sie es mit int, long.");
            }

            T resultValue = default;

            try
            {
                if (typeof(T).IsGenericType == false && typeof(T).IsPrimitive == true && typeof(T).Namespace == "System")
                {
                    if (@this.Transaction != null)
                    {
                        resultValue = ExecuteNonQuery<T>(@this.Connection, @this.SQL, @this.ParameterCollection, @this.SQLiteParameter,@this.Transaction);
                    }
                    else
                    {
                        resultValue = ExecuteNonQuery<T>(@this.Connection, @this.SQL, @this.ParameterCollection, @this.SQLiteParameter);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return new RecordSetResult<T>(@this.Connection, resultValue, @this.SQL);
        }

        private static T ExecuteNonQuery<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection, SQLiteParameter[] sqliteParameter, SQLiteTransaction transaction)
        {
            object getAs = null;
            SQLiteTransaction trans = null;

            try
            {
                trans = connection.BeginTransaction();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Transaction = transaction;

                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    if (sqliteParameter != null && sqliteParameter.Length > 0)
                    {
                        cmd.Parameters.AddRange(sqliteParameter);
                    }

                    int? result = cmd.ExecuteNonQuery();
                    getAs = result == null ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                    trans.Commit();
                }
            }
            catch (SQLiteException ex)
            {
                trans.Rollback();
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                string ErrorText = ex.Message;
                throw;
            }

            return (T)getAs;
        }

        private static T ExecuteNonQuery<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection, SQLiteParameter[] sqliteParameter)
        {
            object getAs = null;

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    if (sqliteParameter != null && sqliteParameter.Length > 0)
                    {
                        cmd.Parameters.AddRange(sqliteParameter);
                    }

                    int? result = cmd.ExecuteNonQuery();
                    getAs = result == null ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (SQLiteException ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)getAs;
        }

        private static T ExecuteNonQuery<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection, SQLiteTransaction transaction)
        {
            object getAs = null;
            SQLiteTransaction trans = null;

            try
            {
                trans = connection.BeginTransaction();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Transaction = transaction;

                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    int? result = cmd.ExecuteNonQuery();
                    getAs = result == null ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                    trans.Commit();
                }
            }
            catch (SQLiteException ex)
            {
                trans.Rollback();
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                string ErrorText = ex.Message;
                throw;
            }

            return (T)getAs;
        }

        private static T ExecuteNonQuery<T>(SQLiteConnection connection, string sql, Dictionary<string, object> parameterCollection)
        {
            object getAs = null;

            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    if (parameterCollection != null && parameterCollection.Count > 0)
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

                    int? result = cmd.ExecuteNonQuery();
                    getAs = result == null ? default(T) : (T)Convert.ChangeType(result, typeof(T));
                }
            }
            catch (SQLiteException ex)
            {
                string ErrorText = ex.Message;
                throw;
            }
            catch (Exception ex)
            {
                string ErrorText = ex.Message;
                throw;
            }

            return (T)getAs;
        }
        #endregion Execute SQL Anweisung

        #region Private Methoden
        private static string ExtractTablename(string sql)
        {
            try
            {
                List<string> tables = new List<string>();

                Regex r = new Regex(@"(from|join|into)\s+(?<table>\S+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match ma = r.Match(sql);
                while (ma.Success)
                {
                    tables.Add(ma.Groups["table"].Value);
                    ma = ma.NextMatch();
                }

                return tables.FirstOrDefault().ToUpper();
            }
            catch (Exception)
            {
                return $"TAB{DateTime.Now.ToString("yyyyMMdd")}";
            }
        }

        private static bool CheckSetResultParameter(Type type)
        {
            bool result = false;

            if (type.Name == typeof(int).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(long).Name)
            {
                result = true;
            }

            return result;
        }

        private static bool CheckNewResultParameter(Type type)
        {
            bool result = false;

            if (type.Name == typeof(DataRow).Name)
            {
                result = true;
            }

            return result;
        }

        private static bool CheckExecuteResultParameter(Type type)
        {
            bool result = false;

            if (type.Name == typeof(int).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(long).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(string).Name)
            {
                result = true;
            }

            return result;
        }

        private static bool CheckGetResultParameter(Type type)
        {
            bool result = false;

            if (type.Name == typeof(DataRow).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(DataTable).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(ICollectionView).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(List<>).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(Dictionary<,>).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(string).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(DateTime).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(bool).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(int).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(long).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(Single).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(decimal).Name)
            {
                result = true;
            }
            else if (type.Name == typeof(float).Name)
            {
                result = true;
            }

            return result;
        }
        #endregion Private Methoden
    }

    public class RecordSetResult<T>
    {
        /// <summary>
        /// Gibt das Ergebnis eines RecordSet zurück
        /// </summary>
        /// <param name="connection">Aktuelles Datenbankverbindung, als Connection-Object</param>
        /// <param name="resultValue">Erwarteterer Wert der SQL Anweisung, werden keine Daten gefunden, wird entweder null oder der Default-Wert des Datentyp zurückgegeben.</param>
        /// <param name="sql">SQL Anweisung</param>
        public RecordSetResult(SQLiteConnection connection, T resultValue, string sql)
        {
            this.Connection = connection;
            this.SQL = sql;
            this.Transaction = null;
            this.Result = resultValue;
        }

        public RecordSetResult(SQLiteConnection connection, T resultValue, string sql, SQLiteTransaction transaction)
        {
            this.Connection = connection;
            this.SQL = sql;
            this.Transaction = transaction;
            this.Result = resultValue;
        }

        /// <summary>
        /// Gibt das Ergebnis eines RecordSet zurück
        /// </summary>
        /// <param name="connection">Aktuelles Datenbankverbindung, als Connection-Object</param>
        /// <param name="resultValue">Erwarteterer Wert der SQL Anweisung, werden keine Daten gefunden, wird entweder null oder der Default-Wert des Datentyp zurückgegeben.</param>
        /// <param name="sql">SQL Anweisung</param>
        /// <param name="parameterCollection">Dictionary mit einer Liste von Parametern als String (Parametername) und Object (Parametervalue)</param>
        public RecordSetResult(SQLiteConnection connection, T resultValue, string sql, Dictionary<string, object> parameterCollection)
        {
            this.Connection = connection;
            this.SQL = sql;
            this.Transaction = null;
            this.ParameterCollection = parameterCollection;
            this.Result = resultValue;
        }

        public RecordSetResult(SQLiteConnection connection, T resultValue, string sql, Dictionary<string, object> parameterCollection, SQLiteTransaction transaction)
        {
            this.Connection = connection;
            this.SQL = sql;
            this.Result = resultValue;
            this.Transaction = transaction;
            this.ParameterCollection = parameterCollection;
        }

        /// <summary>
        /// Gibt das Ergebnis eines RecordSet zurück
        /// </summary>
        /// <param name="connection">Aktuelles Datenbankverbindung, als Connection-Object</param>
        /// <param name="resultValue">Erwarteterer Wert der SQL Anweisung, werden keine Daten gefunden, wird entweder null oder der Default-Wert des Datentyp zurückgegeben.</param>
        /// <param name="sql">SQL Anweisung</param>
        /// <param name="parameterCollection">SQLiteParameter Array mit einer Liste von Parametern als String (Parametername) und Object (Parametervalue)</param>
        public RecordSetResult(SQLiteConnection connection, T resultValue, string sql, SQLiteParameter[] parameterCollection)
        {
            this.Connection = connection;
            this.SQL = sql;
            this.SQLiteParameter = parameterCollection;
            this.Transaction = null;
            this.Result = resultValue;
        }

        public RecordSetResult(SQLiteConnection connection, T resultValue, string sql, SQLiteParameter[] parameterCollection, SQLiteTransaction transaction)
        {
            this.Connection = connection;
            this.SQL = sql;
            this.SQLiteParameter = parameterCollection;
            this.Transaction = transaction;
            this.Result = resultValue;
        }

        /// <summary>
        /// SQL Anweisung
        /// </summary>
        public string SQL { get; private set; }

        /// <summary>
        /// Dictionary mit einer Liste von Parametern als String (Parametername) und Object (Parametervalue)
        /// </summary>
        public Dictionary<string, object> ParameterCollection { get; private set; }

        /// <summary>
        /// SQLiteParameter Array mit einer Liste von Parametern als String (Parametername) und Object (Parametervalue)
        /// </summary>
        public SQLiteParameter[] SQLiteParameter { get; private set; }

        /// <summary>
        /// Aktuelles Datenbankverbindung, als Connection-Object
        /// </summary>
        public SQLiteConnection Connection { get; set; }

        public SQLiteTransaction Transaction { get; set; }

        /// <summary>
        /// Erwarteterer Wert der SQL Anweisung, werden keine Daten gefunden, wird entweder null oder der Default-Wert des Datentyp zurückgegeben.
        /// </summary>
        public T Result { get; private set; }
    }
}
