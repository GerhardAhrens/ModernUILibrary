namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Badge Klasse
    /// </summary>
    public class Badge : Control
    {
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register("Number"
            , typeof(int), typeof(Badge), new FrameworkPropertyMetadata(0));

        public static readonly DependencyProperty IsDotProperty;

        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public bool IsDot
        {
            get { return (bool)GetValue(IsDotProperty); }
            set { SetValue(IsDotProperty, value); }
        }

        static Badge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Badge), new FrameworkPropertyMetadata(typeof(Badge)));

            IsDotProperty = DependencyProperty.Register("IsDot", typeof(bool), typeof(Badge), new FrameworkPropertyMetadata(false));
        }

        public Badge()
        {
        }
    }
}
