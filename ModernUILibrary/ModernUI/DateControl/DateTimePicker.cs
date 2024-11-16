namespace ModernIU.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Shapes;

    using ModernIU.Base;

    [TemplatePart(Name = "PART_TextBox", Type = typeof(DatePickerTextBox))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_TimePicker", Type = typeof(TimePicker))]
    [TemplatePart(Name = "PART_Calendar", Type = typeof(Calendar))]
    [TemplatePart(Name = "PART_Clear_Button", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Today_Button", Type = typeof(Button))]
    [TemplatePart(Name = "PART_Confirm_Button", Type = typeof(Button))]
    public class DateTimePicker : DatePicker
    {
        private DatePickerTextBox PART_TextBox;
        private Popup PART_Popup;
        private TimePicker PART_TimePicker;
        private Calendar PART_Calendar;
        private Button PART_Clear_Button;
        private Button PART_Today_Button;
        private Button PART_Confirm_Button;

        static DateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(typeof(DateTimePicker)));
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly" , typeof(bool), typeof(DateTimePicker));
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty DateFormatProperty = DependencyProperty.Register("DateFormat" , typeof(string), typeof(DateTimePicker), new PropertyMetadata("dd.MM.yyyy HH:mm:ss"));

        public string DateFormat
        {
            get { return (string)GetValue(DateFormatProperty); }
            set { SetValue(DateFormatProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value" , typeof(DateTime?), typeof(DateTimePicker) , new FrameworkPropertyMetadata(new PropertyChangedCallback(OnValueChanged)));

        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DateTimePicker dateTimePicker = (DateTimePicker)sender;
            if (e.Property == ValueProperty)
            {
                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(Convert.ToString(e.NewValue), out dt))
                {
                    dateTimePicker.SelectedDate = dt;
                    dateTimePicker.DisplayDate = dt;
                    dateTimePicker.PART_TimePicker.Value = dt;

                    string datetime = dt.ToString(dateTimePicker.DateFormat);
                    if (dateTimePicker.PART_TextBox != null)
                    {
                        dateTimePicker.PART_TextBox.Text = datetime;
                        if (string.IsNullOrEmpty(datetime))
                        {
                            dateTimePicker.PART_Calendar.SelectedDate = null;
                            dateTimePicker.Value = null;
                        }
                        else
                        {
                            dateTimePicker.PART_Calendar.SelectedDate = DateTime.Parse(datetime);
                            dateTimePicker.PART_Calendar.DisplayDate = DateTime.Parse(datetime);
                        }
                    }
                }
            }
            else
            {
                dateTimePicker.PART_TextBox.Text = string.Empty;
                dateTimePicker.PART_Calendar.SelectedDate = null;
                dateTimePicker.Value = null;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_TextBox = VisualHelper.FindVisualElement<DatePickerTextBox>(this, "PART_TextBox_New");
            this.PART_Popup = VisualHelper.FindVisualElement<Popup>(this, "PART_Popup_New");
            this.PART_Calendar = GetTemplateChild("PART_Calendar") as Calendar;
            this.PART_TimePicker = GetTemplateChild("PART_TimePicker") as TimePicker;
            this.PART_Clear_Button = GetTemplateChild("PART_Clear_Button") as Button;
            this.PART_Today_Button = GetTemplateChild("PART_Today_Button") as Button;
            this.PART_Confirm_Button = GetTemplateChild("PART_Confirm_Button") as Button;
            
            if(this.PART_Calendar != null)
            {
                this.PART_Calendar.AddHandler(Button.MouseLeftButtonDownEvent, new RoutedEventHandler(DayButton_MouseLeftButtonUp), true);
            }

            if(this.PART_TimePicker != null)
            {
                this.PART_TimePicker.SelectedTimeChanged += PART_TimePicker_SelectedTimeChanged; ;
            }

            if(this.PART_Popup != null)
            {
                this.PART_Popup.Opened += PART_Popup_Opened;
                this.PART_Popup.Closed += PART_Popup_Closed;
            }

            if(this.PART_Today_Button != null)
            {
                this.PART_Today_Button.Click += PART_Today_Button_Click;
            }

            if (this.PART_Confirm_Button != null)
            {
                this.PART_Confirm_Button.Click += PART_Confirm_Button_Click;
            }

            if (this.PART_Clear_Button != null)
            {
                this.PART_Clear_Button.Click += PART_Clear_Button_Click;
            }
        }

        private void PART_TimePicker_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            string datepart = string.Empty;
            string timepart = string.Empty;
            if (!string.IsNullOrEmpty(Convert.ToString(e.NewValue)))
            {
                datepart = this.PART_Calendar.DisplayDate.ToString("yyyy-MM-dd");
                timepart = Convert.ToDateTime(e.NewValue).ToString("HH:mm:ss");
                this.SetDateTime(Convert.ToDateTime(datepart + " " + timepart).ToString(this.DateFormat));
            }
        }

        private void PART_Confirm_Button_Click(object sender, RoutedEventArgs e)
        {
            this.IsDropDownOpen = false;
            SetCurrentData(this.PART_TextBox.Text);
        }

        private void PART_Today_Button_Click(object sender, RoutedEventArgs e)
        {
            this.SetDateTime(DateTime.Now.ToString(this.DateFormat));
            this.IsDropDownOpen = false;
            SetCurrentData(this.PART_TextBox.Text);
        }

        private void PART_Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            this.SetDateTime(string.Empty);

            SetCurrentData(this.PART_TextBox.Text);
        }

        private void PART_Popup_Opened(object sender, EventArgs e)
        {
            /*
             * Wenn eingeschaltet, wird der Zeitwähler auf den aktuellen Wert gesetzt, wenn derzeit keine Zeit ausgewählt ist.
            */
            if (this.PART_TextBox != null)
            {
                if (string.IsNullOrEmpty(this.PART_TextBox.Text))
                {
                    this.PART_TimePicker.Value = DateTime.Now;
                }
            }
        }

        private void PART_Popup_Closed(object sender, EventArgs e)
        {
            this.IsDropDownOpen = false;
            SetCurrentData(this.PART_TextBox.Text);
        }

        private void DayButton_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            string date_part = string.Empty;
            string time_part = string.Empty;
            if (sender is Calendar)
            {
                Calendar calendar = sender as Calendar;
                if (calendar != null && calendar.InputHitTest(Mouse.GetPosition(e.Source as FrameworkElement)) is Rectangle
                    || calendar.InputHitTest(Mouse.GetPosition(e.Source as FrameworkElement)) is TextBlock)
                {
                    if (calendar.SelectedDate == null)
                    {
                        return;
                    }

                    date_part = calendar.SelectedDate.Value.ToString("dd.MM.yyyy");

                    if (this.PART_TimePicker != null)
                    {
                        time_part = this.PART_TimePicker.Value.Value.ToString("HH:mm:ss");
                    }

                    this.SetDateTime(Convert.ToDateTime(date_part + " " + time_part).ToString(this.DateFormat));
                }
            }
        }

        private void SetDateTime(string datetime)
        {
            if (this.PART_TextBox != null)
            {
                this.PART_TextBox.Text = datetime;
                if (string.IsNullOrEmpty(datetime))
                {
                    this.PART_Calendar.SelectedDate = null;
                    this.Value = null;
                }
                else
                {
                    this.PART_Calendar.SelectedDate = DateTime.Parse(datetime);
                }
            }
        }

        private void SetCurrentData(string text)
        {
            DateTime dt = DateTime.Now;
            if (DateTime.TryParse(text, out dt))
            {
                this.Value = dt;
            }
            else
            {
                this.Value = null;
            }
        }
    }
}
