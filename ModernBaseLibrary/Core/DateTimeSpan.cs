//-----------------------------------------------------------------------
// <copyright file="DateTimeSpan.cs" company="Lifeprojects.de">
//     Class: DateTimeSpan
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>09.08.2017</date>
//
// <summary>Definition of DateTimeSpan Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;

    public struct DateTimeSpan
    {
        private readonly int years;
        private readonly int months;
        private readonly int days;
        private readonly int hours;
        private readonly int minutes;
        private readonly int seconds;
        private readonly int milliseconds;

        public DateTimeSpan(int years, int months, int days, int hours, int minutes, int seconds, int milliseconds)
        {
            this.years = years;
            this.months = months;
            this.days = days;
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            this.milliseconds = milliseconds;
        }

        private enum Phase : int
        {
            Years = 1,
            Months,
            Days,
            Done
        }

        public int Years {
            get { return this.years; }
        }

        public int Months {
            get { return this.months; }
        }

        public int Days {
            get { return this.days; }
        }

        public int Hours {
            get { return this.hours; }
        }

        public int Minutes {
            get { return this.minutes; }
        }

        public int Seconds {
            get { return this.seconds; }
        }

        public int Milliseconds {
            get { return this.milliseconds; }
        }

        public double AsSeconds
        {
            get
            {
                TimeSpan ts = new TimeSpan(this.Days,this.Hours,this.Minutes,this.seconds);
                double totalSeconds = ts.TotalSeconds;

                return totalSeconds;
            }
        }

        public static DateTimeSpan CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                var sub = date1;
                date1 = date2;
                date2 = sub;
            }

            DateTime current = date1;
            int years = 0;
            int months = 0;
            int days = 0;

            Phase phase = Phase.Years;
            DateTimeSpan span = new DateTimeSpan();

            while (phase != Phase.Done)
            {
                switch (phase)
                {
                    case Phase.Years:
                        if (current.AddYears(years + 1) > date2)
                        {
                            phase = Phase.Months;
                            current = current.AddYears(years);
                        }
                        else
                        {
                            years++;
                        }

                        break;

                    case Phase.Months:
                        if (current.AddMonths(months + 1) > date2)
                        {
                            phase = Phase.Days;
                            current = current.AddMonths(months);
                        }
                        else
                        {
                            months++;
                        }

                        break;

                    case Phase.Days:
                        if (current.AddDays(days + 1) > date2)
                        {
                            current = current.AddDays(days);
                            var timespan = date2 - current;
                            span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                            phase = Phase.Done;
                        }
                        else
                        {
                            days++;
                        }

                        break;
                }
            }

            return span;
        }
    }
}