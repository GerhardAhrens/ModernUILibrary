//-----------------------------------------------------------------------
// <copyright file="MsgDialog.cs" company="NRM Netzdienste Rhein-Main GmbH">
//     Class: MsgDialog
//     Copyright © NRM Netzdienste Rhein-Main GmbH 2023
// </copyright>
//
// <author>DeveloperName - NRM Netzdienste Rhein-Main GmbH</author>
// <email>DeveloperName@nrm-netzdienste.de</email>
// <date>17.11.2023 16:49:00</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernUIDemo
{
    using System;
    using System.Text;

    using ModernIU.Controls;

    public static class MsgNotificationWindow
    {
        public static Tuple<NotificationBoxButton, object> ApplicationExit(this INotificationService @this)
        {
            bool? resultDialog = null;
            Tuple<string,string,double> msgText = new Tuple<string, string, double>("Programm beenden", $"Das ist ein Zusatztext \n mit CheckBox",18);
            Tuple<NotificationBoxButton, object> resultTag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.None, null);

            @this.ShowDialog<QuestionYesCheckBoxVM>(msgText, (result, tag) =>
            {
                resultDialog = result;
                if (tag != null)
                {
                    resultTag = (Tuple<NotificationBoxButton, object>)tag;
                }
            });

            return resultTag;
        }

        public static Tuple<NotificationBoxButton> DeleteCurrent(this INotificationService @this)
        {
            bool? resultDialog = null;
            Tuple<string, string, double> msgText = new Tuple<string, string, double>("Löschen", $"Soll der ausgewählte Datensatz gelöscht werden", 18);
            Tuple<NotificationBoxButton> resultTag = new Tuple<NotificationBoxButton>(NotificationBoxButton.None);

            @this.ShowDialog<QuestionYesNoCheckBoxVM>(msgText, (result, tag) =>
            {
                resultDialog = result;
                if (tag != null)
                {
                    resultTag = (Tuple<NotificationBoxButton>)tag;
                }
            });

            return resultTag;
        }

        public static NotificationBoxButton Hinweis(this INotificationService @this)
        {
            bool? resultDialog = null;

            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append("<html><body scroll=\"no\">");
            htmlContent.Append("Das ist ein <b><em>deutlicher</b></em> Hinweis");
            htmlContent.Append($"<h3 style=\"color:blue;\">Datum/Zeit: {DateTime.Now}</h3>");
            htmlContent.Append("</body></html>");

            Tuple<string, string, double> msgText = new Tuple<string, string, double>("Hinweis", htmlContent.ToString(), 0);
            Tuple<NotificationBoxButton, object> resultTag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.None, null);

            @this.ShowDialog<MessageOk>(msgText, (result, tag) =>
            {
                resultDialog = result;
                if (tag != null)
                {
                    resultTag = (Tuple<NotificationBoxButton, object>)tag;
                }
            });

            return resultTag.Item1;
        }

        public static Tuple<NotificationBoxButton, object> SeletFromListBox(this INotificationService @this)
        {
            bool? resultDialog = null;
            Tuple<string, string, double> msgText = new Tuple<string, string, double>("Eintrag auswählen", null, 18);
            Tuple<NotificationBoxButton, object> resultTag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.None, null);
            @this.ShowDialog<SelectLB>(msgText, (result, tag) =>
            {
                resultDialog = result;
                if (tag != null)
                {
                    resultTag = (Tuple<NotificationBoxButton, object>)tag;
                }
            });

            return resultTag;
        }
    }
}
