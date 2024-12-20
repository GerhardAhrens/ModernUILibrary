//-----------------------------------------------------------------------
// <copyright file="CryptoHelper.cs" company="Lifeprojects.de">
//     Class: CryptoHelper
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>13.12.2022</date>
//
// <summary>
// Helper Klasse zum Ver- und Entschlüsseln von Strings
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security.Cryptography;
    using System.Text;

    [SupportedOSPlatform("windows")]
    public class CryptoHelperInternal
    {
        private static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        private static readonly byte[] internalKey = { 0x16, 0x15, 0x14, 0x13, 0x11, 0x10, 0x09, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        private static readonly byte[] internalVector = { 0x16, 0x15, 0x14, 0x13, 0x11, 0x10, 0x09, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };

        public static string Decrypt(string pInputString)
        {
            string outputString = string.Empty;

            if (string.IsNullOrEmpty(pInputString) == true)
            {
                return string.Empty;
            }

            try
            {
                using (MemoryStream inStream = new MemoryStream())
                {
                    byte[] inBytes = Convert.FromBase64String(pInputString);
                    inStream.Write(inBytes, 0, inBytes.Length);
                    inStream.Position = 0;

                    MemoryStream outStream = new MemoryStream();
                    byte[] buffer = new byte[128];

#pragma warning disable SYSLIB0045 // Typ oder Element ist veraltet
                    SymmetricAlgorithm algorithm = SymmetricAlgorithm.Create("Rijndael");
#pragma warning restore SYSLIB0045 // Typ oder Element ist veraltet
                    algorithm.IV = internalVector;
                    algorithm.Key = internalKey;
                    ICryptoTransform transform = algorithm.CreateDecryptor();
                    CryptoStream cryptedStream = new CryptoStream(inStream, transform, CryptoStreamMode.Read);

                    int restLength = cryptedStream.Read(buffer, 0, buffer.Length);
                    while (restLength > 0)
                    {
                        outStream.Write(buffer, 0, restLength);
                        restLength = cryptedStream.Read(buffer, 0, buffer.Length);
                    }

                    outputString = System.Text.Encoding.Default.GetString(outStream.ToArray());

                    cryptedStream.Close();
                    cryptedStream = null;
                    inStream.Close();
                    outStream.Close();
                    outStream = null;
                }

                return outputString;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Encrypt(string pInputString)
        {
            string outputString = string.Empty;

            if (string.IsNullOrEmpty(pInputString) == true)
            {
                return string.Empty;
            }

            try
            {
                using (MemoryStream inStream = new MemoryStream())
                {
                    byte[] inBytes = new byte[pInputString.Length];
                    inBytes = System.Text.Encoding.Default.GetBytes(pInputString);
                    inStream.Write(inBytes, 0, inBytes.Length);
                    inStream.Position = 0;

                    MemoryStream outStream = new MemoryStream();
                    byte[] buffer = new byte[128];

#pragma warning disable SYSLIB0045 // Typ oder Element ist veraltet
                    SymmetricAlgorithm algorithm = SymmetricAlgorithm.Create("Rijndael");
#pragma warning restore SYSLIB0045 // Typ oder Element ist veraltet
                    algorithm.IV = internalVector;
                    algorithm.Key = internalKey;
                    ICryptoTransform transform = algorithm.CreateEncryptor();
                    CryptoStream cryptedStream = new CryptoStream(outStream, transform, CryptoStreamMode.Write);

                    int restLength = inStream.Read(buffer, 0, buffer.Length);
                    while (restLength > 0)
                    {
                        cryptedStream.Write(buffer, 0, restLength);
                        restLength = inStream.Read(buffer, 0, buffer.Length);
                    }

                    cryptedStream.FlushFinalBlock();

                    outputString = System.Convert.ToBase64String(outStream.ToArray());

                    cryptedStream.Close();
                    cryptedStream = null;
                    inStream.Close();
                    outStream.Close();
                    outStream = null;
                }

                return outputString;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string HashFileMD5(string pFileName)
        {
            if (File.Exists(pFileName) == true)
            {
                using (var md5 = MD5.Create())
                {
                    return BitConverter.ToString(md5.ComputeHash(File.ReadAllBytes(pFileName))).Replace("-", string.Empty);
                }
            }

            return string.Empty;
        }

        public static string HashStringMD5(string text, bool withSeparator = false)
        {
            string result = string.Empty; 
            using (var md5 = MD5.Create())
            {
                byte[] inputByte = StringToByteArray(text);
                if (withSeparator == true)
                {
                    result = BitConverter.ToString(md5.ComputeHash(inputByte));
                }
                else
                {
                    result = BitConverter.ToString(md5.ComputeHash(inputByte)).Replace("-", string.Empty);
                }
            }

            return result;
        }

        public static byte[] GetRandomData(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("length", $"the 'length' must greater 0");
            }

            byte[] data = new byte[4 * length];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return StringToByteArray(result.ToString());
        }

        private static byte[] StringToByteArray(string str)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetBytes(str);
        }

        private static string ByteArrayToString(byte[] arr)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetString(arr);
        }
    }
}
