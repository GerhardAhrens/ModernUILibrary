//-----------------------------------------------------------------------
// <copyright file="GuidVersion.cs" company="Lifeprojects.de"">
//     Class: GuidVersion
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.09.2022 07:49:53</date>
//
// <summary>
// Enum Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    public enum GuidVersion
    {
        TimeBased = 0x01,
        Reserved = 0x02,
        NameBased = 0x03,
        Random = 0x04
    }
}
