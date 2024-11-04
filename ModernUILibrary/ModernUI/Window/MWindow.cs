namespace ModernIU.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Threading;

    using ModernIU.Base;

    public class MWindow : Window
    {
        private DispatcherTimer _AutoCloseTimer = null;

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

        #region CloseButtonType
        public static readonly DependencyProperty CloseButtonTypeProperty =
            DependencyProperty.Register("CloseButtonType", typeof(CloseBoxTypeEnum), typeof(MWindow), new PropertyMetadata(CloseBoxTypeEnum.Close));

        public CloseBoxTypeEnum CloseButtonType
        {
            get { return (CloseBoxTypeEnum)GetValue(CloseButtonTypeProperty); }
            set { SetValue(CloseButtonTypeProperty, value); }
        }
        #endregion CloseButtonType

        #region AutoClose
        public static readonly DependencyProperty AutoCloseProperty =
            DependencyProperty.Register("AutoClose", typeof(bool), typeof(MWindow), new PropertyMetadata(false));

        public bool AutoClose
        {
            get { return (bool)GetValue(AutoCloseProperty); }
            set { SetValue(AutoCloseProperty, value); }
        }
        #endregion AutoClose

        #region AutoCloseInterval
        public static readonly DependencyProperty AutoCloseIntervalProperty =
            DependencyProperty.Register("AutoCloseInterval", typeof(int), typeof(MWindow), new PropertyMetadata(3));

        public int AutoCloseInterval
        {
            get { return (int)GetValue(AutoCloseIntervalProperty); }
            set { SetValue(AutoCloseIntervalProperty, value); }
        }
        #endregion AutoCloseInterval
        #endregion

        #region Constructors

        static MWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MWindow), new FrameworkPropertyMetadata(typeof(MWindow)));
        }

        public MWindow() : base()
        {
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }
        #endregion

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.AutoClose == true)
            {
                this._AutoCloseTimer = new DispatcherTimer();
                WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(this._AutoCloseTimer, "Tick", this.OnAutoCloseTimerElapsed);
                this._AutoCloseTimer.Interval = new TimeSpan(0,0,this.AutoCloseInterval);

                if (this._AutoCloseTimer.IsEnabled == false)
                {
                    this._AutoCloseTimer.Start();
                }
            }
        }


        private void OnAutoCloseTimerElapsed(object sender, EventArgs e)
        {
            if (this._AutoCloseTimer.IsEnabled == true)
            {
                this._AutoCloseTimer.Stop();
                WeakEventManager<DispatcherTimer, EventArgs>.RemoveHandler(this._AutoCloseTimer, "Tick", this.OnAutoCloseTimerElapsed);
                this.Close();
            }
        }

        #region Override
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            switch (this.CloseButtonType)
            {
                case CloseBoxTypeEnum.Close:
                    break;
                case CloseBoxTypeEnum.Hide:
                    this.Hide();
                    e.Cancel = true;
                    break;
                default:
                    break;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion
    }
}
