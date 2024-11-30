namespace ModernUIDemo
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für MessageTimerOk.xaml
    /// </summary>
    public partial class MessageTimerOk : UserControl, INotificationServiceMessage
    {
        DispatcherTimer autoTimer = null;
        public MessageTimerOk()
        {
            this.InitializeComponent();
            this.autoTimer = new DispatcherTimer();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(this.autoTimer, "Tick", this.OnTick);
        }

        public int CountDown { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (string InfoText, string CustomText, double FontSize) textOption = ((string InfoText, string CustomText, double FontSize))this.Tag;

            this.TbHeader.Text = textOption.Item1;
            this.ControlContent.NavigateToString(textOption.Item2);
            if (this.CountDown > 0)
            {
                this.autoTimer.Interval = TimeSpan.FromSeconds(CountDown);
                this.autoTimer.Start();
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (this.autoTimer.IsEnabled == true)
            {
                this.autoTimer.Stop();
                this.BtnOk_Click(this, null);
            }
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
