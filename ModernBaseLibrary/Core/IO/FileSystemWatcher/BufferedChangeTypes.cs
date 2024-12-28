//-----------------------------------------------------------------------
// <copyright file="BufferedChangeTypes.cs" company="Lifeprojects.de">
//     Class: BufferedChangeTypes
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>09.06.2023</date>
//
// <summary>
// Class with BufferedChangeTypes Definition
// </summary>
// <Website>
// https://decatec.de/programmierung/filesystemwatcher-events-werden-mehrfach-ausgeloest-loesungsansaetze/
// </Website>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;

    [Flags]
    public enum BufferedChangeTypes
    {
        None = 0,
        Changed = 1,
        Created = 2,
        Deleted = 4,
        Renamed = 8,
        All = 15
    }
}
