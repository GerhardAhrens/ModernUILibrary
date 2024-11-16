namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class MCalendarDayButton : Button
    {
        public MCalendar Owner { get; set; }

        #region IsSelected
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(MCalendarDayButton), new PropertyMetadata(false));
        #endregion

        #region IsToday
        public bool IsToday
        {
            get { return (bool)GetValue(IsTodayProperty); }
            set { SetValue(IsTodayProperty, value); }
        }
        
        public static readonly DependencyProperty IsTodayProperty =
            DependencyProperty.Register("IsToday", typeof(bool), typeof(MCalendarDayButton), new PropertyMetadata(false));
        #endregion

        #region IsBlackedOut
        public bool IsBlackedOut
        {
            get { return (bool)GetValue(IsBlackedOutProperty); }
            set { SetValue(IsBlackedOutProperty, value); }
        }
        
        public static readonly DependencyProperty IsBlackedOutProperty =
            DependencyProperty.Register("IsBlackedOut", typeof(bool), typeof(MCalendarDayButton), new PropertyMetadata(false));
        #endregion

        #region IsBelongCurrentMonth
        public bool IsBelongCurrentMonth
        {
            get { return (bool)GetValue(IsBelongCurrentMonthProperty); }
            set { SetValue(IsBelongCurrentMonthProperty, value); }
        }
        
        public static readonly DependencyProperty IsBelongCurrentMonthProperty =
            DependencyProperty.Register("IsBelongCurrentMonth", typeof(bool), typeof(MCalendarDayButton), new PropertyMetadata(true));
        #endregion

        #region IsHighlight
        public bool IsHighlight
        {
            get { return (bool)GetValue(IsHighlightProperty); }
            set { SetValue(IsHighlightProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightProperty =
            DependencyProperty.Register("IsHighlight", typeof(bool), typeof(MCalendarDayButton), new PropertyMetadata(true));
        #endregion

        #region Constructors
        static MCalendarDayButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MCalendarDayButton), new FrameworkPropertyMetadata(typeof(MCalendarDayButton)));
        }
        #endregion
    }
}
