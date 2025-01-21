//-----------------------------------------------------------------------
// <copyright file="FileQuery.cs" company="Lifeprojects.de">
//     Class: FileQuery
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.12.2017</date>
//
// <summary>
// LINQ to File Provider
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FileQuery<T> : IDisposable where T : new()
    {
        private readonly FileAttributeBase fileAttribute;
        private readonly Stream stream;
        private readonly bool disposeStream = false;

        public FileQuery(string path) : this(new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            this.disposeStream = true;
        }

        public FileQuery(Stream stream)
        {
            this.stream = stream;
            this.fileAttribute = typeof(T).GetCustomAttributes(typeof(FileAttributeBase), true).OfType<FileAttributeBase>().FirstOrDefault();
        }

        public IEnumerable<T> Content
        {
            get
            {
                this.stream.Seek(0, SeekOrigin.Begin);
                return this.fileAttribute.GetLineParser<T>(this.stream);
            }
        }

        public bool CheckMetadata(int currentColumns)
        {
            bool result = false;

            this.stream.Seek(0, SeekOrigin.Begin);
            List<string> collection = this.fileAttribute.CheckMetadata(this.stream);

            if (collection?.Count == currentColumns)
            {
                result = true;
            }

            return result;
        }

        public List<string> GetCSVColumns()
        {
            this.stream.Seek(0, SeekOrigin.Begin);
            List<string> collection = this.fileAttribute.CheckMetadata(this.stream);

            return collection;
        }

        public void Dispose()
        {
            if (this.disposeStream == true)
            {
                this.stream.Close();
                this.stream.Dispose();
            }
        }
    }
}