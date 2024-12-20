//-----------------------------------------------------------------------
// <copyright file="HashFile.cs" company="www.pta.de">
//     Class: HashFile
//     Copyright © www.pta.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>09.06.2023 09:39:12</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Security.Cryptography;

    using ModernBaseLibrary.Core;

    public sealed class HashFile : DisposableCoreBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashFile"/> class.
        /// </summary>
        public HashFile()
        {
        }

        public HashFile(bool resultToUpper)
        {
            this.ResultToUpper = resultToUpper;
            this.Separator = '\0';
        }

        public HashFile(char separator)
        {
            this.ResultToUpper = true;
            this.Separator = separator;
        }

        public HashFile(char separator, bool resultToUpper)
        {
            this.ResultToUpper = resultToUpper;
            this.Separator = separator;
        }

        private bool ResultToUpper { get; set; } = true;

        private char Separator { get; set; } = '\0';

        public string GetMD5(string filePath)
        {
            string result = string.Empty;

            try
            {
                using (FileStream fileCheck = System.IO.File.OpenRead(filePath))
                {
                    using (var md5 = MD5.Create())
                    {
                        byte[] hashBytes = md5.ComputeHash(fileCheck);
                        result = this.FormatHashResult(hashBytes);
                    }

                    fileCheck.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public string GetChecksumMD5(string file)
        {
            if (System.IO.File.Exists(file) == true)
            {
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 1024, false))
                {
                    return this.GetHash<MD5>(fileStream);
                }
            }

            return string.Empty;
        }

        public string GetChecksumSHA1(string file)
        {
            if (System.IO.File.Exists(file) == true)
            {
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 1024, false))
                {
                    var v2 = this.GetHash<SHA1>(fileStream);
                    var v3 = this.GetHash<SHA256>(fileStream);
                    var v4 = this.GetHash<SHA512>(fileStream);
                    return this.GetHash<SHA1>(fileStream);
                }
            }

            return string.Empty;
        }

        public string GetChecksumSHA256(string file)
        {
            if (System.IO.File.Exists(file) == true)
            {
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 1024, false))
                {
                    return this.GetHash<SHA256>(fileStream);
                }
            }

            return string.Empty;
        }

        public string GetChecksumSHA384(string file)
        {
            if (System.IO.File.Exists(file) == true)
            {
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 1024, false))
                {
                    return this.GetHash<SHA384>(fileStream);
                }
            }

            return string.Empty;
        }

        public string GetChecksumSHA512(string file)
        {
            if (System.IO.File.Exists(file) == true)
            {
                using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 1024, false))
                {
                    return this.GetHash<SHA512>(fileStream);
                }
            }

            return string.Empty;
        }

        private string GetHash<T>(Stream stream) where T : HashAlgorithm
        {
            string result = string.Empty;

            if (typeof(T).Name == "MD5" || typeof(T).Name == "SHA1" || typeof(T).Name == "SHA256" || typeof(T).Name == "SHA384" || typeof(T).Name == "SHA512")
            {
            }
            else
            {
                return result;
            }

            MethodInfo create = typeof(T).GetMethod("Create", new Type[] { });
            using (T crypt = (T)create.Invoke(null, null))
            {
                byte[] hashBytes = crypt.ComputeHash(stream);
                result = this.FormatHashResult(hashBytes);
            }

            return result;
        }

        private string FormatHashResult(byte[] hashBytes)
        {
            string result = string.Empty;

            if (this.ResultToUpper == true)
            {
                if (this.Separator == '\0')
                {
                    result = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToUpper();
                }
                else
                {
                    result = BitConverter.ToString(hashBytes).Replace('-', this.Separator).ToUpper();
                }
            }
            else
            {
                if (this.Separator == '\0')
                {
                    result = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
                }
                else
                {
                    result = BitConverter.ToString(hashBytes).Replace('-', this.Separator).ToLower();
                }
            }

            return result;
        }

        protected override void DisposeManagedResources()
        {
            /* Behandeln von Managed Resources bem verlassen der Klasse */
        }

        protected override void DisposeUnmanagedResources()
        {
            /* Behandeln von UnManaged Resources bem verlassen der Klasse */
        }
    }
}
