namespace ModernTemplate.Core
{
    using System;
    using System.Runtime.Versioning;

    using ModernIU.Controls;

    using ModernTemplate.Views;

    [SupportedOSPlatform("windows")]
    public static class MessageContent
    {
        #region Allgemeine Meldungen
        public static NotificationBoxButton ApplicationExit(this INotificationService @this)
        {
            (string InfoText, string CustomText, double FontSize) msgText = ("Programm beenden", $"Soll das Programm beendet werden?", 18);
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
            (string InfoText, string CustomText, double FontSize) msgText = ("Funktion nicht gefunden", $"Die gewünschte Funktion '{featuresText}' steht nicht zur Verfügung\nBei Bedarf kontaktieren Sie den Verantwortlichen der Applikation.",18);
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
        #endregion Allgemeine Meldungen
    }
}
