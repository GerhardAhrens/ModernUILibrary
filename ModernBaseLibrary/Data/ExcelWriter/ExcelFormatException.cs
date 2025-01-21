//-----------------------------------------------------------------------
// <copyright file="ExcelFormatException.cs" company="Lifeprojects.de">
//     Class: ExcelFormatException
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@Lifeprojects.de</email>
// <date>12.04.2023 11:56:48</date>
//
// <summary>
// Exception Class for ExcelWriter
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.ExcelWriter
{
    using System;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    [Serializable]
    public sealed class ExcelFormatException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFormatException"/> class
        /// </summary>
        public ExcelFormatException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFormatException"/> class
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        public ExcelFormatException(string message): base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFormatException"/> class
        /// </summary>
        /// <param name="title">Title of the exception.</param>
        /// <param name="message">Message of the exception.</param>
        /// <param name="inner">Inner exception.</param>
        public ExcelFormatException(string title, string message, Exception inner) : base(message, inner)
        {
        }
    }
}
