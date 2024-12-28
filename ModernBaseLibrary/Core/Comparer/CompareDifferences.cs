//-----------------------------------------------------------------------
// <copyright file="CompareDifferences.cs" company="Lifeprojects.de">
//     Class: CompareDifferences
//     Copyright © Gerhard Ahrens, 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>18.1.2019</date>
//
// <summary>Class for ObjectComparer Result</summary>
// <example>
// var resultChanges = CompareDifferences.ToDataTable(p1, p2);
// </example>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Diagnostics;
    using System.Reflection;

    [DebuggerStepThrough]
    [Serializable]
    public static class CompareDifferences
    {
        private static readonly DataTable resultCompare = null;

        static CompareDifferences()
        {
            resultCompare = new DataTable();
            resultCompare.Columns.Add("ObjectId", typeof(object));
            resultCompare.Columns.Add("ObjectName", typeof(string));
            resultCompare.Columns.Add("Description", typeof(string));
            resultCompare.Columns.Add("PropertyName", typeof(string));
            resultCompare.Columns.Add("PropertyTyp", typeof(string));
            resultCompare.Columns.Add("CurrentValue", typeof(object));
            resultCompare.Columns.Add("OldValue", typeof(object));
            resultCompare.Columns.Add("FullName", typeof(string));
        }

        public static DataTable ToDataTable<T>(T firstObj, T secondObj,object objectId, string description, params string[] ignoreProperties)
        {
            List<string> ignoreList = null;

            if (firstObj == null && secondObj == null)
            {
                return resultCompare;
            }

            if (ignoreProperties != null)
            {
                ignoreList = new List<string>(ignoreProperties);
            }

            foreach (var member in typeof(T).GetMembers())
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = member as PropertyInfo;

                    if (property != null)
                    {
                        string propertyType = string.Empty;
                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertyType = $"Nullable<{property.PropertyType.GetGenericArguments()[0].Name}>";
                        }
                        else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            propertyType = $"List<{property.PropertyType.GetGenericArguments()[0].Name}>";
                        }
                        else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        {
                            propertyType = $"IEnumerable<{property.PropertyType.GetGenericArguments()[0].Name}>";
                        }
                        else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
                        {
                            propertyType = $"ObservableCollection<{property.PropertyType.GetGenericArguments()[0].Name}>";
                        }
                        else
                        {
                            propertyType = property.PropertyType.Name;
                        }

                        if (ignoreList == null)
                        {
                            if (property.CanRead && property.GetGetMethod().GetParameters().Length == 0)
                            {
                                object currentValue = firstObj == null ? null : property.GetValue(firstObj, null);
                                object oldValue = secondObj == null ? null : property.GetValue(secondObj, null);

                                if (object.Equals(currentValue, oldValue) == false)
                                {
                                    DataRow dr = resultCompare.NewRow();
                                    dr["ObjectId"] = objectId;
                                    dr["ObjectName"] = firstObj.GetType().Name;
                                    dr["Description"] = description;
                                    dr["PropertyName"] = property.Name;
                                    dr["PropertyTyp"] = propertyType;
                                    dr["CurrentValue"] = currentValue;
                                    dr["OldValue"] = oldValue;
                                    dr["FullName"] = $"{firstObj.GetType().Name}.({propertyType}){property.Name}; CurrentValue={currentValue}; OldValue={oldValue}";
                                    resultCompare.Rows.Add(dr);
                                }
                            }
                        }
                        else
                        {
                            if (property.CanRead && property.GetGetMethod().GetParameters().Length == 0 && ignoreList.Contains(property.Name) == false)
                            {
                                object currentValue = firstObj == null ? null : property.GetValue(firstObj, null);
                                object oldValue = secondObj == null ? null : property.GetValue(secondObj, null);

                                if (object.Equals(currentValue, oldValue) == false)
                                {
                                    DataRow dr = resultCompare.NewRow();
                                    dr["ObjectId"] = objectId;
                                    dr["ObjectName"] = firstObj.GetType().Name;
                                    dr["Description"] = description;
                                    dr["PropertyName"] = property.Name;
                                    dr["PropertyTyp"] = propertyType;
                                    dr["CurrentValue"] = currentValue;
                                    dr["OldValue"] = oldValue;
                                    dr["FullName"] = $"{firstObj.GetType().Name}.({propertyType}){property.Name}; CurrentValue={currentValue}; OldValue={oldValue}";
                                    resultCompare.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
            }

            return resultCompare;
        }

        public static DataTable ToDataTable<T>(T firstObj, T secondObj, object objectId, string description)
        {
            return ToDataTable(firstObj, secondObj, objectId, description, null);
        }

        public static DataTable ToDataTable<T>(T firstObj, T secondObj, object objectId)
        {
            return ToDataTable(firstObj, secondObj, objectId, string.Empty, null);
        }

        public static DataTable ToDataTable<T>(T firstObj, T secondObj)
        {
            return ToDataTable(firstObj, secondObj,string.Empty,string.Empty, null);
        }

        public static List<CompareResult> ToList<T>(T firstObj, T secondObj, object objectId, string description, params string[] ignoreProperties)
        {
            List<CompareResult> resultCompare = new List<CompareResult>();
            List<string> ignoreList = null;

            if (firstObj == null && secondObj == null)
            {
                return resultCompare;
            }

            if (ignoreProperties != null)
            {
                ignoreList = new List<string>(ignoreProperties);
            }

            foreach (var member in typeof(T).GetMembers())
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = member as PropertyInfo;
                    if (property != null)
                    {
                        string propertyType = string.Empty;
                        if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertyType = $"Nullable<{property.PropertyType.GetGenericArguments()[0].Name}>";
                        }
                        else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            propertyType = $"List<{property.PropertyType.GetGenericArguments()[0].Name}>";
                        }
                        else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        {
                            propertyType = $"IEnumerable<{property.PropertyType.GetGenericArguments()[0].Name}>";
                        }
                        else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
                        {
                            propertyType = $"ObservableCollection<{property.PropertyType.GetGenericArguments()[0].Name}>";
                        }
                        else
                        {
                            propertyType = property.PropertyType.Name;
                        }

                        if (ignoreList == null)
                        {
                            if (property.CanRead && property.GetGetMethod().GetParameters().Length == 0)
                            {
                                object firstValue = firstObj == null ? null : property.GetValue(firstObj, null);
                                object secondValue = secondObj == null ? null : property.GetValue(secondObj, null);

                                if (object.Equals(firstValue, secondValue) == false)
                                {
                                    resultCompare.Add(new CompareResult(firstObj.GetType().Name, property.Name, propertyType, firstValue, secondValue));
                                }
                            }
                        }
                        else
                        {
                            if (property.CanRead && property.GetGetMethod().GetParameters().Length == 0 && ignoreList.Contains(property.Name) == false)
                            {
                                object firstValue = property.GetValue(firstObj, null);
                                object secondValue = property.GetValue(secondObj, null);

                                if (object.Equals(firstValue, secondValue) == false)
                                {
                                    resultCompare.Add(new CompareResult(firstObj.GetType().Name, property.Name, propertyType, firstValue, secondValue));
                                }
                            }
                        }
                    }
                }
            }

            return resultCompare;
        }

        public static List<CompareResult> ToList<T>(T firstObj, T secondObj, object objectId, string description)
        {
            return ToList(firstObj, secondObj, objectId, description, null);
        }

        public static List<CompareResult> ToList<T>(T firstObj, T secondObj, object objectId)
        {
            return ToList(firstObj, secondObj, objectId, string.Empty, null);
        }

        public static List<CompareResult> ToList<T>(T firstObj, T secondObj)
        {
            return ToList(firstObj, secondObj, null);
        }
    }
}
