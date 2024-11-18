namespace ModernIU.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    using ModernIU.Base;

    using ModernUILibrary.ModernUIBase;

    public class MDatePicker : Control
    {
        #region Popup
        private Popup PART_Popup_New;
        private Popup PART_Popup_TimeSelector;
        private MCalendar PART_Calendar;
        private MCalendar PART_Calendar_Second;
        private TimeSelector PART_TimeSelector;
        private TextBox PART_TextBox_New;
        private Button PART_Btn_Today;
        private Button PART_Btn_Yestday;
        private Button PART_Btn_AWeekAgo;
        private Button PART_Btn_RecentlyAWeek;
        private Button PART_Btn_RecentlyAMonth;
        private Button PART_Btn_RecentlyThreeMonth;
        private Button PART_ClearDate;
        private Button PART_ConfirmSelected;

        #endregion

        public string TextInternal
        {
            get { return (string)GetValue(TextInternalProperty); }
            private set { SetValue(TextInternalProperty, value); }
        }
        
        public static readonly DependencyProperty TextInternalProperty =
            DependencyProperty.Register("TextInternal", typeof(string), typeof(MDatePicker), new PropertyMetadata(string.Empty));

        public EnumDatePickerType Type
        {
            get { return (EnumDatePickerType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(EnumDatePickerType), typeof(MDatePicker), new PropertyMetadata(EnumDatePickerType.SingleDate, TypeChangedCallback));

        private static void TypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MDatePicker datePicker = d as MDatePicker;
            datePicker.SetSelectionMode(datePicker, (EnumDatePickerType)e.NewValue);
        }
        #region IsShowShortCuts 
        public bool IsShowShortCuts
        {
            get { return (bool)GetValue(IsShowShortCutsProperty); }
            set { SetValue(IsShowShortCutsProperty, value); }
        }
        
        public static readonly DependencyProperty IsShowShortCutsProperty =
            DependencyProperty.Register("IsShowShortCuts", typeof(bool), typeof(MDatePicker), new PropertyMetadata(false));
        #endregion

        #region SelectedDate

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(MDatePicker), new PropertyMetadata(null, SelectedDateCallback, SelectedDateCoerceValueCallback));

        private static void SelectedDateCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MDatePicker datePicker = d as MDatePicker;
            DateTime? dateTime = (DateTime?)e.NewValue;
            if (dateTime.HasValue)
            {
                DateTime dt = dateTime.Value;
                if(datePicker.SelectedTime == null)
                {
                    datePicker.SelectedTime = dt;
                }
                
                datePicker.SetSingleDateToTextBox(dt);
            }
            else
            {
                //TODO
                //显示水印
            }
        }

        private static object SelectedDateCoerceValueCallback(DependencyObject d, object value)
        {
            MDatePicker datePicker = d as MDatePicker;

            DateTime? dateTime = (DateTime?)value;
            if(datePicker.PART_Calendar != null)
            {
                datePicker.PART_Calendar.SelectedDate = dateTime;
            }
            return dateTime;
        }

        #endregion

        #region SelectedDates
        public ObservableCollection<DateTime> SelectedDates
        {
            get { return (ObservableCollection<DateTime>)GetValue(SelectedDatesProperty); }
            set { SetValue(SelectedDatesProperty, value); }
        }

        public static readonly DependencyProperty SelectedDatesProperty =
            DependencyProperty.Register("SelectedDates", typeof(ObservableCollection<DateTime>), typeof(MDatePicker), new PropertyMetadata(null, SelectedDateChangedCallback));

        private static void SelectedDateChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MDatePicker datePicker = d as MDatePicker;

            foreach (DateTime date in datePicker.SelectedDates)
            {
                if (date.Year == datePicker.PART_Calendar.DisplayDate.Year
                    && date.Month == datePicker.PART_Calendar.DisplayDate.Month)
                {
                    datePicker.PART_Calendar.SelectedDates.Add(date);
                }
                else
                {
                    datePicker.PART_Calendar_Second.SelectedDates.Add(date);
                }
            }
        }
        #endregion

        #region SelectedDateStart

        public DateTime? SelectedDateStart
        {
            get { return (DateTime?)GetValue(SelectedDateStartProperty); }
            set { SetValue(SelectedDateStartProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedDateStartProperty =
            DependencyProperty.Register("SelectedDateStart", typeof(DateTime?), typeof(MDatePicker), new PropertyMetadata(null));

        #endregion

        #region SelectedDateEnd

        public DateTime? SelectedDateEnd
        {
            get { return (DateTime?)GetValue(SelectedDateEndProperty); }
            set { SetValue(SelectedDateEndProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedDateEndProperty =
            DependencyProperty.Register("SelectedDateEnd", typeof(DateTime?), typeof(MDatePicker), new PropertyMetadata(null, SelectedDateEndCallback, CoerceSelectedDateEnd));

        private static object CoerceSelectedDateEnd(DependencyObject d, object value)
        {
            MDatePicker datePicker = d as MDatePicker;
            DateTime? dateTime = (DateTime?)value;
            if (datePicker.PART_Calendar != null)
            {
                datePicker.PART_Calendar.SelectedDate = dateTime;
            }
            datePicker.SetSelectedDates(datePicker.SelectedDateStart, datePicker.SelectedDateEnd);
            return dateTime;
        }

        private static void SelectedDateEndCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        #endregion

        #region SelectedTime
        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(MDatePicker), new PropertyMetadata(null, SelectedTimeChangedCallback));

        private static void SelectedTimeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MDatePicker datePicker = d as MDatePicker;
            datePicker.SetSingleDateToTextBox(datePicker.SelectedDate);
        }

        #endregion

        #region DisplayDate
        public DateTime DisplayDate
        {
            get { return (DateTime)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }
        
        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register("DisplayDate", typeof(DateTime), typeof(MDatePicker));

        #endregion

        #region DateStringFormat

        public string DateStringFormat
        {
            get { return (string)GetValue(DateStringFormatProperty); }
            set { SetValue(DateStringFormatProperty, value); }
        }
        
        public static readonly DependencyProperty DateStringFormatProperty =
            DependencyProperty.Register("DateStringFormat", typeof(string), typeof(MDatePicker), new PropertyMetadata("dd.MM.yyyy"));

        #endregion

        #region TimeStringFormat

        public string TimeStringFormat
        {
            get { return (string)GetValue(TimeStringFormatProperty); }
            set { SetValue(TimeStringFormatProperty, value); }
        }
        
        public static readonly DependencyProperty TimeStringFormatProperty =
            DependencyProperty.Register("TimeStringFormat", typeof(string), typeof(MDatePicker), new PropertyMetadata("HH:mm:ss"));

        #endregion

        #region CornerRadius

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(MDatePicker));

        #endregion

        #region IsShowConfirm
        public bool IsShowConfirm
        {
            get { return (bool)GetValue(IsShowConfirmProperty); }
            set { SetValue(IsShowConfirmProperty, value); }
        }
        
        public static readonly DependencyProperty IsShowConfirmProperty =
            DependencyProperty.Register("IsShowConfirm", typeof(bool), typeof(MDatePicker), new PropertyMetadata(false));

        #endregion

        #region IsDropDownOpen

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }
        
        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(MDatePicker), new PropertyMetadata(false));

        #endregion

        #region Interne Abhängigkeitsattribute
        public DateTime? SelectedDateInternal
        {
            get { return (DateTime?)GetValue(SelectedDateInternalProperty); }
            set { SetValue(SelectedDateInternalProperty, value); }
        }
        
        public static readonly DependencyProperty SelectedDateInternalProperty =
            DependencyProperty.Register("SelectedDateInternal", typeof(DateTime?), typeof(MDatePicker));


        public DateTime DisplayDateInternal
        {
            get { return (DateTime)GetValue(DisplayDateInternalProperty); }
            set { SetValue(DisplayDateInternalProperty, value); }
        }
        
        public static readonly DependencyProperty DisplayDateInternalProperty =
            DependencyProperty.Register("DisplayDateInternal", typeof(DateTime), typeof(MDatePicker));


        #endregion

        #region Constructors
        static MDatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MDatePicker), new FrameworkPropertyMetadata(typeof(MDatePicker)));
        }

        public MDatePicker()
        {
            this.SelectedDates = new ObservableCollection<DateTime>();
        }
        #endregion

        #region Override
        public override void OnApplyTemplate()
        {
            if(this.PART_Calendar != null)
            {
                this.PART_Calendar.Owner = null;
            }
            if(this.PART_Calendar_Second != null)
            {
                this.PART_Calendar_Second.Owner = null;
            }

            base.OnApplyTemplate();

            this.PART_Popup_New = GetTemplateChild("PART_Popup_New") as Popup;
            this.PART_Popup_TimeSelector = GetTemplateChild("PART_Popup_TimeSelector") as Popup;
            this.PART_Calendar = GetTemplateChild("PART_Calendar") as MCalendar;
            this.PART_Calendar_Second = GetTemplateChild("PART_Calendar_Second") as MCalendar;
            this.PART_TimeSelector = GetTemplateChild("PART_TimeSelector") as TimeSelector;
            this.PART_TextBox_New = GetTemplateChild("PART_TextBox_New") as TextBox;
            this.PART_Btn_Today = GetTemplateChild("PART_Btn_Today") as Button;
            this.PART_Btn_Yestday = GetTemplateChild("PART_Btn_Yestday") as Button;
            this.PART_Btn_AWeekAgo = GetTemplateChild("PART_Btn_AWeekAgo") as Button;
            this.PART_Btn_RecentlyAWeek = GetTemplateChild("PART_Btn_RecentlyAWeek") as Button;
            this.PART_Btn_RecentlyAMonth = GetTemplateChild("PART_Btn_RecentlyAMonth") as Button;
            this.PART_Btn_RecentlyThreeMonth = GetTemplateChild("PART_Btn_RecentlyThreeMonth") as Button;
            this.PART_ConfirmSelected = GetTemplateChild("PART_ConfirmSelected") as Button;
            this.PART_ClearDate = GetTemplateChild("PART_ClearDate") as Button;

            if (this.PART_Popup_New != null)
            {
                this.PART_Popup_New.Opened += PART_Popup_New_Opened;
            }

            if(this.PART_Popup_TimeSelector != null)
            {
                this.PART_Popup_TimeSelector.Opened += PART_Popup_TimeSelector_Opened;
            }

            if (this.PART_Calendar != null)
            {
                this.PART_Calendar.Owner = this;
                this.PART_Calendar.DateClick += PART_Calendar_DateClick;
                this.PART_Calendar.DisplayDateChanged += PART_Calendar_DisplayDateChanged;
                if(this.Type == EnumDatePickerType.SingleDateRange)
                {
                    this.PART_Calendar.DisplayDate = new DateTime(this.DisplayDate.Year, this.DisplayDate.Month, 1);
                }
            }

            if (this.PART_Calendar_Second != null)
            {
                this.PART_Calendar_Second.Owner = this;
                this.PART_Calendar_Second.DisplayMode = CalendarMode.Month;
                this.PART_Calendar_Second.DisplayDate = this.PART_Calendar.DisplayDate.AddMonths(1);

                this.PART_Calendar_Second.DisplayDateChanged += PART_Calendar_Second_DisplayDateChanged;
                this.PART_Calendar_Second.DateClick += PART_Calendar_Second_DateClick;
            }

            if(this.PART_TimeSelector != null)
            {
                this.PART_TimeSelector.SelectedTimeChanged += PART_TimeSelector_SelectedTimeChanged;
            }

            if(this.PART_Btn_Today == null)
            {
                this.PART_Btn_Today.Click -= this.PART_Btn_Today_Click;
            }
            if(this.PART_Btn_Yestday == null)
            {
                this.PART_Btn_Yestday.Click -= this.PART_Btn_Yestday_Click;
            }
            if(this.PART_Btn_AWeekAgo == null)
            {
                this.PART_Btn_AWeekAgo.Click -= PART_Btn_AnWeekAgo_Click;
            }

            if (this.PART_Btn_Today != null)
            {
                this.PART_Btn_Today.Click += PART_Btn_Today_Click;
            }
            if(this.PART_Btn_Yestday != null)
            {
                this.PART_Btn_Yestday.Click += PART_Btn_Yestday_Click;
            }
            if(this.PART_Btn_AWeekAgo != null)
            {
                this.PART_Btn_AWeekAgo.Click += PART_Btn_AnWeekAgo_Click;
            }
            if (this.PART_Btn_RecentlyAWeek != null)
            {
                this.PART_Btn_RecentlyAWeek.Click += PART_Btn_RecentlyAWeek_Click; ;
            }
            if (this.PART_Btn_RecentlyAMonth != null)
            {
                this.PART_Btn_RecentlyAMonth.Click += PART_Btn_RecentlyAMonth_Click; ;
            }
            if (this.PART_Btn_RecentlyThreeMonth != null)
            {
                this.PART_Btn_RecentlyThreeMonth.Click += PART_Btn_RecentlyThreeMonth_Click; ;
            }

            if(this.PART_ConfirmSelected != null)
            {
                this.PART_ConfirmSelected.Click += (o, e) => { this.IsDropDownOpen = false; };
            }

            if(this.PART_ClearDate != null)
            {
                this.PART_ClearDate.Click += PART_ClearDate_Click;
            }

            if(this.SelectedDate.HasValue)
            {
                this.SetSingleDateToTextBox(this.SelectedDate);
                this.SetSelectedDate();
            }

            if(this.SelectedDateStart.HasValue && this.SelectedDateEnd.HasValue)
            {
                this.SetRangeDateToTextBox(this.SelectedDateStart, this.SelectedDateEnd);
                this.SetSelectedDates(this.SelectedDateStart, this.SelectedDateEnd);
            }
            this.SetSelectionMode(this, this.Type);
            this.SetIsShowConfirm();
        }

        private void PART_Popup_TimeSelector_Opened(object sender, EventArgs e)
        {
            if(this.PART_TimeSelector != null)
            {
                this.PART_TimeSelector.SetButtonSelected();
            }
        }

        private void PART_TimeSelector_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if(e.NewValue.HasValue)
            {
                this.SelectedTime = e.NewValue;
            }
        }
        #endregion

        #region Private

        private void SetSelectedDate()
        {
            if (this.PART_Calendar != null)
            {
                this.PART_Calendar.SelectedDate = this.SelectedDate;
            }
        }

        private void PART_ClearDate_Click(object sender, RoutedEventArgs e)
        {
            if(this.PART_TextBox_New != null)
            {
                this.PART_TextBox_New.Text = string.Empty;
            }
            this.ClearSelectedDates();
        }

        private void PART_Calendar_DateClick(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (this.PART_Calendar.DisplayMode == CalendarMode.Month)
            {
                MCalendar calendar = sender as MCalendar;
                if (calendar == null)
                {
                    return;
                }
                if (calendar.SelectedDate == null)
                {
                    return;
                }
                switch (this.Type)
                {
                    case EnumDatePickerType.SingleDate:
                    case EnumDatePickerType.DateTime:
                        this.SetSelectedDate(calendar.SelectedDate.Value);
                        break;
                    case EnumDatePickerType.SingleDateRange:
                        this.HandleSingleDateRange(calendar);
                        break;
                }
            }
        }

        private void PART_Calendar_Second_DateClick(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (this.PART_Calendar_Second.SelectedDate == null)
            {
                return;
            }

            if (sender is MCalendar)
            {
                if (this.PART_Calendar_Second.DisplayMode == CalendarMode.Month)
                {
                    MCalendar calendar = sender as MCalendar;
                    if (calendar == null)
                    {
                        return;
                    }
                    if (calendar.SelectedDate == null)
                    {
                        return;
                    }
                    switch (this.Type)
                    {
                        case EnumDatePickerType.SingleDateRange:
                            this.HandleSingleDateRange(calendar);
                            break;
                    }
                }
            }
        }

        private void HandleSingleDateRange(MCalendar calendar)
        {
            DateTime? dateTime = calendar.SelectedDate;
            if (this.SelectedDateStart != null && this.SelectedDateEnd != null)
            {
                this.SelectedDates.Clear();
                this.PART_Calendar.SelectedDates.Clear();
                this.PART_Calendar_Second.SelectedDates.Clear();
                this.SelectedDateStart = null;
                this.SelectedDateEnd = null;
                this.PART_Calendar.SelectedDate = null;
                this.PART_Calendar_Second.SelectedDate = null;
            }

            if (this.SelectedDateStart == null)
            {
                this.SelectedDateStart = dateTime;
                calendar.SelectedDate = dateTime;
            }
            else if (calendar.SelectedDate < this.SelectedDateStart)
            {
                this.SelectedDates.Clear();
                this.PART_Calendar.SelectedDates.Clear();
                this.PART_Calendar_Second.SelectedDates.Clear();
                this.SelectedDateStart = dateTime;
                this.PART_Calendar.SelectedDate = null;
                this.PART_Calendar_Second.SelectedDate = null;
                calendar.SelectedDate = dateTime;
            }
            else
            {
                this.SelectedDateEnd = dateTime;
                this.SetSelectedDates(this.SelectedDateStart, this.SelectedDateEnd);

                this.SetRangeDateToTextBox(this.SelectedDateStart, this.SelectedDateEnd);
            }
        }

        private void HandleSelectedDatesChanged()
        {
            MDatePicker datePicker = this;
            if (datePicker.PART_Calendar == null || datePicker.PART_Calendar_Second == null)
            {
                return;
            }

            datePicker.PART_Calendar.SelectedDates.Clear();
            datePicker.PART_Calendar_Second.SelectedDates.Clear();

            ObservableCollection<DateTime> dt1 = new ObservableCollection<DateTime>();
            ObservableCollection<DateTime> dt2 = new ObservableCollection<DateTime>();

            foreach (DateTime date in datePicker.SelectedDates)
            {
                /*
                 * Ausgewählte Datumssegmente können sich über mehrere Monate erstrecken. Suchen Sie daher zuerst die Termine, 
                 * die zum ersten Kalender gehören, und zeigen Sie dann die restlichen Termine im zweiten Kalender an.
                 */
                if (DateTimeHelper.MonthIsEqual(date, this.PART_Calendar.DisplayDate))
                {
                    dt1.Add(date);
                }
                else
                {
                    dt2.Add(date);
                }
            }

            datePicker.PART_Calendar.SelectedDates = dt1;
            datePicker.PART_Calendar_Second.SelectedDates = dt2;
        }

        private void PART_Popup_New_Opened(object sender, EventArgs e)
        {
            if (this.PART_Calendar == null)
            {
                return;
            }

            this.PART_Calendar.DisplayMode = CalendarMode.Month;

            switch (this.Type)
            {
                case EnumDatePickerType.SingleDate:
                    break;
                case EnumDatePickerType.SingleDateRange:
                    this.PART_Calendar_Second.DisplayMode = CalendarMode.Month;
                    break;
                default:
                    break;
            }
        }

        private void PART_Calendar_DisplayDateChanged(object sender, RoutedPropertyChangedEventArgs<DateTime> e)
        {
            if (this.Type == EnumDatePickerType.SingleDateRange)
            {
                this.PART_Calendar_Second.DisplayDate = e.NewValue.AddMonths(1);
            }
        }

        private void PART_Calendar_Second_DisplayDateChanged(object sender, RoutedPropertyChangedEventArgs<DateTime> e)
        {
            if (this.PART_Calendar == null)
            {
                return;
            }

            if (this.Type == EnumDatePickerType.SingleDateRange)
            {
                this.PART_Calendar.DisplayDate = e.NewValue.AddMonths(-1);
            }
        }
        #region Klick-Ereignis im Kontextmenü
        private void PART_Btn_Today_Click(object sender, RoutedEventArgs e)
        {
            this.SetSelectedDate(DateTime.Today);
        }

        private void PART_Btn_Yestday_Click(object sender, RoutedEventArgs e)
        {
            this.SetSelectedDate(DateTime.Today.AddDays(-1));
        }

        private void PART_Btn_AnWeekAgo_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedDates.Clear();
            this.PART_Calendar.SelectedDates.Clear();
            this.PART_Calendar_Second.SelectedDates.Clear();
            this.SelectedDateStart = null;
            this.SelectedDateEnd = null;
            this.PART_Calendar.SelectedDate = null;
            this.PART_Calendar_Second.SelectedDate = null;
            this.SetSelectedDate(DateTime.Today.AddDays(-7));
        }

        private void PART_Btn_RecentlyAWeek_Click(object sender, RoutedEventArgs e)
        {
            this.ClearSelectedDates();
            this.FastSetSelectedDates(DateTime.Today.AddDays(-7), DateTime.Today);
        }

        private void PART_Btn_RecentlyAMonth_Click(object sender, RoutedEventArgs e)
        {
            this.ClearSelectedDates();
            this.FastSetSelectedDates(DateTime.Today.AddMonths(-1), DateTime.Today);
        }

        private void PART_Btn_RecentlyThreeMonth_Click(object sender, RoutedEventArgs e)
        {
            this.ClearSelectedDates();
            this.FastSetSelectedDates(DateTime.Today.AddMonths(-3), DateTime.Today);
        }
        #endregion

        private void FastSetSelectedDates(DateTime? startDate, DateTime? endDate)
        {
            if(this.PART_Calendar == null || this.PART_Calendar_Second == null)
            {
                return;
            }

            this.SelectedDateStart = startDate;
            this.SelectedDateEnd = endDate;
            this.PART_Calendar_Second.SelectedDate = null;
            this.PART_Calendar.SelectedDate = null;

            this.PART_Calendar.DisplayDate = new DateTime(startDate.Value.Date.Year, startDate.Value.Date.Month, 1);
            this.PART_Calendar_Second.DisplayDate = new DateTime(endDate.Value.Date.Year, endDate.Value.Date.Month, 1);

            this.SetSelectedDates(this.SelectedDateStart, this.SelectedDateEnd);
        }

        private void SetSelectedDates(DateTime? selectedDateStart, DateTime? selectedDateEnd)
        {
            this.SelectedDates.Clear();
            DateTime? dtTemp = selectedDateStart;
            while (dtTemp <= selectedDateEnd)
            {
                this.SelectedDates.Add(dtTemp.Value);
                dtTemp = dtTemp.Value.AddDays(1);
            }
            this.HandleSelectedDatesChanged();

            if (this.PART_TextBox_New != null && selectedDateStart.HasValue && selectedDateEnd.HasValue)
            {
                this.SetRangeDateToTextBox(selectedDateStart, selectedDateEnd);
            }
        }

        private void SetSelectedDate(DateTime dateTime)
        {
            this.SelectedDate = dateTime;
            this.DisplayDate = dateTime;
            //if(this.PART_Calendar != null)
            //{
            //    this.PART_Calendar.SelectedDate = dateTime;
            //}
            this.IsDropDownOpen = this.IsShowConfirm;
        }

        private void SetSingleDateToTextBox(DateTime? selectedDate)
        {
            if (this.PART_TextBox_New != null)
            {
                if (!selectedDate.HasValue)
                {
                    selectedDate = new DateTime?(DateTime.Today);
                }
                switch (this.Type)
                {
                    case EnumDatePickerType.SingleDate:
                        this.PART_TextBox_New.Text = selectedDate.Value.ToString(this.DateStringFormat);
                        break;
                    case EnumDatePickerType.DateTime:
                        if (this.SelectedTime.HasValue && this.PART_TimeSelector != null)
                        {
                            this.PART_TimeSelector.SelectedTime = this.SelectedTime;
                            this.PART_TextBox_New.Text = $"{selectedDate.Value.ToString(this.DateStringFormat)} {this.SelectedTime.Value.ToString(this.TimeStringFormat)}";
                        }
                        break;
                }

                this.SetSelectedDate(selectedDate.Value);
            }
        }

        private void SetRangeDateToTextBox(DateTime? startDate, DateTime? endDate)
        {
            if (this.PART_TextBox_New == null)
            {
                return;
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                this.PART_TextBox_New.Text = startDate.Value.ToString(this.DateStringFormat) + " - " + endDate.Value.ToString(this.DateStringFormat);
            }
            //Nachdem Sie zwei Daten ausgewählt haben, schließen Sie das Datumsauswahlfeld
            this.IsDropDownOpen = this.IsShowConfirm;
        }

        private void SetSelectionMode(MDatePicker datePicker, EnumDatePickerType type)
        {
            switch (type)
            {
                case EnumDatePickerType.SingleDate:
                    if (datePicker.PART_Calendar != null)
                    {
                        datePicker.PART_Calendar.SelectionMode = CalendarSelectionMode.SingleDate;
                    }
                    break;
                case EnumDatePickerType.SingleDateRange:
                    if(datePicker.PART_Calendar != null)
                    {
                        datePicker.PART_Calendar.SelectionMode = CalendarSelectionMode.SingleRange;
                    }
                    if(datePicker.PART_Calendar_Second != null)
                    {
                        datePicker.PART_Calendar_Second.SelectionMode = CalendarSelectionMode.SingleRange;
                    }
                    break;
                case EnumDatePickerType.Year:
                    break;
                case EnumDatePickerType.Month:
                    break;
                case EnumDatePickerType.DateTime:
                    break;
                case EnumDatePickerType.DateTimeRange:
                    break;
                default:
                    break;
            }
        }

        private void SetIsShowConfirm()
        {
            //Wenn die Steuerung eine Zeit auswählen kann, wird standardmäßig ein Bestätigungsfeld angezeigt.
            switch (this.Type)
            {
                case EnumDatePickerType.DateTime:
                case EnumDatePickerType.DateTimeRange:
                    this.IsShowConfirm = true;
                    break;
                default:
                    break;
            }
        }

        private void ClearSelectedDates()
        {
            this.SelectedDates.Clear();
            this.SelectedDateStart = null;
            this.SelectedDateEnd = null;

            if(this.PART_Calendar != null)
            {
                this.PART_Calendar.SelectedDate = null;
                this.PART_Calendar.SelectedDates.Clear();
            }
            if(this.PART_Calendar_Second != null)
            {
                this.PART_Calendar_Second.SelectedDate = null;
                this.PART_Calendar_Second.SelectedDates.Clear();
            }
        }
        #endregion
    }
}