namespace ModernIU.Controls
{
    using System.Windows;
    using System.Timers;
    using System.Windows.Media;
    using System.Windows.Input;

    using ModernIU.Base;
    using System.ComponentModel;
    using System.Windows.Threading;

    public class MWindow : Window
    {
        private DispatcherTimer mAutoCloseTimer = new DispatcherTimer();
        private bool mIsMaximized = false;

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
            DependencyProperty.Register("AutoCloseInterval", typeof(double), typeof(MWindow), new PropertyMetadata(3d));

        public double AutoCloseInterval
        {
            get { return (double)GetValue(AutoCloseIntervalProperty); }
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
                this.mAutoCloseTimer.Interval = TimeSpan.FromSeconds(this.AutoCloseInterval * 1000);
                WeakEventManager<DispatcherTimer, EventArgs>.AddHandler(this.mAutoCloseTimer, "Tick", this.OnAutoCloseTimerElapsed);

                if (this.mAutoCloseTimer.IsEnabled == false)
                {
                    this.mAutoCloseTimer.Start();
                    this.mAutoCloseTimer.IsEnabled = true;
                }
            }
        }


        private void OnAutoCloseTimerElapsed(object sender, EventArgs e)
        {
            if (this.mAutoCloseTimer.IsEnabled == true)
            {
                this.mAutoCloseTimer.IsEnabled = false;
                this.mAutoCloseTimer.Stop();
                this.Close();
            }
        }

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

            if (this.AutoClose == true && this.mAutoCloseTimer != null)
            {
                this.mAutoCloseTimer.IsEnabled = false;
                WeakEventManager<DispatcherTimer, EventArgs>.RemoveHandler(this.mAutoCloseTimer, "Tick", this.OnAutoCloseTimerElapsed);
            }
        }

        #region Override

        /*
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if ((this.AutoClose && !this.mIsMaximized) || this.mIsMaximized)
            {
                this.mAutoCloseTimer.IsEnabled = false;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (this.mIsMaximized == true)
            {
                this.mAutoCloseTimer.IsEnabled = false;
                return;
            }

            if (this.AutoClose == true && this.IsLoaded == true)
            {
                try
                {
                    this.mAutoCloseTimer.IsEnabled = true;
                }
                catch (Exception) {}
            }
        }
        */

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion
    }
}
