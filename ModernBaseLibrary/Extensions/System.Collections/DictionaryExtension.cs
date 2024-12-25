/*
 * <copyright file="DictionaryExtension.cs" company="Lifeprojects.de">
 *     Class: DictionaryExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class für Dictionary<TKey, TValue>
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

/*
 * https://stackoverflow.com/questions/294138/merging-dictionaries-in-c-sharp
 */

namespace ModernBaseLibrary.Extension
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Windows.Input;

    [SupportedOSPlatform("windows")]
    public static class DictionaryExtension
    {
        /// <summary>
        /// Prüft ob das Dictionary null oder leer (< 0) ist
        /// </summary>
        /// <typeparam name="TKey">Key</typeparam>
        /// <typeparam name="TValue">Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <returns>True wenn das Dictionary Null oder leer ist, False wenn in dem Dictionary Elemente vorhanden sind</returns>
        public static bool IsNullOrEmpty<TKey, TValue>(this Dictionary<TKey, TValue> @this)
        {
            return (@this == null || @this.Count < 1);
        }

        /// <summary>
        /// Fügt einen Dictionary ein neuen Key und Value hinzu wenn der Key noch nicht vorhanden ist
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="key">Wert für Key</param>
        /// <param name="value">Wert für Value</param>
        public static void AddIfNotExists<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key, TValue value)
        {
            CheckDictionaryIsNull(@this);

            if (@this.ContainsKey(key) == false)
            {
                @this.Add(key, value);
            }
        }

        /// <summary>
        /// Einem Dictionary wird ein Element hinzugefügt, oder wenn der Key bereits vorhanden, der Value aktualisiert.
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="key">Wert für Key</param>
        /// <param name="value">Wert für Value</param>
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key, TValue value)
        {
            if (@this.ContainsKey(key) == false)
            {
                @this.Add(key, value);
            }
            else
            {
                @this[key] = value;
            }
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
        public static void AddRange<TKey, TValue, TSource>(this Dictionary<TKey, TValue> @this, IEnumerable<TSource> source, Func<TSource, TKey> key, Func<TSource, TValue> value, bool set = true)
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
        public static void AddRange<TKey, TValue, TSource>(this Dictionary<TKey, TValue> @this, List<TSource> source, Func<TSource, TKey> key, Func<TSource, TValue> value, bool set = true)
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
        /// Fügt einen neuen Eintrag einem Dictionary hinzu oder gibt einen Value zurück.
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="key">Wert für Key</param>
        /// <param name="value">Wert für Value</param>
        /// <returns>Gibt den Value eines vorhandenen Key zurück oder den Value des neu erstellten Eintrags</returns>
        public static TValue AddOrGet<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key, TValue value)
        {
            @this.IsArgumentNull("Parameter source can not be null.");

            if (@this.ContainsKey(key) == true)
            {
                return @this[key];
            }
            else
            {
                @this.Add(key, value);
                return value;
            }
        }

        /// <summary>
        /// Fügt einen neuen Eintrag einem Dictionary hinzu oder gibt einen Value zurück.
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="key">Wert für Key</param>
        /// <param name="valueCreator"></param>
        /// <returns>Gibt den Value eines vorhandenen Key zurück oder den Value des neu erstellten Eintrags</returns>
        public static TValue AddOrGet<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key, Func<TValue> valueCreator)
        {
            @this.IsArgumentNull("Parameter source can not be null.");

            TValue value;
            if (@this.TryGetValue(key, out value) == false)
            {
                value = valueCreator();
                @this.Add(key, value);
            }

            return value;
        }

        /// <summary>
        /// Fügt einen neuen Eintrag einem Dictionary hinzu oder gibt einen Value zurück.
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="key">Wert für Key</param>
        /// <returns>Gibt den Value eines vorhandenen Key zurück oder den Value des neu erstellten Eintrags</returns>
        public static TValue AddOrGet<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key) where TValue : new()
        {
            @this.IsArgumentNull("Parameter source can not be null.");

            return @this.AddOrGet(key, () => new TValue());
        }

        /// <summary>
        /// Gib den Value eines Key zurück
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="key">Wert für Key</param>
        /// <returns>Value dem der übergebene Key zugeordnet ist</returns>
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key)
        {
            @this.IsArgumentNull("Parameter source can not be null.");

            if (@this.ContainsKey(key))
            {
                return (TValue)@this[key];
            }

            return default(TValue);
        }

        /// <summary>
        /// Gib den Value eines Key zurück
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="key">Wert für Key</param>
        /// <returns>Value dem der übergebene Key zugeordnet ist</returns>
        public static TValue TryGet<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key)
        {
            @this.IsArgumentNull("Parameter source can not be null.");

            TValue result;
            if (@this.TryGetValue(key, out result) == true && result is TValue)
            {
                return result;
            }

            return default(TValue);
        }

        /// <summary>
        /// Returns a value that corresponds to the given key or default if the key doesn't exist.
        /// </summary>
        public static TValue GetValueOrDefault<TKey, TValue>([NotNull] this Dictionary<TKey, TValue> @this, [NotNull] TKey key)
        {
            return @this.TryGetValue(key, out var result) ? result : default;
        }

        /// <summary>Retrieves the value for the given key. If there is no entry <code>default (TValue)</code> is returned.</summary>
        [return: MaybeNull]
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TKey : notnull
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            return default;
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TKey : notnull where TValue : new()
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return value;
            }

            value = new TValue();
            dictionary.Add(key, value);

            return value;
        }

        /// <summary>
        /// Löscht in einem Dictionary den Eintrag, wenn der Key vorhanden ist.
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="key">Key</param>
        public static void DeleteIfExistsKey<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key)
        {
            CheckDictionaryIsNull(@this);

            if (@this.ContainsKey(key))
            {
                @this.Remove(key);
            }
        }

        /// <summary>
        /// Ändert in einem Dictionary den Wert zu einem Key
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="key">Wert für Key</param>
        /// <param name="value">Wert für Value</param>
        public static void Update<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key, TValue value)
        {
            CheckDictionaryIsNull(@this);
            CheckKeyValuePairIsNull(key, value);

            if (@this.ContainsKey(key))
            {
                @this[key] = value;
            }
        }

        /// <summary>
        /// Ändert in einem Dictionary den KeyValuePair zu einem Key
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="pair">KeyValuePair</param>
        public static void Update<TKey, TValue>(this Dictionary<TKey, TValue> @this, KeyValuePair<TKey, TValue> pair)
        {
            CheckDictionaryIsNull(@this);
            CheckKeyValuePairIsNull(pair);

            if (@this.ContainsKey(pair.Key))
            {
                @this[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Löscht einen Eintrag wenn der Value vorhanden ist
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="value">Wert für Value</param>
        public static void DeleteIfExistsValue<TKey, TValue>(this Dictionary<TKey, TValue> @this, TValue value)
        {
            CheckDictionaryIsNull(@this);
            if (@this.ContainsValue(value) == true)
            {
                var key = @this.GetKeyFromValue(value);

                @this.Remove(key);
            }
        }

        /// <summary>
        /// Prüft ob in dem Dictionary ob leere Werte vorhanden sind.
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <returns>True wenn in dem Dictionary Values vorhanden sind, False wenn keine Values vorhanden sind.</returns>
        public static bool AreValuesEmpty<TKey, TValue>(this Dictionary<TKey, TValue> @this)
        {
            CheckDictionaryIsNull(@this);
            return @this.All(x => x.Value == null);
        }

        /// <summary>
        /// Prüft ob in dem Dictionary ob leere Keys vorhanden sind.
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <returns>True wenn in dem Dictionary Keys vorhanden sind, False wenn keine Keys vorhanden sind.</returns>
        public static bool AreKeysEmpty<TKey, TValue>(this Dictionary<TKey, TValue> @this)
        {
            CheckDictionaryIsNull(@this);
            return @this.All(x => x.Key == null);
        }

        /// <summary>
        /// Gibt den Inhalt des Dictionary als String zurück
        /// </summary>
        /// <typeparam name="TKey">Typ des Key</typeparam>
        /// <typeparam name="TValue">Typ des Value</typeparam>
        /// <param name="this">Dictionary</param>
        /// <param name="keyValueSeparator">Trennzeichen für Key/Value</param>
        /// <param name="sequenceSeparator">Trennzeichen pro Eintrag</param>
        /// <returns></returns>
        public static string ToString<TKey, TValue>(this Dictionary<TKey, TValue> @this, string keyValueSeparator, string sequenceSeparator)
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
        public static string ToString<TKey, TValue>(this Dictionary<TKey, TValue> @this, string sequenceSeparator)
        {
            @this.IsArgumentNull("Parameter source can not be null.");

            return ToString(@this,"=", sequenceSeparator);
        }

        /// <summary>
        /// Returns a read-only wrapper for the current dictionary.
        /// </summary>
        public static IReadOnlyDictionary<TKey, TValue> AsReadonly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) where TKey : notnull
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys) where TKey : notnull
        {
            keys.ForEach(key => dictionary.Remove(key));
        }

        /// <summary>
        /// Removes the first entry from the dictionary machting the given predicate.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool RemoveFirst<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate) where TKey : notnull
        {
            foreach (var entry in dictionary)
            {
                if (predicate(entry))
                {
                    dictionary.Remove(entry);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes all entries from the dictionary machting the given predicate.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="predicate"></param>
        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Predicate<KeyValuePair<TKey, TValue>> predicate) where TKey : notnull
        {
            var keysToRemove = new List<TKey>();
            foreach (var entry in dictionary)
            {
                if (predicate(entry))
                {
                    keysToRemove.Add(entry.Key);
                }
            }
            foreach (var key in keysToRemove)
            {
                dictionary.Remove(key);
            }
        }

        #region Private Methodes
        private static void CheckDictionaryIsNull<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
        }

        private static TKey GetKeyFromValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
        {
            var keys = new List<TKey>();
            foreach (var pair in dictionary)
            {
                AddToKeysList(keys, pair, value);
            }

            CheckCountGreaterZero(keys.Count, value);

            return !keys.Any() ? default(TKey) : keys.First();
        }

        private static void AddToKeysList<TKey, TValue>(List<TKey> keys, KeyValuePair<TKey, TValue> pair, TValue value)
        {
            if (pair.Value.Equals(value))
            {
                keys.Add(pair.Key);
            }
        }

        private static void CheckCountGreaterZero<TValue>(int count, TValue value)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count > 1)
            {
                throw new ArgumentException(nameof(value));
            }
        }

        private static void CheckKeyValuePairIsNull<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
        {
            if (pair.Key == null || pair.Value == null)
            {
                throw new ArgumentNullException(nameof(pair));
            }
        }

        private static void CheckKeyValuePairIsNull<TKey, TValue>(TKey key, TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            #endregion Private Methodes
        }
    }
}
