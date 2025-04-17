namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ColorRadioButton : RadioButton
    {
        #region Constructors

        static ColorRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorRadioButton), new FrameworkPropertyMetadata(typeof(ColorRadioButton)));
        }

        #endregion

        #region DependencyProperty : CheckedBrush

        public static readonly DependencyProperty CheckedBrushProperty =
            DependencyProperty.Register(nameof(CheckedBrush), typeof(SolidColorBrush), typeof(ColorRadioButton), new PropertyMetadata(default));

        public SolidColorBrush CheckedBrush
        {
            get => (SolidColorBrush)GetValue(CheckedBrushProperty);
            set => SetValue(CheckedBrushProperty, value);
        }

        #endregion

        #region DependencyProperty : BulletCircleBrush

        public static readonly DependencyProperty BulletCircleBrushProperty =
            DependencyProperty.Register(nameof(BulletCircleBrush), typeof(SolidColorBrush), typeof(ColorRadioButton), new PropertyMetadata(default));

        public SolidColorBrush BulletCircleBrush
        {
            get => (SolidColorBrush)GetValue(BulletCircleBrushProperty);
            set => SetValue(BulletCircleBrushProperty, value);
        }

        #endregion

        #region DependencyProperty : BulletSize

        public static readonly DependencyProperty BulletSizeProperty =
            DependencyProperty.Register(nameof(BulletSize), typeof(double), typeof(ColorRadioButton), new PropertyMetadata(default));

        public double BulletSize
        {
            get => (double)GetValue(BulletSizeProperty);
            set => SetValue(BulletSizeProperty, value);
        }

        #endregion

        #region DependencyProperty : UncheckedBulletBorderSize

        public static readonly DependencyProperty UncheckedBulletBorderSizeProperty =
            DependencyProperty.Register(nameof(UncheckedBulletBorderSize), typeof(double),  typeof(ColorRadioButton), new PropertyMetadata(default));

        public double UncheckedBulletBorderSize
        {
            get => (double)GetValue(UncheckedBulletBorderSizeProperty);
            set => SetValue(UncheckedBulletBorderSizeProperty, value);
        }

        #endregion

        #region DependencyProperty : CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ColorRadioButton), new PropertyMetadata(default));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion
    }
}
