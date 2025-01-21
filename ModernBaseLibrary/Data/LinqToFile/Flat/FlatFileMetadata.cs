//-----------------------------------------------------------------------
// <copyright file="FlatFileMetadata.cs" company="Lifeprojects.de">
//     Class: FlatFileMetadata
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.12.2017</date>
//
// <summary>
// Meta Data Class for Flat File
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    public class FlatFileMetadata : FileMetadata
    {
        public int Start { get; set; }

        public int Length { get; set; }
    }
}