namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class MComboBox : ComboBox
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius" , typeof(System.Windows.CornerRadius), typeof(MComboBox));

        public System.Windows.CornerRadius CornerRadius
        {
            get { return (System.Windows.CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark" , typeof(string), typeof(MComboBox));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        static MComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MComboBox), new FrameworkPropertyMetadata(typeof(MComboBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

        }
    }
}
