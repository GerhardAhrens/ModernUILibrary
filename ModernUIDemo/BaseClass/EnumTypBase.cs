//-----------------------------------------------------------------------
// <copyright file="EnumExtended.cs" company="Lifeprojects.de">
//     Class: EnumExtended
//     Copyright © Gerhard Ahrens, 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>16.03.2022</date>
//
// <summary>Class of EnumTypBase Base Implemation</summary>
//-----------------------------------------------------------------------

namespace ModernUIDemo.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;


    public abstract class EnumTypBase : IComparable, IEnumExtended
    {
        protected EnumTypBase(int key, string name, string description = null)
        {
            this.Key = key;
            this.Name = name;
            this.Description = description;  
        }

        public int Key { get; }

        public string Name { get; }

        public string Description { get; }

        public override string ToString() => this.Name;

        public override bool Equals(object obj)
        {
            if (!(obj is EnumTypBase otherValue))
            {
                return false;
            }

            bool typeMatches = GetType() == obj.GetType();
            bool valueMatches = Key.Equals(otherValue.Key);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Key.GetHashCode();

        public static IEnumerable<T> GetAll<T>() where T : EnumTypBase
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public static int GetDifference(EnumTypBase firstValue, EnumTypBase secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Key - secondValue.Key);
            return absoluteDifference;
        }

        public static bool TryGetFromValueOrName<T>(string valueOrName, out T enumeration) where T : EnumTypBase
        {
            bool result = TryParse(item => item.Name == valueOrName, out enumeration) 
                || int.TryParse(valueOrName, out var value) 
                && TryParse(item => item.Key == value, out enumeration);

            return result;
        }

        public static T FromValue<T>(int value) where T : EnumTypBase
        {
            var matchingItem = Parse<T, int>(value, "nameOrValue", item => item.Key == value);
            return matchingItem;
        }

        public static T FromName<T>(string name) where T : EnumTypBase
        {
            var matchingItem = Parse<T, string>(name, "name", item => item.Name == name);
            return matchingItem;
        }

        private static bool TryParse<TEnumeration>(Func<TEnumeration, bool> predicate, out TEnumeration enumeration) where TEnumeration : EnumTypBase
        {
            enumeration = GetAll<TEnumeration>().FirstOrDefault(predicate);

            return enumeration != null;
        }

        private static TEnumeration Parse<TEnumeration, TIntOrString>(TIntOrString nameOrValue, string description, Func<TEnumeration, bool> predicate) where TEnumeration : EnumTypBase
        {
            var matchingItem = GetAll<TEnumeration>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                throw new InvalidOperationException($"'{nameOrValue}' is not a valid {description} in {typeof(TEnumeration)}");
            }

            return matchingItem;
        }

        public int CompareTo(object other) => Key.CompareTo(((EnumTypBase)other).Key);
    }

    public interface IEnumExtended
    {
        /* Empty Interface */
    }
}
