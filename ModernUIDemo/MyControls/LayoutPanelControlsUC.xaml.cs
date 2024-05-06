namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für LayoutPanelControlsUC.xaml
    /// </summary>
    public partial class LayoutPanelControlsUC : UserControl
    {
        public LayoutPanelControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
