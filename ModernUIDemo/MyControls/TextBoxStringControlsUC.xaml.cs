namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für TextBoxStringControlsUC.xaml
    /// </summary>
    public partial class TextBoxStringControlsUC : UserControl
    {
        public TextBoxStringControlsUC()
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
