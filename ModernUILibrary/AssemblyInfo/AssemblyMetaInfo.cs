//-----------------------------------------------------------------------
// <copyright file="AssemblyMetaInfo.cs" company="www.pta.de">
//     Class: AssemblyMetaInfo
//     Copyright © www.pta.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>03.12.2024 07:49:42</date>
//
// <summary>
// Klasse zur Darstellung von Assembly Meta Informationen
// </summary>
//-----------------------------------------------------------------------

namespace ModernUILibrary.AssemblyInfo
{
    using System;

    using ModernBaseLibrary.Core;

    public class AssemblyMetaInfo : IAssemblyInfo
    {
        public string PacketName => "ModernUI";
        public Version PacketVersion => new Version(1, 0, 2025, 10);

        public string AssemblyName => "ModernUILibrary";

        public Version AssemblyVersion => new Version(1, 0, 2025, 10);

        public string Description => "Bibliothek für UI Controls";

        public override string ToString()
        {
            return $"{this.AssemblyName}, {this.AssemblyVersion}";
        }
    }
}
