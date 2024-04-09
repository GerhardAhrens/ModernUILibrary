namespace ModernIU.Base
{
    using System.Windows;
    using System.Windows.Controls;

    public class MTextBoxBase : TextBox
    {
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(MTextBoxBase));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MTextBoxBase), new PropertyMetadata(CornerRadiusChanged));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        

        private static void CornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MTextBoxBase textbox = d as MTextBoxBase;
            if(textbox != null && e.NewValue != null)
            {
                textbox.OnCornerRadiusChanged((CornerRadius)e.NewValue);
            }
        }

        public virtual void OnCornerRadiusChanged(CornerRadius newValue) { }
    }
}
