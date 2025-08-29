namespace ModernTemplate.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für ApplicationAbout.xaml
    /// </summary>
    public partial class ApplicationAbout : UserControl, INotificationServiceMessage
    {
        public ApplicationAbout()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        public int CountDown { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (string InfoText, string CustomText, double FontSize) textOption = ((string InfoText, string CustomText, double FontSize))this.Tag;

            this.TbHeader.Text = textOption.Item1;
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.Ok, null);
            window.DialogResult = true;
            window.Close();
        }
    }
}
