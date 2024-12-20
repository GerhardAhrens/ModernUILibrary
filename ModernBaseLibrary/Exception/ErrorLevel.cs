//-----------------------------------------------------------------------
// <copyright file="ErrorLevel.cs" company="PTA">
//     Class: ErrorLevel
//     Copyright © PTA GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@pta.de</email>
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
