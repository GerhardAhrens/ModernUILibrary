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
            (string InfoText, string CustomText, double FontSize) msgText = ("Programm beenden", $"Das ist ein Zusatztext \n mit CheckBox", 18);
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
            (string InfoText, string CustomText, double FontSize) msgText = ("Löschen", $"Soll der ausgewählte Datensatz gelöscht werden", 18);
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

            (string InfoText, string CustomText, double FontSize) msgText = ("Hinweis", htmlContent.ToString(), 0);
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

        public static Tuple<NotificationBoxButton, object> SelectFromListBox(this INotificationService @this)
        {
            bool? resultDialog = null;
            (string InfoText, string CustomText, double FontSize) msgText = ("Eintrag auswählen", null, 18);
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

        public static Tuple<NotificationBoxButton, object> InputOneLine(this INotificationService @this, string inputText = "")
        {
            bool? resultDialog = null;
            (string InfoText, string CustomText, int MaxLength, double FontSize) msgText = ("Text eingeben", inputText,20, 18);
            Tuple<NotificationBoxButton, object> resultTag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.None, null);
            @this.ShowDialog<InputOneLineYesNo>(msgText, (result, tag) =>
            {
                resultDialog = result;
                if (tag != null)
                {
                    resultTag = (Tuple<NotificationBoxButton, object>)tag;
                }
            });

            return resultTag;
        }

        public static Tuple<NotificationBoxButton, object> InputInteger(this INotificationService @this, int inputText = 0, Type resultType = null)
        {
            bool? resultDialog = null;
            (string InfoText, string CustomText, int MaxLength, double FontSize) msgText = ("Nummerischen Wert eingeben", inputText.ToString(), 5, 18);
            Tuple<NotificationBoxButton, object> resultTag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.None, null);
            @this.ShowDialog<InputIntegerYesNo>(msgText, (result, tag) =>
            {
                resultDialog = result;
                if (tag != null)
                {
                    resultTag = (Tuple<NotificationBoxButton, object>)tag;
                }
            });

            if (resultTag.Item2 != null)
            {
                resultTag = new Tuple<NotificationBoxButton, object>(resultTag.Item1, Convert.ChangeType(resultTag.Item2, resultType));
            }

            return resultTag;
        }

        public static NotificationBoxButton MessageWithTimer(this INotificationService @this, int countDown = 5)
        {
            bool? resultDialog = null;

            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append("<html><body scroll=\"no\">");
            htmlContent.Append("Das ist eine Message mit einem Timer");
            htmlContent.Append("</body></html>");

            (string InfoText, string CustomText, double FontSize) msgText = ("Hinweis", htmlContent.ToString(), 18);
            Tuple<NotificationBoxButton, object> resultTag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.None, null);

            @this.ShowDialog<MessageTimerOk>(5,msgText, (result, tag) =>
            {
                resultDialog = result;
                if (tag != null)
                {
                    resultTag = (Tuple<NotificationBoxButton, object>)tag;
                }
            });

            return resultTag.Item1;
        }
    }
}
