namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für ColorControlsUC.xaml
    /// </summary>
    public partial class ColorControlsUC : UserControl
    {
        public ColorControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
