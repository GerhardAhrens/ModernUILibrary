namespace ModernTemplate.Views
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl, INotificationServiceMessage
    {
        public LoginView()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<TextBox, TextChangedEventArgs>.AddHandler(this.TxtUser, "TextChanged", this.OnUserTextChanged);
            WeakEventManager<TextBox, TextChangedEventArgs>.AddHandler(this.TxtPassword, "TextChanged", this.OnPasswordTextChanged);
        }

        public int CountDown { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (string InfoText, string CustomText, double FontSize) textOption = ((string InfoText, string CustomText, double FontSize))this.Tag;

            this.TbHeader.Text = textOption.InfoText;

            if (string.IsNullOrEmpty(this.TxtUser.Text) == true && string.IsNullOrEmpty(this.TxtPassword.Text) == true)
            {
                this.BtnYes.IsEnabled = false;
            }
            else
            {
                this.BtnYes.IsEnabled = true;
            }
        }

        private void OnUserTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TxtUser.Text) == false && string.IsNullOrEmpty(this.TxtPassword.Text) == false)
            {
                this.BtnYes.IsEnabled = true;
            }
            else
            {
                this.BtnYes.IsEnabled = false;
            }
        }

        private void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TxtUser.Text) == false && string.IsNullOrEmpty(this.TxtPassword.Text) == false)
            {
                this.BtnYes.IsEnabled = true;
            }
            else
            {
                this.BtnYes.IsEnabled = false;
            }
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, string,string>(NotificationBoxButton.Yes, this.TxtUser.Text,this.TxtPassword.Text);
            window.DialogResult = true;
            window.Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, string,string>(NotificationBoxButton.No, string.Empty,string.Empty);
            window.DialogResult = false;
            window.Close();
        }
    }
}
