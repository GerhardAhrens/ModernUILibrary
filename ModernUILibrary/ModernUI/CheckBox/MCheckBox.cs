namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class MCheckBox : CheckBox
    {
        public static readonly DependencyProperty UnCheckedColorProperty = DependencyProperty.Register("UnCheckedColor", typeof(Brush), typeof(MCheckBox));
        public static readonly DependencyProperty CheckedColorProperty = DependencyProperty.Register("CheckedColor", typeof(Brush), typeof(MCheckBox));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MCheckBox));

        static MCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MCheckBox), new FrameworkPropertyMetadata(typeof(MCheckBox)));
        }

        public Brush UnCheckedColor
        {
            get { return (Brush)GetValue(UnCheckedColorProperty); }
            set { SetValue(UnCheckedColorProperty, value); }
        }


        public Brush CheckedColor
        {
            get { return (Brush)GetValue(CheckedColorProperty); }
            set { SetValue(CheckedColorProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
    }
}
