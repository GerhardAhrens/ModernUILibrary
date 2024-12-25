//-----------------------------------------------------------------------
// <copyright file="HashExtension.cs" company="Lifeprojects.de">
//     Class: HashExtension
//     Copyright © company="Lifeprojects.de" 2019
// </copyright>
//
// <author>Gerhard Ahrens - company="Lifeprojects.de"</author>
// <email>developer@lifeprojects.de</email>
// <date>21.8.2019</date>
//
// <summary>Class with HashExtension Definition</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Net;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;

    public static class HashExtension
    {
        /// <summary>
        /// Die Methode konvertiert einen String in einem MD5 Hash
        /// </summary>
        /// <param name="this">String</param>
        /// <returns>MD5 Hash String</returns>
        public static string ToMD5(this string @this, bool setUpper = true)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(@this);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }

            return setUpper == true ? sb.ToString().ToUpper() : sb.ToString().ToLower();
        }

        public static bool VerifyMD5Hash(this string @this, string hash)
        {
            string hashOfInput = @this.ToMD5();

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);

        }
    }
}