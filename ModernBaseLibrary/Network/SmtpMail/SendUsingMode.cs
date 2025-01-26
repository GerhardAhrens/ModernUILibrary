//-----------------------------------------------------------------------
// <copyright file="SendUsingMode.cs" company="Lifeprojects.de">
//     Class: SendUsingMode
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.03.2013</date>
//
// <summary>
// Enum-Klasse gibt den Status eines Mailversand per SMTP zurück (SMTP-Mailer)
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.SMTP
{
    public enum SendUsingMode : int
    {
        Network = 0,
        PickupDirectory = 1,
        SpecifiedPickupDirectory = 2,
        PickupDirectoryFromIis = 3
    }
}