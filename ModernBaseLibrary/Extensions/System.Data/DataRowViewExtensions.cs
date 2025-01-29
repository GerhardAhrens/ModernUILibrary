// <copyright file="DataRowViewExtensions.cs" company="Lifeprojects.de">
//     Class: DataRowViewExtensions
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.09.2020</date>
//
// <summary>Extension Class für DataRowView</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Runtime.Versioning;

    /// <summary>
    /// Extension methods for ADO.NET DataRowView (DataView / DataTable / DataSet)
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class DataRowViewExtensions
    {
        /// <summary>
        /// Gibt eine Column von einem DataRow im gewünschten Typ zurück
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this">Alltuelle DataRow Zeile</param>
        /// <param name="fieldName">Column</param>
        /// <returns>Ergebnis zur angegebenen Column</returns>
        public static TResult GetAs<TResult>(this DataRowView @this, string fieldName)
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
                    getAs = @this[fieldName] == DBNull.Value ? default(TResult) : (TResult)Convert.ChangeType(@this[fieldName], typeof(TResult), CultureInfo.InvariantCulture);
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
        public static TResult GetAs<TResult>(this DataRowView @this, string fieldName, TResult defaultValue)
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

        /// <summary>
        /// Gets the record value casted as byte array.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static byte[] GetBytes(this DataRowView @this, string field)
        {
            return (@this[field] as byte[]);
        }

        /// <summary>
        /// Gets the record value casted as string or null.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static string GetString(this DataRowView @this, string field)
        {
            return @this.GetString(field, null);
        }

        /// <summary>
        /// Gets the record value casted as string or the specified default value.
        /// </summary>
        /// <param name = "row">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static string GetString(this DataRowView @this, string field, string defaultValue)
        {
            var value = @this[field];
            return (value is string ? (string)value : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as Guid or Guid.Empty.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static Guid GetGuid(this DataRowView @this, string field)
        {
            var value = @this[field];
            return (value is Guid ? (Guid)value : Guid.Empty);
        }

        /// <summary>
        /// Gets the record value casted as DateTime or DateTime.MinValue.
        /// </summary>
        /// <param name = "@this">The data @this.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static DateTime GetDateTime(this DataRowView @this, string field)
        {
            return @this.GetDateTime(field, DateTime.MinValue);
        }

        /// <summary>
        /// Gets the record value casted as DateTime or the specified default value.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static DateTime GetDateTime(this DataRowView @this, string field, DateTime defaultValue)
        {
            var value = @this[field];
            return (value is DateTime ? (DateTime)value : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as DateTimeOffset (UTC) or DateTime.MinValue.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static DateTimeOffset GetDateTimeOffset(this DataRowView @this, string field)
        {
            return new DateTimeOffset(@this.GetDateTime(field), TimeSpan.Zero);
        }

        /// <summary>
        /// Gets the record value casted as DateTimeOffset (UTC) or the specified default value.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static DateTimeOffset GetDateTimeOffset(this DataRowView @this, string field, DateTimeOffset defaultValue)
        {
            var dt = @this.GetDateTime(field);
            return (dt != DateTime.MinValue ? new DateTimeOffset(dt, TimeSpan.Zero) : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as int or 0.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static int GetInt32(this DataRowView @this, string field)
        {
            return @this.GetInt32(field, 0);
        }

        /// <summary>
        /// Gets the record value casted as int or the specified default value.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static int GetInt32(this DataRowView @this, string field, int defaultValue)
        {
            var value = @this[field];
            return (value is int ? (int)value : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as long or 0.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static long GetInt64(this DataRowView @this, string field)
        {
            return @this.GetInt64(field, 0);
        }

        /// <summary>
        /// Gets the record value casted as long or the specified default value.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static long GetInt64(this DataRowView @this, string field, int defaultValue)
        {
            var value = @this[field];
            return (value is long ? (long)value : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as decimal or 0.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static decimal GetDecimal(this DataRowView @this, string field)
        {
            return @this.GetDecimal(field, 0);
        }

        /// <summary>
        /// 	Gets the record value casted as decimal or the specified default value.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static decimal GetDecimal(this DataRowView @this, string field, long defaultValue)
        {
            var value = @this[field];
            return (value is decimal ? (decimal)value : defaultValue);
        }

        /// <summary>
        /// 	Gets the record value casted as bool or false.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static bool GetBoolean(this DataRowView @this, string field)
        {
            return @this.GetBoolean(field, false);
        }

        /// <summary>
        /// 	Gets the record value casted as bool or the specified default value.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static bool GetBoolean(this DataRowView @this, string field, bool defaultValue)
        {
            var value = @this[field];
            return (value is bool ? (bool)value : defaultValue);
        }

        /// <summary>
        /// 	Gets the record value as Type class instance or null.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static Type GetType(this DataRowView @this, string field)
        {
            return @this.GetType(field, null);
        }

        /// <summary>
        /// Gets the record value as Type class instance or the specified default value.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static Type GetType(this DataRowView @this, string field, Type defaultValue)
        {
            var classType = @this.GetString(field);
            if (classType.IsNotEmpty())
            {
                var type = Type.GetType(classType);
                if (type != null)
                    return type;
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the record value as class instance from a type name or null.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static object GetTypeInstance(this DataRowView @this, string field)
        {
            return @this.GetTypeInstance(field, null);
        }

        /// <summary>
        /// Gets the record value as class instance from a type name or the specified default type.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static object GetTypeInstance(this DataRowView @this, string field, Type defaultValue)
        {
            var type = @this.GetType(field, defaultValue);
            return (type != null ? Activator.CreateInstance(type) : null);
        }

        /// <summary>
        /// Gets the record value as class instance from a type name or null.
        /// </summary>
        /// <typeparam name = "T">The type to be casted to</typeparam>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static T GetTypeInstance<T>(this DataRowView @this, string field) where T : class
        {
            return (@this.GetTypeInstance(field, null) as T);
        }

        /// <summary>
        /// Gets the record value as class instance from a type name or the specified default type.
        /// </summary>
        /// <typeparam name = "T">The type to be casted to</typeparam>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "type">The type.</param>
        /// <returns>The record value</returns>
        public static T GetTypeInstanceSafe<T>(this DataRowView @this, string field, Type type) where T : class
        {
            var instance = (@this.GetTypeInstance(field, null) as T);
            return (instance ?? Activator.CreateInstance(type) as T);
        }

        /// <summary>
        /// Gets the record value as class instance from a type name or an instance from the specified type.
        /// </summary>
        /// <typeparam name = "T">The type to be casted to</typeparam>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static T GetTypeInstanceSafe<T>(this DataRowView @this, string field) where T : class, new()
        {
            var instance = (@this.GetTypeInstance(field, null) as T);
            return (instance ?? new T());
        }

        /// <summary>
        /// Determines whether the record value is DBNull.Value
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>
        /// 	<c>true</c> if the value is DBNull.Value; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDBNull(this DataRowView @this, string field)
        {
            var value = @this[field];
            return (value == DBNull.Value);
        }

        #region AddRange
        public static void AddRange(this DataRowCollection rc, IEnumerable<object[]> tuples)
        {
            foreach (object[] data in tuples)
            {
                rc.Add(tuples);
            }
        }
        #endregion AddRange
    }
}
