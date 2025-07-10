//-----------------------------------------------------------------------
// <copyright file="ErrorLevel.cs" company="Lifeprojects.de">
//     Class: ErrorLevel
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>14.07.2020</date>
//
// <summary>
// Enum Class with ErrorLevel for BaseException
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System.ComponentModel;

    public enum ErrorLevel
    {
        [Description("Information")]
        Information,
        [Description("Warnung")]
        Warning,
        [Description("Fehler")]
        Error,
        [Description("Schwerer Fehler")]
        Fatal,
        [Description("Unbekannt")]
        Undefined
    }
}
