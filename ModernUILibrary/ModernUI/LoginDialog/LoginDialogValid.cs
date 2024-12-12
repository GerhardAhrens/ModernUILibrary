//-----------------------------------------------------------------------
// <copyright file="LoginDialogValid.cs" company="Lifeprojects.de">
//     Class: LoginDialogValid
//     Copyright � Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>12.12.2024 14:43:18</date>
//
// <summary>
// Enum Klasse f�r 
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
        [Description("Passwort g�ltig")]
        PwdValid = 1,
        [Description("Passwort nicht g�ltig")]
        PwdNotValid = 2,
        [Description("Ergebnis zur�ckgeben")]
        ResultOut = 3,
        [Description("Zeit�berschreitung")]
        TimeOut = 4
    }
}
