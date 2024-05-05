namespace ModernIU.Controls
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class RatingBar : Control
    {
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(RatingBar), new PropertyMetadata(5, MaximumChangedCallback));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(RatingBar), new PropertyMetadata(1, MinimumChangedCallback, CoreMinimumCallback));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(RatingBar), new PropertyMetadata(0d, ValueChangedCallback, ValueCoerce));

        public static readonly DependencyProperty ValueItemTemplateProperty =
            DependencyProperty.Register("ValueItemTemplate", typeof(DataTemplate), typeof(RatingBar));

        public static readonly DependencyProperty ValueItemStyleProperty =
            DependencyProperty.Register("ValueItemStyle", typeof(Style), typeof(RatingBar));

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Brush), typeof(RatingBar));

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(RatingBar), new PropertyMetadata(false));

        public static readonly DependencyProperty UnSelectedColorProperty =
            DependencyProperty.Register("UnSelectedColor", typeof(Brush), typeof(RatingBar));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(RatingBar));

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(RatingBar));

        public static readonly DependencyProperty ContentStringFormatProperty =
            DependencyProperty.Register("ContentStringFormat", typeof(string), typeof(RatingBar));

        public static readonly DependencyProperty IsShowContentProperty =
            DependencyProperty.Register("IsShowContent", typeof(bool), typeof(RatingBar), new PropertyMetadata(true));

        public static readonly DependencyProperty RatingButtonsProperty =
            DependencyProperty.Register("RatingButtons", typeof(IEnumerable), typeof(RatingBar));

        private ObservableCollection<RatingBarButton> RatingButtonsInternal = new ObservableCollection<RatingBarButton>();

        public static RoutedCommand ValueChangedCommand = new RoutedCommand();

        private double mOldValue;
        private bool mIsConfirm;

        static RatingBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RatingBar), new FrameworkPropertyMetadata(typeof(RatingBar)));
        }

        public RatingBar()
        {
            CommandBindings.Add(new CommandBinding(ValueChangedCommand, ValueChangedHanlder));
        }

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private static void MaximumChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RatingBar)d).CreateRatingButtons();
        }
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        private static object CoreMinimumCallback(DependencyObject d, object baseValue)
        {
            int value = (int)baseValue;
            if(value < 1)
            {
                return 1;
            }
            return value;
        }

        private static void MinimumChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((RatingBar)d).CreateRatingButtons();
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static object ValueCoerce(DependencyObject d, object baseValue)
        {
            RatingBar ratingBar = d as RatingBar;
            double value = 0.0;
            if(double.TryParse(Convert.ToString(baseValue), out value))
            {
                if(value < ratingBar.Minimum)
                {
                    value = 0;
                }
                else if(value > ratingBar.Maximum)
                {
                    value = ratingBar.Maximum;
                }
            }
            return value;
        }

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RatingBar ratingBar = d as RatingBar;
            double newValue = Convert.ToDouble(e.NewValue);
            var buttonList = ((RatingBar)d).RatingButtonsInternal;

            foreach (var ratingBarButton in buttonList)
            {
                ratingBarButton.IsHalf = false;
                ratingBarButton.IsSelected = ratingBarButton.Value <= newValue;
            }
            
            //for (int i = 0; i < buttonList.Count; i++)
            //{
            //    RatingBarButton ratingBarButton = buttonList[i];
            //    ratingBarButton.IsSelected = i <= Math.Ceiling(newValue);
            //    ratingBarButton.IsHalf = ratingBar.IsInt(newValue) ? false : Math.Ceiling(newValue) == i;
            //}
        }

        public DataTemplate ValueItemTemplate
        {
            get { return (DataTemplate)GetValue(ValueItemTemplateProperty); }
            set { SetValue(ValueItemTemplateProperty, value); }
        }
        
        public Style ValueItemStyle
        {
            get { return (Style)GetValue(ValueItemStyleProperty); }
            set { SetValue(ValueItemStyleProperty, value); }
        }
        
        public Brush SelectedColor
        {
            get { return (Brush)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public Brush UnSelectedColor
        {
            get { return (Brush)GetValue(UnSelectedColorProperty); }
            set { SetValue(UnSelectedColorProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }
        
        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }
        
        public string ContentStringFormat
        {
            get { return (string)GetValue(ContentStringFormatProperty); }
            set { SetValue(ContentStringFormatProperty, value); }
        }
        
        public bool IsShowContent
        {
            get { return (bool)GetValue(IsShowContentProperty); }
            set { SetValue(IsShowContentProperty, value); }
        }
        
        public IEnumerable RatingButtons
        {
            get { return (IEnumerable)GetValue(RatingButtonsProperty); }
            private set { SetValue(RatingButtonsProperty, value); }
        }

        private void ValueChangedHanlder(object sender, ExecutedRoutedEventArgs e)
        {
            if(!this.IsReadOnly && e.Parameter is int)
            {
                this.Value = Convert.ToDouble(e.Parameter);
                this.mIsConfirm = true;
            }
        }

        public override void OnApplyTemplate()
        {
            this.CreateRatingButtons();

            base.OnApplyTemplate();
        }

        private void CreateRatingButtons()
        {
            this.RatingButtonsInternal.Clear();
            for (var i = this.Minimum; i <= this.Maximum; i++)
            {
                RatingBarButton button = new RatingBarButton()
                {
                    Content = i,
                    Value = i,
                    IsSelected = i <= Math.Ceiling(this.Value),
                    IsHalf = this.IsInt(this.Value) ? false : Math.Ceiling(this.Value) == i,
                    ContentTemplate = this.ValueItemTemplate,
                    Style = this.ValueItemStyle
                };
                button.ItemMouseEnter += (o, n) =>
                {
                    this.mOldValue = this.Value;
                    this.Value = button.Value;
                    this.mIsConfirm = false;
                };
                button.ItemMouseLeave += (o, n) =>
                {
                    if(!this.mIsConfirm)
                    {
                        this.Value = this.mOldValue;
                        this.mIsConfirm = false;
                    }
                };

                this.RatingButtonsInternal.Add(button);
            }
            this.RatingButtons = this.RatingButtonsInternal;
        }

        private bool IsInt(object value)
        {
            if (Regex.IsMatch(value.ToString(), "^\\d+$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
