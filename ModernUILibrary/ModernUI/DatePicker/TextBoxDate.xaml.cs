namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    /// <summary>
    /// Interaktionslogik für TextBoxDate.xaml
    /// </summary>
    public partial class TextBoxDate : UserControl
    {
        public TextBoxDate()
        {
            this.InitializeComponent();

            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(2);
            this.Focusable = true;

            this.DataContext = this;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.BorderBrush = System.Windows.Media.Brushes.Green;
            this.BorderThickness = new Thickness(1);

        }
    }
}
