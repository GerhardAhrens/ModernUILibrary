//-----------------------------------------------------------------------
// <copyright file="DateRange.cs" company="Lifeprojects.de">
//     Class: DateRange
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>09.08.2017</date>
//
// <summary>Definition of generic DateRange Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;

    public class DateRange : IRange<DateTime>
    {
        public DateRange(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }

        public DateTime Start { get; private set; }

        public DateTime End { get; private set; }

        public bool Includes(DateTime value)
        {
            return (this.Start <= value) && (value <= this.End);
        }

        public bool Includes(IRange<DateTime> range)
        {
            return (this.Start <= range.Start) && (range.End <= this.End);
        }
    }

    public interface IRange<T>
    {
        T Start { get; }

        T End { get; }

        bool Includes(T value);

        bool Includes(IRange<T> range);
    }
}