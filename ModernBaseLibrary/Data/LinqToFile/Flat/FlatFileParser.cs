//-----------------------------------------------------------------------
// <copyright file="FlatFileParser.cs" company="Lifeprojects.de">
//     Class: FlatFileParser
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.12.2017</date>
//
// <summary>
// Parser Class for Flat File
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class FlatFileParser<T> : FileParserBase<T, FlatFileMetadata> where T : new()
    {
        private readonly StreamReader streamReader = null;

        public FlatFileParser(List<FlatFileMetadata> metadata, Stream stream) : base(metadata)
        {
            this.streamReader = new StreamReader(stream);
        }

        protected override bool End {
            get {
                return this.streamReader.EndOfStream;
            }
        }

        public override void Dispose()
        {
            this.streamReader.Dispose();
        }

        protected override string ReadRow()
        {
            return this.streamReader.ReadLine();
        }

        protected override IEnumerable<MetadataValue<FlatFileMetadata>> Split(string line, List<FlatFileMetadata> metadata)
        {
            return metadata.Select(i => new MetadataValue<FlatFileMetadata> { Item = line.Substring(i.Start, i.Length), FieldMetadata = i }).ToList();
        }
    }
}