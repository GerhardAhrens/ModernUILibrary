namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;
    using ModernIU.Base;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für MessageBoxControlsUC.xaml
    /// </summary>
    public partial class MessageBoxControlsUC : UserControl
    {
        public MessageBoxControlsUC()
        {
            this.InitializeComponent();
        }

        private void FlatButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBoxResult.None;
            Button btn = sender as Button;
            switch (btn.Content.ToString())
            {
                case "Info":
                    if (MMessageBox.Show("Element", "Bildunterschrift", EnumPromptType.Info) == MessageBoxResult.OK)
                    {
                        MessageBox.Show("Result: OK.");
                    }
                    break;
                case "Error":
                    result = MMessageBox.Show(Window.GetWindow(this), "Sie verwenden derzeit eine zu niedrige Browserversion, die dazu führen kann, dass ein Teil der Funktion des Produkts nicht normal genutzt werden kann", "", MessageBoxButton.YesNoCancel, EnumPromptType.Error);
                    MessageBox.Show($"Result: {result}");

                    break;
                case "Warn":
                    result = MMessageBox.Show("Element", EnumPromptType.Warn);
                    MessageBox.Show($"Result: {result}");
                    break;
                case "Success":
                    result = MMessageBox.Show("Element", EnumPromptType.Success);
                    MessageBox.Show($"Result: {result}");
                    break;
                default:
                    break;
            }
        }
    }
}
