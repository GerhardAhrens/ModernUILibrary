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

namespace ModernMVVMDemo.Message
{
    using System;
    using System.Text;

    using ModernIU.Controls;

    public static class MessageContent
    {
        public static NotificationBoxButton ApplicationExit(this INotificationService @this)
        {
            bool? resultDialog = null;
            (string InfoText, string CustomText, double FontSize) msgText = ("Programm beenden", $"Soll das Programm beendet werden?", 18);
            NotificationBoxButton questionResult = NotificationBoxButton.None;

            @this.ShowDialog<QuestionYesNo>(msgText, (result, tag) =>
            {
                resultDialog = result;
                if (result == true && tag != null)
                {
                    questionResult = ((System.Tuple<ModernIU.Controls.NotificationBoxButton>)tag).Item1;
                }
            });

            return questionResult;
        }

        public static NotificationBoxButton ApplicationExit2(this INotificationService @this)
        {
            bool? resultDialog = null;

            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append("<html><body scroll=\"no\">");
            htmlContent.Append("<h2 style=\"color:black;\">Soll das Programm beendet werden?</h2>");
            htmlContent.Append($"<h3 style=\"color:blue;\">Datum/Zeit: {DateTime.Now}</h3>");
            htmlContent.Append("</body></html>");

            (string InfoText, string CustomText, double FontSize) msgText = ("Programm beenden", htmlContent.ToString(), 0);
            NotificationBoxButton questionResult = NotificationBoxButton.None;

            @this.ShowDialog<QuestionHtmlYesNo>(msgText, (result, tag) =>
            {
                resultDialog = result;
                if (result == true && tag != null)
                {
                    questionResult = ((System.Tuple<ModernIU.Controls.NotificationBoxButton>)tag).Item1;
                }
            });

            return questionResult;
        }
    }
}
