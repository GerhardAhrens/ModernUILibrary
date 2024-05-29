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
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxB, "Click", this.ONotifiactionBoxBClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxC, "Click", this.ONotifiactionBoxCClick);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            NotificationService.RegisterDialog<QuestionYesNoCheckBox, QuestionYesNoCheckBoxVM>();
            NotificationService.RegisterDialog<MessageOk>();
            NotificationService.RegisterDialog<SelectLB>();
        }

        private void ONotifiactionBoxAClick(object sender, RoutedEventArgs e)
        {
            Tuple<NotificationBoxButton, object> result = this._notificationService.ApplicationExit();

            if (result.Item1 != NotificationBoxButton.None)
            {
                MessageBox.Show($"Ausgewählt: {result}\nCheckBox : {(bool?)result.Item2}", "Notification Service");
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
            Tuple<NotificationBoxButton, object> result = this._notificationService.SeletFromListBox();

            if (result.Item1 == NotificationBoxButton.Yes)
            {
                MessageBox.Show($"Ausgewählt: {result}\nListBoxItem : {((FooItem)result.Item2).Full}", "Notification Service");
            }
        }
    }
}
