//-----------------------------------------------------------------------
// <copyright file="SQLDataReaderExtentions.cs" company="Lifeprojects.de">
//     Class: SQLDataReaderExtentions
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>2.7.2018</date>
//
// <summary>SQLDataReaderExtentions Class for SQL Server Database</summary>
//-----------------------------------------------------------------------

namespace System.Data.SQLite
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    using Globalization;

    public static class SQLDataReaderExtentions
    {
        public static List<T> MapToList<T>(this SQLiteDataReader @this)
        {
            List<T> result = null;
            var entity = typeof(T);
            Dictionary<string, PropertyInfo> propertyDict = new Dictionary<string, PropertyInfo>();

            try
            {
                if (@this != null && @this.HasRows == true)
                {
                    result = new List<T>();
                    PropertyInfo[] Props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    propertyDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    while (@this.Read())
                    {
                        T newObject = (T)Activator.CreateInstance(entity);
                        for (int index = 0; index < @this.FieldCount; index++)
                        {
                            if (propertyDict.ContainsKey(@this.GetName(index).ToUpper()))
                            {
                                var info = propertyDict[@this.GetName(index).ToUpper()];
                                if ((info != null) && info.CanWrite == true)
                                {
                                    var fieldValue = @this.GetValue(index);

                                    if (info.PropertyType.Name.ToLower() == "guid")
                                    {
                                        info.SetValue(newObject, (fieldValue == DBNull.Value) ? null : (Guid?)new Guid(fieldValue.ToString()), null);
                                    }
                                    else if (info.PropertyType.Name.ToLower() == "string")
                                    {
                                        object getAs = fieldValue == DBNull.Value ? default(string) : (string)Convert.ChangeType(fieldValue, typeof(string), CultureInfo.InvariantCulture);
                                        info.SetValue(newObject, (fieldValue == DBNull.Value) ? null : getAs, null);
                                    }
                                    else if (info.PropertyType.Name.ToLower().Contains("int") == true)
                                    {
                                        object getAs = fieldValue == DBNull.Value ? default(int) : (int)Convert.ChangeType(fieldValue, typeof(int), CultureInfo.InvariantCulture);
                                        info.SetValue(newObject, (fieldValue == DBNull.Value) ? null : getAs, null);
                                    }
                                    else if (info.PropertyType.Name.ToLower().Contains("double") == true)
                                    {
                                        object getAs = fieldValue == DBNull.Value ? default(double) : (double)Convert.ChangeType(fieldValue, typeof(double), CultureInfo.InvariantCulture);
                                        info.SetValue(newObject, (fieldValue == DBNull.Value) ? null : getAs, null);
                                    }
                                    else if (info.PropertyType.Name.ToLower().Contains("decimal") == true)
                                    {
                                        object getAs = fieldValue == DBNull.Value ? default(decimal) : (decimal)Convert.ChangeType(fieldValue, typeof(decimal), CultureInfo.InvariantCulture);
                                        info.SetValue(newObject, (fieldValue == DBNull.Value) ? null : getAs, null);
                                    }
                                    else if (info.PropertyType.Name.ToLower().Contains("datetime") == true)
                                    {
                                        object getAs = fieldValue == DBNull.Value ? default(DateTime) : (DateTime)Convert.ChangeType(fieldValue, typeof(DateTime), CultureInfo.InvariantCulture);
                                        info.SetValue(newObject, (fieldValue == DBNull.Value) ? null : getAs, null);
                                    }
                                    else if (info.PropertyType.Name.ToLower().Contains("bool") == true)
                                    {
                                        object getAs = fieldValue == DBNull.Value ? default(bool) : (bool)Convert.ChangeType(fieldValue, typeof(bool), CultureInfo.InvariantCulture);
                                        info.SetValue(newObject, (fieldValue == DBNull.Value) ? null : getAs, null);
                                    }
                                    else if (info.PropertyType.Name.ToLower().Contains("byte[]") == true)
                                    {
                                        object getAs = fieldValue == DBNull.Value ? default(byte[]) : (byte[])Convert.ChangeType(fieldValue, typeof(byte[]), CultureInfo.InvariantCulture);
                                        if (getAs != null)
                                        {
                                            info.SetValue(newObject, (fieldValue == DBNull.Value) ? null : fieldValue, null);
                                        }
                                    }
                                    else
                                    {
                                        info.SetValue(newObject, (fieldValue == DBNull.Value) ? null : fieldValue, null);
                                    }
                                }
                            }
                        }

                        result.Add(newObject);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static T MapToSingle<T>(this SQLiteDataReader @this) where T : new()
        {
            T result = new T();
            var entity = typeof(T);
            Dictionary<string, PropertyInfo> propertyDict = new Dictionary<string, PropertyInfo>();

            try
            {
                if (@this != null && @this.HasRows == true)
                {
                    PropertyInfo[] Props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    propertyDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    @this.Read();
                    for (int index = 0; index < @this.FieldCount; index++)
                    {
                        if (propertyDict.ContainsKey(@this.GetName(index).ToUpper()))
                        {
                            var info = propertyDict[@this.GetName(index).ToUpper()];
                            if ((info != null) && info.CanWrite == true)
                            {
                                var fieldValue = @this.GetValue(index);

                                if (info.PropertyType.Name.ToLower() == "guid")
                                {
                                    info.SetValue(result, (fieldValue == DBNull.Value) ? null : (Guid?)new Guid(fieldValue.ToString()), null);
                                }
                                else if (info.PropertyType.Name.ToLower() == "string")
                                {
                                    object getAs = fieldValue == DBNull.Value ? default(string) : (string)Convert.ChangeType(fieldValue, typeof(string), CultureInfo.InvariantCulture);
                                    info.SetValue(result, (fieldValue == DBNull.Value) ? null : getAs, null);
                                }
                                else if (info.PropertyType.Name.ToLower().Contains("int") == true)
                                {
                                    object getAs = fieldValue == DBNull.Value ? default(int) : (int)Convert.ChangeType(fieldValue, typeof(int), CultureInfo.InvariantCulture);
                                    info.SetValue(result, (fieldValue == DBNull.Value) ? null : getAs, null);
                                }
                                else if (info.PropertyType.Name.ToLower().Contains("double") == true)
                                {
                                    object getAs = fieldValue == DBNull.Value ? default(double) : (double)Convert.ChangeType(fieldValue, typeof(double), CultureInfo.InvariantCulture);
                                    info.SetValue(result, (fieldValue == DBNull.Value) ? null : getAs, null);
                                }
                                else if (info.PropertyType.Name.ToLower().Contains("decimal") == true)
                                {
                                    object getAs = fieldValue == DBNull.Value ? default(decimal) : (decimal)Convert.ChangeType(fieldValue, typeof(decimal), CultureInfo.InvariantCulture);
                                    info.SetValue(result, (fieldValue == DBNull.Value) ? null : getAs, null);
                                }
                                else if (info.PropertyType.Name.ToLower().Contains("datetime") == true)
                                {
                                    object getAs = fieldValue == DBNull.Value ? default(DateTime) : (DateTime)Convert.ChangeType(fieldValue, typeof(DateTime), CultureInfo.InvariantCulture);
                                    info.SetValue(result, (fieldValue == DBNull.Value) ? null : getAs, null);
                                }
                                else if (info.PropertyType.Name.ToLower().Contains("bool") == true)
                                {
                                    object getAs = fieldValue == DBNull.Value ? default(bool) : (bool)Convert.ChangeType(fieldValue, typeof(bool), CultureInfo.InvariantCulture);
                                    info.SetValue(result, (fieldValue == DBNull.Value) ? null : getAs, null);
                                }
                                else if (info.PropertyType.Name.ToLower().Contains("byte[]") == true)
                                {
                                    object getAs = fieldValue == DBNull.Value ? default(byte[]) : (byte[])Convert.ChangeType(fieldValue, typeof(byte[]), CultureInfo.InvariantCulture);
                                    if (getAs != null)
                                    {
                                        info.SetValue(result, (fieldValue == DBNull.Value) ? null : fieldValue, null);
                                    }
                                }
                                else
                                {
                                    info.SetValue(result, (fieldValue == DBNull.Value) ? null : fieldValue, null);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static List<T> ToList<T>(this SQLiteDataReader @this) where T : new()
        {
            var list = new List<T>();

            if (@this.HasRows)
            {
                var columnCount = @this.FieldCount;

                while (@this.Read())
                {
                    var item = new T();

                    for (int i = 0; i < columnCount; i++)
                    {
                        var columnName = @this.GetName(i);
                        var itemProperty = item.GetType().GetProperty(columnName);

                        if (itemProperty == null) continue;

                        itemProperty.SetValue(item, @this[i], null);
                    }

                    list.Add(item);
                }

                @this.Close();
            }

            return list;
        }

        public static DataTable ToDataTable(this IDataReader @this)
        {
            DataTable dt = new DataTable();
            for (var i = 0; i > @this.FieldCount; ++i)
            {
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = @this.GetName(i),
                    DataType = @this.GetFieldType(i)
                }
                    );
            }

            while (@this.Read())
            {
                var row = dt.NewRow();
                @this.GetValues(row.ItemArray);
                dt.Rows.Add(row);
            }

            return dt;
        }

        public static DataTable ToDataTable(this SQLiteDataReader @this)
        {
            DataTable dt = new DataTable();
            for (var i = 0; i > @this.FieldCount; ++i)
            {
                dt.Columns.Add(new DataColumn
                {
                    ColumnName = @this.GetName(i),
                    DataType = @this.GetFieldType(i)
                }
                    );
            }

            while (@this.Read())
            {
                var row = dt.NewRow();
                @this.GetValues(row.ItemArray);
                dt.Rows.Add(row);
            }

            return dt;
        }

        public static IEnumerable<IDataRecord> AsEnumerable(this IDataReader @this)
        {
            while (@this.Read())
            {
                yield return @this;
            }
        }

        public static IEnumerable<T> AsEnumerable<T>(this SQLiteDataReader @this, string column) where T : class, new()
        {
            if (@this.HasRows == true && @this.HasColumn(column) == true)
            {
                while (@this.Read())
                {
                    yield return @this.GetAs<T>(column);
                }
            }
            else
            {
                throw new ArgumentException($"No Data or Column {column} not found!");
            }
        }

        public static bool HasColumn(this IDataRecord @this, string columnName)
        {
            bool result = false;

            result = Enumerable.Range(0, @this.FieldCount).Any(i => string.Equals(@this.GetName(i), columnName, StringComparison.OrdinalIgnoreCase));

            return result;
        }

        public static bool HasColumn(this SQLiteDataReader @this, string columnName)
        {
            bool result = false;

            result = Enumerable.Range(0, @this.FieldCount).Any(i => string.Equals(@this.GetName(i), columnName, StringComparison.OrdinalIgnoreCase));

            return result;
        }

        public static bool IsDBNull(this IDataReader @this, string columnName)
        {
            bool result = @this[columnName] == DBNull.Value;

            return result;
        }

        public static bool IsDBNull(this SQLiteDataReader @this, string columnName)
        {
            bool result = @this[columnName] == DBNull.Value;

            return result;
        }

        /// <summary>
        /// Reads all all records from a data reader and performs an action for each.
        /// </summary>
        /// <param name = "this">The data reader.</param>
        /// <param name = "action">The action to be performed.</param>
        /// <returns>
        /// The count of actions that were performed.
        /// </returns>
        /// <example>
        /// ...
        /// int countMax = reader.ReadAll(rdrAction => {entity = GetHistoryRowItem(reader);
        /// ...
        /// private ImportHistoryEntity GetHistoryRowItem(IDataReader rdrAction) {
        ///    ...
        ///    return (entity);
        /// }
        /// </example>
        public static int ReadAll(this IDataReader @this, Action<IDataReader> action)
        {
            var count = 0;

            while (@this.Read())
            {
                action(@this);
                count++;
            }

            return count;
        }

        public static int ReadAll(this SQLiteDataReader @this, Action<SQLiteDataReader> action)
        {
            var count = 0;

            if (@this.HasRows == true)
            {
                while (@this.Read())
                {
                    action(@this);
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Singles the specified reader.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="this">The reader.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        /// <exception cref="System.Data.DataException">multiple rows returned from query</exception>
        /// <example>
        /// using (var reader = cmd.ExecuteReader()) {
        ///     User u = reader.Single(r => new User((int)r["UserId"], r["UserName"].ToString()))
        /// }
        /// </example>
        public static R Single<R>(this IDataReader @this, Func<IDataReader, R> selector)
        {
            R result = default(R);
            if (@this.Read())
            {
                result = selector(@this);
            }

            if (@this.Read())
            {
                throw new DataException("multiple rows returned from query");
            }

            return result;
        }

        /// <summary>
        /// Singles the specified reader.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="this">The reader.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        /// <exception cref="System.Data.DataException">multiple rows returned from query</exception>
        /// <example>
        /// using (var reader = cmd.ExecuteReader()) {
        ///     User u = reader.Single(r => new User((int)r["UserId"], r["UserName"].ToString()))
        /// }
        /// </example>
        public static R Single<R>(this SQLiteDataReader @this, Func<SQLiteDataReader, R> selector)
        {
            R result = default(R);
            if (@this.Read())
            {
                result = selector(@this);
            }

            if (@this.Read())
            {
                throw new DataException("multiple rows returned from query");
            }

            return result;
        }

        public static T GetAs<T>(this SQLiteDataReader @this, string fieldName)
        {
            try
            {
                if (@this.VisibleFieldCount == 0)
                {
                    return default(T);
                }

                object getAs = null;
                if (typeof(T).Name == "Guid")
                {
                    getAs = @this[fieldName] == DBNull.Value ? Guid.Empty : new Guid(@this[fieldName].ToString());
                }
                else if (typeof(T).IsEnum == true)
                {
                    if (@this[fieldName].GetType() == typeof(int))
                    {
                        getAs = (T)@this[fieldName];
                    }
                    else if (@this[fieldName].GetType() == typeof(string))
                    {
                        getAs = (T)Enum.Parse(typeof(T), @this[fieldName].ToString(), true);
                    }
                    else
                    {
                        getAs = (T)Enum.Parse(typeof(T), @this[fieldName].ToString(), true);
                    }
                }
                else
                {
                    getAs = @this[fieldName] == DBNull.Value ? default(T) : (T)Convert.ChangeType(@this[fieldName], typeof(T), CultureInfo.InvariantCulture);
                }

                return (T)getAs;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Error",ex.Message);
                return default(T);
            }
        }

        public static T GetAs<T>(this IDataReader @this, string fieldName)
        {
            try
            {
                if (@this.FieldCount == 0)
                {
                    return default(T);
                }

                object getAs = null;
                if (typeof(T).Name == "Guid")
                {
                    getAs = @this[fieldName] == DBNull.Value ? Guid.Empty : new Guid(@this[fieldName].ToString());
                }
                else if (typeof(T).IsEnum == true)
                {
                    if (@this[fieldName].GetType() == typeof(int))
                    {
                        getAs = (T)@this[fieldName];
                    }
                    else if (@this[fieldName].GetType() == typeof(string))
                    {
                        getAs = (T)Enum.Parse(typeof(T), @this[fieldName].ToString(), true);
                    }
                    else
                    {
                        getAs = (T)Enum.Parse(typeof(T), @this[fieldName].ToString(), true);
                    }
                }
                else
                {
                    getAs = @this[fieldName] == DBNull.Value ? default(T) : (T)Convert.ChangeType(@this[fieldName], typeof(T), CultureInfo.InvariantCulture);
                }

                return (T)getAs;
            }
            catch (Exception ex)
            {
                ex.Data.Add("Error", ex.Message);
                return default(T);
            }
        }

        /// <summary>
        /// Returns field value as string (empty string if value equals DBNull or column ID does not exist)
        /// </summary>
        /// <param name="this">SqlDataReader</param>
        /// <param name="columnId">Column ID</param>
        /// <returns></returns>
        public static string ToString(this SQLiteDataReader @this, int columnId)
        {
            if (@this.FieldCount > columnId - 1)
            {
                return string.Empty;
            }
            else
            {
                return @this.IsDBNull(columnId) ? string.Empty : @this.GetString(columnId);
            }
        }

        /// <summary>
        /// Returns field value as bool?
        /// </summary>
        /// <param name="value">SqlDataReader</param>
        /// <param name="columnId">Column ID</param>
        /// <returns></returns>
        public static bool? ToNullableBool(this SQLiteDataReader @this, int columnId)
        {
            if (@this.FieldCount > columnId - 1)
            {
                throw new InvalidOperationException("No column at this position!");
            }

            try
            {
                return (bool?)@this.GetValue(columnId);
            }
            catch
            {
                throw new InvalidCastException("Value cannot be converted to a bool? value");
            }
        }

        public static List<string> GetColumns(this SQLiteDataReader  @this)
        {
            var result = @this.GetSchemaTable()
                 .Rows
                 .OfType<DataRow>()
                 .Select(row => row["ColumnName"].ToString()).ToList();

            return result;
        }

        /// <summary>
        /// Returns the index of a column by name or -1.
        /// </summary>
        /// <param name="this">The data record.</param>
        /// <param name="name">The field name (case insensitive).</param>
        /// <returns>The index of a column by name, or -1.</returns>
        public static int IndexOf(this SQLiteDataReader @this, string name)
        {
            if(string.IsNullOrEmpty(name) == true)
            {
                return -1;
            }

            for (int i = 0; i < @this.FieldCount; i++)
            {
                if (String.Compare(@this.GetName(i), name, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
