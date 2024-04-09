namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für TextBoxControlsUC.xaml
    /// </summary>
    public partial class TextBoxControlsUC : UserControl
    {
        public TextBoxControlsUC()
        {
            this.InitializeComponent();
        }

        private void IconTextBox_EnterKeyClick(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            IconTextBox si = (IconTextBox)sender;

            var content = ((TextBox)e.OriginalSource).Text;
            MessageBox.Show($"{si.Name}; {content}");
        }
    }
}
