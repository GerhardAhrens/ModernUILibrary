/*
 * <copyright file="ArrayExtension.cs" company="Lifeprojects.de">
 *     Class: ArrayExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 08:40:08</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extension Class for Array Types
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

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.Versioning;
    using System.Security.Cryptography;
    using System.Text;

    [SupportedOSPlatform("windows")]
    public static partial class ArrayExtension
    {
        /// <summary>Zeigt an, ob das angegebene Array null ist oder eine Länge von Null hat.</summary>
        /// <param name="this">Ein Array</param>
        /// <returns>true, wenn der Array-Parameter null ist oder eine Länge von null hat; andernfalls false.</returns>
        public static bool IsNullOrEmpty(this Array @this)
        {
            return (@this == null || @this.Length == 0);
        }

        public static bool IsNullOrEmpty<T>(this T[] @this)
        {
            return @this == null || @this.Length == 0;
        }

        /// <summary>
        /// Die Methode gibt ein Array als IEnumerable of T zurück
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToEnumerable<T>(this Array @this)
        {
            foreach (var item in @this)
            {
                yield return (T)item;
            }
        }

        /// <summary>
        /// Die Methode konvertiert beinen ArrayTyp in einen anderen ArrayTyp
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T[] ConvertTo<T>(this Array @this)
        {
            T[] result = new T[@this.Length];
            System.ComponentModel.TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
            if (tc.CanConvertFrom(@this.GetValue(0).GetType()))
            {
                for (int i = 0; i < @this.Length; i++)
                {
                    result[i] = (T)tc.ConvertFrom(@this.GetValue(i));
                }
            }
            else
            {
                tc = System.ComponentModel.TypeDescriptor.GetConverter(@this.GetValue(0).GetType());
                if (tc.CanConvertTo(typeof(T)))
                {
                    for (int i = 0; i < @this.Length; i++)
                    {
                        result[i] = (T)tc.ConvertTo(@this.GetValue(i), typeof(T));
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
            }

            return result;
        }

        ///<summary>
        /// Prüft nach, ob es an dem Index ein Array-Element gibt
        ///</summary>
        ///<param name = "@this">@this</param>
        ///<param name = "index">Index</param>
        ///<returns>Gibt True zurück wenn an der angegebenen Position eine Array-Element gefunden wurde</returns>
        public static bool WithInIndex(this Array @this, int index)
        {
            return @this != null && index >= 0 && index < @this.Length;
        }

        ///<summary>
        /// Prüft nach, ob es an dem Index ein Array-Element gibt
        ///</summary>
        ///<param name = "@this"></param>
        ///<param name = "index"></param>
        ///<param name="dimension"></param>
        ///<returns>Gibt True zurück wenn an der angegebenen Position eine Array-Element gefunden wurde</returns>
        public static bool WithInIndex(this Array @this, int index, int dimension = 0)
        {
            return @this != null && index >= @this.GetLowerBound(dimension) && index <= @this.GetUpperBound(dimension);
        }


        /// <summary>
        /// Fügt einem bestehenden Array ein weiteres hinzu
        /// </summary>
        /// <typeparam name="T">Type des Array</typeparam>
        /// <param name="@this">Array zu dem ein weiteres Arry mit dem selben Typ hinzugefügt werden kann.</param>
        /// <param name="arrayToCombine">Das Array kann dem ersten Array hinzugefügt werden.</param>
        /// <returns>Kombination der beiden Arrays</returns>
        /// <example>
        /// 	<code>
        /// 		int[] arrayOne = new[] { 1, 2, 3, 4 };
        /// 		int[] arrayTwo = new[] { 5, 6, 7, 8 };
        /// 		Array combinedArray = arrayOne.CombineArray<int>(arrayTwo);
        /// 	</code>
        /// </example>
        /// <remarks>
        /// 	Contributed by Mohammad Rahman, http://mohammad-rahman.blogspot.com/
        /// </remarks>
        public static T[] CombineArray<T>(this T[] @this, T[] arrayToCombine)
        {
            if (@this != default(T[]) && arrayToCombine != default(T[]))
            {
                int initialSize = @this.Length;
                Array.Resize<T>(ref @this, initialSize + arrayToCombine.Length);
                Array.Copy(arrayToCombine, arrayToCombine.GetLowerBound(0), @this, initialSize, arrayToCombine.Length);
            }
            return @this;
        }

        /// <summary>
        /// To clear the contents of the array.
        /// </summary>
        /// <param name="@this"> The array to clear</param>
        /// <returns>Cleared array</returns>
        /// <example>
        ///     <code>
        ///         Array array = Array.CreateInstance(typeof(string), 2);
        ///         array.SetValue("One", 0); array.SetValue("Two", 1);
        ///         Array arrayToClear = array.ClearAll();
        ///     </code>
        /// </example>
        /// <remarks>
        /// 	Contributed by Mohammad Rahman, http://mohammad-rahman.blogspot.com/
        /// </remarks>
        public static Array ClearAll(this Array @this)
        {
            if (@this != null)
            {
                Array.Clear(@this, 0, @this.Length);
            }

            return @this;
        }

        /// <summary>
        /// Entfernt alle Elemente aus dem Array
        /// </summary>
        /// <typeparam name="T">Typ des Array</typeparam>
        /// <param name="@this">Array das gelöscht werden soll</param>
        /// <returns>Leeres Array</returns>
        /// <example>
        ///     <code>
        ///         int[] result = new[] { 1, 2, 3, 4 }.ClearAll<int>();
        ///     </code>
        /// </example>
        /// <remarks>
        /// 	Contributed by Mohammad Rahman, http://mohammad-rahman.blogspot.com/
        /// </remarks>
        public static T[] ClearAll<T>(this T[] @this)
        {
            if (@this != null)
            {
                for (int i = @this.GetLowerBound(0); i <= @this.GetUpperBound(0); ++i)
                {
                    @this[i] = default(T);
                }
            }

            return @this;
        }

        /// <summary>
        /// Entfernt alle Elemente aus dem Array ab einem übergebenen Index
        /// </summary>
        /// <param name="@this">Array in dem Elemente gelöscht werden sollen</param>
        /// <param name="index">Position ab dem die Array Elemente gelöscht werden.</param>
        /// <returns>Array ohne die entfernten Elemente</returns>
        /// <example>
        ///     <code>
        ///         Array array = Array.CreateInstance(typeof(string), 2);
        ///         array.SetValue("One", 0); array.SetValue("Two", 1);
        ///         Array result = array.ClearAt(2);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 	Contributed by Mohammad Rahman, http://mohammad-rahman.blogspot.com/
        /// </remarks>
        public static Array ClearAt(this Array @this, int index)
        {
            if (@this != null)
            {
                int arrayIndex = index.GetArrayIndex();
                if (arrayIndex.IsIndexInArray(@this))
                {
                    Array.Clear(@this, arrayIndex, 1);
                }
            }

            return @this;
        }

        /// <summary>
        /// Entfernt alle Elemente aus dem Array ab einem übergebenen Index
        /// </summary>
        /// <typeparam name="T">Typ des Array</typeparam>
        /// <param name="@this">Array in dem Elemente gelöscht werden sollen.</param>
        /// <param name="at">Position ab dem die Array Elemente gelöscht werden.</param>
        /// <returns>Array ohne die entfernten Elemente</returns>
        /// <example>
        ///     <code>
        ///           string[] clearString = new[] { "A" }.ClearAt<string>(0);
        ///     </code>
        /// </example>
        /// <remarks>
        /// 	Contributed by Mohammad Rahman, http://mohammad-rahman.blogspot.com/
        /// </remarks>
        public static T[] ClearAt<T>(this T[] @this, int at)
        {
            if (@this != null)
            {
                int arrayIndex = at.GetArrayIndex();
                if (arrayIndex.IsIndexInArray(@this))
                {
                    @this[arrayIndex] = default(T);
                }
            }

            return @this;
        }

        /// <summary>
        /// Prüft ob das Array leer ist
        /// </summary>
        /// <param name="@this">Array das geprüft werden soll ob es leer ist.</param>
        /// <returns>True wenn das Array leer ist.</returns>
        public static bool IsEmpty(this Array @this)
        {
            @this.IsArgumentNull("The array cannot be null.");

            return @this.Length == 0;
        }

        public static void Resize<T>(this T[] array, int newSize)
        {
            Array.Resize(ref array, newSize);
        }

        /// <summary>
        /// Converts to base64string.
        /// </summary>
        /// <param name="pArray">The p array.</param>
        /// <returns>System.String.</returns>
        public static string ToBase64String(this byte[] pArray)
        {
            return Convert.ToBase64String(pArray);
        }

        /// <summary>
        /// Converts to image.
        /// </summary>
        /// <param name="pBytes">The p bytes.</param>
        /// <returns>Image.</returns>
        public static Image ByteArrayToImage(this byte[] pBytes)
        {
            var ms = new MemoryStream(pBytes);
            var returnImage = Image.FromStream(ms);
            return returnImage;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="pBytes">The p bytes.</param>
        /// <returns>System.String.</returns>
        public static string BytesToString(this byte[] pBytes)
        {
            var chars = new char[pBytes.Length / sizeof(char)];
            Buffer.BlockCopy(pBytes, 0, chars, 0, pBytes.Length);
            return new string(chars);
        }

        /// <summary>
        /// Gets the MD5 hash.
        /// </summary>
        /// <param name="pBytes">The p bytes.</param>
        /// <returns>System.String.</returns>
        public static string GetMD5Hash(this byte[] pBytes)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(pBytes);

                var s = new StringBuilder();
                foreach (var b in hash)
                {
                    s.Append(b.ToString("x2").ToLower());
                }

                return s.ToString();
            }
        }

        /// <summary>
        /// Gets the sha256 hash.
        /// </summary>
        /// <param name="pBytes">The p bytes.</param>
        /// <returns>System.String.</returns>
        public static string GetSHA256Hash(this byte[] pBytes)
        {
            using (var sha = SHA256.Create())
            {
                var checksum = sha.ComputeHash(pBytes);
                return BitConverter.ToString(checksum).Replace("-", string.Empty);
            }
        }

        #region BlockCopy

        /// <summary>
        /// Returns a block of items from an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="@this"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks>Contributed by Chris Gessler</remarks>
        public static T[] BlockCopy<T>(this T[] @this, int index, int length)
        {
            return BlockCopy(@this, index, length, false);
        }

        /// <summary>
        /// Returns a block of items from an array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="@this"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="padToLength"></param>
        /// <returns></returns>
        /// <remarks>
        /// Test results prove that Array.Copy is many times faster than Skip/Take and LINQ
        /// Item count: 1,000,000
        /// Array.Copy:     15 ms 
        /// Skip/Take:  42,464 ms - 42.5 seconds
        /// LINQ:          881 ms
        /// 
        /// Contributed by Chris Gessler</remarks>
        public static T[] BlockCopy<T>(this T[] @this, int index, int length, bool padToLength)
        {
            @this.IsArgumentNull("The array cannot be null.");

            int n = length;
            T[] b = null;

            if (@this.Length < index + length)
            {
                n = @this.Length - index;
                if (padToLength)
                {
                    b = new T[length];
                }
            }

            if (b == null)
            {
                b = new T[n];
            }

            Array.Copy(@this, index, b, 0, n);

            return b;
        }

        /// <summary>
        /// Allows enumeration over an Array in blocks
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="@this"></param>
        /// <param name="count"></param>
        /// <param name="padToLength"></param>
        /// <returns></returns>
        /// <remarks>Contributed by Chris Gessler</remarks>
        public static IEnumerable<T[]> BlockCopy<T>(this T[] @this, int count, bool padToLength = false)
        {
            for (int i = 0; i < @this.Length; i += count)
            {
                yield return @this.BlockCopy(i, count, padToLength);
            }
        }

        #endregion
    }
}