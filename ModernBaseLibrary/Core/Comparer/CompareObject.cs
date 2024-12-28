//-----------------------------------------------------------------------
// <copyright file="CompareObject.cs" company="Lifeprojects.de">
//     Class: CompareObject
//     Copyright © Gerhard Ahrens, 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>18.1.2019</date>
//
// <summary>Class for ObjectComparer Result</summary>
// <example>
//  List<CompareResult> changes = CompareObject.CompareDifferences(p1, p2);
// </example>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Comparer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    //[DebuggerStepThrough]
    [Serializable]
    public static class CompareObject
    {
        public static List<CompareResult> GetDifferences<T>(T firstObj, T secondObj, params string[] ignoreProperties)
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
                        else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(HashSet<>))
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
                        else if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                        {
                            string key = property.PropertyType.GetGenericArguments()[0].Name;
                            string value = property.PropertyType.GetGenericArguments()[1].Name;
                            propertyType = $"Dictionary<{key},{value}>";
                        }
                        else if (property.PropertyType.BaseType == typeof(Enum))
                        {
                            propertyType = $"Enum.{property.PropertyType.Name}";
                        }
                        else
                        {
                            propertyType = property.PropertyType.Name;
                        }

                        if (ignoreList == null)
                        {
                            if (property.CanRead && property.GetGetMethod().GetParameters().Any() == false)
                            {
                                object firstValue = firstObj == null ? null : property.GetValue(firstObj, null);
                                object secondValue = secondObj == null ? null :  property.GetValue(secondObj, null);

                                if (object.Equals(firstValue, secondValue) == false)
                                {
                                    resultCompare.Add(new CompareResult(firstObj.GetType().Name, property.Name, propertyType, firstValue, secondValue));
                                }
                            }
                        }
                        else
                        {
                            if (property.CanRead && property.GetGetMethod().GetParameters().Any() == false && ignoreList.Contains(property.Name) == false)
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

        public static List<CompareResult> GetDifferences<T>(T firstObj, T secondObj)
        {
            return GetDifferences(firstObj, secondObj, null);
        }
    }
}
