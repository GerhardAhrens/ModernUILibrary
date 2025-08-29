namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;

    public class IconToggleButton : ToggleButton
    {
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(IconToggleButton), new PropertyMetadata(Orientation.Horizontal));
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty CheckedIconProperty = DependencyProperty.Register(nameof(CheckedIcon), typeof(PathGeometry), typeof(IconToggleButton));
        public PathGeometry CheckedIcon
        {
            get { return (PathGeometry)GetValue(CheckedIconProperty); }
            set { SetValue(CheckedIconProperty, value); }
        }
        
        public static readonly DependencyProperty UncheckedIconProperty = DependencyProperty.Register(nameof(UncheckedIcon), typeof(PathGeometry), typeof(IconToggleButton));
        public PathGeometry UncheckedIcon
        {
            get { return (PathGeometry)GetValue(UncheckedIconProperty); }
            set { SetValue(UncheckedIconProperty, value); }
        }

        public static readonly DependencyProperty CheckedContentProperty = DependencyProperty.Register(nameof(CheckedContent), typeof(string), typeof(IconToggleButton));
        public string CheckedContent
        {
            get { return (string)GetValue(CheckedContentProperty); }
            set { SetValue(CheckedContentProperty, value); }
        }

        public static readonly DependencyProperty UncheckedIconSizeProperty = DependencyProperty.Register(nameof(UncheckedIconSize), typeof(double), typeof(IconToggleButton), new PropertyMetadata(12d));
        public double UncheckedIconSize
        {
            get { return (double)GetValue(UncheckedIconSizeProperty); }
            set { SetValue(UncheckedIconSizeProperty, value); }
        }

        public static readonly DependencyProperty CheckedIconSizeProperty = DependencyProperty.Register(nameof(CheckedIconSize), typeof(double), typeof(IconToggleButton), new PropertyMetadata(12d));
        public double CheckedIconSize
        {
            get { return (double)GetValue(CheckedIconSizeProperty); }
            set { SetValue(CheckedIconSizeProperty, value); }
        }

        public static readonly DependencyProperty CheckedForegroundProperty = DependencyProperty.Register(nameof(CheckedForeground), typeof(Brush), typeof(IconToggleButton));
        public Brush CheckedForeground
        {
            get { return (Brush)GetValue(CheckedForegroundProperty); }
            set { SetValue(CheckedForegroundProperty, value); }
        }

        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register(nameof(ContentMargin), typeof(Thickness), typeof(IconToggleButton));
        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(IconToggleButton));
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register(nameof(IconMargin), typeof(Thickness), typeof(IconToggleButton));
        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }

        static IconToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconToggleButton), new FrameworkPropertyMetadata(typeof(IconToggleButton)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
