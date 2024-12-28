//-----------------------------------------------------------------------
// <copyright file="FileCryption.cs" company="Lifeprojects.de"">
//     Class: FileCryption
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.10.2022 13:20:33</date>
//
// <summary>
// Klasse zum verschlüsseln und entschlüsseln von Dateien auf Basis von ASE
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Windows.Forms;

    using ModernBaseLibrary.Core;

    public sealed class FileCryption : DisposableCoreBase
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCryption"/> class.
        /// </summary>
        public FileCryption()
        {
        }

        /// <summary>
        /// Encrypts a file from its path and a plain password.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="password"></param>
        public void FileEncrypt(string inputFile, string outputFile, string password = null)
        {
            if (string.IsNullOrEmpty(password) == true)
            {
                password = typeof(FileCryption).FullName;
            }

            try
            {
                byte[] salt = GenerateRandomSalt();

                using (FileStream fsCrypt = new FileStream(outputFile, FileMode.Create))
                {
                    byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

#pragma warning disable SYSLIB0022 // Typ oder Element ist veraltet
                    RijndaelManaged AES = new RijndaelManaged();
#pragma warning restore SYSLIB0022 // Typ oder Element ist veraltet
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Padding = PaddingMode.PKCS7;

#pragma warning disable SYSLIB0022 // Typ oder Element ist veraltet
#pragma warning disable SYSLIB0041 // Typ oder Element ist veraltet
                    var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
#pragma warning restore SYSLIB0041 // Typ oder Element ist veraltet
#pragma warning restore SYSLIB0022 // Typ oder Element ist veraltet
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CFB;

                    fsCrypt.Write(salt, 0, salt.Length);

                    using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsIn = new FileStream(inputFile, FileMode.Open))
                        {
                            byte[] buffer = new byte[1048576];
                            int read;

                            while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                Application.DoEvents();
                                cs.Write(buffer, 0, read);
                            }

                            fsIn.Close();
                        }

                        cs.Close();
                    }

                    fsCrypt.Close();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void FileEncrypt(FileInfo inputFile, FileInfo outputFile, string password = null)
        {
            if (string.IsNullOrEmpty(password) == true)
            {
                password = typeof(FileCryption).FullName;
            }

            try
            {
                byte[] salt = GenerateRandomSalt();

                using (FileStream fsCrypt = new FileStream(outputFile.FullName, FileMode.Create))
                {
                    byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

#pragma warning disable SYSLIB0022 // Typ oder Element ist veraltet
                    RijndaelManaged AES = new RijndaelManaged();
#pragma warning restore SYSLIB0022 // Typ oder Element ist veraltet
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Padding = PaddingMode.PKCS7;

#pragma warning disable SYSLIB0041 // Typ oder Element ist veraltet
                    var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
#pragma warning restore SYSLIB0041 // Typ oder Element ist veraltet
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CFB;

                    fsCrypt.Write(salt, 0, salt.Length);

                    using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsIn = new FileStream(inputFile.FullName, FileMode.Open))
                        {
                            byte[] buffer = new byte[1048576];
                            int read;

                            while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                Application.DoEvents();
                                cs.Write(buffer, 0, read);
                            }

                            fsIn.Close();
                        }

                        cs.Close();
                    }

                    fsCrypt.Close();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Decrypts an encrypted file with the FileEncrypt method through its path and the plain password.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="password"></param>
        public void FileDecrypt(string inputFile, string outputFile, string password = null)
        {
            if (string.IsNullOrEmpty(password) == true)
            {
                password = typeof(FileCryption).FullName;
            }

            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];

            using (FileStream fsCrypt = new FileStream(inputFile, FileMode.Open))
            {
                fsCrypt.Read(salt, 0, salt.Length);

#pragma warning disable SYSLIB0022 // Typ oder Element ist veraltet
                RijndaelManaged AES = new RijndaelManaged();
#pragma warning restore SYSLIB0022 // Typ oder Element ist veraltet
                AES.KeySize = 256;
                AES.BlockSize = 128;
#pragma warning disable SYSLIB0041 // Typ oder Element ist veraltet
                var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
#pragma warning restore SYSLIB0041 // Typ oder Element ist veraltet
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);
                AES.Padding = PaddingMode.PKCS7;
                AES.Mode = CipherMode.CFB;

                using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                    int read;
                    byte[] buffer = new byte[1048576];

                    try
                    {
                        while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            Application.DoEvents();
                            fsOut.Write(buffer, 0, read);
                        }

                        fsOut.Close();
                    }
                    catch (CryptographicException ex_CryptographicException)
                    {
                        string errorText = ex_CryptographicException.Message;
                    }
                    catch (Exception ex)
                    {
                        string errorText = ex.Message;
                        throw;
                    }

                    cs.Close();
                }

                fsCrypt.Close();
            }
        }

        public void FileDecrypt(FileInfo inputFile, FileInfo outputFile, string password = null)
        {
            if (string.IsNullOrEmpty(password) == true)
            {
                password = typeof(FileCryption).FullName;
            }

            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];

            using (FileStream fsCrypt = new FileStream(inputFile.FullName, FileMode.Open))
            {
                fsCrypt.Read(salt, 0, salt.Length);

#pragma warning disable SYSLIB0022 // Typ oder Element ist veraltet
                RijndaelManaged AES = new RijndaelManaged();
#pragma warning restore SYSLIB0022 // Typ oder Element ist veraltet
                AES.KeySize = 256;
                AES.BlockSize = 128;
#pragma warning disable SYSLIB0041 // Typ oder Element ist veraltet
                var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
#pragma warning restore SYSLIB0041 // Typ oder Element ist veraltet
                AES.Key = key.GetBytes(AES.KeySize / 8);
                AES.IV = key.GetBytes(AES.BlockSize / 8);
                AES.Padding = PaddingMode.PKCS7;
                AES.Mode = CipherMode.CFB;

                using (CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    FileStream fsOut = new FileStream(outputFile.FullName, FileMode.Create);

                    int read;
                    byte[] buffer = new byte[1048576];

                    try
                    {
                        while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            Application.DoEvents();
                            fsOut.Write(buffer, 0, read);
                        }

                        fsOut.Close();
                    }
                    catch (CryptographicException ex_CryptographicException)
                    {
                        string errorText = ex_CryptographicException.Message;
                    }
                    catch (Exception ex)
                    {
                        string errorText = ex.Message;
                        throw;
                    }

                    cs.Close();
                }

                fsCrypt.Close();
            }
        }

        /// <summary>
        /// Creates a random salt that will be used to encrypt your file. This method is required on FileEncrypt.
        /// </summary>
        /// <returns></returns>
        private static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

#pragma warning disable SYSLIB0023 // Typ oder Element ist veraltet
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data);
                }
            }
#pragma warning restore SYSLIB0023 // Typ oder Element ist veraltet

            return data;
        }
    }
}
