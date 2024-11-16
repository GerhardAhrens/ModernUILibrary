namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class MCalendarButton : Button
    {
        static MCalendarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MCalendarButton), new FrameworkPropertyMetadata(typeof(MCalendarButton)));
        }

        public MCalendar Owner { get; set; }

        public bool HasSelectedDates
        {
            get { return (bool)GetValue(HasSelectedDatesProperty); }
            set { SetValue(HasSelectedDatesProperty, value); }
        }
        
        public static readonly DependencyProperty HasSelectedDatesProperty =
            DependencyProperty.Register("HasSelectedDates", typeof(bool), typeof(MCalendarButton), new PropertyMetadata(false));
    }
}
