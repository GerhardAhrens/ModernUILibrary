namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für FlyoutControlsUC.xaml
    /// </summary>
    public partial class FlyoutControlsUC : UserControl
    {
        public FlyoutControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void FlatButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.FlyoutWindow window = new Windows.FlyoutWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }
    }
}
