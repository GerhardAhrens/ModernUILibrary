//-----------------------------------------------------------------------
// <copyright file="FlatFileAttribute.cs" company="Lifeprojects.de">
//     Class: FlatFileAttribute
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.12.2017</date>
//
// <summary>
// Attributes Class for Flat File
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [AttributeUsage(AttributeTargets.Class)]
    public class FlatFileAttribute : FileAttributeBase
    {
        public override List<string> CheckMetadata(Stream stream)
        {
            return null;
        }

        public override IFileParser<T> GetLineParser<T>(Stream stream)
        {
            return new FlatFileParser<T>(
                typeof(T)
                    .GetProperties()
                    .Select(i => new
                    {
                        PropertyInfo = i,
                        Attribute = i
                            .GetCustomAttributes(typeof(FlatFileFieldAttribute), true)
                            .OfType<FlatFileFieldAttribute>()
                            .FirstOrDefault()
                    })
                    .Where(i => i.PropertyInfo.CanWrite && i.PropertyInfo.CanRead && i.Attribute != null)
                    .Select(i => new
                        FlatFileMetadata
                    {
                        PropertyInfo = i.PropertyInfo,
                        Start = i.Attribute.Start,
                        DateTimeFormat = i.Attribute.DateTimeFormat,
                        NumberFormatProvider = i.Attribute.GetNumberFormat(),
                        Length = i.Attribute.Length,
                        Type = Type.GetTypeCode(i.PropertyInfo.PropertyType)
                    }).ToList(),
                    stream);
        }
    }
}