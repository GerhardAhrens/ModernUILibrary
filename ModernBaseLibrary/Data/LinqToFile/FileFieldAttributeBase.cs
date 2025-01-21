//-----------------------------------------------------------------------
// <copyright file="FileFieldAttributeBase.cs" company="Lifeprojects.de">
//     Class: FileFieldAttributeBase
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.12.2017</date>
//
// <summary>
// Class for File Field Attributes
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    using System;
    using System.Globalization;

    public abstract class FileFieldAttributeBase : Attribute
    {
        public string DateTimeFormat { get; set; }

        public string NegativeSign { get; set; }

        public string NumberDecimalSeparator { get; set; }

        public string NumberGroupSeparator { get; set; }

        public string PositiveSign { get; set; }

        internal NumberFormatInfo GetNumberFormat()
        {
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
            if (!string.IsNullOrEmpty(this.NegativeSign))
            {
                numberFormatInfo.NegativeSign = this.NegativeSign;
            }

            if (!string.IsNullOrEmpty(this.NumberDecimalSeparator))
            {
                numberFormatInfo.NumberDecimalSeparator = this.NumberDecimalSeparator;
            }

            if (!string.IsNullOrEmpty(this.NumberGroupSeparator))
            {
                numberFormatInfo.NumberGroupSeparator = this.NumberGroupSeparator;
            }

            if (!string.IsNullOrEmpty(this.PositiveSign))
            {
                numberFormatInfo.PositiveSign = this.PositiveSign;
            }

            return numberFormatInfo;
        }
    }
}