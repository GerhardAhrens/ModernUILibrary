//-----------------------------------------------------------------------
// <copyright file="LogLevelHelper.cs" company="Lifeprojects.de">
//     Class: LogLevelHelper
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>05.05.2025 10:12:04</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Logger
{
    using System.Collections.Generic;

    using ModernBaseLibrary.Extension;

    public static class LogLevelHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogLevelHelper"/> class.
        /// </summary>
        static LogLevelHelper()
        {
        }

        public static Dictionary<int, string> LogLevelToSource()
        {
            LogLevel none = LogLevel.NOTSET;
            Dictionary<int, string> result = none.ToDictionary<LogLevel>(true);
            return result;
        }
    }
}
