namespace ModernInsideVM.Core
{
    using System;
    using System.Text;
    using ModernBaseLibrary.Text;

    using ModernInsideVM.Views;

    using ModernIU.Controls;

    public static class MessageContent
    {
        public static NotificationBoxButton ApplicationExit(this INotificationService @this)
        {
            (string InfoText, string CustomText, double FontSize) msgText = ("Programm beenden", $"Soll das Programm beendet werden?", 18);
            NotificationBoxButton questionResult = NotificationBoxButton.None;

            @this.ShowDialog<QuestionYesNo>(msgText, (result, tag) =>
            {
                if (result == true && tag != null)
                {
                    questionResult = ((System.Tuple<ModernIU.Controls.NotificationBoxButton>)tag).Item1;
                }
            });

            return questionResult;
        }

        public static NotificationBoxButton IsSavedLastChanges(this INotificationService @this)
        {
            (string InfoText, string CustomText, double FontSize) msgText = ("Speichern", $"Es sind noch offene Änderungen nicht gespeichert. Wollen Sie die Eingabe abbrechen?", 18);
            NotificationBoxButton questionResult = NotificationBoxButton.None;

            @this.ShowDialog<QuestionYesNo>(msgText, (result, tag) =>
            {
                if (result == true && tag != null)
                {
                    questionResult = ((System.Tuple<ModernIU.Controls.NotificationBoxButton>)tag).Item1;
                }
            });

            return questionResult;
        }

        public static NotificationBoxButton FeaturesNotFound2(this INotificationService @this, string featuresText = "")
        {
            (string InfoText, string CustomText, double FontSize) msgText = ("Funktion nicht gefunden", $"Die gewünschte Funktion '{featuresText}' steht nicht zur Verfügung\nBei Bedarf kontaktieren Sie den Verantwortlichen der Applikation.",18);
            Tuple<NotificationBoxButton, object> resultOK = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.Ok, null);

            @this.ShowDialog<MessageOk>(msgText, (result, tag) =>
            {
                if (result == true && tag != null)
                {
                    resultOK = (Tuple<NotificationBoxButton, object>)tag;
                }
            });

            return resultOK.Item1;
        }
    }
}
