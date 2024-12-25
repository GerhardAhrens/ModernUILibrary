//-----------------------------------------------------------------------
// <copyright file="ByteArrayExtension.cs" company="Lifeprojects.de">
//     Class: ByteArrayExtension
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.08.2020</date>
//
// <summary>
// Extensions Class for byte[] Type
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Collections;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    public static partial class ByteArrayExtension
    {
        public static bool IsNullOrEmpty(this byte[] @this)
        {
            return @this == null || @this.Length == 0;
        }

        public static int ToBase64CharArray(this byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut)
        {
            return Convert.ToBase64CharArray(inArray, offsetIn, length, outArray, offsetOut);
        }

        public static int ToBase64CharArray(this byte[] inArray, int offsetIn, int length, char[] outArray, int offsetOut, Base64FormattingOptions options)
        {
            return Convert.ToBase64CharArray(inArray, offsetIn, length, outArray, offsetOut, options);
        }

        public static byte[] Resize(this byte[] @this, int newSize)
        {
            Array.Resize(ref @this, newSize);
            return @this;
        }

        public static MemoryStream ToMemoryStream(this byte[] @this)
        {
            return new MemoryStream(@this);
        }

        public static bool IsEqual(this byte[] @this, byte[] with)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(@this, with);
        }

        /// <summary>
        ///     A string extension method that compress the given string to GZip byte array.
        /// </summary>
        /// <param name="this">The stringToCompress to act on.</param>
        /// <returns>The string compressed into a GZip byte array.</returns>
        public static byte[] CompressGZip(this string @this)
        {
            byte[] stringAsBytes = Encoding.Default.GetBytes(@this);
            using (var memoryStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    zipStream.Write(stringAsBytes, 0, stringAsBytes.Length);
                    zipStream.Close();
                    return memoryStream.ToArray();
                }
            }
        }

        /// <summary>
        ///     A string extension method that compress the given string to GZip byte array.
        /// </summary>
        /// <param name="this">The stringToCompress to act on.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The string compressed into a GZip byte array.</returns>
        public static byte[] CompressGZip(this string @this, Encoding encoding)
        {
            byte[] stringAsBytes = encoding.GetBytes(@this);
            using (var memoryStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    zipStream.Write(stringAsBytes, 0, stringAsBytes.Length);
                    zipStream.Close();
                    return memoryStream.ToArray();
                }
            }
        }

        /// <summary>
        /// A byte[] extension method that decompress the byte array gzip to string.
        /// </summary>
        /// <param name="this">Byte[] Object</param>
        /// <returns>decompress as String</returns>
        public static string DecompressGZip(this byte[] @this)
        {
            const int bufferSize = 1024;
            using (var memoryStream = new MemoryStream(@this))
            {
                using (var zipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    using (var outStream = new MemoryStream())
                    {
                        var buffer = new byte[bufferSize];
                        int totalBytes = 0;
                        int readBytes;
                        while ((readBytes = zipStream.Read(buffer, 0, bufferSize)) > 0)
                        {
                            outStream.Write(buffer, 0, readBytes);
                            totalBytes += readBytes;
                        }
                        return Encoding.Default.GetString(outStream.GetBuffer(), 0, totalBytes);
                    }
                }
            }
        }

        /// <summary>
        /// A byte[] extension method that decompress the byte array gzip to string.with Encoding
        /// </summary>
        /// <param name="this">Byte[] Object</param>
        /// <param name="encoding"></param>
        /// <returns>decompress as String</returns>
        public static string DecompressGZip(this byte[] @this, Encoding encoding)
        {
            const int bufferSize = 1024;
            using (var memoryStream = new MemoryStream(@this))
            {
                using (var zipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    // Memory stream for storing the decompressed bytes
                    using (var outStream = new MemoryStream())
                    {
                        var buffer = new byte[bufferSize];
                        int totalBytes = 0;
                        int readBytes;
                        while ((readBytes = zipStream.Read(buffer, 0, bufferSize)) > 0)
                        {
                            outStream.Write(buffer, 0, readBytes);
                            totalBytes += readBytes;
                        }
                        return encoding.GetString(outStream.GetBuffer(), 0, totalBytes);
                    }
                }
            }
        }
    }
}
