//-----------------------------------------------------------------------
// <copyright file="PasswordHashing.cs" company="Lifeprojects.de">
//     Class: PasswordHashing
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>01.09.2023 14:43:08</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    public class PasswordHashing
    {
        /// <summary>
        /// Function to securely hash a password using PBKDF2
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10_000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // 256 bits
                return Convert.ToBase64String(hash);
            }
        }

        public static string HashPassword(string password)
        {
            byte[] salt = new byte[16];

#pragma warning disable SYSLIB0023 // Typ oder Element ist veraltet
            new RNGCryptoServiceProvider().GetBytes(salt);
#pragma warning restore SYSLIB0023 // Typ oder Element ist veraltet

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Function to generate a random salt
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16]; // 128 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
