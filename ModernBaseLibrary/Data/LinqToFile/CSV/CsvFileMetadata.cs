//-----------------------------------------------------------------------
// <copyright file="CsvFileMetadata.cs" company="Lifeprojects.de">
//     Class: CsvFileMetadata
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.12.2017</date>
//
// <summary>
//      Meta Data Class for CSV File
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    public class CsvFileMetadata : FileMetadata
    {
        public int Column { get; set; }

        public string Name { get; set; }
    }
}