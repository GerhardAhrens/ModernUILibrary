/*
 * <copyright file="ListExtension.cs" company="Lifeprojects.de">
 *     Class: ListExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class für List<TContent>
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

namespace System.Collections
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Text;
    using System.Windows.Documents;
    using System.Windows.Forms.VisualStyles;

    [SupportedOSPlatform("windows")]
    public static class ListExtension
    {
        public static bool IsNullOrEmpty<TContent>(this List<TContent> @this)
        {
            return @this == null || @this.Any() == false;
        }

        public static bool IsNotNullOrEmpty<T>(this List<T> @this)
        {
            return @this != null && @this.Any() == true;
        }

        public static List<T> Concate<T>(this List<T> @this, List<T> second, bool withOutDuplicates = false)
        {
            if (withOutDuplicates == false)
            {
                return @this.Concat(second).ToList();
            }
            else
            {
                return @this.Union(second).ToList();
            }
        }

        public static object ConvertTo(this List<object> @this, Type targetValueType)
        {
            if (typeof(object).Equals(targetValueType))
            {
                return @this;
            }
            else
            {
                Type targetGenericListType = typeof(List<>).MakeGenericType(targetValueType);
                object genericList = Activator.CreateInstance(targetGenericListType);

                MethodInfo method = targetGenericListType.GetMethod("Add", new Type[] { targetValueType });
                foreach (object item in @this)
                {
                    if (targetValueType.IsAssignableFrom(item.GetType()))
                    {
                        method.Invoke(genericList, new object[] { item });
                    }
                    else
                    {
                        if (targetValueType == typeof(string))
                        {
                            method.Invoke(genericList, new object[] { item.ToString() });
                        }
                        else
                        {
                            throw new ArgumentException($"List entry ist of type {item.GetType().Name} but should be of type {targetValueType.Name}");
                        }
                    }
                }

                return genericList;
            }
        }

        public static T GetItem<T>(this List<T> @this, int index)
        {
            try
            {
                if (index < @this.Count)
                {
                    T item = @this[index];
                    return item;
                }
                else
                {
                    return default(T);
                }
            }
            catch
            {
                throw new IndexOutOfRangeException($"The index {index} is out of range.");
            }
        }

        public static T GetItemAndRemove<T>(this List<T> @this, int index)
        {
            try
            {
                if (index < @this.Count)
                {
                    T item = @this[index];
                    @this.Remove(item);
                    return item;
                }
                else
                {
                    return default(T);
                }
            }
            catch
            {
                throw new IndexOutOfRangeException($"The index {index} is out of range.");
            }
        }

        /// <summary>
        /// Entfernt ein exaktes 'value' Item aus der Liste unter berücksichtigung der Groß- und Kleinschreibung
        /// </summary>
        /// <param name="this">List of String</param>
        /// <param name="value">String der entfernt werden soll</param>
        /// <param name="compareType">StringComparison</param>
        /// <param name="removeAll">alle zutreffende Item entfernen, Default</param>
        public static void Remove(this List<string> @this, string value, StringComparison compareType, bool removeAll = true)
        {
            if (removeAll)
            {
                @this.RemoveAll(x => x.Equals(value, compareType));
            }
            else
            {
                @this.RemoveAt(@this.FindIndex(x => x.Equals(value, compareType)));
            }
        }

        /// <summary>
        /// Entfernt ein Item beginnend mit 'value' aus der Liste unter berücksichtigung der Groß- und Kleinschreibung
        /// </summary>
        /// <param name="this">List of String</param>
        /// <param name="value">Teil-String der entfernt werden soll</param>
        /// <param name="compareType">StringComparison</param>
        /// <param name="removeAll">alle zutreffende Item entfernen, Default</param>
        public static void RemoveStartsWith(this List<string> @this, string value, StringComparison compareType, bool removeAll = true)
        {
            if (removeAll)
            {
                @this.RemoveAll(x => x.StartsWith(value, compareType));
            }
            else
            {
                @this.RemoveAt(@this.FindIndex(x => x.StartsWith(value, compareType)));
            }
        }

        /// <summary>
        /// An List&lt;T&gt; extension method that removes the range.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="values">A variable-length parameters list containing values.</param>
        public static void RemoveRange<T>(this List<T> @this, List<T> values)
        {
            foreach (T value in values)
            {
                @this.Remove(value);
            }
        }

        public static void InsertMove<T>(this List<T> @this, int currentItem, T newItem)
        {
            @this.Insert(currentItem, newItem);
            @this[currentItem+1] = newItem;
        }

        public static T NextOf<T>(this List<T> @this, T startItem)
        {
            var indexOf = @this.IndexOf(startItem);
            if (indexOf > 0)
            {
                return @this[indexOf == @this.Count - 1 ? 0 : indexOf + 1];
            }
            else
            {
                return default;
            }
        }

        public static T PreviousOf<T>(this List<T> @this, T startItem)
        {
            int indexOf = @this.IndexOf(startItem);
            if (indexOf > 0)
            {
                T result = @this[indexOf - 1];
                return result;
            }
            else
            {
                return default;
            }
        }

        public static string ListToString(this List<string> @this)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string item in @this)
            {
                sb.AppendLine(item);
            }

            return sb.ToString();
        }

        public static string ToStringAll<TSource>(this List<TSource> @this, bool showIndex = false)
        {
            StringBuilder retVal;
            Func<TSource, int, string> formatFunction = null;
            if (showIndex == true)
            {
                formatFunction = (Source, Position) => $"{Position + 1}. {Source}";
            }
            else
            {
                formatFunction = (Source, Position) => $"{Source}";
            }

            Func<StringBuilder, string, StringBuilder> ConcatenateFunction = (Result, Value) => Result.AppendLine(Value);

            retVal = @this
                .Select((a, pos) => formatFunction(a, pos))
                .Aggregate(new StringBuilder(), (returnString, Value) => ConcatenateFunction(returnString, Value));

            return retVal.ToString().Trim();
        }
    }
}
