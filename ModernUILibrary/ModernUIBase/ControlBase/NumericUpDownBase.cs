namespace ModernIU.Base
{
    using System.Windows;

    public class NumericUpDownBase : TextBoxBase
    {
        public static readonly DependencyProperty UpDownOrientationProperty = DependencyProperty.Register("UpDownOrientation", typeof(UpDownOrientationEnum), typeof(TextBoxBase));

        static NumericUpDownBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDownBase), new FrameworkPropertyMetadata(typeof(NumericUpDownBase)));
        }

        public UpDownOrientationEnum UpDownOrientation
        {
            get { return (UpDownOrientationEnum)GetValue(UpDownOrientationProperty); }
            set { SetValue(UpDownOrientationProperty, value); }
        }

        public enum UpDownOrientationEnum
        {
            Vertical,
            Horizontal,
        }
    }
}
