//-----------------------------------------------------------------------
// <copyright file="SqlCommandDebug.cs" company="Lifeprojects.de">
//     Class: SqlCommandDebug
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.09.2020</date>
//
// <summary>
// Sql Command Logger
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Sql Command Logger
    ///
    /// Example Usage:
    /// SQLiteCommand cmd = SQLCommandExtensions.ResultCommand;
    /// string sqlText = SqlCommandDebug.LogToString("ICM", cmd);
    /// </summary>
    public static class SqlCommandDebug
    {
        /// <summary>
        /// Writes the SQL command text to an output file for debugging purposes.
        /// </summary>
        /// <param name="dbName">The database name.</param>
        /// <param name="sqlCommand">The SQL command to debug.</param>
        /// <param name="outputFile">The debug output file where SQL commands will be generated.</param>
        /// <param name="appendFile">Whether or not to append the command to an existing log file.</param>
        [Conditional("DEBUG")]
        public static void Log(string dbName, SQLiteCommand sqlCommand, string outputFile = "SqlCommandDebug.sql", bool appendFile = true)
        {
            var sb = new StringBuilder();
            sb.Append("use [", dbName, "];\r\n");
            sb.Append(GetDeclareParametersText(sqlCommand));
            sb.Append(GetFormattedCommandText(sqlCommand));

            WriteFile(sb.ToString(), outputFile, "utf-8", appendFile);
        }

        public static string LogToString(string dbName, SQLiteCommand sqlCommand)
        {
            var sb = new StringBuilder();
            sb.Append("use [", dbName, "];\r\n");
            sb.Append(GetDeclareParametersText(sqlCommand));
            sb.Append(GetFormattedCommandText(sqlCommand));

            return sb.ToString();
        }

        private static void WriteFile(string text, string path, string encoding = "utf-8", bool appendFile = true)
        {
            string prependText = string.Format("--{0}-- Date: {1}{2}", Environment.NewLine, DateTime.Now.ToString("dd.MM.yyyy HH:mm"), Environment.NewLine);

            using (StreamWriter writer = new StreamWriter(path, appendFile, Encoding.GetEncoding(encoding)))
            {
                writer.Write("{0}{1}{2}", prependText, text, Environment.NewLine);
            }
        }

        private static string GetFormattedCommandText(IDbCommand sqlCommand)
        {
            var sb = new StringBuilder();
            string commandTextTemp = sqlCommand.CommandText;

            // Make it easier to read and put a line break in front of a particular instruction.
            commandTextTemp.RegexMatches(
                GetDelimitersForNewLine().Select(o => string.Format("{0}{1}{2}", @"(?<!\n)\b", o, @"\b"))
                    .Join("|"), RegexOptions.Multiline).Cast<Match>()
                    .Select(o => o.Value)
                    .Distinct()
                    .ForEach(match => commandTextTemp = commandTextTemp.Replace(match, string.Format("\r\n{0}", match)));

            // Comma-separated per line is easier to read and put a line break, once beyond the three.
            commandTextTemp.SplitReturn()
                .ForEach(line => sb.AppendFormat("{0}\r\n", line.Split(',')
                    .Chunk(3)
                    .Select(o => string.Join(",", o))
                    .Join(",\r\n")));

            return sb.ToString();
        }

        private static string GetDeclareParametersText(SQLiteCommand sqlCommand)
        {
            var sb = new StringBuilder();

            // Create a parameter declaration.
            foreach (SQLiteParameter parameter in sqlCommand.Parameters)
            {
                sb.Append("{0, -50}".Params(GetDeclareParameterText(parameter)), "set @", parameter.ParameterName, " = '", parameter.Value.ToString(), "';\r\n");
            }

            return sb.ToString();
        }

        private static string GetDeclareParameterText(SQLiteParameter parameter)
        {
            if (parameter.Size == 0)
            {
                return "declare @{0} {1};".Params(parameter.ParameterName, parameter.DbType.ToString().ToLower());
            }

            return "declare @{0} {1}({2});".Params(parameter.ParameterName, parameter.DbType.ToString().ToLower(), parameter.Size);
        }

        private static IEnumerable<string> GetDelimitersForNewLine()
        {
            // Line breaks in front of keywords are listed here (add/remove per taste).
            return new[]
            {
            "begin", "commit", "select", "insert into", "values", "update", "delete",
            "from", "inner join", "left outer join", "where", "order by"
        };
        }

        private static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> self, int size)
        {
            while (self.Any())
            {
                yield return self.Take(size);
                self = self.Skip(size);
            }
        }

        private static string Join(this IEnumerable<string> self, string delimiter = ",")
        {
            return string.Join(delimiter, self.ToArray());
        }

        private static MatchCollection RegexMatches(this string self, string pattern, RegexOptions regexOptions)
        {
            return Regex.Matches(self, pattern, regexOptions);
        }

        private static void Append(this StringBuilder self, params string[] strings)
        {
            strings.ForEach(str => self.Append(str));
        }

        private static string Params(this string self, params object[] args)
        {
            return string.Format(self, args);
        }

        private static IEnumerable<string> SplitReturn(this string self)
        {
            return self.Replace("\r\n", "\n").Split('\n');
        }

        private static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (T item in self)
            {
                action(item);
            }
        }
    }
}
