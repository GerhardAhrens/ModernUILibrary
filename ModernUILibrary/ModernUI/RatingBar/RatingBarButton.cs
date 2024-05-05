namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;

    public class RatingBarButton : ButtonBase
    {
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(RatingBarButton), new PropertyMetadata(false));

        public static readonly DependencyProperty IsHalfProperty =
            DependencyProperty.Register("IsHalf", typeof(bool), typeof(RatingBarButton), new PropertyMetadata(false));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(RatingBarButton));

        public static readonly RoutedEvent ItemMouseEnterEvent = 
            EventManager.RegisterRoutedEvent("ItemMouseEnter", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(RatingBarButton));

        public static readonly RoutedEvent ItemMouseLeaveEvent = 
            EventManager.RegisterRoutedEvent("ItemMouseLeave", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(RatingBarButton));

        static RatingBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RatingBarButton), new FrameworkPropertyMetadata(typeof(RatingBarButton)));
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public bool IsHalf
        {
            get { return (bool)GetValue(IsHalfProperty); }
            set { SetValue(IsHalfProperty, value); }
        }
        

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        
        public event RoutedPropertyChangedEventHandler<int> ItemMouseEnter
        {
            add
            {
                this.AddHandler(ItemMouseEnterEvent, value);
            }
            remove
            {
                this.RemoveHandler(ItemMouseEnterEvent, value);
            }
        }

        public virtual void OnItemMouseEnter(int oldValue, int newValue)
        {
            RoutedPropertyChangedEventArgs<int> arg = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue, ItemMouseEnterEvent);
            this.RaiseEvent(arg);
        }

        public event RoutedPropertyChangedEventHandler<int> ItemMouseLeave
        {
            add
            {
                this.AddHandler(ItemMouseLeaveEvent, value);
            }
            remove
            {
                this.RemoveHandler(ItemMouseLeaveEvent, value);
            }
        }

        public virtual void OnItemMouseLeave(int oldValue, int newValue)
        {
            RoutedPropertyChangedEventArgs<int> arg = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue, ItemMouseLeaveEvent);
            this.RaiseEvent(arg);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.MouseEnter += RatingBarButton_MouseEnter;
            this.MouseLeave += RatingBarButton_MouseLeave;
        }

        private void RatingBarButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.OnItemMouseEnter(this.Value, this.Value);
        }

        private void RatingBarButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.OnItemMouseLeave(this.Value, this.Value);
        }
    }
}
