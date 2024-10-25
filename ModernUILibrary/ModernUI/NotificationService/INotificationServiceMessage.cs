//-----------------------------------------------------------------------
// <copyright file="INotificationServiceMessage.cs" company="Lifeprojects.de">
//     Class: INotificationServiceMessage
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>10.11.2023 14:30:16</date>
//
// <summary>
// Interface für NotificationService Klasse
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;

    public interface INotificationServiceMessage
    {
        int CountDown { get; set; }
        object Tag { get; set; }
    }
}
