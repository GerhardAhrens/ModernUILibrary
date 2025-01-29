//-----------------------------------------------------------------------
// <copyright file="BuildDateAttribute.cs" company="Lifeprojects.de">
//     Class: BuildDateAttribute
//     Copyright © Gerhard Ahrens, 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>16.04.2019</date>
//
// <summary>Class for BuildDateAttribute</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Globalization;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    [AttributeUsage(AttributeTargets.Assembly)]
    public class BuildDateAttribute : Attribute
    {
        public BuildDateAttribute(string value)
        {
            if (value.CountToken('.') == 2 && value.CountToken(':') == 0)
            {
                DateTime = DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            else if (value.CountToken('.') == 2 && value.CountToken(':') == 1)
            {
                DateTime = DateTime.ParseExact(value, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            else if (value.CountToken('.') == 2 && value.CountToken(':') == 2)
            {
                DateTime = DateTime.ParseExact(value, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            else
            {
                if (value.Length == 8)
                {
                    DateTime = DateTime.ParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                }
                else if (value.Length == 12)
                {
                    DateTime = DateTime.ParseExact(value, "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None);
                }
                else if (value.Length == 14)
                {
                    DateTime = DateTime.ParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
                }
                else
                {
                    DateTime = DateTime.Now.DefaultDate();
                }
            }
        }

        public DateTime DateTime { get; }
    }
}