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
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyMetaInfo"/> class.
        /// </summary>
        public AssemblyMetaInfo()
        {
        }

        public string PacketName => "ModernUI";
        public Version PacketVersion => new Version(1, 0, 2024, 0);

        public string AssemblyName => "ModernUILibrary";

        public Version AssemblyVersion => new Version(1, 0, 2024, 10);

        public override string ToString()
        {
            return $"{this.AssemblyName}, {this.AssemblyVersion}";
        }
    }
}
