//-----------------------------------------------------------------------
// <copyright file="MailSignature.cs" company="Lifeprojects.de">
//     Class: MailSignature
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.03.2013</date>
//
// <summary>
// Klasse zum Lesen und Schreiben einer Signaturdatei (SMTP-Mailer)
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.SMTP
{
    using System;
    using System.IO;
    using System.Reflection;

    public class MailSignature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MailSignature"/> class.
        /// </summary>
        public MailSignature()
        {
            if (string.IsNullOrEmpty(this.UserName) == true)
            {
                this.UserName = Environment.UserName;
            }

            if (string.IsNullOrEmpty(this.SignaturePath) == true)
            {
                this.SignaturePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }

            this.GetSignatureName = this.GetPathAndSignatureName();
        }

        public string SignaturePath { get; set; }

        public string UserName { get; set; }

        private string GetSignatureName { get; set; }


        public void CreateSignatur(string pNewSignatur)
        {
            string signatureFullName = this.GetPathAndSignatureName();

            if (File.Exists(signatureFullName) == true)
            {
                File.Delete(signatureFullName);
            }

            File.WriteAllText(signatureFullName, pNewSignatur);
        }

        public string ReadSignatur()
        {
            string readSignatur = string.Empty;
            string signatureFullName = this.GetPathAndSignatureName();

            if (File.Exists(signatureFullName) == true)
            {
                readSignatur = File.ReadAllText(signatureFullName);
            }
            else
            {
                readSignatur = "\n\r<no signature file>";
            }

            return readSignatur;
        }

        private string GetPathAndSignatureName()
        {
            string signatureName = Path.Combine(this.SignaturePath, $"{this.UserName}.signatur");

            return signatureName;
        }
    }
}