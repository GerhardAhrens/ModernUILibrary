//-----------------------------------------------------------------------
// <copyright file="INummer.cs" company="Lifeprojects.de">
//     Class: INummer
//     Copyright © Gerhard Ahrens, 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.04.2021</date>
//
// <summary>
// Concept functions for Custom Linq Extensions
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    public interface INummer
    {
        bool Active { get; }

        int IntValue { get; }
    }
}