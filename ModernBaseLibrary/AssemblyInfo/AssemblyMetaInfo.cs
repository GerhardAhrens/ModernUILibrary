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

namespace ModernBaseLibrary.AssemblyInfo
{
    using System;

    using ModernBaseLibrary.Core;

    public class AssemblyMetaInfo : IAssemblyInfo
    {
        public string PacketName => "ModernUI";
        public Version PacketVersion => new Version(1, 0, 2025, 16);

        public string AssemblyName => "ModernBaseLibrary";

        public Version AssemblyVersion => new Version(1, 0, 2025, 16);

        public string Description => "Bibliothek mit einer Sammlung von Basis C# Klassen, Extensions Methodes, Helper Klassen";

        public override string ToString()
        {
            return $"{this.AssemblyName}, {this.AssemblyVersion}";
        }
    }
}
