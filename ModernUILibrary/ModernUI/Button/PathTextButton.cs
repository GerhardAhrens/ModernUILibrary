namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class PathTextButton : Button
    {
        public static readonly DependencyProperty PathWidthProperty = 
            DependencyProperty.Register("PathWidth", typeof(double), typeof(PathTextButton), new FrameworkPropertyMetadata(13d));

        public static readonly DependencyProperty PathDataProperty = 
            DependencyProperty.Register("PathData", typeof(PathGeometry), typeof(PathTextButton));

        public static readonly DependencyProperty MouseOverForegroundProperty = 
            DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(PathTextButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(40, 139, 225))));

        public static readonly DependencyProperty PressedForegroundProperty = 
            DependencyProperty.Register("PressedForeground", typeof(Brush), typeof(PathTextButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(36, 127, 207))));

        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(PathTextButton));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(PathTextButton), new PropertyMetadata(Orientation.Horizontal));

        public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register("DisabledForeground", typeof(Brush), typeof(PathTextButton));

        static PathTextButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathTextButton), new FrameworkPropertyMetadata(typeof(PathTextButton)));
        }

        public double PathWidth
        {
            get { return (double)GetValue(PathWidthProperty); }
            set { SetValue(PathWidthProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public PathGeometry PathData
        {
            get { return (PathGeometry)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
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

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        public Brush DisabledForeground
        {
            get { return (Brush)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }
    }
}
