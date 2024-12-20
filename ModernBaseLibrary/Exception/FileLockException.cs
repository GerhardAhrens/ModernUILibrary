//-----------------------------------------------------------------------
// <copyright file="FileLockException.cs" company="PTA">
//     Class: FileLockException
//     Copyright © PTA GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>14.07.2020</date>
//
// <summary>
// Custom Exception Class for FileLock
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    [Serializable]
    public class FileLockException : BaseException
    {
        private const string MESSAGE = "Es ist ein Fehler beim bearbeiten einer Datei aufgetreten.";

        public FileLockException() : base(string.Empty)
        {
            this.Data.Add("Msg", MESSAGE);
            this.Data.Add("File", "Unbekannte Datei");
            this.ErrorLevel = ErrorLevel.Error;
        }

        public FileLockException(string pMessage) : base(pMessage)
        {
            this.Data.Add("Msg", pMessage);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public FileLockException(string pMessage, string fileName) : base(pMessage)
        {
            this.Data.Add("Msg", pMessage);
            this.Data.Add("File", fileName);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public FileLockException(string pMessage, Exception pInnerException) : base(pMessage, pInnerException)
        {
            this.Data.Add("Msg", pMessage);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public override IDictionary CustomMessage()
        {
            if (this.Data.IsNullOrEmpty() == true)
            {
                this.Data.Add("Msg", MESSAGE);
                this.Data.Add("File", "Unbekannte Datei");

                return this.Data;
            }
            else
            {
                return this.Data;
            }
        }
    }
}