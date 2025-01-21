//-----------------------------------------------------------------------
// <copyright file="FileAttributeBase.cs" company="Lifeprojects.de">
//     Class: FileAttributeBase
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.12.2017</date>
//
// <summary>
//  Class for File Attributes
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    [AttributeUsage(AttributeTargets.Class)]
    public abstract class FileAttributeBase : Attribute
    {
        public abstract IFileParser<T> GetLineParser<T>(Stream stream) where T : new();

        public abstract List<string> CheckMetadata(Stream stream);
    }
}