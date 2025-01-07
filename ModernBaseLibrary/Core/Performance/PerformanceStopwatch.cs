//-----------------------------------------------------------------------
// <copyright file="PerformanceStopwatch.cs" company="Lifeprojects.de">
//     Class: PerformanceStopwatch
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.02.2023</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Versioning;

    /// <summary>
    /// <see cref="Stopwatch"/> with added features.
    /// </summary>
    /// <seealso cref="Stopwatch" />
    [SupportedOSPlatform("windows")]
    [ExcludeFromCodeCoverage]
    public class PerformanceStopwatch : Stopwatch
    {
        public static new PerformanceStopwatch StartNew()
        {
            var sw = TypeHelper.Create<PerformanceStopwatch>();
            sw.Start();

            return sw;
        }

        public TimeSpan StopReset()
        {
            this.Stop();
            var result = this.Elapsed;
            this.Reset();

            return result;
        }
        public TimeSpan StopRestart()
        {
            var result = this.Elapsed;

            this.Restart();

            return result;
        }
    }
}
