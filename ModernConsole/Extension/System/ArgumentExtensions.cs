/*
 * <copyright file="ArgumentExtensions.cs" company="Lifeprojects.de">
 *     Class: ArgumentExtensions
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class für Methoden Argument's
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

namespace ModernConsole.Extension
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;

    using ModernConsole.Exception;

    [SupportedOSPlatform("windows")]
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    internal static class ArgumentExtensions
    {
        /// <summary>
        /// Überprüft das übergebene Argument in einer Methode auf Null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="paramName"></param>
        public static TValue IsArgumentNull<TValue>(this TValue @this, string paramName) where TValue : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException(paramName);
            }
            else
            {
                return (TValue)@this;
            }
        }

        public static TValue IsArgumentNull<TValue>(this TValue @this, string paramName, TValue defaultValue) where TValue : class
        {
            if (@this == null)
            {
                return (TValue)defaultValue;
            }
            else
            {
                return (TValue)@this;
            }
        }

        public static Array IsArgumentNull(this Array @this, string paramName)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(paramName);
            }
            else
            {
                return @this;
            }
        }

        public static DateTime IsArgumentNull(this DateTime? @this, string paramName)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(paramName);
            }
            else
            {
                return (DateTime)@this;
            }
        }

        public static DateTime IsArgumentNull(this DateTime? @this, string paramName, DateTime? defaultValue = null)
        {
            if (@this == null)
            {
                if (defaultValue == null)
                {
                    return new DateTime(1900,1,1);
                }
                else
                {
                    return (DateTime)defaultValue;
                }
            }
            else
            {
                return (DateTime)@this;
            }
        }

        public static T IsArgumentNotNull<T>(this T o, string argName = null) where T : class
        {
            return o ?? throw new ArgumentNullException(argName);
        }

        public static void IsArgumentNullOrEmpty(this string @this, string paramName)
        {
            if (string.IsNullOrEmpty(@this.Trim()) == true)
            {
                throw new ArgumentException(paramName);
            }
        }

        public static string IsArgumentNullOrEmpty(this string @this, string paramName, string defaultValue)
        {
            if (string.IsNullOrEmpty(@this.Trim()) == true)
            {
                return defaultValue;
            }
            else
            {
                return @this;
            }
        }

        public static void IsArgumentNullOrWhiteSpace(this string @this, string paramName)
        {
            if (string.IsNullOrWhiteSpace(@this) == true)
            {
                throw new ArgumentException(paramName);
            }
        }

        public static string IsArgumentNullOrWhiteSpace(this string @this, string paramName, string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(@this) == true)
            {
                return defaultValue;
            }
            else
            {
                return @this;
            }
        }

        public static void IsArgumentOutOfRange<T>(this T @this, string paramName, T min, T max) where T : IComparable<T>
        {
            if (@this.CompareTo(min) < 0 || @this.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(paramName);
            }
        }

        public static void IsArgumentInRange<T>(this T @this, string paramName, T min, T max) where T : IComparable<T>
        {
            if (@this.CompareTo(min) >= 0 && @this.CompareTo(max) <= 0)
            {
            }
            else
            {
                throw new ArgumentOutOfRangeException(paramName);
            }
        }

        public static void IsArgumentOutOfRange<T>(this T @this, string paramName, T min, T max, string message) where T : IComparable<T>
        {
            if (@this.CompareTo(min) < 0 || @this.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(paramName, message);
            }
        }

        public static void IsArgumentInRange<T>(this T @this, string paramName, T min, T max, string message) where T : IComparable<T>
        {
            if (@this.CompareTo(min) >= 0 && @this.CompareTo(max) <= 0)
            {
            }
            else
            {
                throw new ArgumentOutOfRangeException(paramName, message);
            }
        }

        public static void IsArgumentOutOfRange<T>(this T @this, string paramName, bool conditon, string message) where T : IComparable<T>
        {
            if (conditon == false)
            {
                throw new ArgumentOutOfRangeException(paramName, message);
            }
        }

        public static void IsArgumentInRange<T>(this T @this, string paramName, bool conditon, string message) where T : IComparable<T>
        {
            if (conditon == true)
            {
                throw new ArgumentOutOfRangeException(paramName, message);
            }
        }

        public static void IsArgumentInEnum(this Enum @this, string paramName)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(paramName);
            }
            else if (Enum.IsDefined(@this.GetType(), @this) == false)
            {
                throw new ArgumentOutOfRangeException(paramName);
            }
        }

        public static Enum IsArgumentInEnum(this Enum @this, string paramName, Enum defaultValue = null)
        {
            if (@this == null)
            {
                if (defaultValue == null)
                {
                    return (Enum)defaultValue;
                }
                else
                {
                    return (Enum)defaultValue;
                }
            }
            if (Enum.IsDefined(@this.GetType(), @this) == false)
            {
                if (defaultValue == null)
                {
                    return (Enum)defaultValue;
                }
                else
                {
                    return (Enum)defaultValue;
                }
            }
            else
            {
                return (Enum)@this;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ICollection<T> IsArgumentNotReadOnly<T>(this ICollection<T> @this, string errorMessage = "")
        {
            var isValid = @this is not null && @this!.IsReadOnly is false;

            if (isValid is false)
            {
                throw new ReadOnlyCollectionException(nameof(@this));
            }

            return @this!;
        }

        /// <summary>
        /// Gibt einen Vorbedingungsvertrag für die einschließende Methode oder Eigenschaft
        /// an, und löst eine Ausnahme mit der angegebenen Meldung aus, wenn die Bedingung
        /// erfüllt wird.
        /// </summary>
        /// <param name="predicate">
        /// Der bedingte Ausdruck, der getestet werden soll.
        /// </param>
        /// <param name="message">
        /// Die Meldung, die angezeigt werden soll, wenn die Bedingung false lautet.
        /// </param>
        public static void IsArgumentEnsures<T>(this T @this, Func<bool> predicate, string message)
        {
            if (predicate() == true)
            {
                throw new ArgumentException(message);
            }
        }

        public static void ThrowIfNull<T>(this T @this) where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException();
            }
        }

        public static void ThrowIfNull<T>(this T @this, string parameterName) where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void ThrowIfNull<T>(this T argument, string parameterName, string message) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(parameterName, message);
            }
        }

        public static void ThrowIfNull<T>(this T argument, Expression<Func<T>> exprParameterName) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(GetName(exprParameterName));
            }
        }

        public static void ThrowIfNull<T>(this T @this, Expression<Func<T>> exprParameterName, string message) where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException(GetName(exprParameterName), message);
            }
        }

        public static bool TryCast<T>(this object @this, out T result)
        {
            result = default(T);
            if (@this is T)
            {
                result = (T)@this;
                return true;
            }

            if (@this != null)
            {
                try
                {
                    var converter = TypeDescriptor.GetConverter(typeof(T));
                    if (converter.CanConvertFrom(@this.GetType()))
                    {
                        result = (T)converter.ConvertFrom(@this);
                    }
                    else
                    {
                        return false;
                    }

                    return true;
                }
                catch (System.Exception)
                {
                    throw new InvalidCastException($"Incorrect type, cannot cast to type {typeof(T).FullName}");
                }
            }

            return !typeof(T).IsValueType;
        }

        private static string GetName<T>(Expression<Func<T>> expr)
        {
            var body = ((MemberExpression)expr.Body);
            return body.Member.Name;
        }
    }
}
