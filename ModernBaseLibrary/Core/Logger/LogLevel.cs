//-----------------------------------------------------------------------
// <copyright file="LogLevel.cs" company="Lifeprojects.de">
//     Class: LogLevel
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2022</date>
//
// <summary>Enum Definition of Logger Level</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Logger
{
    using System.ComponentModel;

    [DefaultValue(NOTSET)]
    public enum LogLevel : int
    {
        [Description("Kritische Fehler")]
        CRITICAL = 50,
        [Description("Fehler")]
        ERROR = 40,
        [Description("Warnungen")]
        WARNING = 30,
        [Description("Allgemeine Informationen")]
        INFO = 20,
        [Description("Debug-Informationen")]
        DEBUG = 10,
        [Description("Kein Logger aktiv")]
        NOTSET = 0,
    }
}
