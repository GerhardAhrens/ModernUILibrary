namespace ModernUIDemo
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für InputOneLineYesNo.xaml
    /// </summary>
    public partial class InputOneLineYesNo : UserControl
    {
        public InputOneLineYesNo()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<TextBox, TextChangedEventArgs>.AddHandler(this.TxtInput, "TextChanged", this.OnTextChanged);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Tuple<string, string, double> textOption = (Tuple<string, string, double>)this.Tag;

            this.TbHeader.Text = textOption.Item1;
            this.TxtInput.Text = textOption.Item2;

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
