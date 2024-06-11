namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für ContentFrameControlsUC.xaml
    /// </summary>
    public partial class ContentFrameControlsUC : UserControl
    {
        public ContentFrameControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
