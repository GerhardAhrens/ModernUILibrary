//-----------------------------------------------------------------------
// <copyright file="StringHash.cs" company="Lifeprojects.de">
//     Class: StringHash
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>25.06.2019</date>
//
// <summary>
//      Compute Hash from File
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using ModernBaseLibrary.Core;

    public class StringHash : DisposableCoreBase
    {
        public StringHash(string text)
        {
            this.Text = text;
        }

        public string Text { get; private set; }


        public string ComputeHash(HashTyp hashTyp = HashTyp.SHA256)
        {
            string result = string.Empty;

            try
            {
                if (hashTyp == HashTyp.MD5)
                {
                    result = this.ComputeHashMD5();
                }
                else if (hashTyp == HashTyp.SHA1)
                {
                    result = this.ComputeHashSHA1();
                }
                else if (hashTyp == HashTyp.SHA256)
                {
                    result = this.ComputeHashSHA256();
                }
                else if (hashTyp == HashTyp.SHA384)
                {
                    result = this.ComputeHashSHA384();
                }
                else if (hashTyp == HashTyp.SHA512)
                {
                    result = this.ComputeHashSHA512();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        private string ComputeHashMD5()
        {
            if (string.IsNullOrEmpty(this.Text) == false)
            {
                byte[] textNoHash = Encoding.UTF8.GetBytes(this.Text);
                return GetHash<MD5>(textNoHash);
            }

            return string.Empty;
        }

        private string ComputeHashSHA1()
        {
            if (string.IsNullOrEmpty(this.Text) == false)
            {
                byte[] textNoHash = Encoding.UTF8.GetBytes(this.Text);
                return GetHash<SHA1>(textNoHash);
            }

            return string.Empty;
        }

        private string ComputeHashSHA256()
        {
            if (string.IsNullOrEmpty(this.Text) == false)
            {
                byte[] textNoHash = Encoding.UTF8.GetBytes(this.Text);
                return GetHash<SHA256>(textNoHash);
            }

            return string.Empty;
        }

        private string ComputeHashSHA384()
        {
            if (string.IsNullOrEmpty(this.Text) == false)
            {
                byte[] textNoHash = Encoding.UTF8.GetBytes(this.Text);
                return GetHash<SHA384>(textNoHash);
            }

            return string.Empty;
        }

        private string ComputeHashSHA512()
        {
            if (string.IsNullOrEmpty(this.Text) == false)
            {
                byte[] textNoHash = Encoding.UTF8.GetBytes(this.Text);
                return GetHash<SHA512>(textNoHash);
            }

            return string.Empty;
        }

        private string GetHash<T>(byte[] text) where T : HashAlgorithm
        {
            string hashTyp = typeof(T).Name;

            try
            {
                MethodInfo create = typeof(T).GetMethod("Create", new Type[] { });
                using (T crypt = (T)create.Invoke(null, null))
                {
                    byte[] hash = crypt.ComputeHash(text);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("HashTyp", hashTyp);
                throw;
            }
        }
    }

    public enum HashTyp : int
    {
        None = 0,
        MD5 = 1,
        SHA1 = 2,
        SHA256 = 3,
        SHA384 = 4,
        SHA512 = 5
    }
}
