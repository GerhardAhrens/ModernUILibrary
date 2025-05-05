/*
 * <copyright file="EnumExtensions.cs" company="Lifeprojects.de">
 *     Class: EnumExtensions
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * EnumExtensions Definition
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

namespace ModernBaseLibrary.Extension
{
    using System.ComponentModel;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;
    using ModernBaseLibrary.Core;
    using System.Collections;
    using System;
    using static System.Net.Mime.MediaTypeNames;

    public static partial class EnumExtensions
    {
        /// <summary>
        ///   Return a list of item in Enumeration
        /// </summary>
        public static List<Enum> ToList(this Enum @this)
        {
            return
                @this.GetType()
                    .GetFields(BindingFlags.Static | BindingFlags.Public)
                    .Select(fieldInfo => (Enum)fieldInfo.GetValue(@this))
                    .ToList();
        }

        /// <summary>
        /// Die Methode gibt den nummerischen Wert eines Enum-Elementes als Int zurück
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static int ToInt<TEnum>(this TEnum @this) where TEnum : struct, Enum
        {
            if (typeof(TEnum).IsEnum == false)
            {
                throw new ArgumentException("TEnum must be an enumerated type");
            }

            return (int)(IConvertible)@this;
        }

        /// <summary>
        /// Die Methode gibt den nummerischen Wert eines Enum-Elementes als String zurück
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ToValueAsString<TEnum>(this TEnum @this) where TEnum : struct, Enum
        {
            if (typeof(TEnum).IsEnum == false)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return ((int)(IConvertible)@this).ToString();
        }

        public static string ToUpperString<TEnum>(this TEnum @this) where TEnum : struct, Enum
        {
            if (typeof(TEnum).IsEnum == false)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return @this.ToString().ToUpper();
        }

        public static string ToLowerString<TEnum>(this TEnum @this) where TEnum : struct, Enum
        {
            if (typeof(TEnum).IsEnum == false)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return @this.ToString().ToLower();
        }

        /// <summary>
        /// Die Methode gibt die Anzahl der Enum Elemente zurück
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static int Count<TEnum>(this TEnum @this) where TEnum : IConvertible
        {
            if (typeof(TEnum).IsEnum == false)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return Enum.GetNames(typeof(TEnum)).Length;
        }

        /// <summary>
        /// A T extension method to determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list contains the object, else false.</returns>
        public static bool In(this Enum @this, params Enum[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        /// <summary>
        /// A T extension method to determines whether the object is not equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list doesn't contains the object, else false.</returns>
        public static bool NotIn(this Enum @this, params Enum[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        // <summary>
        /// Checks if the given type <typeparamref name="T"/> is an enum type.
        /// </summary>
        /// <typeparam name="T">Generic param type.</typeparam>
        /// <returns>True if given type is an enum, otherwise, false.</returns>
        public static bool IsEnum<TEnum>(this TEnum @this) where TEnum : struct, Enum
        {
            return EnumUtils.IsEnum<TEnum>();
        }

        /// <summary>
        /// Checks if the given parameter <paramref name="value"/> is an enum type.
        /// </summary>
        /// <returns>True if given type is an enum, otherwise, false.</returns>
        /// <param name="value"></param>
        /// <exception cref="T:System.ArgumentNullException">If <paramref name="value"/> is null.</exception>
        public static bool IsEnum(this object @this)
        {
            if (@this == null)
            {
                return false;
            }

            return EnumUtils.IsEnum(@this);
        }

        public static TEnum FromEnumDescription<TEnum>(this string @this) where TEnum : struct, Enum
        {

            int count = typeof(TEnum).GetFields().Count(f => f.GetCustomAttributes<DescriptionAttribute>()
                                 .Any(a => a.Description.Equals(@this, StringComparison.OrdinalIgnoreCase)));
            if (count > 0)
            {
                return (TEnum)typeof(TEnum)
                    .GetFields()
                    .First(f => f.GetCustomAttributes<DescriptionAttribute>()
                    .Any(a => a.Description.Equals(@this, StringComparison.OrdinalIgnoreCase)))
                    .GetValue(null);
            }

            return default(TEnum);
        }

        public static TEnum FromEnumDescription<TEnum>(this string @this, TEnum defaultEnum) where TEnum : struct
        {

            int count = typeof(TEnum).GetFields().Count(f => f.GetCustomAttributes<DescriptionAttribute>()
                                 .Any(a => a.Description.Equals(@this, StringComparison.OrdinalIgnoreCase)));
            if (count > 0)
            {
                return (TEnum)typeof(TEnum)
                    .GetFields()
                    .First(f => f.GetCustomAttributes<DescriptionAttribute>()
                    .Any(a => a.Description.Equals(@this, StringComparison.OrdinalIgnoreCase)))
                    .GetValue(null);
            }

            return defaultEnum;
        }

        public static TEnum GetAttributeOfType<TEnum>(this Enum @this) where TEnum : Attribute
        {
            object[] attributes = null;

            try
            {
                var type = @this.GetType();
                var memInfo = type.GetMember(@this.ToString());
                attributes = memInfo[0].GetCustomAttributes(typeof(TEnum), false);
            }
            catch (Exception)
            {

                throw;
            }

            return (attributes.Length > 0) ? (TEnum)attributes[0] : null;
        }

        public static string ToReadableString(this Enum @this, StringSensitive stringSensitive = StringSensitive.Normal)
        {
            string str = @this.ToString();

            Type enumType = @this.GetType();
            FieldInfo currentValue = enumType.GetField(@this.ToString());

            object[] attributes = currentValue.GetCustomAttributes(typeof(ReadableStringAttribute), inherit: false) ?? new object[0];
            if (attributes.Length > 0)
            {
                str = ((ReadableStringAttribute)attributes[0]).ReadableString;
            }

            if (stringSensitive == StringSensitive.Normal)
            {
                return str;
            }
            else if (stringSensitive == StringSensitive.Lower)
            {
                return str.ToLower();
            }
            else if (stringSensitive == StringSensitive.Upper)
            {
                return str.ToUpper();
            }

            return str;
        }

        public static TEnum ToEnum<TEnum>(this int @this) where TEnum : struct
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), @this);
        }

        public static TEnum ToEnum<TEnum>(this string @this) where TEnum : struct
        {
            if (string.IsNullOrEmpty(@this) == true)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            TEnum enumsval;
            if (Enum.TryParse<TEnum>(@this.ToString(), out enumsval) == true)
            {
                return (TEnum)enumsval;
            }
            else
            {
                return default(TEnum);
            }
        }

        public static TEnum ToEnum<TEnum>(this char @this, bool toUpper = true) where TEnum : struct
        {
            if (@this == '\0' || @this == ' ')
            {
                throw new ArgumentNullException(nameof(@this));
            }

            string enumValue = string.Empty;
            if (toUpper == true)
            {
                enumValue = @this.ToString().ToUpper();
            }
            else
            {
                enumValue = @this.ToString();
            }

            TEnum outValue;
            if (Enum.TryParse<TEnum>(enumValue, out outValue) == true)
            {
                return (TEnum)outValue;
            }
            else
            {
                return default(TEnum);
            }
        }

        public static TEnum ToEnum<TEnum>(this string @this, TEnum defaultValue) where TEnum : struct
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (string.IsNullOrEmpty(@this))
            {
                return defaultValue;
            }

            TEnum result;
            return Enum.TryParse(@this, true, out result) ? result : defaultValue;
        }

        public static TEnum ToEnum<TEnum>(this char @this, TEnum defaultValue, bool toUpper = true) where TEnum : struct
        {
            if (@this == '\0' || @this == ' ')
            {
                return defaultValue;
            }

            string enumValue = string.Empty;
            if (toUpper == true)
            {
                enumValue = @this.ToString().ToUpper();
            }
            else
            {
                enumValue = @this.ToString();
            }

            TEnum result;
            return Enum.TryParse(enumValue, true, out result) ? result : defaultValue;
        }

        public static string ToFriendlyString(this Enum @this)
        {
            Type type = @this.GetType();
            if (type.IsEnum == false)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "source");
            }

            return Enum.GetName(@this.GetType(), @this);
        }

        public static TResult ToValue<TResult>(this Enum @this) where TResult : struct
        {
            TResult result = default(TResult);

            try
            {
                result = (TResult)Convert.ChangeType(@this, typeof(TResult));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static string ToDescription<TEnum>(this TEnum @this) where TEnum : struct
        {
            FieldInfo fieldInfo = @this.GetType().GetField(@this.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return @this.ToString();
            }
        }

        public static string ToDescription(this Enum @this)
        {
            FieldInfo fieldInfo = @this.GetType().GetField(@this.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return @this.ToString();
            }
        }

        public static bool TryConvertToEnum<TEnum>(this int @this, out TEnum result) where TEnum : struct
        {
            var enumType = typeof(TEnum);
            var success = Enum.IsDefined(enumType, @this);
            if (success == true)
            {
                result = (TEnum)Enum.ToObject(enumType, @this);
            }
            else
            {
                result = default(TEnum);
            }

            return success;
        }

        /// <summary>
        /// Removes a flag and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="variable">Source enum</param>
        /// <param name="flag">Dumped flag</param>
        /// <returns>Result enum value</returns>
        /// <remarks>
        /// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
        /// </remarks>
        public static T ClearFlag<T>(this Enum variable, T flag)
        {
            return ClearFlags(variable, flag);
        }

        /// <summary>
        /// Removes flags and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="variable">Source enum</param>
        /// <param name="flags">Dumped flags</param>
        /// <returns>Result enum value</returns>
        /// <remarks>
        /// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
        /// </remarks>
        public static T ClearFlags<T>(this Enum variable, params T[] flags)
        {
            var result = Convert.ToUInt64(variable);
            foreach (T flag in flags)
            {
                result &= ~Convert.ToUInt64(flag);
            }

            return (T)Enum.Parse(variable.GetType(), result.ToString());
        }

        /// <summary>
        /// Includes a flag and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="variable">Source enum</param>
        /// <param name="flag">Established flag</param>
        /// <returns>Result enum value</returns>
        /// <remarks>
        /// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
        /// </remarks>
        public static T SetFlag<T>(this Enum variable, T flag)
        {
            return SetFlags(variable, flag);
        }

        /// <summary>
        /// Includes flags and returns the new value
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="variable">Source enum</param>
        /// <param name="flags">Established flags</param>
        /// <returns>Result enum value</returns>
        /// <remarks>
        /// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
        /// </remarks>
        public static T SetFlags<T>(this Enum variable, params T[] flags)
        {
            var result = Convert.ToUInt64(variable);
            foreach (T flag in flags)
            {
                result |= Convert.ToUInt64(flag);
            }

            return (T)Enum.Parse(variable.GetType(), result.ToString());
        }

        /// <summary>
        /// Check to see if enumeration has a specific flag set
        /// </summary>
        /// <param name="variable">Enumeration to check</param>
        /// <param name="flags">Flags to check for</param>
        /// <returns>Result of check</returns>
        /// <remarks>
        /// 	Contributed by nagits, http://about.me/AlekseyNagovitsyn
        /// </remarks>
        public static bool HasFlags<E>(this E variable, params E[] flags)
            where E : struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(E).IsEnum)
            {
                throw new ArgumentException("variable must be an Enum", "variable");
            }

            foreach (var flag in flags)
            {
                if (!Enum.IsDefined(typeof(E), flag))
                {
                    return false;
                }

                ulong numFlag = Convert.ToUInt64(flag);
                if ((Convert.ToUInt64(variable) & numFlag) != numFlag)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Konvertiert ein enum zu einem Dictionary<int,string>()
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static Dictionary<int, string> ToDictionary<TEnum>(this TEnum @this, bool setDescription = false) where TEnum : struct
        {
            if (typeof(TEnum).IsEnum == false)
            {
                throw new ArgumentException("Type must be an enumeration");
            }

            var type = typeof(TEnum);
            if (setDescription == true)
            {
                var dict = Enum.GetValues(type).Cast<int>().ToDictionary(e => e, e => Enum.GetName(type, e));

                foreach (TEnum enumItem in (TEnum[])Enum.GetValues(typeof(TEnum)))
                {
                    int index = Convert.ToInt32(enumItem);
                    dict[index] = enumItem.ToDescription();
                }

                return dict;
            }
            else
            {
                return Enum.GetValues(type).Cast<int>().ToDictionary(e => e, e => Enum.GetName(type, e));
            }
        }

        /// <summary>
        /// Gib ein Enum als Dictionary zurüxk
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IDictionary ToDictionary<TEnumValueType>(this Enum @this)
        {
            if (typeof(TEnumValueType).FullName != Enum.GetUnderlyingType(@this.GetType()).FullName)
            {
                throw new ArgumentException("Invalid type specified.");
            }

            return Enum.GetValues(@this.GetType()).Cast<object>().ToDictionary(key => Enum.GetName(@this.GetType(), key),value => (TEnumValueType)value);
        }

        public static int EnumToInt<TValue>(this TValue value) where TValue : struct, IConvertible
        {
            if (!typeof(TValue).IsEnum)
            {
                throw new ArgumentException(nameof(value));
            }

            return (int)(object)value;
        }
    }
}