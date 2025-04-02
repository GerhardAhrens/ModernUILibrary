//-----------------------------------------------------------------------
// <copyright file="ObjectRuntime.cs" company="PTA GmbH">
//     Class: ObjectRuntime
//     Copyright © PTA GmbH 2020
// </copyright>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>17.11.2020</date>
//
// <summary>
// Die Klasse stellt ein Stopwatch Objekt zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernInsideVM.Core
{
    using System;
    using System.Diagnostics;

    public class LoadingViewTime : IDisposable
    {
        private bool disposedClass = false;
        private Stopwatch sw = null;

        public LoadingViewTime()
        {
            this.sw = new Stopwatch();
            this.sw.Start();
        }

        ~LoadingViewTime()
        {
            this.Dispose(false);
        }

        public string Result()
        {
            this.sw.Stop();

            return GetTimeString(this.sw);
        }

        public long ResultTicks()
        {
            this.sw.Stop();

            return this.sw.ElapsedTicks;
        }

        public long ResultMilliseconds()
        {
            this.sw.Stop();

            return this.sw.ElapsedMilliseconds;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposedClass == false)
            {
                if (disposing)
                {
                    /* ManagedResources */
                }

                /* UnmanagedResources */

                this.disposedClass = true;
            }
        }

        private string GetTimeString(Stopwatch @this, int numberofDigits = 1)
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
