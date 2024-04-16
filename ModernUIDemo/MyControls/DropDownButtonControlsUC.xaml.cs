namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für DropDownButtonControlsUC.xaml
    /// </summary>
    public partial class DropDownButtonControlsUC : UserControl
    {
        public DropDownButtonControlsUC()
        {
            this.InitializeComponent();

            List<string> buttonList = new List<string>();
            buttonList.Add("Button 1");
            buttonList.Add("Button 2");
            this.SplitButton.ItemsSource = buttonList;
            this.SplitButton2.ItemsSource = buttonList;
            this.SplitButton3.ItemsSource = buttonList;
            this.SplitButton4.ItemsSource = buttonList;
            this.SplitButton5.ItemsSource = buttonList;
            this.SplitButton6.ItemsSource = buttonList;
        }

        private void FlatButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FlatButton senderBtn = (FlatButton)sender;
            MessageBox.Show($"Hello: {senderBtn.Content}");
        }

        private void SplitButton_ItemClick(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MessageBox.Show(e.NewValue.ToString());
        }
    }
}
