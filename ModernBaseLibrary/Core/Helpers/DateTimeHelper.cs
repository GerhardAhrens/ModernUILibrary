//-----------------------------------------------------------------------
// <copyright file="DateTimeHelper.cs" company="Lifeprojects.de">
//     Class: DateTimeHelper
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>08.12.2022</date>
//
// <summary>
// Helper Klasse zum Tp DateTime
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public class DateTimeHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeHelper"/> class.
        /// </summary>
        public DateTimeHelper()
        {
        }

        public static int GetQuarter(DateTime date)
        {
            return (date.Month + 2) / 3;
        }

        public static string GetQuarterString(DateTime date)
        {
            return $"{(date.Month + 2) / 3}. Quartal {date.Year}";
        }

        public static int GetFinancialQuarter(DateTime date)
        {
            return (date.AddMonths(-3).Month + 2) / 3;
        }

        public static string GetFinancialQuarterString(DateTime date)
        {
            return $"{(date.AddMonths(-3).Month + 2) / 3}. Quartal {date.Year}";
        }

        public static IEnumerable<int> GetQuartersAsInt(DateTime from, DateTime to)
        {
            if (to < from)
            {
                throw new ArgumentException($"{to} cannot be smaller than {from}", nameof(to));
            }

            DateTime date = from;
            int lastQuarter = -1;
            while (date <= to)
            {
                int currentQuarter = (date.Month + 2) / 3;
                if (currentQuarter != lastQuarter)
                {
                    yield return currentQuarter;
                }

                date = date.AddDays(1);
                lastQuarter = currentQuarter;
            }
        }

        public static IEnumerable<string> GetQuartersAsString(DateTime from, DateTime to, bool formatLong = false )
        {
            if (to < from)
            {
                throw new ArgumentException($"{to} cannot be smaller than {from}", nameof(to));
            }

            DateTime date = from;
            int lastQuarter = -1;
            while (date <= to)
            {
                int currentQuarter = (date.Month + 2) / 3;
                if (currentQuarter != lastQuarter)
                {
                    if (formatLong == false)
                    {
                        yield return $"{currentQuarter}.{date.Year}";
                    }
                    else
                    {
                        yield return $"{currentQuarter}. Quartal {date.Year}";
                    }
                }

                date = date.AddDays(1);
                lastQuarter = currentQuarter;
            }
        }

        public static Dictionary<DateTime, string> GetQuartersAsDictionary(DateTime from, DateTime to, bool formatLong = false)
        {
            if (to < from)
            {
                throw new ArgumentException($"{to} cannot be smaller than {from}", nameof(to));
            }

            Dictionary<DateTime,string> quartals= new Dictionary<DateTime,string>();

            DateTime date = from;
            int lastQuarter = -1;
            while (date <= to)
            {
                int currentQuarter = (date.Month + 2) / 3;
                if (currentQuarter != lastQuarter)
                {
                    if (formatLong == false)
                    {
                        quartals.Add(date, $"{currentQuarter}.{date.Year}");
                    }
                    else
                    {
                        quartals.Add(date, $"{currentQuarter}. Quartal {date.Year}");
                    }
                }

                date = date.AddDays(1);
                lastQuarter = currentQuarter;
            }

            return quartals;
        }

        public static int CalendarWeek(DateTime currentDate)
        {
            return CalendarWeek((ushort)currentDate.Day, (ushort)currentDate.Month, (ushort)currentDate.Year);
        }


        /// <summary>
        /// Calendar week of a given date (ISO 8601)
        /// </summary>
        /// <param name="Day">Day of date</param>
        /// <param name="Month">Monthof date</param>
        /// <param name="Year">Year of date</param>
        /// <returns>Number of calender week</returns>
        public static int CalendarWeek(ushort Day, ushort Month, ushort Year)
        {
            ushort[] days = new ushort[] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            short result = (short)(days[Month - 1] + Day);
            short daysInYear = 365;

            if (DateTime.IsLeapYear(Year) && Month > 1)
            {
                result++;
                daysInYear++;
            }

            DateTime dt = new DateTime(Year, 1, 1);
            byte firstDow = (byte)dt.DayOfWeek;
            if (firstDow == 0)
            {
                firstDow = 7;
            }

            dt = dt.AddDays(result - 1);// Now current date
            byte curDow = (byte)dt.DayOfWeek;

            if (curDow == 0)
            {
                curDow = 7;
            }

            int week = (result - curDow + 14 - firstDow) / 7;
            if (week == 0)
            {
                week = CalendarWeek((byte)31, (byte)12, (ushort)(Year - 1));// From previous year
            }

            if (daysInYear - result < 3)
            {
                byte lastDow = (byte)new DateTime(Year, 12, 31).DayOfWeek;
                if (lastDow == 0) lastDow = 7;
                if (lastDow >= curDow && lastDow <= 3)
                {
                    week = 1;
                }
            }

            return (byte)week;
        }

        /// <summary>
        /// Ostern in einem Jahr ermitteln
        /// </summary>
        /// <param name="year">Jahr</param>
        /// <returns></returns>
        public static DateTime Easter(int year)
        {
            // 1997 von H. Lichtenberg modifizierte Gaußsche Osterformel
            // Wikipedia: http://de.wikipedia.org/wiki/Gau%C3%9Fsche_Osterformel
            int K = year / 100;// die Säkularzahl
            int M = 15 + (3 * K + 3) / 4 - (8 * K + 13) / 25;// die säkulare Mondschaltung
            int S = 2 - (3 * K + 3) / 4;// die säkulare Sonnenschaltung
            int A = year % 19;// den Mondparameter
            int D = (19 * A + M) % 30;// den Keim für den ersten Vollmond im Frühling
            int R = D / 29 + (D / 28 - D / 29) * (A / 11);// die kalendarische Korrekturgröße
            int OG = 21 + D - R;// die Ostergrenze
            int SZ = 7 - (year + year / 4 + S) % 7;// den ersten Sonntag im März
            int OE = 7 - (OG - SZ) % 7;// die Entfernung in Tagen, die der Ostersonntag von der Ostergrenze hat (Osterentfernung)
            int OS = OG + OE;// das Datum des Ostersonntags als Märzdatum (32. März = 1. April etc.)
            int month = 3;
            if (OS > 31) { OS -= 31; month++; }
            DateTime result = new DateTime(year, month, (byte)OS);
            return result;
        }
    }
}
