//-----------------------------------------------------------------------
// <copyright file="ChecksumStringFormat.cs" company="Lifeprojects.de">
//     Class: ChecksumStringFormat
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.01.2021</date>
//
// <summary>
//  Enum-Klasse mit möglichen Checksum-Formaten
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Cryptography
{
    public enum ChecksumStringFormat
    {
        /// <summary>
        /// Base64.
        /// </summary>
        Base64,

        /// <summary>
        /// Hexadecimal.
        /// </summary>
        Hex
    }
}