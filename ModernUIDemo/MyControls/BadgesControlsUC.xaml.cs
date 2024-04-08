namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für BadgesControlsUC.xaml
    /// </summary>
    public partial class BadgesControlsUC : UserControl
    {
        public BadgesControlsUC()
        {
            InitializeComponent();
        }

        private void FlatButton_Click(object sender, RoutedEventArgs e)
        {
            int number = (int)this.button.GetValue(BadgeAdorner.NumberProperty);
            this.button.SetValue(BadgeAdorner.NumberProperty, ++number);
        }

        private void btnChangeAdornerType_Click(object sender, RoutedEventArgs e)
        {
            //BadgeType badgeType = (BadgeType)this.text.GetValue(BadgeAdorner.BadgeTypeProperty);

            if (this.btnChangeAdornerType.IsChecked == true)
            {
                this.text.SetValue(BadgeAdorner.BadgeTypeProperty, BadgeType.Dot);
            }
            else
            {
                this.text.SetValue(BadgeAdorner.BadgeTypeProperty, BadgeType.Normal);
            }
        }
    }
}
