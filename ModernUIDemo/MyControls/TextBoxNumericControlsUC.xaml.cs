namespace ModernUIDemo.MyControls
{
    using System.Windows.Controls;

    using ModernUIDemo.Core;

    /// <summary>
    /// Interaktionslogik für TextBoxNumericControlsUC.xaml
    /// </summary>
    public partial class TextBoxNumericControlsUC : UserControl
    {
        public TextBoxNumericControlsUC()
        {
            this.InitializeComponent();
            this.ValueDate.Value = DateTime.Now;
            this.DataContext = this;
        }

        public XamlProperty<DateTime?> ValueDate { get; set; } = XamlProperty.Set<DateTime?>();

    }
}
