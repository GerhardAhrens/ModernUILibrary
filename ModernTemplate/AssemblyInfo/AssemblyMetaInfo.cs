//-----------------------------------------------------------------------
// <copyright file="AssemblyMetaInfo.cs" company="company">
//     Class: AssemblyMetaInfo
//     Copyright � company 2025
// </copyright>
//
// <author>Gerhard Ahrens - company</author>
// <email>gerhard.ahrens@company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Klasse f�r die Festlegung von Metainformationen zum Projekt
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

        public string Description => "Template f�r Modern InsideVM";

        public string Unternehmen => "Company";

        public string Copyright => "� Company 2025";

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
