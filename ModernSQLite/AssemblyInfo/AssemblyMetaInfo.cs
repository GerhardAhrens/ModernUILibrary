//-----------------------------------------------------------------------
// <copyright file="AssemblyMetaInfo.cs" company="www.lifeprojects.de">
//     Class: AssemblyMetaInfo
//     Copyright © www.lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - www.lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>03.12.2024 07:49:42</date>
//
// <summary>
// Klasse zur Darstellung von Assembly Meta Informationen
// </summary>
//-----------------------------------------------------------------------

namespace ModernSQLite.AssemblyInfo
{
    using System;

    using ModernBaseLibrary.Core;

    public class AssemblyMetaInfo : IAssemblyInfo
    {
        public string PacketName => "ModernUI";
        public Version PacketVersion => new Version(1, 0, 2025, 16);

        public string AssemblyName => "Modern";

        public Version AssemblyVersion => new Version(1, 0, 2025, 1);

        public string Description => "Bibliothek mit einer Sammlung C# Klassen zum lesen und schreiben in Verbindung mit einer SQLite Datenbank";

        public override string ToString()
        {
            return $"{this.AssemblyName}, {this.AssemblyVersion}";
        }
    }
}
