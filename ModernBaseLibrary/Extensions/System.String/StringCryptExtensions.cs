//-----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Lifeprojects.de">
//     Class: StringExtensions
//     Copyright © Lifeprojects.de 202021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.4.2021</date>
//
// <summary>Extensions Class for String Types</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Globalization;
    using System.IO;
    using System.Runtime.Versioning;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Cryptography;

    [SupportedOSPlatform("windows")]
    public static partial class StringExtension
    {
        // <summary>
        /// A string extension method that encrypts the string with fixed Vector.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string Encrypt(this string @this)
        {
            if (string.IsNullOrEmpty(@this) == true)
            {
                return (string.Empty);
            }

            try
            {
                return CryptoHelperInternal.Encrypt(@this);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// A string extension method that encrypts the string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key.</param>
        /// <returns>The encrypted string.</returns>
        public static string EncryptRSA(this string @this, string key)
        {
            if (string.IsNullOrEmpty(key) == true)
            {
                return string.Empty;
            }

            var cspp = new CspParameters { KeyContainerName = key };
            var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
            byte[] bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(@this), true);

            return BitConverter.ToString(bytes);
        }

        /// <summary>
        /// A string extension method that encrypts the string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key.</param>
        /// <returns>The encrypted string.</returns>
        public static byte[] EncryptRSAToByte(this string @this, string key)
        {
            if (string.IsNullOrEmpty(key) == true)
            {
                return new byte[] { };
            }

            var cspp = new CspParameters { KeyContainerName = key };
            var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
            byte[] bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(@this), true);

            return bytes;
        }

        public static string EncryptAES(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                byte[] encrypted;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
                return Convert.ToBase64String(encrypted);
            }
        }

        /// <summary>
        /// A string extension method that decrypt a string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key.</param>
        /// <returns>The decrypted string.</returns>
        public static string DecryptRSA(this string @this, string key)
        {
            if (string.IsNullOrEmpty(key) == true)
            {
                return string.Empty;
            }

            var cspp = new CspParameters { KeyContainerName = key };
            var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
            string[] decryptArray = @this.Split(new[] { "-" }, StringSplitOptions.None);
            byte[] decryptByteArray = Array.ConvertAll(decryptArray, (s => Convert.ToByte(byte.Parse(s, NumberStyles.HexNumber))));
            byte[] bytes = rsa.Decrypt(decryptByteArray, true);

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// A string extension method that decrypt a string.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="key">The key.</param>
        /// <returns>The decrypted string.</returns>
        public static string DecryptRSAFromByte(this byte[] @this, string key)
        {
            if (string.IsNullOrEmpty(key) == true)
            {
                return string.Empty;
            }

            var cspp = new CspParameters { KeyContainerName = key };
            var rsa = new RSACryptoServiceProvider(cspp) { PersistKeyInCsp = true };
            byte[] bytes = rsa.Decrypt(@this, true);

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// A string extension method that decrypt a string with fixed Vector.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string Decrypt(this string @this)
        {

            if (string.IsNullOrEmpty(@this) == true)
            {
                return (string.Empty);
            }

            try
            {
                return CryptoHelperInternal.Decrypt(@this);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*
        /// <summary>
        /// A string extension method that encode the string to Base64.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The encoded string to Base64.</returns>
        public static string EncodeBase64(this string @this)
        {
            return Convert.ToBase64String(Activator.CreateInstance<ASCIIEncoding>().GetBytes(@this));
        }

        // <summary>
        /// A string extension method that decode a Base64 String.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The Base64 String decoded.</returns>
        public static string DecodeBase64(this string @this)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(@this));
        }

        public static bool IsBase64String(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                return false;
            }

            return (@this.Trim().Length % 4 == 0) && Regex.IsMatch(@this, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
        */
    }
}