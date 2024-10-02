namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Media;

    public class MWindow : Window
    {
        #region DependencyProperty
        #region TitleBackground

        public Brush TitleBackground
        {
            get { return (Brush)GetValue(TitleBackgroundProperty); }
            set { SetValue(TitleBackgroundProperty, value); }
        }
        
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(MWindow));

        #endregion

        #region TitleForeground

        public Brush TitleForeground
        {
            get { return (Brush)GetValue(TitleForegroundProperty); }
            set { SetValue(TitleForegroundProperty, value); }
        }
        
        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(MWindow));

        #endregion

        #region TitleFontSize

        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }
        
        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.Register("TitleFontSize", typeof(double), typeof(MWindow), new PropertyMetadata(12d));

        #endregion

        #region TitleFontFamily

        public FontFamily TitleFontFamily
        {
            get { return (FontFamily)GetValue(TitleFontFamilyProperty); }
            set { SetValue(TitleFontFamilyProperty, value); }
        }
        
        public static readonly DependencyProperty TitleFontFamilyProperty =
            DependencyProperty.Register("TitleFontFamily", typeof(FontFamily), typeof(MWindow));

        #endregion

        #region MaximizeBox

        public static readonly DependencyProperty MaximizeBoxProperty = 
            DependencyProperty.Register("MaximizeBox" , typeof(bool), typeof(MWindow), new PropertyMetadata(true));

        public bool MaximizeBox
        {
            get { return (bool)GetValue(MaximizeBoxProperty); }
            set { SetValue(MaximizeBoxProperty, value); }
        }

        #endregion

        #region MinimizeBox

        public static readonly DependencyProperty MinimizeBoxProperty = 
            DependencyProperty.Register("MinimizeBox" , typeof(bool), typeof(MWindow), new PropertyMetadata(true));

        public bool MinimizeBox
        {
            get { return (bool)GetValue(MinimizeBoxProperty); }
            set { SetValue(MinimizeBoxProperty, value); }
        }

        #endregion

        #region CloseBox

        public static readonly DependencyProperty CloseBoxProperty = 
            DependencyProperty.Register("CloseBox" , typeof(bool), typeof(MWindow), new PropertyMetadata(true));

        public bool CloseBox
        {
            get { return (bool)GetValue(CloseBoxProperty); }
            set { SetValue(CloseBoxProperty, value); }
        }

        #endregion

        #region MoreOnTitle

        public object MoreOnTitle
        {
            get { return (object)GetValue(MoreOnTitleProperty); }
            set { SetValue(MoreOnTitleProperty, value); }
        }
        
        public static readonly DependencyProperty MoreOnTitleProperty =
            DependencyProperty.Register("MoreOnTitle", typeof(object), typeof(MWindow));

        #endregion

        #endregion

        #region Constructors

        static MWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MWindow), new FrameworkPropertyMetadata(typeof(MWindow)));
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion
    }
}
