//-----------------------------------------------------------------------
// <copyright file="CsvFileFieldAttribute.cs" company="Lifeprojects.de">
//     Class: CsvFileFieldAttribute
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.12.2017</date>
//
// <summary>
//      Field Attributes Class for CSV File
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class CsvFileFieldAttribute : FileFieldAttributeBase
    {
        public CsvFileFieldAttribute()
        {
            this.Column = -1;
        }

        public CsvFileFieldAttribute(string name)
        {
            this.Name = name;
            this.Column = -1;
        }

        public CsvFileFieldAttribute(int column)
        {
            this.Name = null;
            this.Column = column;
        }

        public string Name { get; private set; }

        public int Column { get; private set; }
    }
}