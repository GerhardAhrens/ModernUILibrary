﻿//-----------------------------------------------------------------------
// <copyright file="ContentCompare.cs" company="Lifeprojects.de">
//     Class: ContentCompare
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developerName@lifeprojects.de</email>
// <date>17.07.2024 09:17:54</date>
//
// <summary>
//  Klasse für ViewModel
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Comparer
{
    using System.Data;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.Json;

    using ModernBaseLibrary.Core.LINQ;

    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public class ContentCompare<TViewModel> where TViewModel : class
    {
        public static bool IsObjectEqual(TViewModel vm, DataRow row)
        {
            bool result = true;

            if (vm == null || row == null)
            {
                return false;
            }

            try
            {
                PropertyInfo[] properties = typeof(TViewModel).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(s => s.CanWrite == true).ToArray();

                foreach (DataColumn column in row.Table.Columns)
                {
                    string colName = column.ColumnName.ToUpper();

                    PropertyInfo columnName = properties.FirstOrDefault(f => f.Name.ToUpper() == column.ColumnName.ToUpper());
                    if (columnName != null)
                    {
                        if (row[colName] != DBNull.Value)
                        {
                            if (colName == "ID" && columnName.PropertyType.Name == typeof(ID).Name)
                            {
                                object colValue = row[colName];
                                ID propertyValue = (ID)typeof(TViewModel).GetProperty(columnName.Name).GetValue(vm);
                                if (AreEqual(propertyValue.Value, colValue) == false)
                                {
                                    result = false;
                                }
                            }
                            else
                            {
                                object colValue = row[colName];
                                object propertyValue = typeof(TViewModel).GetProperty(columnName.Name).GetValue(vm);
                                if (AreEqual(propertyValue, colValue) == false)
                                {
                                    result = false;
                                }
                            }
                        }
                        else
                        {
                            object propertyValue = typeof(TViewModel).GetProperty(columnName.Name).GetValue(vm);
                            if (propertyValue != null)
                            {
                                result = false;
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

        public static bool IsObjectEqual(TViewModel vm, DataRowView row)
        {
            bool result = true;

            if (vm == null || row == null)
            {
                return false;
            }

            try
            {
                PropertyInfo[] properties = typeof(TViewModel).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(s => s.CanWrite == true).ToArray();

                foreach (DataColumn column in row.Row.Table.Columns)
                {
                    string colName = column.ColumnName.ToUpper();

                    PropertyInfo columnName = properties.FirstOrDefault(f => f.Name.ToUpper() == column.ColumnName.ToUpper());
                    if (columnName != null)
                    {
                        if (row[colName] != DBNull.Value)
                        {
                            if (colName == "ID" && columnName.PropertyType.Name == typeof(ID).Name)
                            {
                                object colValue = row[colName];
                                ID propertyValue = (ID)typeof(TViewModel).GetProperty(columnName.Name).GetValue(vm);
                                if (AreEqual(propertyValue.Value, colValue) == false)
                                {
                                    result = false;
                                }
                            }
                            else
                            {
                                object colValue = row[colName];
                                object propertyValue = typeof(TViewModel).GetProperty(columnName.Name).GetValue(vm);
                                if (AreEqual(propertyValue, colValue) == false)
                                {
                                    result = false;
                                }
                            }
                        }
                        else
                        {
                            object propertyValue = typeof(TViewModel).GetProperty(columnName.Name).GetValue(vm);
                            if (propertyValue != null)
                            {
                                result = false;
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

        /// <summary>
        /// Die Methode vergleicht den Wert zwischen DataRow Column und ViewModel Property
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="vm">ViewModel</param>
        /// <param name="row">DataRow</param>
        /// <param name="expression">List from Properties</param>
        /// <returns>True = Value are equal</returns>
        public static bool IsFieldsEqual(TViewModel vm, DataRow row, params Expression<Func<TViewModel, object>>[] expression)
        {
            bool result = true;
            if (expression.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < expression.Length; i++)
            {
                string propertyName = ExpressionPropertyName.For<TViewModel>(expression[i]);
                if (string.IsNullOrEmpty(propertyName) == false)
                {
                    object propertyValue = vm.GetType().GetProperty(propertyName).GetValue(vm);
                    if (propertyValue != null)
                    {
                        if (row[propertyName] == DBNull.Value && propertyValue != null)
                        {
                            result = false;
                            break;
                        }
                        else
                        {
                            object dbvalue = row[propertyName];
                            if (AreEqual(propertyValue, dbvalue) == false)
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static bool IsEqualByIgnorFields(TViewModel vm, DataRow row, params Expression<Func<TViewModel, object>>[] expression)
        {
            bool result = true;

            if (vm == null || row == null)
            {
                return false;
            }

            try
            {
                PropertyInfo[] properties = typeof(TViewModel).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(s => s.CanWrite == true).ToArray();

                foreach (DataColumn column in row.Table.Columns)
                {
                    string colName = column.ColumnName.ToUpper();
                    if (SearchIgnorField(expression, colName) == true)
                    {
                        continue;
                    }

                    PropertyInfo columnName = properties.FirstOrDefault(f => f.Name.ToUpper() == column.ColumnName.ToUpper());
                    if (columnName != null)
                    {
                        if (row[colName] != DBNull.Value)
                        {
                            if (colName == "ID" && columnName.PropertyType.Name == typeof(ID).Name)
                            {
                                object colValue = row[colName];
                                ID propertyValue = (ID)typeof(TViewModel).GetProperty(columnName.Name).GetValue(vm);
                                if (AreEqual(propertyValue.Value, colValue) == false)
                                {
                                    result = false;
                                }
                            }
                            else
                            {
                                object colValue = row[colName];
                                object propertyValue = typeof(TViewModel).GetProperty(columnName.Name).GetValue(vm);
                                if (AreEqual(propertyValue, colValue) == false)
                                {
                                    result = false;
                                }
                            }
                        }
                        else
                        {
                            object propertyValue = typeof(TViewModel).GetProperty(columnName.Name).GetValue(vm);
                            if (propertyValue != null)
                            {
                                result = false;
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

        private static bool SearchIgnorField(Expression<Func<TViewModel, object>>[] expression, string colName)
        {
            for (int i = 0; i < expression.Length; i++)
            {
                string expressionProperty = ExpressionPropertyName.For<TViewModel>(expression[i]);
                if (expressionProperty.ToUpper() == colName.ToUpper())
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AreEqual<T>(T objProp, T objDataRow)
        {
            if (objProp is decimal)
            {
                decimal propVal1 = Convert.ToDecimal(objProp);
                decimal propVal2 = Convert.ToDecimal(objDataRow);

                if ((int)(((decimal)propVal1 % 1) * 100) == 0 && (int)(((decimal)propVal2 % 1) * 100) == 0)
                {
                    var obj1Serialized = JsonSerializer.Serialize((Convert.ToInt32(objProp)));
                    var obj2Serialized = JsonSerializer.Serialize(Convert.ToInt32(objDataRow));

                    return obj1Serialized == obj2Serialized;
                }
                else
                {
                    var obj1Serialized = JsonSerializer.Serialize(objProp);
                    var obj2Serialized = JsonSerializer.Serialize(objDataRow);

                    return obj1Serialized == obj2Serialized;
                }
            }
            else
            {
                var obj1Serialized = JsonSerializer.Serialize(objProp);
                var obj2Serialized = JsonSerializer.Serialize(objDataRow);

                return obj1Serialized == obj2Serialized;
            }
        }
    }
}
