using System.Windows;
using System.Windows.Controls.Primitives;

namespace ModernIU.Base
{
    public class CircleBase : RangeBase
    {
        public static readonly DependencyProperty StartAngleProperty;

        public static readonly DependencyProperty EndAngleProperty;

        static CircleBase()
        {
            CircleBase.StartAngleProperty = DependencyProperty.Register("StartAngle",
                typeof(double),
                typeof(CircleBase),
                new PropertyMetadata(0d));
            CircleBase.EndAngleProperty = DependencyProperty.Register("EndAngle",
                typeof(double),
                typeof(CircleBase),
                new PropertyMetadata(360d));
        }

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }
    }
}
