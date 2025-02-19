namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_UP", Type = typeof(Button))]
    [TemplatePart(Name = "PART_DOWN", Type = typeof(Button))]
    public class IntegerUpDown : NumericUpDown<int>
    {
        public IntegerUpDown() : base()
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
            this.Minimum = 0;
            this.Maximum = 100;
            this.Value = this.Minimum;
            this.Increment = 1;
        } 

        protected override int IncrementValue(int value, int increment)
        {
            return value + increment;
        }

        protected override int DecrementValue(int value, int increment)
        {
            return value - increment;
        }

        protected override int ParseValue(string value)
        {
            int temp = 0;
            if(int.TryParse(value, out temp))
            {
                return temp;
            }
            else
            {
                return int.MinValue;
            }
        }
    }
}
