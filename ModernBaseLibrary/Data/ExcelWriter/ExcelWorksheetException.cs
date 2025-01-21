//-----------------------------------------------------------------------
// <copyright file="ExcelWorksheetException.cs" company="Lifeprojects.de">
//     Class: ExcelWorksheetException
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
    public sealed class ExcelWorksheetException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWorksheetException"/> class
        /// </summary>
        public ExcelWorksheetException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWorksheetException"/> class
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        public ExcelWorksheetException(string message) : base(message)
        {
        }
    }
}
