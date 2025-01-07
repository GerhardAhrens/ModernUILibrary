//-----------------------------------------------------------------------
// <copyright file="CompressBytes.cs" company="Lifeprojects.de">
//     Class: CompressBytes
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>16.06.2017</date>
//
// <summary>Definition of CompressBytes Class for compress strings</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    /// <summary>
    /// Diese statische Klasse stellt Methoden zum komprimieren 
    /// und dekomprimieren von Strings zur Verfügung.
    /// </summary>
    public static class CompressBytes
    {
        /// <summary>
        /// Gibt einen String als Byte-Array zurück.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// Erstellt aus einem Byte-Array einen String und gibt diesen zurück.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Die Methode gibt ein komprimiertes ByteArray zurück
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] CompressByteArray(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentException("The parameter 'bytes' is null");
            }

            using (MemoryStream output = new MemoryStream())
            {
                GZipStream gzip = new GZipStream(output, CompressionMode.Compress, true);
                gzip.Write(bytes, 0, bytes.Length);
                gzip.Close();
                return output.ToArray();
            }
        }

        /// <summary>
        /// Die Methode gibt ein dekomprimiertes ByteArray zurück
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] DecompressBytesArray(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                throw new ArgumentException("The parameter 'bytes' is null");
            }

            MemoryStream input = new MemoryStream();
            input.Write(bytes, 0, bytes.Length);
            input.Position = 0;
            GZipStream gzip = new GZipStream(input, CompressionMode.Decompress, true);

            using (MemoryStream output = new MemoryStream())
            {
                byte[] buff = new byte[64];
                int read = -1;
                read = gzip.Read(buff, 0, buff.Length);
                while (read > 0)
                {
                    output.Write(buff, 0, read);
                    read = gzip.Read(buff, 0, buff.Length);
                }

                gzip.Close();
                return output.ToArray();
            }
        }

    }
}
