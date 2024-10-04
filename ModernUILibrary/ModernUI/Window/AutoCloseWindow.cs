namespace ModernIU.Controls
{
    using System.ComponentModel;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Threading;

    using ModernIU.Base;

    [TemplatePart(Name = "PART_Btn_Close", Type = typeof(System.Windows.Controls.Button))]
    [TemplatePart(Name = "PART_Btn_Minimized", Type = typeof(System.Windows.Controls.Button))]
    [TemplatePart(Name = "PART_Btn_Maximized", Type = typeof(System.Windows.Controls.Button))]
    [TemplatePart(Name = "PART_Btn_Restore", Type = typeof(System.Windows.Controls.Button))]
    [TemplatePart(Name = "PART_TitleBar", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_Btn_More", Type = typeof(System.Windows.Controls.Button))]
    [TemplatePart(Name = "PART_Popup_Menu", Type = typeof(Popup))]
    public class AutoCloseWindow : Window
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

        public static readonly DependencyProperty ShowMoreProperty =
            DependencyProperty.Register("ShowMore", typeof(bool), typeof(AutoCloseWindow), new PropertyMetadata(false));

        public bool ShowMore
        {
            get { return (bool)GetValue(ShowMoreProperty); }
            set { SetValue(ShowMoreProperty, value); }
        }

        public static readonly DependencyProperty MaximizeBoxProperty =
            DependencyProperty.Register("MaximizeBox", typeof(bool), typeof(AutoCloseWindow), new PropertyMetadata(true));

        public bool MaximizeBox
        {
            get { return (bool)GetValue(MaximizeBoxProperty); }
            set { SetValue(MaximizeBoxProperty, value); }
        }

        public static readonly DependencyProperty MinimizeBoxProperty =
            DependencyProperty.Register("MinimizeBox", typeof(bool), typeof(AutoCloseWindow), new PropertyMetadata(true));

        public bool MinimizeBox
        {
            get { return (bool)GetValue(MinimizeBoxProperty); }
            set { SetValue(MinimizeBoxProperty, value); }
        }

        public static readonly DependencyProperty CloseBoxProperty =
            DependencyProperty.Register("CloseBox", typeof(bool), typeof(AutoCloseWindow), new PropertyMetadata(true));

        public bool CloseBox
        {
            get { return (bool)GetValue(CloseBoxProperty); }
            set { SetValue(CloseBoxProperty, value); }
        }

        public static readonly DependencyProperty CaptionHeightProperty =
            DependencyProperty.Register("CaptionHeight", typeof(double), typeof(AutoCloseWindow), new PropertyMetadata(30d));

        public double CaptionHeight
        {
            get { return (double)GetValue(CaptionHeightProperty); }
            set { SetValue(CaptionHeightProperty, value); }
        }

        public static readonly DependencyProperty CanMoveWindowProperty =
            DependencyProperty.Register("CanMoveWindow", typeof(bool), typeof(AutoCloseWindow), new PropertyMetadata(true));

        public bool CanMoveWindow
        {
            get { return (bool)GetValue(CanMoveWindowProperty); }
            set { SetValue(CanMoveWindowProperty, value); }
        }

        public static readonly DependencyProperty TitleBackgroundProperty = 
            DependencyProperty.Register("TitleBackground" , typeof(System.Windows.Media.Brush), typeof(AutoCloseWindow));

        public System.Windows.Media.Brush TitleBackground
        {
            get { return (System.Windows.Media.Brush)GetValue(TitleBackgroundProperty); }
            set { SetValue(TitleBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MenuPanelProperty =
            DependencyProperty.Register("MenuPanel", typeof(System.Windows.Controls.Panel), typeof(AutoCloseWindow));

        public System.Windows.Controls.Panel MenuPanel
        {
            get { return (System.Windows.Controls.Panel)GetValue(MenuPanelProperty); }
            set { SetValue(MenuPanelProperty, value); }
        }

        public static readonly RoutedEvent ShowMoreClickEvent =
            EventManager.RegisterRoutedEvent("ShowMoreClick", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(AutoCloseWindow));

        public event RoutedPropertyChangedEventHandler<object> ShowMoreClick
        {
            add
            {
                this.AddHandler(ShowMoreClickEvent, value);
            }
            remove
            {
                this.RemoveHandler(ShowMoreClickEvent, value);
            }
        }

        protected virtual void OnShowMoreClick(object oldValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg =
                new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, ShowMoreClickEvent);
            this.RaiseEvent(arg);
        }
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
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Maximized:
                    this.WindowState = WindowState.Normal;
                    this.SetWindowMaximized();
                    break;
                default:
                    break;
            }

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

        private void GridTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.MaximizeBox) return;

            mouseClickCount += 1;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; mouseClickCount = 0; };
            timer.IsEnabled = true;
            if (mouseClickCount % 2 == 0)
            {
                timer.IsEnabled = false;
                mouseClickCount = 0;

                if (this.mIsMaximized)
                {
                    this.SetWindowSizeRestore();
                }
                else
                {
                    this.SetWindowMaximized();
                }
            }
        }

        private void PART_Btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            this.SetWindowSizeRestore();
        }

        private void PART_Btn_Maximized_Click(object sender, RoutedEventArgs e)
        {
            this.SetWindowMaximized();
        }

        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PART_Btn_More_Click(object sender, RoutedEventArgs e)
        {
            if (this.PART_Btn_More != null)
            {
                this.PART_Popup_Menu.Child = this.MenuPanel;
                this.PART_Popup_Menu.IsOpen = true;
            }
        }

        private void SetWindowMaximized()
        {
            if (VisualTreeHelper.GetChildrenCount(this) > 0)
            {
                this.restore_window_width = this.Width;
                this.restore_window_height = this.Height;
                this.resotre_left = this.Left;
                this.resotre_top = this.Top;

                Grid a = (Grid)VisualTreeHelper.GetChild(this, 0);
                a.Margin = new Thickness(0, 0, 0, 0);
                Border b = (Border)VisualTreeHelper.GetChild(a, 0);
                b.Visibility = Visibility.Collapsed;
                this.Left = 0;
                this.Top = 0;
                Rect rc = SystemParameters.WorkArea;
                this.Width = rc.Width;
                this.Height = rc.Height;

                this.mIsMaximized = true;
                this.PART_Btn_Maximized.Visibility = Visibility.Hidden;
                this.PART_Btn_Restore.Visibility = Visibility.Visible;
            }
        }

        private void SetWindowSizeRestore()
        {
            if (VisualTreeHelper.GetChildrenCount(this) > 0)
            {
                Grid a = (Grid)VisualTreeHelper.GetChild(this, 0);
                a.Margin = new Thickness(20, 20, 20, 20);
                Border b = (Border)VisualTreeHelper.GetChild(a, 0);
                b.Visibility = Visibility.Visible;
                this.Left = this.resotre_left;
                this.Top = this.resotre_top;
                this.Width = this.restore_window_width;
                this.Height = this.restore_window_height;

                this.mIsMaximized = false;
                this.PART_Btn_Restore.Visibility = Visibility.Hidden;
                this.PART_Btn_Maximized.Visibility = Visibility.Visible;
            }
        }

        Storyboard storyboard = new Storyboard();
        private void Animation(double oldWidth, double oldHeight, double newWidth, double newHeight)
        {
            DoubleAnimation widthAnimation = new DoubleAnimation(oldWidth, newWidth, new Duration(TimeSpan.FromMilliseconds(500)));
            Storyboard.SetTarget(widthAnimation, this);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath("(Window.Width)"));
            storyboard.Children.Add(widthAnimation);

            DoubleAnimation heightAnimation = new DoubleAnimation(oldWidth, newWidth, new Duration(TimeSpan.FromMilliseconds(500)));
            Storyboard.SetTarget(heightAnimation, this);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath("(Window.Height)"));
            storyboard.Children.Add(heightAnimation);

            storyboard.Begin();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_Btn_Close = VisualHelper.FindVisualElement<System.Windows.Controls.Button>(this, "PART_Btn_Close");
            this.PART_Btn_Minimized = VisualHelper.FindVisualElement<System.Windows.Controls.Button>(this, "PART_Btn_Minimized");
            this.PART_Btn_Maximized = VisualHelper.FindVisualElement<System.Windows.Controls.Button>(this, "PART_Btn_Maximized");
            this.PART_Btn_Restore = VisualHelper.FindVisualElement<System.Windows.Controls.Button>(this, "PART_Btn_Restore");
            this.PART_TitleBar = VisualHelper.FindVisualElement<Grid>(this, "PART_TitleBar");
            this.PART_Btn_More = VisualHelper.FindVisualElement<System.Windows.Controls.Button>(this, "PART_Btn_More");
            this.PART_Popup_Menu = VisualHelper.FindVisualElement<Popup>(this, "PART_Popup_Menu");

            if (this.PART_Btn_Close != null)
            {
                this.PART_Btn_Close.Click += Btn_close_Click;
            }

            if (this.PART_Btn_Maximized != null)
            {
                this.PART_Btn_Maximized.Click += PART_Btn_Maximized_Click;
            }

            if (this.PART_Btn_Restore != null)
            {
                this.PART_Btn_Restore.Click += PART_Btn_Restore_Click;
            }

            if (!this.MaximizeBox && !this.MinimizeBox && !this.CloseBox && string.IsNullOrEmpty(this.Title.Trim()))
            {
                this.PART_TitleBar.Visibility = Visibility.Collapsed;
            }

            if (this.PART_Btn_More != null)
            {
                this.PART_Btn_More.Click += PART_Btn_More_Click; ;
            }

            if (this.PART_TitleBar != null)
            {
                this.PART_TitleBar.MouseLeftButtonDown += GridTitleBar_MouseLeftButtonDown;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!this.CanMoveWindow) return;

            if (this.mIsMaximized) return;

            base.OnMouseLeftButtonDown(e);

            if (PART_TitleBar != null)
            {
                System.Windows.Point position = e.GetPosition(PART_TitleBar);

                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (position.X >= 0 && position.X < PART_TitleBar.ActualWidth && position.Y >= 0
                        && position.Y < PART_TitleBar.ActualHeight)
                    {
                        this.DragMove();
                    }
                }
            }
        }
    }
}
