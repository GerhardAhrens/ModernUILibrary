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
    public enum LogLevel : int
    {
        CRITICAL = 50,
        ERROR = 40,
        WARNING = 30,
        INFO = 20,
        DEBUG = 10,
        NOTSET = 0,
    }
}
