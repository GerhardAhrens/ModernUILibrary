namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für TextBoxRtfControlsUC.xaml
    /// </summary>
    public partial class TextBoxRtfControlsUC : UserControl
    {
        private TextBoxRtfControlsVM vmRoot = null;

        public TextBoxRtfControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            vmRoot = new TextBoxRtfControlsVM();
            this.DataContext = vmRoot;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
