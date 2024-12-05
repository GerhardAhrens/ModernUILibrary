//-----------------------------------------------------------------------
// <copyright file="IAssemblyInfo.cs" company="www.pta.de">
//     Class: IAssemblyInfo
//     Copyright © www.pta.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>03.12.2024 07:44:32</date>
//
// <summary>
// Interface Class for 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    public interface IAssemblyInfo
    {
        string PacketName { get; }
        Version PacketVersion { get; }
        string AssemblyName { get; }
        Version AssemblyVersion { get; }
    }
}
