namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class PathButton : Button
    {
        public static readonly DependencyProperty PathWidthProperty = 
            DependencyProperty.Register("PathWidth", typeof(double), typeof(PathButton), new FrameworkPropertyMetadata(13d));

        public static readonly DependencyProperty PathDataProperty = 
            DependencyProperty.Register("PathData", typeof(PathGeometry), typeof(PathButton));

        public static readonly DependencyProperty MouseOverBackgroundProperty = 
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(PathButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(79, 193, 233))));

        public static readonly DependencyProperty PressedBackgroundProperty = 
            DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(PathButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(74, 137, 220))));

        public static readonly DependencyProperty MouseOverForegroundProperty = 
            DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(PathButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(79, 193, 233))));

        public static readonly DependencyProperty PressedForegroundProperty = 
            DependencyProperty.Register("PressedForeground", typeof(Brush), typeof(PathButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(74, 137, 220))));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(PathButton));

        static PathButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathButton), new FrameworkPropertyMetadata(typeof(PathButton)));
        }

        public double PathWidth
        {
            get { return (double)GetValue(PathWidthProperty); }
            set { SetValue(PathWidthProperty, value); }
        }

        public PathGeometry PathData
        {
            get { return (PathGeometry)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        public Brush PressedForeground
        {
            get { return (Brush)GetValue(PressedForegroundProperty); }
            set { SetValue(PressedForegroundProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
