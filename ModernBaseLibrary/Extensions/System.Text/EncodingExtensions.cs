//-----------------------------------------------------------------------
// <copyright file="EncodingExtensions.cs" company="Lifeprojects.de">
//     Class: EncodingExtensions
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.05.2023 08:26:08</date>
//
// <summary>
// Extension Class for Text Encoding
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    public static class EncodingExtensions
    {
        /// <summary>
        /// Converts a string to byte array.
        /// </summary>
        public static byte[] GetBytes([NotNull] this string s, [NotNull] Encoding encoding)
        {
            return encoding.GetBytes(s);
        }

        /// <summary>
        /// Converts a string to byte array using unicode encoding.
        /// </summary>
        public static byte[] GetBytes([NotNull] this string s) => s.GetBytes(Encoding.Unicode);

        /// <summary>
        /// Converts a byte array to string.
        /// </summary>
        public static string GetString([NotNull] this byte[] bytes, [NotNull] Encoding encoding)
        {
            return encoding.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Converts a byte array to string using unicode encoding.
        /// </summary>
        public static string GetString([NotNull] this byte[] bytes) => bytes.GetString(Encoding.Unicode);

        /// <summary>
        /// Converts a byte array to a base64 string.
        /// </summary>
        public static string ToBase64([NotNull] this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Converts a base64 string to a byte array.
        /// </summary>
        public static byte[] FromBase64([NotNull] this string s)
        {
            return Convert.FromBase64String(s);
        }
    }
}
