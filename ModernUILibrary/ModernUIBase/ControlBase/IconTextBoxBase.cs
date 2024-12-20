namespace ModernIU.Base
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;

    public class IconTextBoxBase : MTextBoxBase
    {
        public static readonly DependencyProperty IsShowIconProperty = DependencyProperty.Register("IsShowIcon", typeof(bool), typeof(IconTextBoxBase), new PropertyMetadata(true));
        public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register("IconBackground", typeof(Brush), typeof(IconTextBoxBase));
        public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register("IconForeground", typeof(Brush), typeof(IconTextBoxBase));
        public static readonly DependencyProperty IconBorderBrushProperty = DependencyProperty.Register("IconBorderBrush", typeof(Brush), typeof(IconTextBoxBase));
        public static readonly DependencyProperty IconBorderThicknessProperty = DependencyProperty.Register("IconBorderThickness", typeof(Thickness), typeof(IconTextBoxBase));
        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register("IconWidth", typeof(double), typeof(IconTextBoxBase));
        public static readonly DependencyProperty IconPaddingProperty = DependencyProperty.Register("IconPadding", typeof(Thickness), typeof(IconTextBoxBase));
        public static readonly DependencyProperty IconCornerRadiusProperty = DependencyProperty.Register("IconCornerRadius", typeof(CornerRadius), typeof(IconTextBoxBase));
        public static readonly DependencyProperty IconPathDataProperty = DependencyProperty.Register("IconPathData", typeof(PathGeometry), typeof(IconTextBoxBase));



        [Bindable(true), Description("IsShowIcon")]
        public bool IsShowIcon
        {
            get { return (bool)GetValue(IsShowIconProperty); }
            set { SetValue(IsShowIconProperty, value); }
        }

        
        [Bindable(true), Description("IconBackground")]
        public Brush IconBackground
        {
            get { return (Brush)GetValue(IconBackgroundProperty); }
            set { SetValue(IconBackgroundProperty, value); }
        }


        [Bindable(true), Description("IconForeground")]
        public Brush IconForeground
        {
            get { return (Brush)GetValue(IconForegroundProperty); }
            set { SetValue(IconForegroundProperty, value); }
        }


        [Bindable(true), Description("IconBorderBrush")]
        public Brush IconBorderBrush
        {
            get { return (Brush)GetValue(IconBorderBrushProperty); }
            set { SetValue(IconBorderBrushProperty, value); }
        }

        [Bindable(true), Description("IconBorderThickness")]
        public Thickness IconBorderThickness
        {
            get { return (Thickness)GetValue(IconBorderThicknessProperty); }
            set { SetValue(IconBorderThicknessProperty, value); }
        }

        [Bindable(true), Description("IconWidth")]
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }


        [Bindable(true), Description("IconPadding")]
        public Thickness IconPadding
        {
            get { return (Thickness)GetValue(IconPaddingProperty); }
            set { SetValue(IconPaddingProperty, value); }
        }


        [Bindable(true), Description("IconCornerRadius")]
        public CornerRadius IconCornerRadius
        {
            get { return (CornerRadius)GetValue(IconCornerRadiusProperty); }
            set { SetValue(IconCornerRadiusProperty, value); }
        }


        [Bindable(true), Description("IconPathData")]
        public PathGeometry IconPathData
        {
            get { return (PathGeometry)GetValue(IconPathDataProperty); }
            set { SetValue(IconPathDataProperty, value); }
        }

        public override void OnCornerRadiusChanged(CornerRadius newValue)
        {
            this.SetValue(IconTextBoxBase.IconCornerRadiusProperty, new CornerRadius(newValue.TopLeft, 0, 0, newValue.BottomLeft));
        }
    }
}
