namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using ModernIU.Base;

    public class FlatButton : Button
    {
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type" , typeof(FlatButtonSkinEnum), typeof(FlatButton));

        public static readonly DependencyProperty MouseOverBackgroundProperty = 
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(FlatButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(79, 193, 233))));

        public static readonly DependencyProperty PressedBackgroundProperty = 
            DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(FlatButton), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(74, 137, 220))));

        public static readonly DependencyProperty CornerRadiusProperty = 
            DependencyProperty.Register("CornerRadius" , typeof(CornerRadius), typeof(FlatButton));

        public static readonly DependencyProperty MouseOverBackground1Property = 
            DependencyProperty.Register("MouseOverBackground1", typeof(Color), typeof(FlatButton), new FrameworkPropertyMetadata(Color.FromRgb(79, 193, 233)));

        static FlatButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatButton), new FrameworkPropertyMetadata(typeof(FlatButton)));
        }

        public FlatButtonSkinEnum Type
        {
            get { return (FlatButtonSkinEnum)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
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

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public Color MouseOverBackground1
        {
            get { return (Color)GetValue(MouseOverBackground1Property); }
            set { SetValue(MouseOverBackground1Property, value); }
        }
    }
}
