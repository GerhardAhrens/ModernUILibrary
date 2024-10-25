namespace ModernUIDemo
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für InputOneLineYesNo.xaml
    /// </summary>
    public partial class InputOneLineYesNo : UserControl, INotificationServiceMessage
    {
        public InputOneLineYesNo()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<TextBox, TextChangedEventArgs>.AddHandler(this.TxtInput, "TextChanged", this.OnTextChanged);
        }

        public int CountDown { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (string InfoText, string CustomText, int MaxLength, double FontSize) textOption = ((string InfoText, string CustomText, int MaxLength, double FontSize))this.Tag;

            this.TbHeader.Text = textOption.InfoText;
            this.TxtInput.Text = textOption.CustomText;
            this.TxtInput.MaxLength = textOption.MaxLength;

            if (string.IsNullOrEmpty(this.TxtInput.Text) == true)
            {
                this.BtnYes.IsEnabled = false;
            }
            else
            {
                this.BtnYes.IsEnabled = true;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TxtInput.Text) == true)
            {
                this.BtnYes.IsEnabled = false;
            }
            else
            {
                this.BtnYes.IsEnabled = true;
            }
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.Yes, this.TxtInput.Text);
            window.DialogResult = true;
            window.Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.No, null);
            window.DialogResult = false;
            window.Close();
        }
    }
}
