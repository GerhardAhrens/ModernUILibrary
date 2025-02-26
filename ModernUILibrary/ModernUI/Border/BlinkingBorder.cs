namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Controls;
    using System.Runtime.CompilerServices;

    public class BlinkingBorder : Border
    {
        static BlinkingBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BlinkingBorder), new FrameworkPropertyMetadata(typeof(BlinkingBorder)));
        }

        public static readonly DependencyProperty IsBlinkingProperty =
            DependencyProperty.Register(nameof(IsBlinking), typeof(bool), typeof(BlinkingBorder), new PropertyMetadata(false, UpdateBorderBrush));

        public bool IsBlinking
        {
            get => (bool)GetValue(IsBlinkingProperty);
            set => SetValue(IsBlinkingProperty, value);
        }

        public static readonly DependencyProperty DefaultBorderBrushProperty =
            DependencyProperty.Register(nameof(DefaultBorderBrush), typeof(Brush), typeof(BlinkingBorder), new PropertyMetadata(Brushes.Chartreuse, UpdateBorderBrush));

        public Brush DefaultBorderBrush
        {
            get => (Brush)GetValue(DefaultBorderBrushProperty);
            set => SetValue(DefaultBorderBrushProperty, value);
        }

        public static readonly DependencyProperty BlinkingBorderBrushProperty =
            DependencyProperty.Register(nameof(BlinkingBorderBrush), typeof(Brush), typeof(BlinkingBorder), new PropertyMetadata(Brushes.Fuchsia, UpdateBorderBrush));

        public Brush BlinkingBorderBrush
        {
            get => (Brush)GetValue(BlinkingBorderBrushProperty);
            set => SetValue(BlinkingBorderBrushProperty, value);
        }

        // frequency
        public static readonly DependencyProperty SetFrequencyProperty =
            DependencyProperty.Register(nameof(SetFrequency), typeof(string), typeof(BlinkingBorder), new PropertyMetadata("0:0:0.5", UpdateBorderBrush));

        public Brush SetFrequency
        {
            get => (Brush)GetValue(SetFrequencyProperty);
            set => SetValue(SetFrequencyProperty, value);
        }

        private static void UpdateBorderBrush(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is BlinkingBorder blinkingBorder))
            {
                return;
            }

            blinkingBorder.BorderBrush = blinkingBorder.IsBlinking ? blinkingBorder.BlinkingBorderBrush : blinkingBorder.DefaultBorderBrush;
        }
    }
}