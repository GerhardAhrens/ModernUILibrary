namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für CheckBoxUC.xaml
    /// </summary>
    public partial class CheckBoxUC : UserControl
    {
        public CheckBoxUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
