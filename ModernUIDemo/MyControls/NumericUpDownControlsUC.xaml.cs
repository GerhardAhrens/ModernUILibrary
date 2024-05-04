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

        public XamlProperty<List<string>> ListUpDownStringSource { get; set; } = XamlProperty.Set<List<string>>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ListUpDownIntSource.Value = Enumerable.Range(DateTime.Today.Year - 5, 30).Select(x => (x - 1) + 1);
            this.ListUpDownStringSource.Value = new List<string> { "Affe", "Bär", "Elefant", "Hund", "Zebra" };

        }
    }
}
