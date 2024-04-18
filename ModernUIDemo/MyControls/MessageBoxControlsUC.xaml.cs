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
            Button btn = sender as Button;
            switch (btn.Content.ToString())
            {
                case "Info":
                    if (MMessageBox.Show("Element", "Bildunterschrift", EnumPromptType.Error) == MessageBoxResult.OK)
                    {
                        MessageBox.Show("Klicken Sie auf OK.");
                    }
                    break;
                case "Error":
                    MMessageBox.Show(Window.GetWindow(this), "Sie verwenden derzeit eine zu niedrige Browserversion, die dazu führen kann, dass ein Teil der Funktion des Produkts nicht normal genutzt werden kann", "", MessageBoxButton.YesNoCancel, EnumPromptType.Warn);
                    break;
                case "Warn":
                    MMessageBox.Show("Element", EnumPromptType.Warn);
                    break;
                case "Success":
                    MMessageBox.Show("Element", EnumPromptType.Success);
                    break;
                default:
                    break;
            }
        }
    }
}
