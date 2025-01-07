//-----------------------------------------------------------------------
// <copyright file="PerformanceChecker.cs" company="Lifeprojects.de">
//     Class: PerformanceChecker
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>18.09.2018</date>
//
// <summary>Class with PerformanceChecker Definition</summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Core
{
    using System;
    using System.Diagnostics;

    public class PerformanceChecker
    {
        /// <summary>
        /// The description for the checker instance.
        /// </summary>
        private readonly string description;

        /// <summary>
        /// The stop watch.
        /// </summary>
        private readonly Stopwatch watch = new Stopwatch();

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceChecker"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public PerformanceChecker(string description, bool isMilliseconds = false)
        {
            this.description = description;
            this.IsMilliseconds = isMilliseconds;
        }

        public bool IsMilliseconds { get; private set; }

        /// <summary>
        /// Starts the checker.
        /// </summary>
        public void Start()
        {
            watch.Reset();
            watch.Start();
        }

        /// <summary>
        /// The watch reference.
        /// </summary>
        public Stopwatch StopWatch => this.watch;

        public (string, long, TimeSpan) StopResult()
        {
            watch.Stop();
            long milliSec = watch.ElapsedMilliseconds;
            TimeSpan t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
            return (this.description, milliSec, t);
        }

        /// <summary>
        /// Stops the checker.
        /// </summary>
        public void Stop()
        {
            watch.Stop();

            long milliSec = watch.ElapsedMilliseconds;
            if (IsMilliseconds == false)
            {
                TimeSpan t = TimeSpan.FromMilliseconds(milliSec);
                string timeString = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
                Console.WriteLine($"==> {this.description} run for {timeString}");
            }
            else
            {
                Console.WriteLine($"==> {this.description} run for {milliSec} ms");
            }
        }
    }
}
