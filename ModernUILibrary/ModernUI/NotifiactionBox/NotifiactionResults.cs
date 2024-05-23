//-----------------------------------------------------------------------
// <copyright file="NotifiactionResults.cs" company="Lifeprojects.de">
//     Class: NotifiactionResults
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>24.08.2018</date>
//
// <summary>Enum für das Auswahlergebnis der MessageBoxEx</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.ComponentModel;

    public enum NotifiactionResults
    {
        [Description("Keine Auswahl")]
        None = 0,
        [Description("Auswahl: Ok Button")]
        Ok = 1,
        [Description("Auswahl: Cancle Button")]
        Cancel = 2,
        [Description("Auswahl: Yes Button")]
        Yes = 3,
        [Description("Auswahl: No Button")]
        No = 4,
        [Description("Auswahl: Button Right")]
        ButtonRight = 5,
        [Description("Auswahl: Button Middle")]
        ButtonMiddle = 6,
        [Description("Auswahl: Button Left")]
        ButtonLeft = 7,
    }
}
