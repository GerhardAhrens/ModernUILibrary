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
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernUIDemo.Core
{
    using System;

    using ModernUILibrary.Core;

    public class AssemblyMetaInfo : IAssemblyInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyMetaInfo"/> class.
        /// </summary>
        public AssemblyMetaInfo()
        {
        }

        

        public string AssemblyName => "ModernUIDemo";

        public Version AssemblyVersion => new Version(1,0,2024,1);

        public string PacketVersion => "BlaBla 1.0";
    }
}
