//-----------------------------------------------------------------------
// <copyright file="RoundingDirection.cs" company="Lifeprojects.de">
//     Class: RoundingDirection
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>05.09.2023 13:04:01</date>
//
// <summary>
// Enum Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System;

    public enum RoundingDirection : int
    {
        None = 0,
        Up,
        Down,
        Nearest
    }
}
