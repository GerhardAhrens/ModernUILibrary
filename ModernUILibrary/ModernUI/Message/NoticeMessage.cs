namespace ModernIU.Controls
{
    using System.Timers;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    public class NoticeMessage : System.Windows.Controls.Control
    {
        private Timer mTimer;
        public static readonly DependencyProperty ContentProperty = 
            DependencyProperty.Register("Content" , typeof(string), typeof(NoticeMessage));

        public static readonly DependencyProperty IsShowProperty = 
            DependencyProperty.Register("IsShow", typeof(bool), typeof(NoticeMessage), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsShowChanged)));

        public static readonly DependencyProperty MessageTypeProperty = 
            DependencyProperty.Register("MessageType" , typeof(EnumMessageType), typeof(NoticeMessage) , new FrameworkPropertyMetadata(new PropertyChangedCallback(OnMessageTypeChanged)));

        public static readonly DependencyProperty MessageTypeStrProperty = DependencyProperty.Register("MessageTypeStr"
            , typeof(string), typeof(NoticeMessage));

        public static readonly DependencyProperty IconWidthProperty = 
            DependencyProperty.Register("IconWidth" , typeof(double), typeof(NoticeMessage), new FrameworkPropertyMetadata(13d));

        public static readonly DependencyProperty PathDataProperty = 
            DependencyProperty.Register("PathData" , typeof(PathGeometry), typeof(NoticeMessage));

        public static readonly DependencyProperty IconColorProperty = 
            DependencyProperty.Register("IconColor" , typeof(Brush), typeof(NoticeMessage));

        public static readonly DependencyProperty CornerRadiusProperty = 
            DependencyProperty.Register("CornerRadius" , typeof(System.Windows.CornerRadius), typeof(NoticeMessage));

        public static readonly DependencyProperty DurationProperty = 
            DependencyProperty.Register("Duration" , typeof(double), typeof(NoticeMessage) , new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDurationChanged)));

        static NoticeMessage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NoticeMessage), new FrameworkPropertyMetadata(typeof(NoticeMessage)));
        }

        public NoticeMessage() : base()
        {
            mTimer = new Timer();
            mTimer.Interval = this.Duration == 0 ? 1500 : this.Duration;
            mTimer.Elapsed += MTimer_Elapsed;
            this.Opacity = 0;
        }

        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public bool IsShow
        {
            get { return (bool)GetValue(IsShowProperty); }
            set { SetValue(IsShowProperty, value); }
        }

        public EnumMessageType MessageType
        {
            get { return (EnumMessageType)GetValue(MessageTypeProperty); }
            set
            {
                SetValue(MessageTypeProperty, value);
                this.MessageTypeStr = value.ToString();
            }
        }

        public string MessageTypeStr
        {
            get { return (string)GetValue(MessageTypeStrProperty); }
            set { SetValue(MessageTypeStrProperty, value); }
        }

        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public PathGeometry PathData
        {
            get { return (PathGeometry)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        public Brush IconColor
        {
            get { return (Brush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public System.Windows.CornerRadius CornerRadius
        {
            get { return (System.Windows.CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        private static void OnIsShowChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NoticeMessage noticeMessage = (NoticeMessage)sender;
            if(e.Property == IsShowProperty)
            {
                if(Convert.ToBoolean(e.NewValue))
                {
                    noticeMessage.IsShow = false;
                    noticeMessage.mTimer.Enabled = false;

                    noticeMessage.mTimer.Enabled = true;
                    noticeMessage.ShowAnimation();
                }
            }
        }

        private static void OnDurationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            NoticeMessage noticeMessage = (NoticeMessage)sender;
            if (e.Property == DurationProperty)
            {
                noticeMessage.mTimer.Interval = Convert.ToDouble(e.NewValue);
            }
        }

        private static void OnMessageTypeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //NoticeMessage noticeMessage = (NoticeMessage)sender;
            //if (e.Property == MessageTypeProperty)
            //{
            //    switch ((EnumMessageType)e.NewValue)
            //    {
            //        case EnumMessageType.Warn:
            //            noticeMessage.IconColor = new SolidColorBrush(Color.FromRgb(239, 186, 72));
            //            break;
            //        case EnumMessageType.Info:
            //            noticeMessage.IconColor = new SolidColorBrush(Color.FromRgb(83, 194, 232));
            //            break;
            //        case EnumMessageType.Error:
            //            noticeMessage.IconColor = new SolidColorBrush(Color.FromRgb(228, 99, 99));
            //            break;
            //        case EnumMessageType.Success:
            //            noticeMessage.IconColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        private void MTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke((System.Threading.ThreadStart)delegate
            {
                IsShow = false;
                this.HideAnimation();
                this.mTimer.Enabled = false;

            }, System.Windows.Threading.DispatcherPriority.Normal);
        }

        private void ShowAnimation()
        {
            DoubleAnimation animation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            this.BeginAnimation(OpacityProperty, animation);
        }

        private void HideAnimation()
        {
            DoubleAnimation animation = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
            this.BeginAnimation(OpacityProperty, animation);
        }
    }

    public enum EnumMessageType
    {
        Warn,
        Info,
        Error,
        Success,
    }
}
