namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernUIDemo.Core;

    /// <summary>
    /// Interaktionslogik für BehaviorExcelCellControlsUC.xaml
    /// </summary>
    public partial class BehaviorExcelCellControlsUC : UserControl
    {
        public BehaviorExcelCellControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        public XamlProperty<DateTime> DateSelected { get; set; } = XamlProperty.Set<DateTime>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.DateSelected.Value = DateTime.Now;
        }
    }
}
