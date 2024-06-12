namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für MessagingAControlsUC.xaml
    /// </summary>
    public partial class MessagingAControlsUC : UserControl
    {
        public MessagingAControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
