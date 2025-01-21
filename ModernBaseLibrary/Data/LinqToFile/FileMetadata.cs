//-----------------------------------------------------------------------
// <copyright file="FileMetadata.cs" company="Lifeprojects.de">
//     Class: FileMetadata
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.12.2017</date>
//
// <summary>
// Class for Meta Data Container
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    using System;
    using System.Reflection;

    public class FileMetadata
    {
        public PropertyInfo PropertyInfo { get; set; }

        public string DateTimeFormat { get; set; }

        public IFormatProvider NumberFormatProvider { get; set; }

        public TypeCode Type { get; set; }
    }
}