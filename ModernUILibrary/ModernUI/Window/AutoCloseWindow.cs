namespace ModernIU.Controls
{
    using System.ComponentModel;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    using ModernIU.Base;

    public class AutoCloseWindow : BaseWindow
    {
        static AutoCloseWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCloseWindow), new FrameworkPropertyMetadata(typeof(AutoCloseWindow)));
        }

        private Button PART_Btn_Close;
        private Button PART_Btn_Minimized;
        private Button PART_Btn_Maximized;
        private Button PART_Btn_Restore;
        private Button PART_Btn_More;

        private Grid PART_TitleBar;
        private Popup PART_Popup_Menu;

        private double restore_window_width;
        private double restore_window_height;
        private double resotre_left;
        private double resotre_top;
        private int mouseClickCount;
        private bool mIsMaximized = false;
        private Timer mAutoCloseTimer = new Timer();

        public static readonly DependencyProperty CloseButtonTypeProperty =
        DependencyProperty.Register("CloseButtonType", typeof(CloseBoxTypeEnum), typeof(AutoCloseWindow), new PropertyMetadata(CloseBoxTypeEnum.Close));

        public CloseBoxTypeEnum CloseButtonType
        {
            get { return (CloseBoxTypeEnum)GetValue(CloseButtonTypeProperty); }
            set { SetValue(CloseButtonTypeProperty, value); }
        }

        public static readonly DependencyProperty AutoCloseProperty =
        DependencyProperty.Register("AutoClose", typeof(bool), typeof(AutoCloseWindow), new PropertyMetadata(false));

        public bool AutoClose
        {
            get { return (bool)GetValue(AutoCloseProperty); }
            set { SetValue(AutoCloseProperty, value); }
        }

        public static readonly DependencyProperty AutoCloseIntervalProperty =
        DependencyProperty.Register("AutoCloseInterval", typeof(double), typeof(AutoCloseWindow), new PropertyMetadata(3d));

        public double AutoCloseInterval
        {
            get { return (double)GetValue(AutoCloseIntervalProperty); }
            set { SetValue(AutoCloseIntervalProperty, value); }
        }

        public AutoCloseWindow() : base()
        {
            this.Loaded += AutoCloseWindow_Loaded;
        }

        private void AutoCloseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.AutoClose)
            {
                this.mAutoCloseTimer.Interval = this.AutoCloseInterval * 1000;
                this.mAutoCloseTimer.Elapsed += MAutoCloseTimer_Elapsed;
                if (!this.mAutoCloseTimer.Enabled)
                {
                    this.mAutoCloseTimer.Enabled = true;
                }
            }
        }

        private void MAutoCloseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.mAutoCloseTimer.Enabled)
            {
                this.mAutoCloseTimer.Enabled = false;
                this.Dispatcher.Invoke(new Action(delegate
                {
                    this.Close();
                }));
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

            if (this.mAutoCloseTimer != null)
            {
                this.mAutoCloseTimer.Enabled = false;
                this.mAutoCloseTimer.Dispose();
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if ((this.AutoClose && !this.mIsMaximized) || this.mIsMaximized)
            {
                this.mAutoCloseTimer.Enabled = false;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (this.mIsMaximized)
            {
                this.mAutoCloseTimer.Enabled = false;
                return;
            }

            if (this.AutoClose && this.IsLoaded)
            {
                this.mAutoCloseTimer.Enabled = true;
            }
        }
    }
}
