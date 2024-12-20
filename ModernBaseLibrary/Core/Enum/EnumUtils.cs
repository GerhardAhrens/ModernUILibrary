/*
 * <copyright file="EnumUtils.cs" company="Lifeprojects.de">
 *     Class: EnumUtils
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>27.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Die Klasse beinhaltet eine Sammlung von Methoden zur Behaldlung und Bearbeitung von Enums
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using ModernBaseLibrary.Extension;

    public static class EnumUtils
    {
        private static readonly Random Rng = new Random();

        /// <summary>
        ///     Checks if the given type <typeparamref name="T" /> is an enum type.
        /// </summary>
        /// <typeparam name="T">Generic param type.</typeparam>
        /// <returns>True if given type is an enum, otherwise, false.</returns>
        public static bool IsEnum<T>()
        {
            return GetUnderlyingType<T>().IsEnum();
        }

        /// <summary>
        ///     Checks if the given parameter <paramref name="value" /> is an enum type.
        /// </summary>
        /// <returns>True if given type is an enum, otherwise, false.</returns>
        /// <param name="value"></param>
        /// <exception cref="T:System.ArgumentNullException">If <paramref name="value" /> is null.</exception>
        public static bool IsEnum(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Type typ = GetUnderlyingType(value.GetType());

            return typ.IsEnum;
        }

        /// <summary>
        /// Counts the number of enums values contained in a given enum type.
        /// </summary>
        /// <typeparam name="TEnum">Generic enum type.</typeparam>
        /// <returns>The number of enum values.</returns>
        public static int Count<TEnum>()
        {
            return GetValues(typeof(TEnum)).Length;
        }

        public static IEnumerable<TEnum> GetValues<TEnum>()
        {
            var enumType = GetUnderlyingType(typeof(TEnum));

            return GetValues(enumType).Cast<TEnum>().ToList();
        }

        public static Array GetValues(Type enumType)
        {
            ThrowIfEnum(enumType);

            return Enum.GetValues(enumType);
        }

        public static object GetValue(Type enumType, int index)
        {
            return Enumerable.ToList(GetValues(enumType).Cast<object>())[index];
        }

        public static string GetName<TEnum>(TEnum value)
        {
            var enumType = GetUnderlyingType(typeof(TEnum));
            ThrowIfEnum(enumType);

            return Enum.GetName(enumType, value);
        }

        public static IEnumerable<string> GetNames<TEnum>()
        {
            var enumType = GetUnderlyingType(typeof(TEnum));
            ThrowIfEnum(enumType);

            return Enum.GetNames(enumType);
        }

        public static TEnum Parse<TEnum>(string value) where TEnum : struct
        {
            var enumType = GetUnderlyingType(typeof(TEnum));
            ThrowIfEnum(enumType);

            return (TEnum)Enum.Parse(enumType, value, true);
        }

        public static TEnum TryParse<TEnum>(string value, bool ignoreCase = true) where TEnum : struct
        {
            var enumType = GetUnderlyingType(typeof(TEnum));
            ThrowIfEnum(typeof(TEnum));

            TEnum returnValue;
            Enum.TryParse(value, ignoreCase, out returnValue);

            return returnValue;
        }

        /// <summary>
        ///     Safely cast an integer to the underlying enum value.
        /// </summary>
        /// <typeparam name="TEnum">Generic param type.</typeparam>
        /// <param name="value">The integer value for the desired enum.</param>
        /// <param name="defaultValue">The value returned if the integer does not map to the enum.</param>
        /// <returns>The enum value which maps to the given integer value.</returns>
        public static TEnum Cast<TEnum>(int value, TEnum defaultValue = default(TEnum)) where TEnum : struct
        {
            var enumType = GetUnderlyingType(typeof(TEnum));
            ThrowIfEnum(enumType);

            if (Enum.IsDefined(typeof(TEnum), value))
            {
                return (TEnum)Enum.ToObject(typeof(TEnum), value);
            }

            return defaultValue;
        }

        public static Type GetUnderlyingType(Type nullableType)
        {
            var underlyingType = Nullable.GetUnderlyingType(nullableType);
            if (underlyingType != null)
            {
                return underlyingType;
            }

            return nullableType;
        }

        public static Type GetUnderlyingType<T>()
        {
            return GetUnderlyingType(typeof(T));
        }

        private static void ThrowIfEnum(Type type)
        {
            if (type.IsEnum == false)
            {
                throw new ArgumentException($"Type {type.Name} must be an enum.");
            }
        }

        public static TEnum GetRandom<TEnum>()
        {
            var values = Enum.GetValues(typeof(TEnum));
            var item = Rng.Next(0, values.Length);
            return (TEnum)values.GetValue(item);
        }

        public static TEnum GetRandom<TEnum>(params TEnum[] excluded)
        {
            return GetValues<TEnum>()
                .Where(v => !excluded.Contains(v))
                .OrderBy(e => Guid.NewGuid())
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns all enums <typeparam name="T"/> with their descriptions in a dictionary.
        /// <see cref="DescriptionAttribute"/> needs to be applied on enum values to set a description text.
        /// </summary>
        /// <typeparam name="T">Generic enum type.</typeparam>
        /// <returns>Dictionary of enum-to-description mappings.</returns>
        public static IDictionary<T, string> GetDescriptions<T>()
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException($"Given type '{type.Name}' is not an enum.");
            }

            var dictionary = new Dictionary<T, string>();

            foreach (var key in GetValues<T>())
            {
                var field = type.GetField($"{key}");
                string description = null;
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    description = attribute.Description;
                }

                dictionary.Add(key, description);
            }

            return dictionary;
        }

        /// <summary>
        /// Die Methode konvertiert ein Enum in eine Liste vom Typ Enum/>
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <returns>Liste vom Typ Enum</returns>
        public static List<T> EnumToList<T>()
        {
            Type enumType = typeof(T);

            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
            {
                throw new ArgumentException("T must be of type System.Enum");
            }

            Array enumValArray = Enum.GetValues(enumType);

            List<T> enumValList = new List<T>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                enumValList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValList;
        }
    }
}
