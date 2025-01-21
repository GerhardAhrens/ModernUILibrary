//-----------------------------------------------------------------------
// <copyright file="MetadataValue.cs" company="Lifeprojects.de">
//     Class: MetadataValue
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.12.2017</date>
//
// <summary>
// MetaData Content to File Provider
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    public class MetadataValue<M> where M : FileMetadata
    {
        public string Item { get; set; }

        public M FieldMetadata { get; set; }
    }
}