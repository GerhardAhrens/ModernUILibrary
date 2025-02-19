namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_UP", Type = typeof(Button))]
    [TemplatePart(Name = "PART_DOWN", Type = typeof(Button))]
    public class DoubleUpDown : NumericUpDown<double>
    {
        public DoubleUpDown() : base()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 23;
            this.IsReadOnly = false;
            this.Focusable = true;
            this.Minimum = 0d;
            this.Maximum = 100d;
            this.Value = this.Minimum;
            this.Increment = 1d;
        }

        protected override double IncrementValue(double value, double increment)
        {
            return value + increment;
        }

        protected override double DecrementValue(double value, double increment)
        {
            return value - increment;
        }

        protected override double ParseValue(string value)
        {
            double temp = 0;
            if (double.TryParse(value, out temp))
            {
                return temp;
            }
            else
            {
                return double.MinValue;
            }
        }
    }
}
