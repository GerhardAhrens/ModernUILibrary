//-----------------------------------------------------------------------
// <copyright file="NotificationIcon.cs" company="Lifeprojects.de">
//     Class: NotificationIcon
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>24.08.2018</date>
//
// <summary>Enum für das Icon zur Darstelleung in der MessageBoxEx</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.ComponentModel;

    public enum NotificationIcon
    {
        [Description("Keine Auswahl")]
        None = 0,
        Application = 1,
        Asterisk = 2,
        Error = 3,
        Exclamation = 4,
        Hand = 5,
        Information = 6,
        Question = 7,
        Warning = 9,
        WinLogo = 10
    }
}
