namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class StepBarItem : ContentControl
    {
        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register(nameof(Number), typeof(string), typeof(StepBarItem));

        static StepBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StepBarItem), new FrameworkPropertyMetadata(typeof(StepBarItem)));
        }

        public string Number
        {
            get { return (string)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }
    }
}
