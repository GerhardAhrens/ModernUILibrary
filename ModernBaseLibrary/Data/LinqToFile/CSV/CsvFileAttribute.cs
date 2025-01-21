//-----------------------------------------------------------------------
// <copyright file="CsvFileAttribute.cs" company="Lifeprojects.de">
//     Class: CsvFileAttribute
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.12.2017</date>
//
// <summary>
//      Attributes Class for CSV File
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Class)]
    public class CsvFileAttribute : FileAttributeBase
    {
        private Regex regex = null;
        private StreamReader streamReader = null;

        public CsvFileAttribute(bool hasHeader, char textDelimiter, char fieldDelimiter)
        {
            this.HasHeader = hasHeader;
            this.TextDemimiter = textDelimiter;
            this.FieldDemimiter = fieldDelimiter;
        }

        public bool HasHeader { get; private set; }

        public char TextDemimiter { get; private set; }

        public char FieldDemimiter { get; private set; }

        public override IFileParser<T> GetLineParser<T>(Stream stream)
        {
            return new CsvFileParser<T>(
                this.HasHeader,
                this.TextDemimiter,
                this.FieldDemimiter,
                typeof(T)
                    .GetProperties()
                    .Select(i => new
                    {
                        PropertyInfo = i,
                        Attribute = i
                                .GetCustomAttributes(typeof(CsvFileFieldAttribute), true)
                                .OfType<CsvFileFieldAttribute>()
                                .FirstOrDefault()
                    })
                    .Where(i => i.PropertyInfo.CanWrite && i.PropertyInfo.CanRead && i.Attribute != null)
                    .Select(i => new
                        CsvFileMetadata
                    {
                        PropertyInfo = i.PropertyInfo,
                        Column = i.Attribute.Column,
                        DateTimeFormat = i.Attribute.DateTimeFormat,
                        NumberFormatProvider = i.Attribute.GetNumberFormat(),
                        Name = string.IsNullOrWhiteSpace(i.Attribute.Name) ? i.PropertyInfo.Name : i.Attribute.Name,
                        Type = Type.GetTypeCode(i.PropertyInfo.PropertyType)
                    })
                    .ToList(),
                stream);
        }

        public override List<string> CheckMetadata(Stream stream)
        {
            List<string> collection = new List<string>();
            string sep = this.FieldDemimiter.ToString();
            string tq = this.TextDemimiter.ToString();

            this.regex = new Regex(sep + @"(?=(?:[^" + tq + "]*" + tq + "[^" + tq + "]*" + tq + ")*(?![^" + tq + "]*" + tq + "))", RegexOptions.Compiled | RegexOptions.Singleline);
            this.streamReader = new StreamReader(stream);
            if (this.HasHeader == true)
            {
                collection = this.ParseCsvLine(this.streamReader.ReadLine());
            }

            return collection;
        }

        private List<string> ParseCsvLine(string line)
        {
            List<string> cells = new List<string>();
            foreach (string cell in this.regex.Split(line))
            {
                string newCell = cell;
                if (cell.StartsWith(this.TextDemimiter.ToString()) && cell.EndsWith(this.TextDemimiter.ToString()))
                {
                    newCell = cell.Substring(1, cell.Length - 2).Replace("\"\"", "\"");
                }

                cells.Add(newCell);
            }

            return cells;
        }
    }
}