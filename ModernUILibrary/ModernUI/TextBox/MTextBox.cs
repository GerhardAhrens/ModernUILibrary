namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using ModernIU.Base;

    public class MTextBox : TextBox
    {
        public static readonly DependencyProperty CornerRadiusProperty;
        public static readonly DependencyProperty WatermarkProperty;
        public static readonly DependencyProperty MultiRowProperty;
        public static readonly DependencyProperty SetBorderProperty;

        #region Constructors
        public MTextBox()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.Margin = ControlBase.DefaultMargin;
            this.Height = ControlBase.DefaultHeight;
            this.BorderBrush = ControlBase.BorderBrush;
            this.BorderThickness = ControlBase.BorderThickness;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.IsReadOnly = false;
            this.Focusable = true;
        }

        static MTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTextBox), new FrameworkPropertyMetadata(typeof(MTextBox)));

            CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(System.Windows.CornerRadius), typeof(MTextBox));
            WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(MTextBox));
            MultiRowProperty = DependencyProperty.Register("MultiRow", typeof(bool), typeof(MTextBox));
            SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(MTextBox), new PropertyMetadata(true, OnSetBorderChanged));
        }
        #endregion

        public System.Windows.CornerRadius CornerRadius
        {
            get { return (System.Windows.CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public bool MultiRow
        {
            get { return (bool)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public bool SetBorder
        {
            get { return (bool)GetValue(SetBorderProperty); }
            set { SetValue(SetBorderProperty, value); }
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (MTextBox)d;

                if (e.NewValue.GetType() == typeof(bool))
                {
                    if ((bool)e.NewValue == true)
                    {
                        control.BorderBrush = ControlBase.BorderBrush;
                        control.BorderThickness = ControlBase.BorderThickness;
                    }
                    else
                    {
                        control.BorderBrush = Brushes.Transparent;
                        control.BorderThickness = new Thickness(0);
                    }
                }
            }
        }
    }
}
