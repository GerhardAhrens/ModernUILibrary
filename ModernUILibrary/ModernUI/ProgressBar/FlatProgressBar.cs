namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media.Animation;

    using ModernIU.Base;

    public class FlatProgressBar : RangeBase
    {
        private FrameworkElement Indicator;

        public static readonly DependencyProperty CornerRadiusProperty;

        public static readonly DependencyProperty InnerCornerRadiusProperty;

        public static readonly DependencyProperty SkinProperty;

        #region Constructors
        static FlatProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatProgressBar), new FrameworkPropertyMetadata(typeof(FlatProgressBar)));

            CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(FlatProgressBar));
            InnerCornerRadiusProperty = DependencyProperty.Register("InnerCornerRadius", typeof(CornerRadius), typeof(FlatProgressBar));
            SkinProperty = DependencyProperty.Register("Skin", typeof(ProgressBarSkinEnum), typeof(FlatProgressBar));
        }

        #endregion

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public CornerRadius InnerCornerRadius
        {
            get { return (CornerRadius)GetValue(InnerCornerRadiusProperty); }
            set { SetValue(InnerCornerRadiusProperty, value); }
        }

        public ProgressBarSkinEnum Skin
        {
            get { return (ProgressBarSkinEnum)GetValue(SkinProperty); }
            set { SetValue(SkinProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Indicator = GetTemplateChild("Indicator") as FrameworkElement;
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            var perWidth = this.Width / (this.Maximum - this.Minimum);
            var oldWidth = oldValue * perWidth;
            var newWidth = newValue * perWidth;

            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = oldWidth;
            doubleAnimation.To = newWidth - this.BorderThickness.Right * 2;
            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(400));
            if (this.Indicator != null)
            {
                this.Indicator.BeginAnimation(FrameworkElement.WidthProperty, doubleAnimation);
            }
        }
    }
}
