//-----------------------------------------------------------------------
// <copyright file="StopwatchExtension.cs" company="Lifeprojects.de">
//     Class: StopwatchExtension
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.08.2020</date>
//
// <summary>Class for Stopwatch Extension</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Diagnostics;

    public static class StopwatchExtension
    {
        public static string GetTimeString(this Stopwatch @this, int numberofDigits = 1)
        {
            double time = @this.ElapsedTicks / (double)Stopwatch.Frequency;
            if (time > 1)
            {
                return Math.Round(time, numberofDigits) + " s";
            }

            if (time > 1e-3)
            {
                return Math.Round(1e3 * time, numberofDigits) + " ms";
            }

            if (time > 1e-6)
            {
                return Math.Round(1e6 * time, numberofDigits) + " µs";
            }

            if (time > 1e-9)
            {
                return Math.Round(1e9 * time, numberofDigits) + " ns";
            }

            return @this.ElapsedTicks + " ticks";
        }
    }
}
