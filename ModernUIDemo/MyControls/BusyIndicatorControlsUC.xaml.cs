namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für BusyIndicatorControlsUC.xaml
    /// </summary>
    public partial class BusyIndicatorControlsUC : UserControl
    {
        public BusyIndicatorControlsUC()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hallo, BusyIndicator nicht aktive");
        }
    }
}
