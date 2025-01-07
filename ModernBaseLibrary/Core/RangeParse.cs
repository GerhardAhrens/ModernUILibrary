//-----------------------------------------------------------------------
// <copyright file="Range.cs" company="Lifeprojects.de">
//     Class: Range
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.09.2020</date>
//
// <summary>
// Die Klasse vom Typ Range<T> wertet einen Bereich von <T> aus.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public sealed class RangeParse
    {
        private readonly static char[] Separators = { ',' };

        public static List<int> Explode(int from, int to)
        {
            return Enumerable.Range(from, (to - from) + 1).ToList();
        }

        public static List<int> Interpret(string input)
        {
            var result = new List<int>();
            var values = input.Split(Separators);

            string rangePattern = @"(?<range>(?<from>\d+)-(?<to>\d+))";
            var regex = new Regex(rangePattern);

            foreach (string value in values)
            {
                var match = regex.Match(value);
                if (match.Success == true)
                {
                    var from = Parse(match.Groups["from"].Value);
                    var to = Parse(match.Groups["to"].Value);
                    result.AddRange(Explode(from, to));
                }
                else
                {
                    result.Add(Parse(value));
                }
            }

            return result;
        }

        /// <summary>
        /// Split this out to allow custom throw etc
        /// </summary>
        private static int Parse(string value)
        {
            int output;
            var ok = int.TryParse(value, out output);
            if (!ok)
            {
                throw new FormatException($"Failed to parse '{value}' as an integer");
            }

            return output;
        }
    }
}
