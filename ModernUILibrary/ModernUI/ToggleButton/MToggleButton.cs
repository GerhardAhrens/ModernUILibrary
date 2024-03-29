namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;

    public class MToggleButton : ToggleButton
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(System.Windows.CornerRadius), typeof(MToggleButton));
        public System.Windows.CornerRadius CornerRadius
        {
            get { return (System.Windows.CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        static MToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MToggleButton), new FrameworkPropertyMetadata(typeof(MToggleButton)));
        }
    }
}
