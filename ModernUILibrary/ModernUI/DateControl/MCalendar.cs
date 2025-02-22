﻿namespace ModernIU.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public enum EnumCalendarType
    {
        One,
        Second,
    }

    public class MCalendar : Control
    {
        private MCalendarItem PART_CalendarItem;

        public MDatePicker Owner { get; set; }


        #region SelectedDateChanged
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(MCalendar));

        public event RoutedPropertyChangedEventHandler<DateTime?> SelectedDateChanged
        {
            add
            {
                this.AddHandler(SelectedDateChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(SelectedDateChangedEvent, value);
            }
        }

        public virtual void OnSelectedDateChanged(DateTime? oldValue, DateTime? newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime?> arg = new RoutedPropertyChangedEventArgs<DateTime?>(oldValue, newValue, SelectedDateChangedEvent);
            this.RaiseEvent(arg);
        }
        #endregion

        #region DateClick

        public static readonly RoutedEvent DateClickEvent = EventManager.RegisterRoutedEvent("DateClick",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime?>), typeof(MCalendar));

        public event RoutedPropertyChangedEventHandler<DateTime?> DateClick
        {
            add
            {
                this.AddHandler(DateClickEvent, value);
            }
            remove
            {
                this.RemoveHandler(DateClickEvent, value);
            }
        }

        public virtual void OnDateClick(DateTime? oldValue, DateTime? newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime?> arg = new RoutedPropertyChangedEventArgs<DateTime?>(oldValue, newValue, DateClickEvent);
            this.RaiseEvent(arg);
        }

        #endregion

        #region DisplayDateChanged

        public static readonly RoutedEvent DisplayDateChangedEvent = EventManager.RegisterRoutedEvent("DisplayDateChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<DateTime>), typeof(MCalendar));

        public event RoutedPropertyChangedEventHandler<DateTime> DisplayDateChanged
        {
            add
            {
                this.AddHandler(DisplayDateChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(DisplayDateChangedEvent, value);
            }
        }

        public virtual void OnDisplayDateChanged(DateTime oldValue, DateTime newValue)
        {
            RoutedPropertyChangedEventArgs<DateTime> arg = new RoutedPropertyChangedEventArgs<DateTime>(oldValue, newValue, DisplayDateChangedEvent);
            this.RaiseEvent(arg);
        }

        #endregion

        #region DisplayModeChanged

        public static readonly RoutedEvent DisplayModeChangedEvent = EventManager.RegisterRoutedEvent("DisplayModeChanged",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<CalendarMode>), typeof(MCalendar));

        public event RoutedPropertyChangedEventHandler<CalendarMode> DisplayModeChanged
        {
            add
            {
                this.AddHandler(DisplayModeChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(DisplayModeChangedEvent, value);
            }
        }

        public virtual void OnDisplayModeChanged(CalendarMode oldValue, CalendarMode newValue)
        {
            RoutedPropertyChangedEventArgs<CalendarMode> arg = new RoutedPropertyChangedEventArgs<CalendarMode>(oldValue, newValue, DisplayModeChangedEvent);
            this.RaiseEvent(arg);
        }

        public Style CalendarItemStyle
        {
            get { return (Style)GetValue(CalendarItemStyleProperty); }
            set { SetValue(CalendarItemStyleProperty, value); }
        }

        public static readonly DependencyProperty CalendarItemStyleProperty =
            DependencyProperty.Register("CalendarItemStyle", typeof(Style), typeof(MCalendar));
        #endregion

        #region CalendarDayButtonStyle
        public Style CalendarDayButtonStyle
        {
            get { return (Style)GetValue(CalendarDayButtonStyleProperty); }
            set { SetValue(CalendarDayButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty CalendarDayButtonStyleProperty =
            DependencyProperty.Register("CalendarDayButtonStyle", typeof(Style), typeof(MCalendar));
        #endregion

        #region DayTitleTemplate
        public DataTemplate DayTitleTemplate
        {
            get { return (DataTemplate)GetValue(DayTitleTemplateProperty); }
            set { SetValue(DayTitleTemplateProperty, value); }
        }
        
        public static readonly DependencyProperty DayTitleTemplateProperty =
            DependencyProperty.Register("DayTitleTemplate", typeof(DataTemplate), typeof(MCalendar));
        #endregion

        #region DisplayDate
        public DateTime DisplayDate
        {
            get { return (DateTime)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register("DisplayDate", typeof(DateTime), typeof(MCalendar), new PropertyMetadata(DateTime.Today, DisplayDateChangedCalllback));

        private static void DisplayDateChangedCalllback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MCalendar calendar = d as MCalendar;
            DateTime oldTime = Convert.ToDateTime(e.OldValue);
            DateTime newTime = Convert.ToDateTime(e.NewValue);
            if (calendar != null)
            {
                calendar.UpdateCellItems();
                calendar.OnDisplayDateChanged(oldTime, newTime);
            }
        }
        #endregion

        #region DisplayDateStart
        public DateTime DisplayDateStart
        {
            get { return (DateTime)GetValue(DisplayDateStartProperty); }
            set { SetValue(DisplayDateStartProperty, value); }
        }
        
        public static readonly DependencyProperty DisplayDateStartProperty =
            DependencyProperty.Register("DisplayDateStart", typeof(DateTime), typeof(MCalendar), new PropertyMetadata(DateTime.MinValue));
        #endregion

        #region DisplayDateEnd
        public DateTime DisplayDateEnd
        {
            get { return (DateTime)GetValue(DisplayDateEndProperty); }
            set { SetValue(DisplayDateEndProperty, value); }
        }
        
        public static readonly DependencyProperty DisplayDateEndProperty =
            DependencyProperty.Register("DisplayDateEnd", typeof(DateTime), typeof(MCalendar), new PropertyMetadata(DateTime.MaxValue));
        #endregion

        #region DisplayMode
        public CalendarMode DisplayMode
        {
            get { return (CalendarMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }
        
        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(CalendarMode), typeof(MCalendar), new PropertyMetadata(CalendarMode.Month, DisplayModeChangedCallback));

        private static void DisplayModeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MCalendar calendar = d as MCalendar;
            if (calendar != null)
            {
                calendar.UpdateCellItems();
                calendar.OnDisplayModeChanged((CalendarMode)e.OldValue, (CalendarMode)e.NewValue);
            }
        }
        #endregion

        #region SelectedDate
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(MCalendar), new PropertyMetadata(null, SelectedDateChangedCallback));

        private static void SelectedDateChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MCalendar calendar = d as MCalendar;
            if(calendar.SelectionMode == CalendarSelectionMode.SingleDate)
            {
                calendar.OnSelectedDateChanged(new DateTime?(Convert.ToDateTime(e.OldValue)), new DateTime?(Convert.ToDateTime(e.NewValue)));
            }
            if(calendar.PART_CalendarItem != null)
            {
                calendar.PART_CalendarItem.UpdateMonthMode();
            }
        }
        #endregion

        #region SelectedDates
        public ObservableCollection<DateTime> SelectedDates
        {
            get { return (ObservableCollection<DateTime>)GetValue(SelectedDatesProperty); }
            set { SetValue(SelectedDatesProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedDatesProperty =
            DependencyProperty.Register("SelectedDates", typeof(ObservableCollection<DateTime>), typeof(MCalendar), new PropertyMetadata(null, SelectedDatesChangedCallback));

        private static void SelectedDatesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MCalendar calendar = d as MCalendar;
            if(calendar.PART_CalendarItem == null)
            {
                return;
            }
            calendar.PART_CalendarItem.SetSelectedDatesHighlight(calendar.SelectedDates);
        }
        #endregion

        #region FirstDayOfWeek
        public DayOfWeek FirstDayOfWeek
        {
            get { return (DayOfWeek)GetValue(FirstDayOfWeekProperty); }
            set { SetValue(FirstDayOfWeekProperty, value); }
        }
        
        public static readonly DependencyProperty FirstDayOfWeekProperty =
            DependencyProperty.Register("FirstDayOfWeek", typeof(DayOfWeek), typeof(MCalendar), new PropertyMetadata(DayOfWeek.Monday));
        #endregion

        #region SelectionMode
        public CalendarSelectionMode SelectionMode
        {
            get { return (CalendarSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }
        
        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register("SelectionMode", typeof(CalendarSelectionMode), typeof(MCalendar), new PropertyMetadata(CalendarSelectionMode.SingleDate));
        #endregion

        #region IsShowExtraDays 
        public bool IsShowExtraDays
        {
            get { return (bool)GetValue(IsShowExtraDaysProperty); }
            set { SetValue(IsShowExtraDaysProperty, value); }
        }
        
        public static readonly DependencyProperty IsShowExtraDaysProperty =
            DependencyProperty.Register("IsShowExtraDays", typeof(bool), typeof(MCalendar), new PropertyMetadata(true));
        #endregion

        #region Type
        public EnumCalendarType Type
        {
            get { return (EnumCalendarType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(EnumCalendarType), typeof(MCalendar), new PropertyMetadata(EnumCalendarType.One));
        #endregion

        #region Constructors
        static MCalendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MCalendar), new FrameworkPropertyMetadata(typeof(MCalendar)));
        }

        public MCalendar()
        {
            this.SelectedDates = new ObservableCollection<DateTime>();
            this.SelectedDates.CollectionChanged += SelectedDates_CollectionChanged;
        }
        #endregion

        #region Override
        public override void OnApplyTemplate()
        {
            if (this.PART_CalendarItem != null)
            {
                this.PART_CalendarItem.Owner = null;
            }

            base.OnApplyTemplate();
            
            this.DisplayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            this.PART_CalendarItem = this.GetTemplateChild("PART_CalendarItem") as MCalendarItem;
            if (this.PART_CalendarItem != null)
            {
                this.PART_CalendarItem.Owner = this;
            }

            /*
             * Es gibt ein Problem: Nach dem Auswählen eines Datums scheint der Fokus nicht freigegeben zu werden, und wenn die Maus an eine andere Position bewegt wird, muss man erst mit der Maus klicken!
             * Dann kann der entsprechende Teil der Maus den Fokus bekommen. Um dieses Problem zu lösen, machen Sie folgende Verarbeitung
             */
            this.PreviewMouseUp += ZCalendar_PreviewMouseUp;
        }

        private void SelectedDates_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (this.PART_CalendarItem == null)
            {
                return;
            }
            this.PART_CalendarItem.SetSelectedDatesHighlight(this.SelectedDates);
        }

        private void ZCalendar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem)
            {
                Mouse.Capture(null);
            }
        }
        
        #endregion

        #region Private
        private void UpdateCellItems()
        {
            if(this.PART_CalendarItem != null)
            {
                switch (this.DisplayMode)
                {
                    case CalendarMode.Month:
                        this.PART_CalendarItem.UpdateMonthMode();
                        break;
                    case CalendarMode.Year:
                        this.PART_CalendarItem.UpdateYearMode();
                        break;
                    case CalendarMode.Decade:
                        this.PART_CalendarItem.UpdateDecadeMode();
                        break;
                    default:
                        break;
                }
            }
        }

        private DateTime TryParseToDateTime(string str)
        {
            DateTime dt = DateTime.MinValue;
            if(DateTime.TryParse(str, out dt))
            {
                
            }
            return dt;
        }
        #endregion
    }
}
