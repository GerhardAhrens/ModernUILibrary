//-----------------------------------------------------------------------
// <copyright file="AssertMyExtensions.cs" company="Lifeprojects.de">
//     Class: AssertMyExtensions
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.09.2022 14:52:20</date>
//
// <summary>
// Extension Class for 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary
{
    using UnitTest = Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Linq;
    using System.Collections;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using global::ModernBaseLibrary.Core;

    public static partial class AssertExtension
    {
        public static void HaveCount<Ttype>(this Assert @this, object input, int equalCount)
        {
            bool result = false;
            string typeAsText = string.Empty;

            if (input == null)
            {
                return;
            }

            if (IsEnumerable<Ttype>(input) == true)
            {
                typeAsText = "IEnumerable";
                IEnumerable<Ttype> collection = (IEnumerable<Ttype>)input;
                if (collection.Count() == equalCount)
                {
                    result = true;
                }
            }
            else if (IsIDictionary(input) == true)
            {
                typeAsText = "IDictionary";
                int count = ((IDictionary)input).Values.Count;
                if (count == equalCount)
                {
                    result = true;
                }
            }
            else if (IsList(input) == true)
            {
                typeAsText = "List";
                int count = ((List<EnumSource>)input).Count;
                if (count == equalCount)
                {
                    result = true;
                }
            }
            else if (IsHashtable(input) == true)
            {
                typeAsText = "Hashtable";
                int count = ((Hashtable)input).Count;
                if (count == equalCount)
                {
                    result = true;
                }
            }
            else if (IsArray(input) == true)
            {
                typeAsText = "Array";
                if (((Array)input).Length > 0)
                {
                    result = true;
                }
            }

            if (result == true)
            {
                return;
            }

            throw new AssertFailedException($"{typeAsText}<{typeof(Ttype).Name}> != {equalCount}");
        }

        public static bool IsList(object testedObject)
        {
            Type t = testedObject.GetType();
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>);
        }

        public static bool IsEnumerable<T>(object testedObject)
        {
            try
            {
                return (testedObject is IEnumerable<T>);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsHashtable(object testedObject)
        {
            return (testedObject is Hashtable);
        }

        public static bool IsArray(object testedObject)
        {
            return (testedObject is Array);
        }

        private static bool IsIDictionary(object testedObject)
        {
            Type t = testedObject.GetType();
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        private static IEnumerable<Type> GetGenericIEnumerables(object o)
        {
            return o.GetType()
                    .GetInterfaces()
                    .Where(t => t.IsGenericType
                        && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    .Select(t => t.GetGenericArguments()[0]);
        }

        private static Type GetAnyElementType(Type type)
        {
            // Type is Array
            // short-circuit if you expect lots of arrays 
            if (type.IsArray)
                return type.GetElementType();

            // type is IEnumerable<T>;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];

            // type implements/extends IEnumerable<T>;
            var enumType = type.GetInterfaces()
                                    .Where(t => t.IsGenericType &&
                                           t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                                    .Select(t => t.GenericTypeArguments[0]).FirstOrDefault();
            return enumType ?? type;
        }
    }
}
