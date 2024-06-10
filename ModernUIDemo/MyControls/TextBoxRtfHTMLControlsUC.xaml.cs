namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für TextBoxRtfHTMLControlsUC.xaml
    /// </summary>
    public partial class TextBoxRtfHTMLControlsUC : UserControl
    {
        public TextBoxRtfHTMLControlsUC()
        {
            this.InitializeComponent();

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.DataContext = new TextBoxRtfHTMLControlsVM();

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
