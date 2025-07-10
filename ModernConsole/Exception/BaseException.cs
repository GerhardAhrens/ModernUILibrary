//-----------------------------------------------------------------------
// <copyright file="BaseException.cs" company="PTA">
//     Class: BaseException
//     Copyright © PTA GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>14.07.2020</date>
//
// <summary>
// Base Class for Custom Exception
// </summary>
//-----------------------------------------------------------------------

namespace ModernConsole.Exception
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.Versioning;

    using ModernConsole.Extension;

    [SupportedOSPlatform("windows")]
    [Serializable]
    public class BaseException : Exception
    {
        private ErrorLevel errorLevel;
        private StackTrace stackTrace;

        public BaseException()
        {
            this.errorLevel = ErrorLevel.Undefined;
        }

        public BaseException(string pMessage) : base(pMessage)
        {
            this.errorLevel = ErrorLevel.Undefined;
        }

        public BaseException(string pMessage, Exception pInnerException) : base(pMessage, pInnerException)
        {
            StackTrace trace = new StackTrace(pInnerException, true);
            this.stackTrace = trace;

            this.errorLevel = ErrorLevel.Undefined;
        }

        public ErrorLevel ErrorLevel
        {
            get { return this.errorLevel; }
            set { this.errorLevel = value; }
        }

        public StackTrace Stack
        {
            get { return this.stackTrace; }
            set { this.stackTrace = value; }
        }

        public void ClearData()
        {
            this.Data.Clear();
        }

        public virtual IDictionary CustomMessage()
        {
            const string MESSAGE = "Es ist ein Fehler aufgetreten.";

            if (this.Data.IsNullOrEmpty() == true)
            {
                this.Data.Add("Msg", MESSAGE);
            }

            return this.Data;
        }
    }
}