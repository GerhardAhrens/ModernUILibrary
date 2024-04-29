namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernUIDemo.Core;

    /// <summary>
    /// Interaktionslogik für NumericUpDownControlsUC.xaml
    /// </summary>
    public partial class NumericUpDownControlsUC : UserControl
    {
        public NumericUpDownControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.DataContext = this;
        }

        public XamlProperty<IEnumerable<int>> ListUpDownIntSource { get; set; } = XamlProperty.Set<IEnumerable<int>>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ListUpDownIntSource.Value = Enumerable.Range(DateTime.Today.Year - 5, 30).Select(x => (x - 1) + 1);

        }
    }
}
