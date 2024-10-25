namespace ModernUIDemo.MyControls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für NotificationServiceControlsUC.xaml
    /// </summary>
    public partial class NotificationServiceControlsUC : UserControl
    {
        private INotificationService _notificationService = new NotificationService();

        public NotificationServiceControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxA, "Click", this.ONotifiactionBoxAClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxA1, "Click", this.ONotifiactionBoxA1Click);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxB, "Click", this.ONotifiactionBoxBClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxC, "Click", this.ONotifiactionBoxCClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxD1, "Click", this.ONotifiactionBoxD1Click);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxD2, "Click", this.ONotifiactionBoxD2Click);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxT1, "Click", this.ONotifiactionBoxT1Click);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NotificationService.RegisterDialog<QuestionYesCheckBox, QuestionYesCheckBoxVM>();
            NotificationService.RegisterDialog<QuestionYesNoCheckBox, QuestionYesNoCheckBoxVM>();
            NotificationService.RegisterDialog<MessageOk>();
            NotificationService.RegisterDialog<SelectLB>();
            NotificationService.RegisterDialog<InputOneLineYesNo>();
            NotificationService.RegisterDialog<InputNumericYesNo>();
            NotificationService.RegisterDialog<MessageTimerOk>();
        }

        private void ONotifiactionBoxAClick(object sender, RoutedEventArgs e)
        {
            Tuple<NotificationBoxButton, object> result = this._notificationService.ApplicationExit();

            if (result.Item1 != NotificationBoxButton.None)
            {
                MessageBox.Show($"Ausgewählt: {result}\nCheckBox : {(bool?)result.Item2}", "Notification Service");
            }
        }

        private void ONotifiactionBoxA1Click(object sender, RoutedEventArgs e)
        {
            Tuple<NotificationBoxButton> result = this._notificationService.DeleteCurrent();

            if (result.Item1 != NotificationBoxButton.None)
            {
                MessageBox.Show($"Ausgewählt: {result.Item1}", "Notification Service");
            }
        }

        private void ONotifiactionBoxBClick(object sender, RoutedEventArgs e)
        {
            NotificationBoxButton result = this._notificationService.Hinweis();

            if (result == NotificationBoxButton.Ok)
            {
                MessageBox.Show($"Ausgewählt: {result}", "Notification Service");
            }
        }

        private void ONotifiactionBoxCClick(object sender, RoutedEventArgs e)
        {
            Tuple<NotificationBoxButton, object> result = this._notificationService.SelectFromListBox();

            if (result.Item1 == NotificationBoxButton.Yes)
            {
                MessageBox.Show($"Ausgewählt: {result}\nListBoxItem : {((FooItem)result.Item2).Full}", "Notification Service");
            }
        }

        private void ONotifiactionBoxD1Click(object sender, RoutedEventArgs e)
        {
            Tuple<NotificationBoxButton, object> result = this._notificationService.InputOneLine("Hallo");
            if (result.Item1 == NotificationBoxButton.Yes)
            {
                MessageBox.Show($"Eingegebener Text: {result}\nTextBox.Text : {(string)result.Item2}", "Notification Service");
            }
        }

        private void ONotifiactionBoxD2Click(object sender, RoutedEventArgs e)
        {
            Tuple<NotificationBoxButton, object> result = this._notificationService.InputInteger(0, typeof(decimal));
            if (result.Item1 == NotificationBoxButton.Yes)
            {
                MessageBox.Show($"Eingegebener Text: {result}\nTextBox.Text : {result.Item2}", "Notification Service");
            }
        }

        private void ONotifiactionBoxT1Click(object sender, RoutedEventArgs e)
        {
            NotificationBoxButton result = this._notificationService.MessageWithTimer(5);
            if (result == NotificationBoxButton.Ok)
            {
                MessageBox.Show($"Ausgewählt: {result}", "Notification Service");
            }
        }
    }
}
