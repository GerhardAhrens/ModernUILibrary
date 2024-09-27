namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// Enum zur Darstellung
    /// </summary>
    public enum BadgeType
    {
        Dot,
        Normal,
    }

    public class BadgeAdorner : Adorner
    {
        public static readonly DependencyProperty HasAdornerProperty = 
            DependencyProperty.RegisterAttached("HasAdorner", typeof(bool), typeof(BadgeAdorner), new PropertyMetadata(false, HasAdornerChangedCallBack));

        public static readonly DependencyProperty IsShowAdornerProperty = 
            DependencyProperty.RegisterAttached("IsShowAdorner", typeof(bool), typeof(BadgeAdorner), new PropertyMetadata(false, IsShowChangedCallBack));

        public static readonly DependencyProperty NumberProperty = 
            DependencyProperty.RegisterAttached("Number", typeof(int), typeof(BadgeAdorner), new PropertyMetadata(0, NumberChangedCallBack, CoerceNumberCallback));

        public static readonly DependencyProperty BackgroundProperty = 
            DependencyProperty.RegisterAttached("Background", typeof(Brush), typeof(BadgeAdorner), new PropertyMetadata(Brushes.Red, BackgroundCallBack));

        public static readonly DependencyProperty BadgeTypeProperty =
            DependencyProperty.RegisterAttached("BadgeType", typeof(BadgeType), typeof(BadgeAdorner), new PropertyMetadata(BadgeType.Normal, BadgeTypeChangeCallback));

        private Badge badge = null;
        private VisualCollection _visuals = null;

        public BadgeAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _visuals = new VisualCollection(this);
            TranslateTransform tt = new TranslateTransform();
            tt.X = 13;
            tt.Y = -10;
            this.badge = new Badge()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                RenderTransform = tt,
            };

            this.badge.Background = this.BackColor;
            this.badge.DataContext = adornedElement;

            this._visuals.Add(badge);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.badge.Background = this.BackColor;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return this._visuals.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            badge.Arrange(new Rect(finalSize));

            return base.ArrangeOverride(finalSize);
        }

        public static bool GetHasAdorner(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasAdornerProperty);
        }

        public static void SetHasAdorner(DependencyObject obj, bool value)
        {
            obj.SetValue(HasAdornerProperty, value);
        }
        public static bool GetIsShowAdorner(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsShowAdornerProperty);
        }

        public static void SetIsShowAdorner(DependencyObject obj, bool value)
        {
            obj.SetValue(IsShowAdornerProperty, value);
        }

        public static int GetNumber(DependencyObject obj)
        {
            return (int)obj.GetValue(NumberProperty);
        }

        public static void SetNumber(DependencyObject obj, int value)
        {
            obj.SetValue(NumberProperty, value);
        }

        public static Brush GetBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(Badge.BackgroundProperty);
        }

        public static void SetBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(Badge.BackgroundProperty, value);
        }

        public Brush BackColor
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static BadgeType GetBadgeType(DependencyObject obj)
        {
            return (BadgeType)obj.GetValue(BadgeTypeProperty);
        }

        public static void SetBadgeType(DependencyObject obj, BadgeType value)
        {
            obj.SetValue(BadgeTypeProperty, value);
        }

        private static void HasAdornerChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if ((bool)e.NewValue)
            {
                if (element != null)
                {
                    var adornerLayer = AdornerLayer.GetAdornerLayer(element);

                    if (adornerLayer != null)
                    {
                        adornerLayer.Add(new BadgeAdorner(element as UIElement));
                    }
                    else
                    {
                        element.Loaded += (s1, e1) => {
                            var adorner = new BadgeAdorner(element);
                            AdornerLayer.GetAdornerLayer(element).Add(adorner);
                        };
                    }
                }
            }
            else
            {
                AdornerLayer layer = AdornerLayer.GetAdornerLayer(element);
                if (layer != null)
                {
                    Adorner[] AllAdorners = layer.GetAdorners(element);
                    if (AllAdorners != null)
                    {
                        IEnumerable<Adorner> desAdorners = AllAdorners.Where(p => p is BadgeAdorner);
                        if (desAdorners != null && desAdorners.Count() > 0)
                        {
                            desAdorners.ToList().ForEach(p => layer.Remove(p));
                        }
                    }
                }
            }
        }

        private static void IsShowChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BadgeAdorner adorner = BadgeAdorner.GetAdorner(d);
            if (adorner == null)
            {
                return;
            }

            if ((bool)e.NewValue)
            {
                adorner.ShowAdorner();
            }
            else
            {
                adorner.HideAdorner();
            }
        }

        private static void NumberChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void BackgroundCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BadgeAdorner adorner = BadgeAdorner.GetAdorner(d);

            if (adorner == null)
            {
                return;
            }

            adorner.SetBackground(Brushes.Green);

        }

        private static object CoerceNumberCallback(DependencyObject d, object baseValue)
        {
            int promptCount = (int)baseValue;
            promptCount = Math.Max(0, promptCount);

            return promptCount;
        }

        private static void BadgeTypeChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BadgeAdorner adorner = BadgeAdorner.GetAdorner(d);

            if (adorner == null)
            {
                return;
            }

            if ((BadgeType)e.NewValue == BadgeType.Dot)
            {
                adorner.SowDot();
            }
            else
            {
                adorner.ShowNormal();
            }
        }

        private void SetBackground(Brush backColor)
        {
            this.badge.Background = backColor;
        }

        private void ShowAdorner()
        {
            badge.Visibility = Visibility.Visible;
        }

        private void HideAdorner()
        {
            badge.Visibility = Visibility.Collapsed;
        }

        private void ShowNormal()
        {
            TranslateTransform tt = new TranslateTransform();
            tt.X = 10;
            tt.Y = -10;
            this.badge.RenderTransform = tt;
            this.badge.IsDot = false;
        }

        private void SowDot()
        {
            TranslateTransform tt = new TranslateTransform();
            tt.X = 4;
            tt.Y = -4;
            this.badge.RenderTransform = tt;
            this.badge.IsDot = true;
        }

        private static BadgeAdorner GetAdorner(DependencyObject d)
        {
            var element = d as FrameworkElement;

            if (element != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(element);
                if (adornerLayer != null)
                {
                    var adorners = adornerLayer.GetAdorners(element);
                    if (adorners != null && adorners.Count() != 0)
                    {
                        var adorner = adorners.FirstOrDefault() as BadgeAdorner;

                        return adorner;
                    }
                }
            }

            return null;
        }
    }
}