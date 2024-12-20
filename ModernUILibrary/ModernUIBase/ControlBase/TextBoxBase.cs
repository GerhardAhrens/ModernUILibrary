
namespace ModernIU.Base
{
    using System.Windows;
    using System.Windows.Controls;

    public class TextBoxBase : TextBox
    {
        
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark" , typeof(string), typeof(TextBoxBase));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
    }
}
