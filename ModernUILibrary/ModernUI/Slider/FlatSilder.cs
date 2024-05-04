namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    public class FlatSilder : Slider
    {
        public static readonly DependencyProperty IsVideoVisibleWhenPressThumbProperty = DependencyProperty.Register("IsVideoVisibleWhenPressThumb", typeof(bool), typeof(FlatSilder), new PropertyMetadata(false));

        public static readonly DependencyProperty IncreaseColorProperty = DependencyProperty.Register("IncreaseColor", typeof(Brush), typeof(FlatSilder));

        public static readonly DependencyProperty DecreaseColorProperty = DependencyProperty.Register("DecreaseColor", typeof(Brush), typeof(FlatSilder));

        public static readonly RoutedEvent DropValueChangedEvent = EventManager.RegisterRoutedEvent("DropValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(FlatSilder));

        private Thumb PART_Thumb;
        private Track PART_Track;
        private bool _thumbIsPressed;

        static FlatSilder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatSilder), new FrameworkPropertyMetadata(typeof(FlatSilder)));
        }

        public Brush DecreaseColor
        {
            get { return (Brush)GetValue(DecreaseColorProperty); }
            set { SetValue(DecreaseColorProperty, value); }
        }
        
        public Brush IncreaseColor
        {
            get { return (Brush)GetValue(IncreaseColorProperty); }
            set { SetValue(IncreaseColorProperty, value); }
        }
        
        public bool IsVideoVisibleWhenPressThumb
        {
            get { return (bool)GetValue(IsVideoVisibleWhenPressThumbProperty); }
            set { SetValue(IsVideoVisibleWhenPressThumbProperty, value); }
        }

        public event RoutedPropertyChangedEventHandler<double> DropValueChanged
        {
            add
            {
                this.AddHandler(DropValueChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(DropValueChangedEvent, value);
            }
        }

        public virtual void OnDropValueChanged(double oldValue, double newValue)
        {
            RoutedPropertyChangedEventArgs<double> arg = new RoutedPropertyChangedEventArgs<double>(oldValue, newValue, DropValueChangedEvent);
            this.RaiseEvent(arg);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_Thumb = this.GetTemplateChild("PART_Thumb") as Thumb;
            this.PART_Track = this.GetTemplateChild("PART_Track") as Track;
            if(this.PART_Thumb != null)
            {
                this.PART_Thumb.PreviewMouseLeftButtonDown += PART_Thumb_PreviewMouseLeftButtonDown;
                this.PART_Thumb.PreviewMouseLeftButtonUp += PART_Thumb_PreviewMouseLeftButtonUp;
            }
            if(this.PART_Track != null)
            {
                this.PART_Track.MouseLeftButtonDown += PART_Track_MouseLeftButtonDown;
                this.PART_Track.MouseLeftButtonUp += PART_Track_MouseLeftButtonUp;
            }
            this.ValueChanged += FlatSilder_ValueChanged;
        }

        private void PART_Track_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this._thumbIsPressed = this.IsVideoVisibleWhenPressThumb && true;
        }

        private void PART_Thumb_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this._thumbIsPressed = this.IsVideoVisibleWhenPressThumb && true;
        }

        private void FlatSilder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(this.IsVideoVisibleWhenPressThumb && this._thumbIsPressed)
            {
                this.OnDropValueChanged(this.Value, this.Value);
            }
        }

        private void PART_Thumb_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.IsVideoVisibleWhenPressThumb) return;
            this.OnDropValueChanged(this.Value, this.Value);
        }

        private void PART_Track_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.IsVideoVisibleWhenPressThumb) return;
            this.OnDropValueChanged(this.Value, this.Value);
        }
    }
}
