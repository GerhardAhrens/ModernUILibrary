//-----------------------------------------------------------------------
// <copyright file="AssemblyLocation.cs" company="Lifeprojects.de"">
//     Class: AssemblyLocation
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.08.2022 08:09:54</date>
//
// <summary>
// Enum Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Base
{
    using System;

    public enum AssemblyLocation : int
    {
        None = 0,
        EntryAssembly = 1,
        CallingAssembly = 2,
        ExecutingAssembly = 3
    }
}
