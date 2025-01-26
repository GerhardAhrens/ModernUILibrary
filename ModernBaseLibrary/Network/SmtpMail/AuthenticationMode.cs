//-----------------------------------------------------------------------
// <copyright file="AuthenticationMode.cs" company="Lifeprojects.de">
//     Class: AuthenticationMode
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.03.2013</date>
//
// <summary>
// Enum-Klasse für den Modus der Authentifizierung (SMTP-Mailer)
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.SMTP
{
    public enum AuthenticationMode : int
    {
        NoAuthentication = 0,
        PlainText = 1,
        NTLMAuthentication = 2
    }
}