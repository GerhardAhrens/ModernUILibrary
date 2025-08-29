namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;

    public class AnimationButton : ToggleButton
    {
        static AnimationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationButton), new FrameworkPropertyMetadata(typeof(AnimationButton)));
        }

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(AnimationButton), new PropertyMetadata(12d));
        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(AnimationButton));
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty AnimationIconProperty = DependencyProperty.Register(nameof(AnimationIcon), typeof(PathGeometry), typeof(AnimationButton));
        public PathGeometry AnimationIcon
        {
            get { return (PathGeometry)GetValue(AnimationIconProperty); }
            set { SetValue(AnimationIconProperty, value); }
        }

        public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(nameof(IconForeground), typeof(Brush), typeof(AnimationButton));
        public Brush IconForeground
        {
            get { return (Brush)GetValue(IconForegroundProperty); }
            set { SetValue(IconForegroundProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
