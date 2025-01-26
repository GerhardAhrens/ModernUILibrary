//-----------------------------------------------------------------------
// <copyright file="HumanDateTimeSpan.cs" company="Lifeprojects.de">
//     Class: HumanDateTimeSpan
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.03.2023</date>
//
// <summary>
// Klasse erstellt für eine DateiTime oder TimeSpan einen entsprechenden String zu Zeitdarstellung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Text
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using ModernBaseLibrary.Core;

    public class HumanDateTimeSpan : DisposableCoreBase
    {
        public HumanDateTimeSpan(CultureInfo cultureInfo)
        {
            this.CultureInfo = cultureInfo;
        }

        public HumanDateTimeSpan()
        {
            this.CultureInfo = CultureInfo.CurrentCulture;
        }

        private CultureInfo CultureInfo { get; set; }

        public string ToStringAsHuman(TimeSpan timeSpan)
        {
            Func<Tuple<int, string>, string> tupleFormatter = t => $"{t.Item1} {t.Item2}{(t.Item1 == 1 ? string.Empty : GetLanguagePlural())}";
            List<Tuple<int, string>> components = null;

            if (components == null)
            {
                components = new List<Tuple<int, string>>();
                if (this.CultureInfo.Name == "de-DE")
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

        public string ToStringAsHuman(DateTime datetime)
        {
            TimeSpan ts = DateTime.Now.Subtract(datetime);
            DateTime date = DateTime.MinValue + ts;

            if (this.CultureInfo.Name == "de-DE")
            {
                return ProcessPeriod(date.Year - 1, date.Month - 1, "Jahr")
                       ?? ProcessPeriod(date.Month - 1, date.Day - 1, "Monat")
                       ?? ProcessPeriod(date.Day - 1, date.Hour, "Tag", "Gestern")
                       ?? ProcessPeriod(date.Hour, date.Minute, "Stunde")
                       ?? ProcessPeriod(date.Minute, date.Second, "Minute")
                       ?? ProcessPeriod(date.Second, 0, "Sekunde")
                       ?? "Jetzt";
            }
            else
            {
                return ProcessPeriod(date.Year - 1, date.Month - 1, "year")
                       ?? ProcessPeriod(date.Month - 1, date.Day - 1, "month")
                       ?? ProcessPeriod(date.Day - 1, date.Hour, "day", "Yesterday")
                       ?? ProcessPeriod(date.Hour, date.Minute, "hour")
                       ?? ProcessPeriod(date.Minute, date.Second, "minute")
                       ?? ProcessPeriod(date.Second, 0, "second")
                       ?? "Right now";
            }
        }

        private string GetLanguagePlural()
        {
            if (this.CultureInfo.Name == "de-DE")
            {
                return "n";
            }
            else
            {
                return "s";
            }
        }

        private string ProcessPeriod(int value, int subValue, string name, string singularName = null)
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
                if (this.CultureInfo.Name == "de-DE")
                {
                    if (name.ToUpper() == "SEKUNDE" || name.ToUpper() == "MINUTE" || name.ToUpper() == "STUNDE")
                    {
                        result = subValue == 0 ? $"vor einer {articleSuffix.Trim()}{name}" : $"Über einer {articleSuffix.Trim()} {name}";
                    }
                    else
                    {
                        result = subValue == 0 ? $"vor einem {articleSuffix.Trim()}{name}" : $"Über einem {articleSuffix.Trim()} {name}";
                    }
                }
                else
                {
                    result = subValue == 0 ? $"A {articleSuffix.Trim()} {name} ago" : $"About {articleSuffix.Trim()} {name}s ago";
                }

                return result;
            }

            if (this.CultureInfo.Name == "de-DE")
            {
                if (name.ToUpper() == "SEKUNDE" || name.ToUpper() == "MINUTE" || name.ToUpper() == "STUNDE")
                {
                    return subValue == 0 ? $"vor {value} {name}n" : $"Vor über {value} {name}n";
                }
                else if (name.ToUpper() == "TAG")
                {
                    return subValue == 0 ? $"vor {value} {name}en" : $"Vor über {value} {name}en";
                }
                else
                {
                    return subValue == 0 ? $"vor {value} {name}e" : $"Vor über {value} {name}e";
                }
            }
            else
            {
                return subValue == 0 ? $"{value} {name}s ago" : $"About {value} {name}s ago";
            }
        }
    }
}
