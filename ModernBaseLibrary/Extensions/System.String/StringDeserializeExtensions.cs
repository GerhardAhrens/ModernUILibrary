//-----------------------------------------------------------------------
// <copyright file="StringDeserializeExtensions.cs" company="Lifeprojects.de">
//     Class: StringDeserializeExtensions
//     Copyright © company="Lifeprojects.de" 2021
// </copyright>
//
// <author>Gerhard Ahrens - company="Lifeprojects.de"</author>
// <email>developer@lifeprojects.de</email>
// <date>15.4.2021</date>
//
// <summary>Extensions Class for String Types</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    public static partial class StringExtension
    {
        /// <summary>
        ///     A string extension method that deserialize a Json string to object.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The object deserialized.</returns>
        public static T DeserializeJson<T>(this string @this)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream(Encoding.Default.GetBytes(@this)))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        ///     A string extension method that deserialize a Json string to object.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">The @this to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The object deserialized.</returns>
        public static T DeserializeJson<T>(this string @this, Encoding encoding)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream(encoding.GetBytes(@this)))
            {
                return (T)serializer.ReadObject(stream);
            }
        }
    }
}