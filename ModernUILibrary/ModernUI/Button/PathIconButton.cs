namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class PathIconButton : Button
    {
        public static readonly DependencyProperty PathWidthProperty = DependencyProperty.Register("PathWidth", typeof(double), typeof(PathIconButton), new FrameworkPropertyMetadata(13d));

        public static readonly DependencyProperty PathDataProperty = DependencyProperty.Register("PathData", typeof(PathGeometry), typeof(PathIconButton));

        public static readonly DependencyProperty NormalPathColorProperty = 
            DependencyProperty.Register("NormalPathColor", typeof(Brush), typeof(PathIconButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(40, 139, 225))));

        public static readonly DependencyProperty MouseOverPathColorProperty = 
            DependencyProperty.Register("MouseOverPathColor", typeof(Brush), typeof(PathIconButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(40, 139, 225))));

        public static readonly DependencyProperty PressedPathColorProperty = 
            DependencyProperty.Register("PressedPathColor", typeof(Brush), typeof(PathIconButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(36, 127, 207))));

        public static readonly DependencyProperty DisabledPathColorProperty = DependencyProperty.Register("DisabledPathColor", typeof(Brush), typeof(PathIconButton));

        public static readonly DependencyProperty CornerRadiusProperty =  DependencyProperty.Register("CornerRadius" , typeof(CornerRadius), typeof(PathIconButton));

        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register("MouseOverBackground" , typeof(Brush), typeof(PathIconButton));

        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(PathIconButton));

        static PathIconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathIconButton), new FrameworkPropertyMetadata(typeof(PathIconButton)));
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

        public Brush NormalPathColor
        {
            get { return (Brush)GetValue(NormalPathColorProperty); }
            set { SetValue(NormalPathColorProperty, value); }
        }

        public Brush MouseOverPathColor
        {
            get { return (Brush)GetValue(MouseOverPathColorProperty); }
            set { SetValue(MouseOverPathColorProperty, value); }
        }

        public Brush PressedPathColor
        {
            get { return (Brush)GetValue(PressedPathColorProperty); }
            set { SetValue(PressedPathColorProperty, value); }
        }


        public Brush DisabledPathColor
        {
            get { return (Brush)GetValue(DisabledPathColorProperty); }
            set { SetValue(DisabledPathColorProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
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
    }
}
