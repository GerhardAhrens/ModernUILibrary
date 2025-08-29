//-----------------------------------------------------------------------
// <copyright file="Humanizer.cs" company="www.pta.de">
//     Class: Humanizer
//     Copyright © www.pta.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>08.08.2025 13:38:19</date>
//
// <summary>
// Klasse für gibt einen string für das Plural bzw. Singular einer Menge zurück. 
// Die Segmente werden von links nach rechts gezählt. [0/1/>1]. Maximal zwei Sgemnete
// </summary>
// <Remark>
// string aa1 = Humanizer.Get("Bereit: [kein/ein/{0}] [Datensatz/Datensätze/Datensätze]", 0);
// string aa2 = Humanizer.Get("Bereit: [kein/ein/{0}] [Datensatz/Datensätze/Datensätze]", 1);
// string aa3 = Humanizer.Get("Bereit: [kein/ein/{0}] [Datensatz/Datensätze/Datensätze]", 2);
//
// string del1 = Humanizer.Get("[Es wurde kein Datensatz zum löschen gefunden/Soll der gefundene Datensatz gelöscht werden/Sollen die gefunden Datensätze '{0}' gelöscht werden]", 0);
// string del2 = Humanizer.Get("[Es wurde kein Datensatz zum löschen gefunden/Soll der gefundene Datensatz gelöscht werden/Sollen die gefunden Datensätze '{0}' gelöscht werden]", 1);
// string del3 = Humanizer.Get("[Es wurde kein Datensatz zum löschen gefunden/Soll der gefundene Datensatz gelöscht werden/Sollen die gefunden Datensätze '{0}' gelöscht werden]", 2);
// string var1 = Humanizer.Get("[Es ist kein Datensatz vorhanden/Es sind mehrere Datenätze vorhanden]",0);
// string var1 = Humanizer.Get("[Es ist kein Datensatz vorhanden/Es sind mehrere Datenätze vorhanden]",2);
// </Remark>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class Humanizer
    {
        public static string Get(string msg, int args)
        {
            string result = string.Empty;

            List<string> msgSource = msg.ExtractFromString("[", "]").ToList();
            if (msgSource != null)
            {
                int countSegments = msgSource.Count;
                if (countSegments == 0 || countSegments > 2)
                {
                    return "Fehler im Textsegment!!";
                }
                else if (countSegments == 1)
                {
                    string[] segmentText = msgSource[0].Split('/');
                    if (args == 0)
                    {
                        result = String.Format(CultureInfo.CurrentCulture, $"{segmentText.First()}", args);
                    }
                    else if (args == 1)
                    {
                        if (segmentText.Length < 2)
                        {
                            result = String.Format(CultureInfo.CurrentCulture, $"{segmentText.Last()}", args);
                        }
                        else
                        {
                            result = String.Format(CultureInfo.CurrentCulture, $"{segmentText[1]}", args);
                        }
                    }
                    else if (args > 1)
                    {
                        result = String.Format(CultureInfo.CurrentCulture, $"{segmentText.Last()}", args);
                    }
                }
                else if (countSegments == 2)
                {
                    string[] segmentTextA = msgSource[0].Split('/');
                    string[] segmentTextB = msgSource[1].Split('/');
                    if (args == 0)
                    {
                        result = String.Format(CultureInfo.CurrentCulture, $"{segmentTextA.First()} {segmentTextB.First()}", args);
                    }
                    else if (args == 1)
                    {
                        if (segmentTextA.Length < 2 && segmentTextB.Length < 2)
                        {
                            result = String.Format(CultureInfo.CurrentCulture, $"{segmentTextA.Last()} {segmentTextB.Last()}", args);
                        }
                        else
                        {
                            result = String.Format(CultureInfo.CurrentCulture, $"{segmentTextA[1]} {segmentTextB[1]}", args);
                        }
                    }
                    else if (args > 1)
                    {
                        result = String.Format(CultureInfo.CurrentCulture, $"{segmentTextA.Last()} {segmentTextB.Last()}", args);
                    }
                }
            }


            return result;
        }
    }

    internal static class StringExtractExtensions
    {
        public static IEnumerable<string> ExtractFromString(this string @this, string startString, string endString)
        {
            if (@this == null || startString == null || endString == null)
            {
                yield return null;
            }

            Regex r = new Regex(Regex.Escape(startString) + "(.*?)" + Regex.Escape(endString));
            MatchCollection matches = r.Matches(@this);
            foreach (Match match in matches)
            {
                yield return match.Groups[1].Value;
            }
        }

        public static T[] ExtractContent<T>(this string @this, string regex)
        {
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
            if (tc.CanConvertFrom(typeof(string)) == false)
            {
                throw new ArgumentException("Type does not have a TypeConverter from string", "T");
            }
            if (string.IsNullOrEmpty(@this) == false)
            {
                return
                    Regex.Matches(@this, regex)
                    .Cast<Match>()
                    .Select(f => f.ToString())
                    .Select(f => (T)tc.ConvertFrom(f))
                    .ToArray();
            }
            else
            {
                return [];
            }
        }

        public static int[] ExtractInts(this string @this)
        {
            return @this.ExtractContent<int>(@"\d+");
        }
    }
}
