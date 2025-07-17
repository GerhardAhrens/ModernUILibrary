/*
 * <copyright file="ConsoleTable.cs" company="Lifeprojects.de">
 *     Class: ConsoleTable
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>28.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Die Klasse stellt für ein Consolenprogramm eine Tabellenausgabe zur Verfügung
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

namespace ModernConsole.Table
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Erstellen einer Tabelle in einer Consolen Applikation
    /// </summary>
    public class ConsoleTable
    {
        private const string LINEONECHAR = "-";

        public IList<object> Columns { get; set; }

        public IList<object[]> Rows { get; protected set; }

        public ConsoleTableOptions Options { get; protected set; }

        public Type[] ColumnTypes { get; private set; }

        public static HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        };

        public ConsoleTable(params string[] columns)
            : this(new ConsoleTableOptions { Columns = new List<string>(columns) })
        {
        }

        public ConsoleTable(ConsoleTableOptions options)
        {
            this.Options = options ?? throw new ArgumentNullException("options");
            this.Rows = new List<object[]>();
            this.Columns = new List<object>(options.Columns);
        }

        public ConsoleTable AddColumn(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                this.Columns.Add(name);
            }

            return this;
        }

        /// <summary>
        /// Zeilen zu einer Tabelle hinzeufügen
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public ConsoleTable AddRow(params object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (Columns.Any() == false)
            {
                throw new Exception("Please set the columns first");
            }

            if (Columns.Count != values.Length)
            {
                throw new Exception($"The number columns in the row ({Columns.Count}) does not match the values ({values.Length})");
            }

            Rows.Add(values);

            return this;
        }

        public ConsoleTable Configure(Action<ConsoleTableOptions> action)
        {
            action(this.Options);
            return this;
        }

        public static ConsoleTable From<T>(IEnumerable<T> values)
        {
            var table = new ConsoleTable
            {
                ColumnTypes = GetColumnsType<T>().ToArray()
            };

            var columns = GetColumns<T>();

            table.AddColumn(columns);

            foreach (var propertyValues in values.Select(value => columns.Select(column => GetColumnValue<T>(value, column))))
            {
                table.AddRow(propertyValues.ToArray());
            }

            return table;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            // find the longest column by searching each row
            var columnLengths = ColumnLengths();

            // set right alinment if is a number
            var columnAlignment = Enumerable.Range(0, Columns.Count)
                .Select(GetNumberAlignment)
                .ToList();

            // create the string format with padding
            var format = Enumerable.Range(0, Columns.Count)
                .Select(i => " | {" + i + "," + columnAlignment[i] + columnLengths[i] + "}")
                .Aggregate((s, a) => s + a) + " |";

            // find the longest formatted line
            var maxRowLength = Math.Max(0, Rows.Any() ? Rows.Max(row => string.Format(format, row).Length) : 0);
            var columnHeaders = string.Format(format, Columns.ToArray());

            // longest line is greater of formatted columnHeader and longest row
            var longestLine = Math.Max(maxRowLength, columnHeaders.Length);

            // add each row
            var results = Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            var divider = " " + string.Join("", Enumerable.Repeat(LINEONECHAR, longestLine - 1)) + " ";

            builder.AppendLine(divider);
            builder.AppendLine(columnHeaders);

            foreach (var row in results)
            {
                builder.AppendLine(divider);
                builder.AppendLine(row);
            }

            builder.AppendLine(divider);

            if (this.Options.EnableCount)
            {
                builder.AppendLine("");
                builder.AppendFormat(" Count: {0}", Rows.Count);
            }

            return builder.ToString();
        }

        public string ToMarkDownString()
        {
            return ToMarkDownString('|');
        }

        public string ToMinimalString()
        {
            return ToMarkDownString(char.MinValue);
        }

        public string ToStringAlternative()
        {
            var builder = new StringBuilder();

            // find the longest column by searching each row
            var columnLengths = ColumnLengths();

            // create the string format with padding
            var format = Format(columnLengths);

            // find the longest formatted line
            var columnHeaders = string.Format(format, Columns.ToArray());

            // add each row
            var results = Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            var divider = Regex.Replace(columnHeaders, @"[^|]", LINEONECHAR);
            var dividerPlus = divider.Replace("|", "+");

            builder.AppendLine(dividerPlus);
            builder.AppendLine(columnHeaders);

            foreach (var row in results)
            {
                builder.AppendLine(dividerPlus);
                builder.AppendLine(row);
            }
            builder.AppendLine(dividerPlus);

            return builder.ToString();
        }

        public void Show(ConsoleTableFormat format = ConsoleTableFormat.Default)
        {
            switch (format)
            {
                case ConsoleTableFormat.Default:
                    this.Options.OutputTo.WriteLine(this.ToString());
                    break;
                case ConsoleTableFormat.MarkDown:
                    this.Options.OutputTo.WriteLine(this.ToMarkDownString());
                    break;
                case ConsoleTableFormat.Alternative:
                    this.Options.OutputTo.WriteLine(this.ToStringAlternative());
                    break;
                case ConsoleTableFormat.Minimal:
                    this.Options.OutputTo.WriteLine(this.ToMinimalString());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        #region Private Methodes
        private string ToMarkDownString(char delimiter)
        {
            var builder = new StringBuilder();

            // find the longest column by searching each row
            var columnLengths = ColumnLengths();

            // create the string format with padding
            var format = Format(columnLengths, delimiter);

            // find the longest formatted line
            var columnHeaders = string.Format(format, Columns.ToArray());

            // add each row
            var results = Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            var divider = Regex.Replace(columnHeaders, @"[^|]", LINEONECHAR);

            builder.AppendLine(columnHeaders);
            builder.AppendLine(divider);
            results.ForEach(row => builder.AppendLine(row));

            return builder.ToString();
        }

        private string Format(List<int> columnLengths, char delimiter = '|')
        {
            // set right alinment if is a number
            var columnAlignment = Enumerable.Range(0, Columns.Count)
                .Select(GetNumberAlignment)
                .ToList();

            var delimiterStr = delimiter == char.MinValue ? string.Empty : delimiter.ToString();
            var format = (Enumerable.Range(0, Columns.Count)
                .Select(i => " " + delimiterStr + " {" + i + "," + columnAlignment[i] + columnLengths[i] + "}")
                .Aggregate((s, a) => s + a) + " " + delimiterStr).Trim();

            return format;
        }

        private string GetNumberAlignment(int i)
        {
            return Options.NumberAlignment == ConsoleTableAlignment.Right
                    && ColumnTypes != null
                    && NumericTypes.Contains(ColumnTypes[i]) ? string.Empty : LINEONECHAR;
        }

        private List<int> ColumnLengths()
        {
            var columnLengths = Columns
                .Select((t, i) => Rows.Select(x => x[i])
                    .Union(new[] { Columns[i] })
                    .Where(x => x != null)
                    .Select(x => x.ToString().Length).Max())
                .ToList();

            return columnLengths;
        }

        private static IEnumerable<string> GetColumns<T>()
        {
            return typeof(T).GetProperties().Select(x => x.Name).ToArray();
        }

        private static object GetColumnValue<T>(object target, string column)
        {
            return typeof(T).GetProperty(column).GetValue(target, null);
        }

        private static IEnumerable<Type> GetColumnsType<T>()
        {
            return typeof(T).GetProperties().Select(x => x.PropertyType).ToArray();
        }
        #endregion Private Methodes
    }
}
