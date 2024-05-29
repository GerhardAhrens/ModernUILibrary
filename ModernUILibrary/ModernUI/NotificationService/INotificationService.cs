//-----------------------------------------------------------------------
// <copyright file="INotificationService.cs" company="Lifeprojects.de">
//     Class: INotificationService
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

    public interface INotificationService
    {
        void ShowDialog(string name, Action<bool?, object> callBack);
        void ShowDialog(string name,string addText, Action<bool?, object> callBack);

        void ShowDialog(string name, Tuple<string, string, double> addText, Action<bool?, object> callBack);

        void ShowDialog<ViewModel>(Action<bool?,object> callBack);
        void ShowDialog<ViewModel>(string addText,Action<bool?, object> callBack);
        void ShowDialog<ViewModel>(Tuple<string, string, double> addText, Action<bool?, object> callBack);
    }
}
