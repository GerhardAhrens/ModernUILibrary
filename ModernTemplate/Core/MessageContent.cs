//-----------------------------------------------------------------------
// <copyright file="MessageContent.cs" company="Company">
//     Class: MessageContent
//     Copyright © Company yyyy
// </copyright>
//
// <authorAutor - Company</author>
// <email>autor@Company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Klasse zur Sammlung von Meldungen die im der gesamten Anwendung aufgerufen werden können
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System;
    using System.Runtime.Versioning;
    using System.Text;

    using ModernIU.Controls;

    using ModernTemplate.Views;

    [SupportedOSPlatform("windows")]
    public static class MessageContent
    {
        private const int MSG_FONTSIZE = 18;

        #region Allgemeine Meldungen 
        public static NotificationBoxButton ApplicationExit(this INotificationService @this)
        {
            (string InfoText, string CustomText, double FontSize) msgText = ("Programm beenden", $"Soll das Programm beendet werden?", MSG_FONTSIZE);
            NotificationBoxButton questionResult = NotificationBoxButton.No;

            @this.ShowDialog<QuestionYesNo>(msgText, (result, tag) =>
            {
                if (result == true && tag != null)
                {
                    questionResult = ((Tuple<NotificationBoxButton>)tag).Item1;
                }
            });

            return questionResult;
        }

        public static NotificationBoxButton FeaturesNotFound(this INotificationService @this, string featuresText = "")
        {
            (string InfoText, string CustomText, double FontSize) msgText = ("Funktion nicht gefunden", $"Die gewünschte Funktion '{featuresText}' steht nicht zur Verfügung\nBei Bedarf kontaktieren Sie den Verantwortlichen der Applikation.", MSG_FONTSIZE);
            NotificationBoxButton resultOK = NotificationBoxButton.Ok;

            @this.ShowDialog<MessageOk>(msgText, (result, tag) =>
            {
                if (result == true && tag != null)
                {
                    resultOK = ((Tuple<NotificationBoxButton>)tag).Item1;
                }
            });

            return resultOK;
        }

        public static Tuple<NotificationBoxButton, string, string> LoginDialog(this INotificationService @this)
        {
            bool? resultDialog = null;
            (string InfoText, string CustomText, double FontSize) msgText = ("Benutzer und Passwort eingeben", string.Empty, MSG_FONTSIZE);
            Tuple<NotificationBoxButton, string, string> resultTag = new Tuple<NotificationBoxButton, string, string>(NotificationBoxButton.None, null, null);
            @this.ShowDialog<LoginView>(msgText, (result, tag) =>
            {
                resultDialog = result;
                if (tag != null)
                {
                    resultTag = (Tuple<NotificationBoxButton, string, string>)tag;
                }
            });

            return resultTag;
        }


        #endregion Allgemeine Meldungen

        #region Meldungen zu Bearbeitungsdialoge
        public static NotificationBoxButton DeleteRow(this INotificationService @this, string titel)
        {
            (string InfoText, string CustomText, double FontSize) msgText = ("Eintrag löschen", $"Wollen Sie den Eintrag '{titel}' löschen?", MSG_FONTSIZE);
            NotificationBoxButton questionResult = NotificationBoxButton.No;

            @this.ShowDialog<QuestionYesNo>(msgText, (result, tag) =>
            {
                if (result == true && tag != null)
                {
                    questionResult = ((Tuple<NotificationBoxButton>)tag).Item1;
                }
            });

            return questionResult;
        }
        #endregion Meldungen zu Bearbeitungsdialoge

        #region Meldungen zur Datenbank
        #endregion Meldungen zur Datenbank

        #region Meldungen zum Datenexport/Reporting
        #endregion Meldungen zum Datenexport/Reporting

        #region Weitere Bespiele für Benachrichtigungsdialoge
        public static NotificationBoxButton MessageWithTimer(this INotificationService @this, int countDown = 5)
        {
            bool? resultDialog = null;

            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append("<html><body scroll=\"no\">");
            htmlContent.Append($"Das ist eine Message mit einem Timer. Die Meldung schliesst sich in {countDown} Sek. autoamtsich. ");
            htmlContent.Append("</body></html>");

            (string InfoText, string CustomText, double FontSize) msgText = ("Hinweis", htmlContent.ToString(), MSG_FONTSIZE);
            Tuple<NotificationBoxButton, object> resultTag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.None, null);

            @this.ShowDialog<MessageTimerOk>(countDown, msgText, (result, tag) =>
            {
                resultDialog = result;
                if (tag != null)
                {
                    resultTag = (Tuple<NotificationBoxButton, object>)tag;
                }
            });

            return resultTag.Item1;
        }
        #endregion Weitere Bespiele für Benachrichtigungsdialoge
    }
}
