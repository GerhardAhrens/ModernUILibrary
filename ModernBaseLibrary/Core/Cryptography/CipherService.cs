//-----------------------------------------------------------------------
// <copyright file="CipherService.cs" company="Lifeprojects.de">
//     Class: CipherService
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <Framework>8.0</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>01.12.2024 18:42:02</date>
//
// <summary>
// Klasse für ver- und entschlüsselung von Strings
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public interface ICipherService
    {
        string Decrypt(string text, string key);
        string Encrypt(string text, string key);
    }

    /// <summary>
    /// Serviceklasse zum Verschlüsseln, Entschlüsseln
    /// </summary>
    /// <remarks>
    /// https://www.siakabaro.com/how-to-create-aes-encryption-256-bit-key-in-c/
    /// https://propertyguru.tech/doing-aes-encryption-correct-in-your-net-application-5d66168b5b44
    /// </remarks>
    public class CipherService : ICipherService
    {
        public string Decrypt(string text, string cryptKey)
        {
            var fullCipher = Convert.FromBase64String(text);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(cryptKey);

            using (var aesAlg = Aes.Create())
            {
                if (key.Length != aesAlg.IV.Length)
                {
                    throw new InvalidOperationException($"Der Key zur Verschlüsselung muß {aesAlg.IV.Length} Byte sein, ist aber {cryptKey.Length} Byte.");
                }

                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        public string Encrypt(string text, string cryptKey)
        {
            var key = Encoding.UTF8.GetBytes(cryptKey);

            using (var aesAlg = Aes.Create())
            {
                if (key.Length != aesAlg.IV.Length)
                {
                    throw new InvalidOperationException($"Der Key zur Verschlüsselung muß {aesAlg.IV.Length} Byte sein, ist aber {cryptKey.Length} Byte.");
                }

                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }
    }
}
