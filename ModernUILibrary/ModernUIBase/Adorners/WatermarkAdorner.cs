namespace ModernIU.Base
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    public enum EnumWatermarkShowMode
    {
        VisibleWhenIsEmpty,
        VisibleWhenLostFocusAndEmpty,
    }

    public class WatermarkAdorner : Adorner
    {
        private TextBox adornedTextBox;
        private VisualCollection _visuals;
        private TextBlock textBlock;
        private EnumWatermarkShowMode showModel;

        public static readonly DependencyProperty WatermarkProperty = 
            DependencyProperty.RegisterAttached("Watermark", typeof(string), typeof(WatermarkAdorner), new PropertyMetadata(string.Empty, WatermarkChangedCallBack));

        public static readonly DependencyProperty WatermarkShowModeProperty =
            DependencyProperty.RegisterAttached("WatermarkShowMode", typeof(EnumWatermarkShowMode), typeof(WatermarkAdorner), new PropertyMetadata(EnumWatermarkShowMode.VisibleWhenLostFocusAndEmpty));

        public static string GetWatermark(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }

        public static EnumWatermarkShowMode GetWatermarkShowMode(DependencyObject obj)
        {
            return (EnumWatermarkShowMode)obj.GetValue(WatermarkShowModeProperty);
        }

        public static void SetWatermarkShowMode(DependencyObject obj, EnumWatermarkShowMode value)
        {
            obj.SetValue(WatermarkShowModeProperty, value);
        }
        
        private static void WatermarkChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                var element = d as FrameworkElement;
                if (element != null)
                {
                    var adornerLayer = AdornerLayer.GetAdornerLayer(element);

                    if (adornerLayer != null)
                    {
                        adornerLayer.Add(new WatermarkAdorner(element as UIElement));
                    }
                    else
                    {
                        WatermarkAdorner adorner = null;

                        element.Initialized += (o1, e1) =>
                        {
                            adorner = new WatermarkAdorner(element);
                        };

                        element.Loaded += (s1, e1) => 
                        {
                            var v = AdornerLayer.GetAdornerLayer(element);
                            if(v != null && adorner != null)
                            {
                                v.Add(adorner);
                            }
                        };
                        element.Unloaded += (s1, e1) => 
                        {
                            var v = AdornerLayer.GetAdornerLayer(element);
                            if (v != null && adorner != null)
                            {
                                v.Remove(adorner);
                            }
                        };
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        public WatermarkAdorner(UIElement adornedElement) : base(adornedElement)
        {
            if (adornedElement is TextBox)
            {
                adornedTextBox = adornedElement as TextBox;
                adornedTextBox.TextChanged += (s1, e1) => 
                {
                    this.SetWatermarkVisible(true);
                };
                adornedTextBox.GotFocus += (s1, e1) => 
                {
                    this.SetWatermarkVisible(true);
                };
                adornedTextBox.LostFocus += (s1, e1) => 
                {
                    this.SetWatermarkVisible(false);
                };
                adornedTextBox.IsVisibleChanged += (o, e) =>
                {
                    if(string.IsNullOrEmpty(this.adornedTextBox.Text))
                    {
                        this.textBlock.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
                    }
                    else
                    {
                        this.textBlock.Visibility = Visibility.Collapsed;
                    }
                };

                _visuals = new VisualCollection(this);
                
                textBlock = new TextBlock()
                {
                    HorizontalAlignment = adornedTextBox.HorizontalContentAlignment,
                    VerticalAlignment = adornedTextBox.VerticalContentAlignment,
                    Text = WatermarkAdorner.GetWatermark(adornedElement),
                    Foreground = new SolidColorBrush(Color.FromRgb(153, 153, 153)),
                    Margin = new Thickness(5,0,2,0),
                };

                _visuals.Add(textBlock);

                this.showModel = WatermarkAdorner.GetWatermarkShowMode(adornedElement);
            }
            this.IsHitTestVisible = false;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return _visuals.Count;
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
            textBlock.Arrange(new Rect(finalSize));

            return base.ArrangeOverride(finalSize);
        }

        private void SetWatermarkVisible(bool isFocus)
        {
            switch (this.showModel)
            {
                case EnumWatermarkShowMode.VisibleWhenIsEmpty:
                    if (string.IsNullOrEmpty(this.adornedTextBox.Text))
                    {
                        this.textBlock.Visibility = Visibility.Visible;
                        if(!this.adornedTextBox.IsVisible)
                        {
                            this.textBlock.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        this.textBlock.Visibility = Visibility.Collapsed;
                    }
                    break;
                case EnumWatermarkShowMode.VisibleWhenLostFocusAndEmpty:
                    if(!isFocus && string.IsNullOrEmpty(this.adornedTextBox.Text))
                    {
                        this.textBlock.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.textBlock.Visibility = Visibility.Collapsed;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
