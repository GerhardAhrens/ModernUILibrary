//-----------------------------------------------------------------------
// <copyright file="DataRowExtensions.cs" company="Lifeprojects.de">
//     Class: DataRowExtensions
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.09.2020</date>
//
// <summary>Extension Class für DataRow</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Dynamic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Text;

    [SupportedOSPlatform("windows")]
    public static class DataRowExtensions
    {
        public static TResult MapToSingle<TResult>(this DataRow @this) where TResult : new()
        {
            Type type = typeof(TResult);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            var entity = new TResult();

            try
            {
                foreach (PropertyInfo property in properties)
                {
                    if (@this.Table.Columns.Contains(property.Name))
                    {
                        if ((property != null) && property.CanWrite == true)
                        {
                            Type valueType = property.PropertyType;
                            property.SetValue(entity, @this[property.Name].To(valueType), null);
                        }
                    }
                }

                foreach (FieldInfo field in fields)
                {
                    if (@this.Table.Columns.Contains(field.Name))
                    {
                       Type valueType = field.FieldType;
                        field.SetValue(entity, @this[field.Name].To(valueType));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return entity;
        }

        /// <summary>
        /// Gibt eine Column von einem DataRow im gewünschten Typ zurück
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this">Alltuelle DataRow Zeile</param>
        /// <param name="fieldName">Column</param>
        /// <returns>Ergebnis zur angegebenen Column</returns>
        public static TResult GetAs<TResult>(this DataRow @this, string fieldName)
        {
            try
            {
                object getAs = null;
                if (typeof(TResult).Name == "Guid")
                {
                    getAs = @this[fieldName] == DBNull.Value ? Guid.Empty : new Guid(@this[fieldName].ToString());
                }
                else if (typeof(TResult).IsEnum == true)
                {
                    if (@this[fieldName].GetType() == typeof(int))
                    {
                        getAs = (TResult)@this[fieldName];
                    }
                    else if (@this[fieldName].GetType() == typeof(string))
                    {
                        getAs = (TResult)Enum.Parse(typeof(TResult), @this[fieldName].ToString(), true);
                    }
                    else
                    {
                        getAs = (TResult)Enum.Parse(typeof(TResult), @this[fieldName].ToString(), true);
                    }
                }
                else
                {
                    if (@this != null)
                    {
                        getAs = @this[fieldName] == DBNull.Value ? default(TResult) : (TResult)Convert.ChangeType(@this[fieldName], typeof(TResult), CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        return default(TResult);
                    }
                }

                return (TResult)getAs;
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                return default(TResult);
            }
        }

        /// <summary>
        /// Gibt eine Column von einem DataRow im gewünschten Typ zurück, mit der möglichkeit einen Default-Wert anzugeben
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this">Alltuelle DataRow Zeile</param>
        /// <param name="fieldName">Column</param>
        /// <param name="defaultValue">Default-Wert</param>
        /// <returns>Ergebnis zur angegebenen Column</returns>
        public static TResult GetAs<TResult>(this DataRow @this, string fieldName, TResult defaultValue)
        {
            try
            {
                object getAs = null;
                if (@this[fieldName] != DBNull.Value)
                {
                    if (typeof(TResult).Name == "Guid")
                    {
                        getAs = @this[fieldName] == DBNull.Value ? Guid.Empty : new Guid(@this[fieldName].ToString());
                    }
                    else if (typeof(TResult).IsEnum == true)
                    {
                        if (@this[fieldName].GetType() == typeof(int))
                        {
                            getAs = (TResult)@this[fieldName];
                        }
                        else if (@this[fieldName].GetType() == typeof(string))
                        {
                            getAs = (TResult)Enum.Parse(typeof(TResult), @this[fieldName].ToString(), true);
                        }
                        else
                        {
                            getAs = (TResult)Enum.Parse(typeof(TResult), @this[fieldName].ToString(), true);
                        }
                    }
                    else
                    {
                        getAs = @this[fieldName] == DBNull.Value ? default(TResult) : (TResult)Convert.ChangeType(@this[fieldName], typeof(TResult), CultureInfo.InvariantCulture);
                    }

                    return (TResult)getAs;
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                return default(TResult);
            }
        }

        public static Dictionary<string,Type> ColumnsToDictionary(this DataRow @this)
        {
            return @this.Table.Columns
                .Cast<DataColumn>()
                .AsEnumerable<DataColumn>()
                .ToDictionary<DataColumn, string, Type>(col => col.ColumnName, col => col.DataType);
        }

        public static bool HasColumn(this DataRow @this, string columnName)
        {
            bool result = false;

            int columnFound = @this.Table.Columns.OfType<DataColumn>().ToList().Count(c => c.ColumnName.ToLower() == columnName.ToLower());
            if (columnFound > 0)
            {
                result = true;
            }

            return result;
        }

        public static T Clone<T>(this DataRow @this, DataTable parentTable) where T : DataRow
        {
            T clonedRow = (T)parentTable.NewRow();
            clonedRow.ItemArray = @this.ItemArray;
            return clonedRow;
        }

        /// <summary>
        /// Converts a DataRow object into a Hashtable object, 
        /// where the key is the ColumnName and the value is the row value.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static Hashtable ToHashTable(this DataRow @this)
        {
            Hashtable result = new Hashtable(@this.ItemArray.Length);
            foreach (DataColumn dc in @this.Table.Columns)
            {
                result.Add(dc.ColumnName, @this[dc.ColumnName]);
            }

            return result;
        }

        public static string ItemArrayToString(this DataRow @this, char separator = ',')
        {
            return string.Join(separator, @this.ItemArray);
        }

        public static string ToString(this DataRow @this, char separator = ',')
        {
            return string.Join(separator, @this.ItemArray.Select(c => c.ToString()).ToArray()); 
        }

        public static string ToString(this DataRow @this, string columns, char separator = ',')
        {
            StringBuilder sb = new StringBuilder();

            string[] columnList = columns.Split(',');

            foreach (string column in columnList)
            {
                if (@this.HasColumn(column) == true)
                {
                    sb.Append(@this[column].ToString());
                    sb.Append(separator);
                }
            }

            sb.Remove(sb.ToString().Trim().Length - 1, 1);

            return sb.ToString();
        }

        public static bool Equals(this DataRow @this, DataRow secondDataRow)
        {
            bool result = false;

            if (@this.GetType() != typeof(DataRow) || secondDataRow.GetType() != typeof(DataRow))
            {
                return result;
            }

            if (@this.ItemArray.Length != secondDataRow.ItemArray.Length)
            {
                return result;
            }

            DataRowComparer<DataRow> drc = DataRowComparer.Default;
            result = drc.Equals(@this, secondDataRow);

            return result;
        }

        public static bool ToCompareContent(this DataRow @this, DataRow secondDataRow)
        {
            bool result = false;

            if (@this.GetType() != typeof(DataRow) || secondDataRow.GetType() != typeof(DataRow))
            {
                return result;
            }

            if (@this.ItemArray.Length != secondDataRow.ItemArray.Length)
            {
                return result;
            }


            result = true;
            for (int i = 0; i < @this.ItemArray.Length; i++)
            {
                if (@this.ItemArray[i].Equals(secondDataRow.ItemArray[i]) == false)
                {
                    result = false;
                }
            }

            return result;
        }

        public static T ToObject<T>(this DataRow dataRow) where T : new()
        {
            T item = new T();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                PropertyInfo property = GetProperty(typeof(T), column.ColumnName);

                if (property != null && dataRow[column] != DBNull.Value && dataRow[column].ToString() != "NULL")
                {
                    property.SetValue(item, ChangeType(dataRow[column], property.PropertyType), null);
                }
            }

            return item;
        }

        public static dynamic ToDynamicObject(this DataRow dataRow)
        {
            dynamic item = new ExpandoObject();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                var dic = (IDictionary<string, object>)item;
                dic[column.ColumnName] = dataRow[column];

            }

            return item;
        }

        private static PropertyInfo GetProperty(Type type, string attributeName)
        {
            PropertyInfo property = type.GetProperty(attributeName);

            if (property != null)
            {
                return property;
            }

            return type.GetProperties()
                 .Where(p => p.IsDefined(typeof(DisplayAttribute), false) && p.GetCustomAttributes(typeof(DisplayAttribute), false)
                 .Cast<DisplayAttribute>()
                 .Single().Name == attributeName)
                 .FirstOrDefault();
        }

        public static object ChangeType(object value, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                return Convert.ChangeType(value, Nullable.GetUnderlyingType(type));
            }

            return Convert.ChangeType(value, type);
        }

        /// <summary>
        /// Determines whether the record value is DBNull.Value
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>
        /// 	<c>true</c> if the value is DBNull.Value; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDBNull(this DataRow @this, string columnName)
        {
            bool columnFound = @this.Table.Columns.OfType<DataColumn>().ToList().Any(c => c.ColumnName == columnName);
            if (columnFound == true)
            {
                var value = @this[columnName];
                return (value == DBNull.Value);
            }
            else
            {
                return false;
            }
        }
    }
}