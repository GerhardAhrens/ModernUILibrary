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
// <summary>Class of EnumBase Base Implemation</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;


    public abstract class EnumExtended : IComparable, IEnumExtended
    {
        protected EnumExtended(int value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public string Name { get; }

        public int Value { get; }

        public override string ToString() => this.Name;

        public override bool Equals(object obj)
        {
            if (!(obj is EnumExtended otherValue))
            {
                return false;
            }

            bool typeMatches = GetType() == obj.GetType();
            bool valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static IEnumerable<T> GetAll<T>() where T : EnumExtended
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public static int AbsoluteDifference(EnumExtended firstValue, EnumExtended secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }

        public static bool TryGetFromValueOrName<T>(string valueOrName, out T enumeration) where T : EnumExtended
        {
            bool result = TryParse(item => item.Name == valueOrName, out enumeration) 
                || int.TryParse(valueOrName, out var value) 
                && TryParse(item => item.Value == value, out enumeration);

            return result;
        }

        public static T FromValue<T>(int value) where T : EnumExtended
        {
            var matchingItem = Parse<T, int>(value, "nameOrValue", item => item.Value == value);
            return matchingItem;
        }

        public static T FromName<T>(string name) where T : EnumExtended
        {
            var matchingItem = Parse<T, string>(name, "name", item => item.Name == name);
            return matchingItem;
        }

        private static bool TryParse<TEnumeration>(Func<TEnumeration, bool> predicate, out TEnumeration enumeration) where TEnumeration : EnumExtended
        {
            enumeration = GetAll<TEnumeration>().FirstOrDefault(predicate);

            return enumeration != null;
        }

        private static TEnumeration Parse<TEnumeration, TIntOrString>(TIntOrString nameOrValue, string description, Func<TEnumeration, bool> predicate) where TEnumeration : EnumExtended
        {
            var matchingItem = GetAll<TEnumeration>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                throw new InvalidOperationException($"'{nameOrValue}' is not a valid {description} in {typeof(TEnumeration)}");
            }

            return matchingItem;
        }

        public int CompareTo(object other) => Value.CompareTo(((EnumExtended)other).Value);
    }

    public interface IEnumExtended
    {
        /* Empty Interface */
    }
}
