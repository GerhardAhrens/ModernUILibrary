/*
 * <copyright file="IDictionaryExtension.cs" company="Lifeprojects.de">
 *     Class: IDictionaryExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class für IDictionary
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Versioning;

    using Microsoft.Extensions.ObjectPool;

    [SupportedOSPlatform("windows")]
    public static class IDictionaryExtension
    {
        private static readonly ObjectPool<System.Text.StringBuilder> _stringBuilderPool = new DefaultObjectPoolProvider().CreateStringBuilderPool();

        public static bool IsNullOrEmpty(this IDictionary @this)
        {
            return (@this == null || @this.Count < 1);
        }

        /// <summary>
        /// Fügt einem Dictionary eine Liste vom Typ IEnumerable<typeparamref name="TSource"></t> an.
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <typeparam name="TSource">Typ der Liste die angefügt werden soll</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="source">Liste die angefügt werden soll</param>
        /// <param name="key">Wert für Key</param>
        /// <param name="value">Wert für Value</param>
        /// <param name="set">True Values werden aktualisiert, False Key/Values werden hinzugefügt</param>
        public static void AddRange<TKey, TValue, TSource>(this IDictionary<TKey, TValue> @this, IEnumerable<TSource> source, Func<TSource, TKey> key, Func<TSource, TValue> value, bool set = true)
        {
            source.ForEach(i =>
            {
                var dKey = key(i);
                var dValue = value(i);
                if (set == true)
                {
                    @this[dKey] = dValue;
                }
                else
                {
                    if (@this.ContainsKey(key(i)) == false)
                    {
                        @this.Add(key(i), value(i));
                    }
                }
            });
        }

        /// <summary>
        /// Fügt einem Dictionary eine Liste vom Typ List<typeparamref name="TSource"></t> an.
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <typeparam name="TSource">Typ der Liste die angefügt werden soll</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="source">Liste die angefügt werden soll</param>
        /// <param name="key">Wert für Key</param>
        /// <param name="value">Wert für Value</param>
        /// <param name="set">True Values werden aktualisiert, False Key/Values werden hinzugefügt</param>
        public static void AddRange<TKey, TValue, TSource>(this IDictionary<TKey, TValue> @this, List<TSource> source, Func<TSource, TKey> key, Func<TSource, TValue> value, bool set = true)
        {
            source.ForEach(i =>
            {
                var dKey = key(i);
                var dValue = value(i);
                if (set == true)
                {
                    @this[dKey] = dValue;
                }
                else
                {
                    if (@this.ContainsKey(key(i)) == false)
                    {
                        @this.Add(key(i), value(i));
                    }
                }
            });
        }

        /// <summary>
        /// Gibt den Inhalt des Dictionary als String zurück
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="keyValueSeparator">Trennzeichen für Key/Value</param>
        /// <param name="sequenceSeparator">Trennzeichen pro Eintrag</param>
        /// <returns>String mit dem Inhalt eines Dictionary</returns>
        public static string ToString<TKey, TValue>(this IDictionary<TKey, TValue> @this, string keyValueSeparator, string sequenceSeparator)
        {
            @this.IsArgumentNull("Parameter source can not be null.");

            var pairs = @this.Select(x => $"{x.Key}{keyValueSeparator}{x.Value}");

            return string.Join(sequenceSeparator, pairs);
        }

        /// <summary>
        /// Gibt den Inhalt des Dictionary als String zurück
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <returns>String mit dem Inhalt eines Dictionary</returns>
        public static string ToString<TKey, TValue>(this IDictionary<TKey, TValue> @this, string sequenceSeparator)
        {
            @this.IsArgumentNull("Parameter source can not be null.");

            return ToString(@this, "=", sequenceSeparator);
        }

        /// <summary>
        /// Converts IDictionary to delimited string.
        /// </summary>
        /// <param name="@this">The @this.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>System.String.</returns>
        public static string ToDelimitedString([NotNull] this IDictionary @this, char delimiter = ',')
        {
            if (string.IsNullOrEmpty(delimiter.ToString()))
            {
                throw new NullReferenceException(nameof(delimiter));
            }

            if (@this.Count == 0)
            {
                return string.Empty;
            }

            var sb = _stringBuilderPool.Get();

            try
            {
                foreach (DictionaryEntry item in @this)
                {
                    if (sb.Length > 0)
                    {
                        _ = sb.Append(delimiter.ToString(CultureInfo.CurrentCulture));
                    }

                    _ = sb.Append($"{item.Key}: {item.Value}".ToString(CultureInfo.CurrentCulture));
                }

                return sb.ToString();
            }
            finally
            {
                _stringBuilderPool.Return(sb);
            }
        }

        /// <summary>
        /// Returns a value that corresponds to the given key or default if the key doesn't exist.
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> @this, [NotNull] TKey key)
        {
            return @this.TryGetValue(key, out var result) ? result : default;
        }
    }
}
