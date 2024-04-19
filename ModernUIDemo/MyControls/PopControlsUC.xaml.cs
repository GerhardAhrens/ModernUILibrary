namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Behaviors;

    /// <summary>
    /// Interaktionslogik für PopUpControlsUC.xaml
    /// </summary>
    public partial class PopUpControlsUC : UserControl
    {
        public PopUpControlsUC()
        {
            this.InitializeComponent();
        }

        private void btnOpenMaskLayer_Click(object sender, RoutedEventArgs e)
        {
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, true);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.maskLayer.SetValue(MaskLayerBehavior.IsOpenProperty, false);
        }
    }
}
