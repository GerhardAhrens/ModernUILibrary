//-----------------------------------------------------------------------
// <copyright file="ExcelRangeException.cs" company="Lifeprojects.de">
//     Class: ExcelRangeException
//     Copyright � Lifeprojects.de 2023
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
    public sealed class ExcelRangeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelRangeException"/> class
        /// </summary>
        public ExcelRangeException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelRangeException"/> class
        /// </summary>
        /// <param name="title">The title<see cref="string"/>.</param>
        /// <param name="message">Message of the exception.</param>
        public ExcelRangeException(string title, string message) : base(message)
        {
        }
    }
}
