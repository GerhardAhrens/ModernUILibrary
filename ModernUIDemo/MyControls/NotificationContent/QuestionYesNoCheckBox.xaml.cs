namespace ModernUIDemo
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für QuestionYesNoCheckBox.xaml
    /// </summary>
    public partial class QuestionYesNoCheckBox : UserControl
    {
        public QuestionYesNoCheckBox()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Tuple<string, string, double> textOption = (Tuple<string, string, double>)this.Tag;

            this.TbHeader.Text = textOption.Item1;
            this.TbFull.Text = textOption.Item2;
            this.TbFull.FontSize = textOption.Item3;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.Yes, this.ChkOption.IsChecked);
            window.DialogResult = true;
            window.Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.No, this.ChkOption.IsChecked);
            window.DialogResult = false;
            window.Close();
        }
    }
}
