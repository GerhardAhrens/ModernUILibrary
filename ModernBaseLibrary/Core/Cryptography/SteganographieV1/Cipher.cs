namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.Runtime.Versioning;
    using System.Security.Cryptography;

    using ModernBaseLibrary.Extension;

    /// <summary>
    ///     Example of <see cref="ICrypt" /> (Cipher En/De-crypt Text with password-keys)
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class Cipher : ICrypt
    {
        private readonly int _keysize;
        private const int DerivationIterations = 1000;

        public Cipher(int keySize = 128)
        {
            _keysize = keySize;
        }

        /// <summary>
        ///     Encrypt a plain text using a string password-key
        /// </summary>
        /// <param name="unencryptedText">The plain text to encrypt</param>
        /// <param name="key">Encryption Key, or "password"</param>
        /// <returns>Encrypted Text</returns>
        public string Encrypt(string key, string unencryptedText)
        {
            return Convert.ToBase64String(unencryptedText.EncryptRSAToByte(key));
        }

        /// <summary>
        ///     Decrypt an encrypted text using a string password-key
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt</param>
        /// <param name="key">Encryption Key, or "password"</param>
        /// <returns>Decrypted Text</returns>
        public string Decrypt(string key, string encryptedText)
        {
            byte[] cipherTextBytesWithSaltAndIv = Convert.FromBase64String(encryptedText);
            return cipherTextBytesWithSaltAndIv.DecryptRSAFromByte(key);
        }

        private static byte[] GenerateBitsOfRandomEntropy(int size = 128)
        {
            var randomBytes = new byte[size / 8];
#pragma warning disable SYSLIB0023 // Typ oder Element ist veraltet
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
#pragma warning restore SYSLIB0023 // Typ oder Element ist veraltet
            return randomBytes;
        }
    }
}