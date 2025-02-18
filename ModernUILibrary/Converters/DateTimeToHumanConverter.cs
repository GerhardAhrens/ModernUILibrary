//-----------------------------------------------------------------------
// <copyright file="DateTimeToHumanConverter.cs" company="Lifeprojects.de">
//     Class: DateTimeToHumanConverter
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.09.2019</date>
//
// <summary>Definition of DateTimeToHumanConverter Class</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using ModernBaseLibrary.Core;

    public class DateTimeToHumanConverter : DisposableCoreBase
    {
        public string Get(DateTime datetime, CultureInfo cultureInfo)
        {
            TimeSpan ts = DateTime.Now.Subtract(datetime);
            DateTime date = DateTime.MinValue + ts;

            if (cultureInfo.Name == "de-DE")
            {
                return this.ProcessPeriod(date.Year - 1, date.Month - 1, cultureInfo, "Jahr")
                       ?? this.ProcessPeriod(date.Month - 1, date.Day - 1, cultureInfo, "Monat")
                       ?? this.ProcessPeriod(date.Day - 1, date.Hour, cultureInfo, "Tag", "Gestern")
                       ?? this.ProcessPeriod(date.Hour, date.Minute, cultureInfo, "Stunde")
                       ?? this.ProcessPeriod(date.Minute, date.Second, cultureInfo, "Minute")
                       ?? this.ProcessPeriod(date.Second, 0, cultureInfo, "Sekunde")
                       ?? "Jetzt";
            }
            else
            {
                return this.ProcessPeriod(date.Year - 1, date.Month - 1, cultureInfo, "year")
                       ?? this.ProcessPeriod(date.Month - 1, date.Day - 1, cultureInfo, "month")
                       ?? this.ProcessPeriod(date.Day - 1, date.Hour, cultureInfo, "day", "Yesterday")
                       ?? this.ProcessPeriod(date.Hour, date.Minute, cultureInfo, "hour")
                       ?? this.ProcessPeriod(date.Minute, date.Second, cultureInfo, "minute")
                       ?? this.ProcessPeriod(date.Second, 0, cultureInfo, "second")
                       ?? "Right now";
            }
        }

        public string Get(DateTime datetime)
        {
            CultureInfo CultureInfo = CultureInfo.CurrentCulture;
            return this.Get(datetime, CultureInfo);
        }

        public string FormatTimeSpan(TimeSpan timeSpan)
        {
            Func<Tuple<int, string>, string> tupleFormatter = t => $"{t.Item1} {t.Item2}{(t.Item1 == 1 ? string.Empty : this.GetLanguagePlural())}";
            List<Tuple<int, string>> components = null;

            if (components == null)
            {
                components = new List<Tuple<int, string>>();
                if (CultureInfo.CurrentCulture.Name == "de-DE")
                {
                    components.Add(Tuple.Create((int)timeSpan.TotalDays, "Tag"));
                    components.Add(Tuple.Create(timeSpan.Hours, "Stunde"));
                    components.Add(Tuple.Create(timeSpan.Minutes, "Minute"));
                    components.Add(Tuple.Create(timeSpan.Seconds, "Sekunde"));
                }
                else
                {
                    components.Add(Tuple.Create((int)timeSpan.TotalDays, "day"));
                    components.Add(Tuple.Create(timeSpan.Hours, "hour"));
                    components.Add(Tuple.Create(timeSpan.Minutes, "minute"));
                    components.Add(Tuple.Create(timeSpan.Seconds, "second"));
                }
            }

            components.RemoveAll(i => i.Item1 == 0);

            string extra = "";

            if (components.Count > 1)
            {
                var finalComponent = components[components.Count - 1];
                components.RemoveAt(components.Count - 1);
                extra = $" und {tupleFormatter(finalComponent)}";
            }

            return $"{string.Join(", ", components.Select(tupleFormatter))}{extra}";
        }

        private string ProcessPeriod(int value, int subValue, CultureInfo cultureInfo, string name, string singularName = null)
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
                return subValue == 0 ? $"vor {value} {name}e" : $"Vor über {value} {name}e";
            }
            else
            {
                return subValue == 0 ? $"{value} {name}s ago" : $"About {value} {name}s ago";
            }
        }

        private string GetLanguagePlural()
        {
            if (CultureInfo.CurrentCulture.Name == "de-DE")
            {
                return "n";
            }
            else
            {
                return "s";
            }
        }

        private string GetReadableTimespan(TimeSpan ts)
        {
            // formats and its cutoffs based on totalseconds
            var cutoff = new SortedList<long, string> {
                {59, "{3:S}" },
                {60, "{2:M}" },
                {60*60-1, "{2:M}, {3:S}"},
                {60*60, "{1:H}"},
                {24*60*60-1, "{1:H}, {2:M}"},
                {24*60*60, "{0:D}"},
                {Int64.MaxValue , "{0:D}, {1:H}"}};

            // find nearest best match
            var find = cutoff.Keys.ToList().BinarySearch((long)ts.TotalSeconds);

            // negative values indicate a nearest match
            var near = find < 0 ? Math.Abs(find) - 1 : find;

            // use custom formatter to get the string
            string timeSpanFormat = String.Format(
                new HumanDateTimeFormat(),
                cutoff[cutoff.Keys[near]],
                ts.Days,
                ts.Hours,
                ts.Minutes,
                ts.Seconds);

            return timeSpanFormat;
        }
    }
}
