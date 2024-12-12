//-----------------------------------------------------------------------
// <copyright file="LoginDialogValid.cs" company="Lifeprojects.de">
//     Class: LoginDialogValid
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>12.12.2024 14:43:18</date>
//
// <summary>
// Enum Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;

    public enum LoginDialogValid : int
    {
        [Description("Keine")]
        None = 0,
        [Description("Passwort gültig")]
        PwdValid = 1,
        [Description("Passwort nicht gültig")]
        PwdNotValid = 2,
        [Description("Ergebnis zurückgeben")]
        ResultOut = 3,
        [Description("Zeitüberschreitung")]
        TimeOut = 4
    }
}
