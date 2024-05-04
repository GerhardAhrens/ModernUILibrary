namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Media.Animation;

    using Microsoft.Expression.Shapes;

    using ModernIU.Base;

    public class CircleProgressBar : CircleBase
    {
        private Arc Indicator;
        private double oldAngle;

        public static readonly DependencyProperty AngleProperty;

        public static readonly DependencyProperty DurtionProperty;

        static CircleProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CircleProgressBar), new FrameworkPropertyMetadata(typeof(CircleProgressBar)));

            CircleProgressBar.AngleProperty = DependencyProperty.Register("Angle",
                typeof(double),
                typeof(CircleProgressBar),
                new PropertyMetadata(0d));

            CircleProgressBar.DurtionProperty = DependencyProperty.Register("Durtion",
                typeof(double),
                typeof(CircleProgressBar),
                new PropertyMetadata(500d));
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            private set { SetValue(AngleProperty, value); }
        }

        public double Durtion
        {
            get { return (double)GetValue(DurtionProperty); }
            set { SetValue(DurtionProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.Indicator = GetTemplateChild("Indicator") as Arc;
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            this.oldAngle = this.Angle;

            var valueDiff = this.Value - this.Minimum;
            this.Angle = this.StartAngle + (Math.Abs(this.EndAngle - this.StartAngle)) / (this.Maximum - this.Minimum) * valueDiff;
            this.TransformAngle(this.oldAngle, this.Angle, this.Durtion);
        }

        private void SetAngle()
        {
            if(this.Value < this.Minimum)
            {
                this.Angle = this.StartAngle;
                return;
            }
            if(this.Value > this.Maximum)
            {
                this.Angle = this.EndAngle;
                return;
            }
        }

        private void TransformAngle(double From, double To, double durtion)
        {
            if (this.Indicator != null)
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation(From, this.Angle, new Duration(TimeSpan.FromMilliseconds(durtion)));
                this.Indicator.BeginAnimation(Arc.EndAngleProperty, doubleAnimation);
            }
        }
    }
}
