//-----------------------------------------------------------------------
// <copyright file="AssemblyMetaInfo.cs" company="www.lifeprojects.de">
//     Class: AssemblyMetaInfo
//     Copyright © www.lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - www.lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>09.04.2025</date>
//
// <summary>
// Klasse für die Festlegung von Metainformationen zum Projekt
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public class AssemblyMetaInfo : IAssemblyInfo
    {
        public string PacketName => "Template ModernInsideVM";
        public Version PacketVersion => new Version(1, 0, 2025, 1);

        public string AssemblyName => $"Template ModernInsideVM";

        public Version AssemblyVersion => new Version(1,0,2025,1);

        public string Description => "Template für Modern InsideVM";

        public override string ToString()
        {
            return $"{this.AssemblyName}, {this.AssemblyVersion}";
        }
    }
}
