/*
 * <copyright file="DateTimeExtensions.cs" company="Lifeprojects.de">
 *     Class: DateTimeExtensions
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Extensions Class for DateTime Types
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Extension
{
    using System.Globalization;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public static class DateTimeExtensions
    {
        static readonly DateTime Date1970 = new DateTime(1970, 1, 1);

        /// <summary>
        /// Gib die Differnez zwischen Start- und Enddatum unter Berücksichtigung des übergebenen pattern zurück
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="dateTimePattern"></param>
        /// <param name="enddate"></param>
        /// <returns>Wert für die Start-End Differenz</returns>
        public static double DateDiff(this DateTime startdate, string dateTimePattern, DateTime enddate)
        {
            double result = 0;


            TimeSpan tsDiff = new TimeSpan(enddate.Ticks - startdate.Ticks);

            switch (dateTimePattern.ToLower())
            {
                case "year":
                case "yyyy":
                case "yy":
                    result = enddate.Year - startdate.Year;
                    break;

                case "quarter":
                case "qq":
                case "q":
                    const double AvgQuarterDays = 365 / 4;
                    result = Math.Floor(tsDiff.TotalDays / AvgQuarterDays);
                    break;

                case "month":
                case "mm":
                case "m":
                    const double AvgMonthDays = 365 / 12;
                    result = Math.Floor(tsDiff.TotalDays / AvgMonthDays);
                    break;

                case "day":
                case "dd":
                case "d":
                    result = tsDiff.TotalDays;
                    break;

                case "week":
                case "wk":
                case "ww":
                    result = Math.Floor(tsDiff.TotalDays / 7);
                    break;

                case "hour":
                case "hh":
                    result = tsDiff.TotalHours;
                    break;

                case "minute":
                case "mi":
                case "n":
                    result = tsDiff.TotalMinutes;
                    break;

                case "second":
                case "ss":
                case "s":
                    result = tsDiff.TotalSeconds;
                    break;

                case "millisecond":
                case "ms":
                    result = tsDiff.TotalMilliseconds;
                    break;

                default:
                    throw new ArgumentException("You input a invalid 'datepart' parameter.");
            }

            return result;
        }

        /// <summary>
        /// A T extension method that check if the value is between (exclusif) the minValue and maxValue.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between the minValue and maxValue, otherwise false.</returns>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        public static bool Between(this DateTime @this, DateTime minValue, DateTime maxValue)
        {
            return minValue.CompareTo(@this) == -1 && @this.CompareTo(maxValue) == -1;
        }

        /// <summary>
        ///     A T extension method to determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list contains the object, else false.</returns>
        /// ###
        /// <typeparam name="T">Generic type parameter.</typeparam>
        public static bool In(this DateTime @this, params DateTime[] values)
        {
            return Array.IndexOf(values, @this) != -1;
        }

        /// <summary>
        ///     A T extension method to determines whether the object is not equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list doesn't contains the object, else false.</returns>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        public static bool NotIn(this DateTime @this, params DateTime[] values)
        {
            return Array.IndexOf(values, @this) == -1;
        }

        /// <summary>
        /// Round Up <see cref="DateTime"/> to nearest timespan.
        /// </summary>
        /// <param name="this">Input object</param>
        /// <param name="d">Time span unit. Example: TimeSpan.FromMinutes(1) rounded to 1 minute.</param>
        /// <returns></returns>
        public static DateTime RoundUp(this DateTime @this, TimeSpan d)
        {
            return new DateTime(((@this.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        /// <summary>
        /// Round Down <see cref="DateTime"/> to nearest timespan.
        /// </summary>
        /// <param name="this">Input object</param>
        /// <param name="d">Time span unit. Example: TimeSpan.FromMinutes(1) rounded to 1 minute.</param>
        /// <returns></returns>
        public static DateTime RoundDown(this DateTime @this, TimeSpan d)
        {
            return new DateTime((@this.Ticks / d.Ticks) * d.Ticks);
        }

        public static bool InRange(this DateTime @this, DateTime minValue, DateTime maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        public static bool InRange(this DateTime? @this, DateTime startDate, DateTime endDate)
        {
            return @this >= startDate && @this < endDate;
        }

        public static DateTime FirstDayInYear(this DateTime @this)
        {
            return new DateTime(@this.Year, 1, 1);
        }

        public static DateTime LastDayInYear(this DateTime @this)
        {
            return new DateTime(@this.Year, 12, 31);
        }

        public static DateTime NextDay(this DateTime @this)
        {
            return @this + 1.Days();
        }

        public static DateTime PreviousDay(this DateTime @this)
        {
            return @this - 1.Days();
        }

        public static DateTime AddBusinessDays(this DateTime @this, int days)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);

            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    @this = @this.AddDays(sign);
                }
                while (@this.DayOfWeek == DayOfWeek.Saturday || @this.DayOfWeek == DayOfWeek.Sunday);
            }

            return @this;
        }

        public static DateTime EndOfDay(this DateTime @this)
        {
            return new DateTime(@this.Year, @this.Month, @this.Day, 23, 59, 59, 999);
        }

        public static DateTime BeginningOfDay(this DateTime @this)
        {
            return new DateTime(@this.Year, @this.Month, @this.Day, 0, 0, 0, 0);
        }

        public static DateTime NextYear(this DateTime @this)
        {
            var nextYear = @this.Year + 1;
            var numberOfDaysInSameMonthNextYear = DateTime.DaysInMonth(nextYear, @this.Month);

            if (numberOfDaysInSameMonthNextYear < @this.Day)
            {
                var differenceInDays = @this.Day - numberOfDaysInSameMonthNextYear;
                var dateTime = new DateTime(nextYear, @this.Month, numberOfDaysInSameMonthNextYear, @this.Hour, @this.Minute, @this.Second, @this.Millisecond);
                return dateTime + differenceInDays.Days();
            }

            return new DateTime(nextYear, @this.Month, @this.Day, @this.Hour, @this.Minute, @this.Second, @this.Millisecond);
        }

        public static DateTime Default<T>(this T @this, DateTime? defaultValue = null) where T : struct
        {
            DateTime defaultDate;

            if(defaultValue == null)
            {
                defaultDate = new DateTime(1900, 1, 1);
            }
            else
            {
                defaultDate = (DateTime)defaultValue;
            }

            return defaultDate;
        }

        public static DateTime DefaultDate(this DateTime @this)
        {
            return new DateTime(1900, 1, 1);
        }

        public static DateTime DefaultDate(this DateTime? @this)
        {
            return new DateTime(1900, 1, 1);
        }

        public static bool IsDateEmpty(this DateTime @this)
        {
            if (@this == new DateTime(1900,1,1) || @this == new DateTime(1, 1, 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsDateEmpty(this DateTime? @this)
        {
            if (@this == new DateTime(1900, 1, 1) || @this == new DateTime(1, 1, 1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DateTime DateOrDefault(this DateTime? @this)
        {
            if (@this == new DateTime(1900, 1, 1) || @this == new DateTime(1, 1, 1) || @this == null)
            {
                return new DateTime(1900,1,1);
            }
            else
            {
                return (DateTime)@this;
            }
        }

        public static DateTime DateOrDefault(this DateTime @this)
        {
            if (@this == new DateTime(1900, 1, 1) || @this == new DateTime(1, 1, 1))
            {
                return new DateTime(1900, 1, 1);
            }
            else
            {
                return @this;
            }
        }

        public static string ToStringGerman(this DateTime @this)
        {
            return @this.ToString("d", new System.Globalization.CultureInfo("de-DE"));
        }

        public static string ToStringEnglish(this DateTime @this)
        {
            return @this.ToString("d", new System.Globalization.CultureInfo("en-US"));
        }

        public static DateTime Years(this int @this)
        {
            return DateTime.MinValue.AddYears(@this - 1);
        }

        public static DateTime Months(this int @this)
        {
            return DateTime.MinValue.AddMonths(@this - 1);
        }

        public static TimeSpan Weeks(this int @this)
        {
            return new TimeSpan(@this * 7, 0, 0, 0);
        }

        public static TimeSpan Days(this int @this)
        {
            return new TimeSpan(@this, 0, 0, 0);
        }

        public static TimeSpan Hours(this int @this)
        {
            return new TimeSpan(@this, 0, 0);
        }

        public static TimeSpan Minutes(this int @this)
        {
            return new TimeSpan(0, @this, 0);
        }

        public static TimeSpan Seconds(this int @this)
        {
            return new TimeSpan(0, 0, @this);
        }

        public static TimeSpan Milliseconds(this int @this)
        {
            return new TimeSpan(0, 0, 0, 0, @this);
        }

        public static int GetAge(this DateTime @this)
        {
            return GetAge(@this, DateTime.Now);
        }

        public static int GetAge(this DateTime? @this)
        {
            return GetAge(@this, DateTime.Now);
        }

        public static int GetAge(this DateTime @this, DateTime today)
        {
            today.IsArgumentOutOfRange("today", today > @this, "At date can not be before @this");

            bool hadBirthday = @this.Month < today.Month || (@this.Month == today.Month && @this.Day <= today.Day);

            return today.Year - @this.Year - (hadBirthday ? 0 : 1);
        }

        public static int GetAge(this DateTime? @this, DateTime today)
        {
            @this.IsArgumentNull("@this is missing");
            today.IsArgumentOutOfRange("today", today > @this, "At date can not be before @this");

            bool hadBirthday = ((DateTime)@this).Month < today.Month || (((DateTime)@this).Month == today.Month && ((DateTime)@this).Day <= today.Day);

            return today.Year - ((DateTime)@this).Year - (hadBirthday ? 0 : 1);
        }

        public static string ToStringAsHuman(this DateTime @this, CultureInfo cultureInfo)
        {
            TimeSpan ts = DateTime.Now.Subtract(@this);
            DateTime date = DateTime.MinValue + ts;

            if (cultureInfo.Name == "de-DE")
            {
                return ProcessPeriod(date.Year - 1, date.Month - 1, cultureInfo, "Jahr")
                       ?? ProcessPeriod(date.Month - 1, date.Day - 1, cultureInfo, "Monat")
                       ?? ProcessPeriod(date.Day - 1, date.Hour, cultureInfo, "Tag", "Gestern")
                       ?? ProcessPeriod(date.Hour, date.Minute, cultureInfo, "Stunde")
                       ?? ProcessPeriod(date.Minute, date.Second, cultureInfo, "Minute")
                       ?? ProcessPeriod(date.Second, 0, cultureInfo, "Sekunde")
                       ?? "Jetzt";
            }
            else
            {
                return ProcessPeriod(date.Year - 1, date.Month - 1, cultureInfo, "year")
                       ?? ProcessPeriod(date.Month - 1, date.Day - 1, cultureInfo, "month")
                       ?? ProcessPeriod(date.Day - 1, date.Hour, cultureInfo, "day", "Yesterday")
                       ?? ProcessPeriod(date.Hour, date.Minute, cultureInfo, "hour")
                       ?? ProcessPeriod(date.Minute, date.Second, cultureInfo, "minute")
                       ?? ProcessPeriod(date.Second, 0, cultureInfo, "second")
                       ?? "Right now";
            }
        }

        public static string ToStringAsHuman(this DateTime @this)
        {
            CultureInfo CultureInfo = CultureInfo.CurrentCulture;
            return ToStringAsHuman(@this, CultureInfo);
        }

        public static string ToMonthName(this DateTime @this)
        {
            return @this.ToString("MM_MMMM");
        }

        /// <summary>
        /// A DateTime extension method that tomorrows the given this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>Tomorrow date at same time.</returns>
        public static DateTime Tomorrow(this DateTime @this)
        {
            return @this.AddDays(1);
        }

        /// <summary>
        /// A DateTime extension method that yesterdays the given this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>Yesterday date at same time.</returns>
        public static DateTime Yesterday(this DateTime @this)
        {
            return @this.AddDays(-1);
        }

        /// <summary>
        /// A DateTime extension method that return a DateTime with the time set to "00:00:00:000". The first moment of the day.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DateTime of the day with the time set to "00:00:00:000".</returns>
        public static DateTime StartOfDay(this DateTime @this)
        {
            return new DateTime(@this.Year, @this.Month, @this.Day);
        }

        /// <summary>
        /// A DateTime extension method that return a DateTime of the first day of the month with the time set to
        /// "00:00:00:000". The first moment of the first day of the month.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DateTime of the first day of the month with the time set to "00:00:00:000".</returns>
        public static DateTime StartOfMonth(this DateTime @this)
        {
            return new DateTime(@this.Year, @this.Month, 1);
        }

        /// <summary>
        /// A DateTime extension method that starts of week.
        /// </summary>
        /// <param name="dt">The dt to act on.</param>
        /// <param name="startDayOfWeek">(Optional) the start day of week.</param>
        /// <returns>A DateTime.</returns> 
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startDayOfWeek = DayOfWeek.Sunday)
        {
            var start = new DateTime(dt.Year, dt.Month, dt.Day);

            if (start.DayOfWeek != startDayOfWeek)
            {
                int d = startDayOfWeek - start.DayOfWeek;
                if (startDayOfWeek <= start.DayOfWeek)
                {
                    return start.AddDays(d);
                }
                return start.AddDays(-7 + d);
            }

            return start;
        }

        /// <summary>
        /// A DateTime extension method that return a DateTime of the first day of the year with the time set to
        /// "00:00:00:000". The first moment of the first day of the year.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DateTime of the first day of the year with the time set to "00:00:00:000".</returns>
        public static DateTime StartOfYear(this DateTime @this)
        {
            return new DateTime(@this.Year, 1, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://www.c-sharpcorner.com/uploadfile/b942f9/extending-the-datetime-structure-in-C-Sharp-part-ii/
        /// </remarks>
        private static int DateValue(this DateTime @this)
        {
            return @this.Year * 372 + (@this.Month - 1) * 31 + @this.Day - 1;
        }

        /// <summary>
        /// Die Methode gibt die Differnenz zwischen Start-End-Datum als Jahre zurück
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dateEnd"></param>
        /// <returns>Differenz in Jahre</returns>
        public static int YearsBetween(this DateTime @this, DateTime dateEnd)
        {
            return @this.MonthsBetween(dateEnd) / 12;
        }

        public static int YearsBetween(this DateTime @this, DateTime dateEnd, bool includeLastDay)
        {
            return @this.MonthsBetween(dateEnd, includeLastDay) / 12;
        }

        public static int YearsBetween(this DateTime @this, DateTime dateEnd, bool includeLastDay, out int excessMonths)
        {
            int months = @this.MonthsBetween(dateEnd, includeLastDay);
            excessMonths = months % 12;
            return months / 12;
        }

        public static int MonthsBetween(this DateTime @this, DateTime dateEnd)
        {
            int months = (dateEnd.DateValue() - @this.DateValue()) / 31;
            return Math.Abs(months);
        }

        public static int MonthsBetween(this DateTime @this, DateTime dateEnd, bool includeLastDay)
        {
            if (!includeLastDay) return @this.MonthsBetween(dateEnd);
            int days;
            if (dateEnd >= @this)
            {
                days = dateEnd.AddDays(1).DateValue() - @this.DateValue();
            }
            else
            {
                days = @this.AddDays(1).DateValue() - dateEnd.DateValue();
            }

            return days / 31;
        }

        public static int WeeksBetween(this DateTime @this, DateTime dateEnd)
        {
            return @this.DaysBetween(dateEnd) / 7;
        }

        public static int WeeksBetween(this DateTime @this, DateTime dateEnd, bool includeLastDay)
        {
            return @this.DaysBetween(dateEnd, includeLastDay) / 7;
        }

        public static int WeeksBetween(this DateTime @this, DateTime dateEnd, bool includeLastDay, out int excessDays)
        {
            int days = @this.DaysBetween(dateEnd, includeLastDay);
            excessDays = days % 7;
            return days / 7;
        }

        public static int DaysBetween(this DateTime @this, DateTime dateEnd)
        {
            return (dateEnd.Date - @this.Date).Duration().Days;
        }

        public static int DaysBetween(this DateTime @this, DateTime dateEnd, bool includeLastDay)
        {
            int days = @this.DaysBetween(dateEnd);
            if (!includeLastDay) return days;
            return days + 1;
        }

        /// <summary>
        /// Returns whether the DateTime falls on a weekday
        /// </summary>
        /// <param name="date">The date to be processed</param>
        /// <returns>Whether the specified date occurs on a weekday</returns>
        public static bool IsWeekDay(this DateTime date)
        {
            return !date.IsWeekend();
        }

        /// <summary>
        /// 	Indicates whether the specified date is a weekend (Saturday or Sunday).
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>
        /// 	<c>true</c> if the specified date is a weekend; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek.EqualsAny(DayOfWeek.Saturday, DayOfWeek.Sunday);
        }

        /// <summary>
        ///     Indicates whether the specified date is Easter in the Christian calendar.
        /// </summary>
        /// <param name="date">Instance value.</param>
        /// <returns>True if the instance value is a valid Easter Date.</returns>
        public static bool IsEaster(this DateTime date)
        {
            int Y = date.Year;
            int a = Y % 19;
            int b = Y / 100;
            int c = Y % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int L = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * L) / 451;
            int Month = (h + L - 7 * m + 114) / 31;
            int Day = ((h + L - 7 * m + 114) % 31) + 1;

            DateTime dtEasterSunday = new DateTime(Y, Month, Day);

            return date == dtEasterSunday;
        }

        /// <summary>
        /// Gets the week number for a provided date time value based on a specific culture.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="culture">Specific culture</param>
        /// <returns>The week number</returns>
        public static int GetWeekOfYear(this DateTime dateTime, CultureInfo culture)
        {
            var calendar = culture.Calendar;
            var dateTimeFormat = culture.DateTimeFormat;

            return calendar.GetWeekOfYear(dateTime, dateTimeFormat.CalendarWeekRule, dateTimeFormat.FirstDayOfWeek);
        }

        /// <summary>
        /// Gets the week number for a provided date time value based on the current culture settings. 
        /// Uses DefaultCulture from ExtensionMethodSetting
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>The week number</returns>
        public static int GetWeekOfYear(this DateTime dateTime)
        {
            return GetWeekOfYear(dateTime, CultureInfo.CurrentCulture);
        }

        ///<summary>
        ///	Get the number of days within that year.
        ///</summary>
        ///<param name = "year">The year.</param>
        ///<returns>the number of days within that year</returns>
        public static int GetDays(int year)
        {
            return GetDays(year, CultureInfo.CurrentCulture);
        }

        ///<summary>
        ///	Get the number of days within that year. Uses the culture specified.
        ///</summary>
        ///<param name = "year">The year.</param>
        ///<param name="culture">Specific culture</param>
        ///<returns>the number of days within that year</returns>
        public static int GetDays(int year, CultureInfo culture)
        {
            var first = new DateTime(year, 1, 1, culture.Calendar);
            var last = new DateTime(year + 1, 1, 1, culture.Calendar);
            return GetDays(first, last);
        }


        ///<summary>
        ///	Get the number of days within that date year. Allows user to specify culture.
        ///</summary>
        ///<param name = "date">The date.</param>
        ///<param name="culture">Specific culture</param>
        ///<returns>the number of days within that year</returns>
        public static int GetDays(this DateTime date)
        {
            return GetDays(date.Year, CultureInfo.CurrentCulture);
        }

        ///<summary>
        ///	Get the number of days within that date year. Allows user to specify culture
        ///</summary>
        ///<param name = "date">The date.</param>
        ///<param name="culture">Specific culture</param>
        ///<returns>the number of days within that year</returns>
        public static int GetDays(this DateTime date, CultureInfo culture)
        {
            return GetDays(date.Year, culture);
        }

        ///<summary>
        ///	Get the number of days between two dates.
        ///</summary>
        ///<param name = "fromDate">The origin year.</param>
        ///<param name = "toDate">To year</param>
        ///<returns>The number of days between the two years</returns>
        public static int GetDays(this DateTime fromDate, DateTime toDate)
        {
            return Convert.ToInt32(toDate.Subtract(fromDate).TotalDays);
        }

        /// <summary>
        /// Adds the specified amount of weeks (=7 days gregorian calendar) to the passed date value.
        /// </summary>
        /// <param name = "date">The origin date.</param>
        /// <param name = "value">The amount of weeks to be added.</param>
        /// <returns>The enw date value</returns>
        public static DateTime AddWeeks(this DateTime date, int value)
        {
            return date.AddDays(value * 7);
        }

        /// <summary>
        /// Get milliseconds of UNIX area. This is the milliseconds since 1/1/1970
        /// </summary>
        /// <param name = "datetime">Up to which time.</param>
        /// <returns>number of milliseconds.</returns>
        public static long GetMillisecondsSince1970(this DateTime datetime)
        {
            var ts = datetime.Subtract(Date1970);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// Determines whether the date only part of twi DateTime values are equal.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <param name = "dateToCompare">The date to compare with.</param>
        /// <returns>
        /// <c>true</c> if both date values are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDateEqual(this DateTime date, DateTime dateToCompare)
        {
            return (date.Date == dateToCompare.Date);
        }

        /// <summary>
        /// Determines whether the time only part of two DateTime values are equal.
        /// </summary>
        /// <param name = "time">The time.</param>
        /// <param name = "timeToCompare">The time to compare.</param>
        /// <returns>
        /// <c>true</c> if both time values are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTimeEqual(this DateTime time, DateTime timeToCompare)
        {
            return (time.TimeOfDay == timeToCompare.TimeOfDay);
        }

        /// <summary>
        /// Gets the next occurence of the specified weekday within the current week using the current culture.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <returns>The calculated date.</returns>
        /// <example>
        /// 	<code>
        /// 		var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        /// <remarks>
        ///     modified by jtolar to implement culture settings
        /// </remarks>
        public static DateTime GetWeeksWeekday(this DateTime date, DayOfWeek weekday)
        {
            return date.GetWeeksWeekday(weekday, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the next occurence of the specified weekday within the current week using the specified culture.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The calculated date.</returns>
        /// <example>
        /// 	<code>
        /// 		var thisWeeksMonday = DateTime.Now.GetWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        public static DateTime GetWeeksWeekday(this DateTime date, DayOfWeek weekday, CultureInfo cultureInfo)
        {
            var firstDayOfWeek = date.GetFirstDayOfWeek(cultureInfo);
            return firstDayOfWeek.GetNextWeekday(weekday);
        }

        /// <summary>
        /// Gets the next occurence of the specified weekday.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <returns>The calculated date.</returns>
        /// <example>
        /// 	<code>
        /// 		var lastMonday = DateTime.Now.GetNextWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        public static DateTime GetNextWeekday(this DateTime date, DayOfWeek weekday)
        {
            while (date.DayOfWeek != weekday)
                date = date.AddDays(1);
            return date;
        }

        /// <summary>
        /// Gets the previous occurence of the specified weekday.
        /// </summary>
        /// <param name = "date">The base date.</param>
        /// <param name = "weekday">The desired weekday.</param>
        /// <returns>The calculated date.</returns>
        /// <example>
        /// 	<code>
        /// 		var lastMonday = DateTime.Now.GetPreviousWeekday(DayOfWeek.Monday);
        /// 	</code>
        /// </example>
        public static DateTime GetPreviousWeekday(this DateTime date, DayOfWeek weekday)
        {
            while (date.DayOfWeek != weekday)
                date = date.AddDays(-1);
            return date;
        }

        /// <summary>
        /// Gets the first day of the week using the current culture.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>The first day of the week</returns>
        /// <remarks>
        ///     modified by jtolar to implement culture settings
        /// </remarks>
        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            return date.GetFirstDayOfWeek(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the first day of the week using the specified culture.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetFirstDayOfWeek(this DateTime date, CultureInfo cultureInfo)
        {
            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            var firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            while (date.DayOfWeek != firstDayOfWeek)
                date = date.AddDays(-1);

            return date;
        }

        /// <summary>
        /// Gets the last day of the week using the current culture.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>The first day of the week</returns>
        /// <remarks>
        ///     modified by jtolar to implement culture settings
        /// </remarks>
        public static DateTime GetLastDayOfWeek(this DateTime date)
        {
            return date.GetLastDayOfWeek(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Gets the last day of the week using the specified culture.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <param name = "cultureInfo">The culture to determine the first weekday of a week.</param>
        /// <returns>The first day of the week</returns>
        public static DateTime GetLastDayOfWeek(this DateTime date, CultureInfo cultureInfo)
        {
            return date.GetFirstDayOfWeek(cultureInfo).AddDays(6);
        }

        /// <summary>
        /// Returns the first day of the month of the provided date.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>The first day of the month</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Returns the first day of the month of the provided date.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <param name = "dayOfWeek">The desired day of week.</param>
        /// <returns>The first day of the month</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetFirstDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
                dt = dt.AddDays(1);
            return dt;
        }

        /// <summary>
        /// Returns the last day of the month of the provided date.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>The last day of the month.</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, GetCountDaysOfMonth(date));
        }

        /// <summary>
        /// Returns the last day of the month of the provided date.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <param name = "dayOfWeek">The desired day of week.</param>
        /// <returns>The date time</returns>
        public static DateTime GetLastDayOfMonth(this DateTime @this, DayOfWeek dayOfWeek)
        {
            var dt = @this.GetLastDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
            {
                dt = dt.AddDays(-1);
            }

            return dt;
        }

        /// <summary>
        /// Returns the number of days in the month of the provided date.
        /// </summary>
        /// <param name = "date">The date.</param>
        /// <returns>The number of days.</returns>
        public static int GetCountDaysOfMonth(this DateTime date)
        {
            var nextMonth = date.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1).AddDays(-1).Day;
        }

        /// <summary>
        /// Die Methode rundet eine Zeit mit den angegeben Minuten auf oder ab
        /// </summary>
        /// <param name="this"></param>
        /// <param name="minutes"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static DateTime RoundDateTime(this DateTime @this, int minutes, RoundingDirection direction)
        {
            TimeSpan t;

            switch (direction)
            {
                case RoundingDirection.Up:
                    t = (@this.Subtract(DateTime.MinValue)).Add(new TimeSpan(0, minutes, 0));
                    break;
                case RoundingDirection.Down:
                    t = (@this.Subtract(DateTime.MinValue));
                    break;
                default:
                    int halfMinutes = minutes / 2;
                    int secs = 0;
                    if ((minutes - (halfMinutes * 2)) == 1)
                    {
                        secs = 30;
                    }

                    t = (@this.Subtract(DateTime.MinValue)).Add(new TimeSpan(0, halfMinutes, secs));
                    break;
            }

            return DateTime.MinValue.Add(new TimeSpan(0,
                   (((int)t.TotalMinutes) / minutes) * minutes, 0));
        }

        public static DateTime GetDateTimeRounded15MinuteExact(this DateTime @this)
        {
            return @this.RoundDateTime(15, RoundingDirection.Nearest);
        }


        #region Helper Methodes
        private static string ProcessPeriod(int value, int subValue, CultureInfo cultureInfo, string name, string singularName = null)
        {
            string result = string.Empty;
            if (value == 0)
            {
                return null;
            }

            if (value == 1)
            {
                if (string.IsNullOrEmpty(singularName) == false)
                {
                    return singularName;
                }

                string articleSuffix = name[0] == 'h' ? "n" : string.Empty;
                if (cultureInfo.Name == "de-DE")
                {
                    result = subValue == 0 ? $"vor einem {articleSuffix} {name}" : $"Über einem {articleSuffix} {name}";
                }
                else
                {
                    result = subValue == 0 ? $"A {articleSuffix} {name} ago" : $"About {articleSuffix} {name}s ago";
                }

                return result;
            }

            if (cultureInfo.Name == "de-DE")
            {
                return subValue == 0 ? $"vor {value} {name}e" : $"Vor über {value} {name}en";
            }
            else
            {
                return subValue == 0 ? $"{value} {name}s ago" : $"About {value} {name}s ago";
            }
        }
        #endregion Helper Methodes
    }
}