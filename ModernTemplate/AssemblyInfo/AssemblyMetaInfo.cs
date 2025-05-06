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
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public class AssemblyMetaInfo : IAssemblyInfo
    {
        public string PacketName => "Template ModernInsideVM";

        public Version PacketVersion => new Version(1, 0, 2025, 1);

        public string AssemblyName => $"Template ModernInsideVM";

        public Version AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version;

        public DateTime AssemblyDate => new FileInfo(Assembly.GetExecutingAssembly().Location).LastAccessTime;

        public string Description => "Template für Modern InsideVM";

        public string Unternehmen => "Company";

        public string Copyright => "© Company 2025";

        public string GitRepository => "https://github.com/GerhardAhrens/ModernUILibrary/tree/master/ModernTemplate/";

        public string FrameworkVersion => RuntimeInformation.FrameworkDescription;

        public string OSPlatform => RuntimeInformation.OSArchitecture.ToString();

        public string RuntimeIdentifier => RuntimeInformation.RuntimeIdentifier;

        public override string ToString()
        {
            return $"{this.AssemblyName}, {this.AssemblyVersion}";
        }
    }
}
