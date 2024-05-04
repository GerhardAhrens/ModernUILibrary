namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class MGroupBox : HeaderedContentControl
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MGroupBox), new PropertyMetadata(new CornerRadius(0), CornerRadiusCallback));

        public static readonly DependencyProperty HorizontalHeaderAlignmentProperty = DependencyProperty.Register("HorizontalHeaderAlignment", typeof(HorizontalAlignment), typeof(MGroupBox), new PropertyMetadata(HorizontalAlignment.Stretch));

        public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register("HeaderPadding", typeof(Thickness), typeof(MGroupBox));

        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(MGroupBox));

        public static readonly DependencyProperty CornerRadiusInnerProperty =DependencyProperty.Register("CornerRadiusInner", typeof(CornerRadius), typeof(MGroupBox));

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        public HorizontalAlignment HorizontalHeaderAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalHeaderAlignmentProperty); }
            set { SetValue(HorizontalHeaderAlignmentProperty, value); }
        }
        

        public Thickness HeaderPadding
        {
            get { return (Thickness)GetValue(HeaderPaddingProperty); }
            set { SetValue(HeaderPaddingProperty, value); }
        }
        
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private static void CornerRadiusCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MGroupBox groupBox = d as MGroupBox;
            if(groupBox != null)
            {
                CornerRadius radius = (CornerRadius)e.NewValue;
                groupBox.CornerRadiusInner = new CornerRadius(radius.TopLeft, radius.TopRight, 0, 0);
            }
        }

        public CornerRadius CornerRadiusInner
        {
            get { return (CornerRadius)GetValue(CornerRadiusInnerProperty); }
            private set { SetValue(CornerRadiusInnerProperty, value); }
        }
        
        static MGroupBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MGroupBox), new FrameworkPropertyMetadata(typeof(MGroupBox)));
        }
    }
}
