//-----------------------------------------------------------------------
// <copyright file="EnumerationPartA.cs" company="Lifeprojects.de">
//     Class: Enumeration
//     Copyright © Gerhard Ahrens, 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.04.2023</date>
//
// <summary>
// Die Klasse stellt die basis für eine Enummeration zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public abstract partial class Enumeration
    {
        private static readonly Dictionary<Type, IEnumerable<Enumeration>> _allValuesCache = new Dictionary<Type, IEnumerable<Enumeration>>();

        public static TEnumeration Parse<TEnumeration>(string name) where TEnumeration : Enumeration
        {
            return Parse<TEnumeration>(name, false);
        }

        public static TEnumeration Parse<TEnumeration>(string name, bool ignoreCase) where TEnumeration : Enumeration
        {
            return ParseImpl<TEnumeration>(name, ignoreCase, true);
        }

        private static TEnumeration ParseImpl<TEnumeration>(string name, bool ignoreCase, bool throwEx) where TEnumeration : Enumeration
        {
            var value = GetValues<TEnumeration>().FirstOrDefault(entry => StringComparisonPredicate(entry.Name, name, ignoreCase));
            if (value == null && throwEx)
            {
                throw new InvalidOperationException($"Requested value {name} was not found.");
            }

            return value;
        }

        public static bool TryParse<TEnumeration>(string name, out TEnumeration value) where TEnumeration : Enumeration
        {
            return TryParse(name, false, out value);
        }

        public static bool TryParse<TEnumeration>(string name, bool ignoreCase, out TEnumeration value) where TEnumeration : Enumeration
        {
            value = ParseImpl<TEnumeration>(name, ignoreCase, false);
            return value != null;
        }

        public static string Format<TEnumeration>(TEnumeration value, string format) where TEnumeration : Enumeration
        {
            return value.ToString(format);
        }

        public static IEnumerable<string> GetNames<TEnumeration>() where TEnumeration : Enumeration
        {
            return GetValues<TEnumeration>().Select(e => e.Name);
        }

        public static IEnumerable<TEnumeration> GetValues<TEnumeration>() where TEnumeration : Enumeration
        {
            var enumerationType = typeof(TEnumeration);
            if (_allValuesCache.TryGetValue(enumerationType, out var value))
            {
                return value.Cast<TEnumeration>();
            }

            return AddValueToCache(enumerationType, enumerationType
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(enumerationType)).Cast<TEnumeration>());
        }

        private static IEnumerable<TEnumeration> AddValueToCache<TEnumeration>(Type key,
            IEnumerable<TEnumeration> value) where TEnumeration : Enumeration
        {
            _allValuesCache.Add(key, value);
            return value;
        }

        public static bool IsDefined<TEnumeration>(string name) where TEnumeration : Enumeration
        {
            return IsDefined<TEnumeration>(name, false);
        }

        public static bool IsDefined<TEnumeration>(string name, bool ignoreCase) where TEnumeration : Enumeration
        {
            return GetValues<TEnumeration>().Any(e => StringComparisonPredicate(e.Name, name, ignoreCase));
        }

        private static bool StringComparisonPredicate(string item1, string item2, bool ignoreCase)
        {
            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return string.Compare(item1, item2, comparison) == 0;
        }
    }
}
