/*
 * <copyright file="DatePickerEx.cs" company="Lifeprojects.de">
 *     Class: DatePickerEx
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>01.02.2023 19:54:43</date>
 * <Project>CurrentProject</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernIU.Controls
{
    using System;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using ModernIU.Base;

    [SupportedOSPlatform("windows")]
    public class DatePickerEx : DatePicker
    {
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(DatePickerEx), new PropertyMetadata(1000));
        public static readonly DependencyProperty WatermarkTextProperty = DependencyProperty.Register("WatermarkText", typeof(string), typeof(DatePickerEx), new PropertyMetadata(WaterMarkDefaultText));
        public static readonly DependencyProperty WatermarkDefaultDateProperty = DependencyProperty.Register("WatermarkDefaultDate", typeof(string), typeof(DatePickerEx), new PropertyMetadata(WaterMarkDefalutDate));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DatePickerEx), new PropertyMetadata(false, OnIsReadOnly));
        public static readonly DependencyProperty ReadOnlyBackgroundColorProperty = DependencyProperty.Register("ReadOnlyBackgroundColor", typeof(Brush), typeof(DatePickerEx), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(222, 222, 222))));
        public static readonly DependencyProperty ShowTodayButtonProperty = DependencyProperty.RegisterAttached("ShowTodayButton", typeof(bool), typeof(DatePickerEx), new FrameworkPropertyMetadata(false, ShowTodayButtonChanged));
        public static readonly DependencyProperty ShowTodayButtonContentProperty = DependencyProperty.RegisterAttached("ShowTodayButtonContent", typeof(string), typeof(DatePickerEx), new FrameworkPropertyMetadata("Heute", ShowTodayButtonContentChanged));

        private static readonly string[] DateFormats = new string[] { "d.M.yyyy", "dd.MM.yyyy","yyyy.MM", "yyyy.M", "MM.yyyy", "M.yyyy", "yyyy.MM" };
        private static readonly string WaterMarkDefaultText = "dd.MM.yyyy";
        private static readonly string WaterMarkDefalutDate = "01.01.1900";

        private DatePickerTextBox _datePickerTextBox;
        private Calendar _calendar = null;
        private Button _prevBtn = null;
        private Button _nextBtn = null;


        public DatePickerEx() : base()
        {
            WeakEventManager<DatePicker, KeyEventArgs>.AddHandler(this, "PreviewKeyDown", this.OnPreviewKeyDown);
            WeakEventManager<DatePicker, RoutedEventArgs>.AddHandler(this, "CalendarOpened", this.DatePicker_CalendarOpen);
            WeakEventManager<DatePicker, RoutedEventArgs>.AddHandler(this, "CalendarClosed", this.DatePicker_CalendarClosed);

            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Background = Brushes.White;
            this.BorderBrush = Brushes.Green;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(2);
            this.ClipToBounds = false;
            this.MinHeight = 18;
            this.Height = 23;
            this.Width = 100;
            this.IsTodayHighlighted = true;
            this.IsReadOnly = false;
            this.Focusable = true;
            this.ReadOnlyBackgroundColor = Brushes.LightYellow;
        }

        ~DatePickerEx()
        {
            WeakEventManager<DatePicker, RoutedEventArgs>.RemoveHandler(this, "CalendarOpened", this.DatePicker_CalendarOpen);
            WeakEventManager<DatePicker, KeyEventArgs>.RemoveHandler(this, "PreviewKeyDown", this.OnPreviewKeyDown);
            WeakEventManager<DatePicker, RoutedEventArgs>.RemoveHandler(this, "CalendarClosed", this.DatePicker_CalendarClosed);
        }

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { this.SetValue(MaxLengthProperty, value); }
        }

        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { this.SetValue(WatermarkTextProperty, value); }
        }

        public string WatermarkDefaultDate
        {
            get { return (string)GetValue(WatermarkDefaultDateProperty); }
            set { this.SetValue(WatermarkDefaultDateProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public Brush ReadOnlyBackgroundColor
        {
            get { return GetValue(ReadOnlyBackgroundColorProperty) as Brush; }
            set { this.SetValue(ReadOnlyBackgroundColorProperty, value); }
        }

        [AttachedPropertyBrowsableForType(typeof(DatePicker))]
        public static bool GetShowTodayButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowTodayButtonProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(DatePicker))]
        public static void SetShowTodayButton(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowTodayButtonProperty, value);
        }

        [AttachedPropertyBrowsableForType(typeof(DatePicker))]
        public static string GetShowTodayButtonContent(Control obj)
        {
            return (string)obj.GetValue(ShowTodayButtonContentProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(DatePicker))]
        public static void SetShowTodayButtonContent(Control obj, string value)
        {
            obj.SetValue(ShowTodayButtonContentProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Button datePickerButton = this.Template.FindName("PART_Button", this) as Button;
            if (datePickerButton != null)
            {
                datePickerButton.Margin = new Thickness(12, 0, 0, 0);
                datePickerButton.HorizontalAlignment = HorizontalAlignment.Right;
            }

            this._datePickerTextBox = this.Template.FindName("PART_TextBox", this) as DatePickerTextBox;
            if (this._datePickerTextBox != null)
            {
                if (this.SelectedDate == null)
                {
                    this._datePickerTextBox.Text = string.Empty;
                    this._datePickerTextBox.HorizontalAlignment = HorizontalAlignment.Left;
                }

                WeakEventManager<DatePickerTextBox, TextChangedEventArgs>.AddHandler(this._datePickerTextBox, "TextChanged", this.OnTextChanged);
                WeakEventManager<DatePickerTextBox, TextCompositionEventArgs>.AddHandler(this._datePickerTextBox, "PreviewTextInput", this.OnPreviewTextInput);
                this._datePickerTextBox.Background = this.Background;
                this._datePickerTextBox.MaxLength = this.MaxLength;

                /* Trigger an Style übergeben */
                this.Style = this.SetTriggerFunction();
            }

            /* Spezifisches Kontextmenü für Control übergeben */
            this._datePickerTextBox.ContextMenu = this.BuildContextMenu();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            this.SetWatermark();
        }

        private static void ShowTodayButtonContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SetShowTodayButton(d, !GetShowTodayButton(d));
        }

        private static void ShowTodayButtonChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DatePicker)
            {
                var d = (DatePicker)sender;
                var showButton = (bool)(e.NewValue);

                if (showButton == true)
                {
                    Setter setter = new Setter();
                    setter.Property = Calendar.TemplateProperty;
                    ControlTemplate template = new ControlTemplate(typeof(Calendar));
                    var stackPanel = new FrameworkElementFactory(typeof(StackPanel));
                    stackPanel.Name = "PART_Root";
                    stackPanel.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Center);

                    var calendar = new FrameworkElementFactory(typeof(CalendarItem));
                    calendar.Name = "PART_CalendarItem";

                    calendar.SetBinding(CalendarItem.BackgroundProperty,
                        new Binding(CalendarItem.BackgroundProperty.Name)
                        {
                            Path = new PropertyPath(Control.BackgroundProperty),
                            RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
                        });

                    calendar.SetBinding(CalendarItem.BorderBrushProperty, new Binding(CalendarItem.BorderBrushProperty.Name)
                    {
                        Path = new PropertyPath(Control.BorderBrushProperty),
                        RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
                    });

                    calendar.SetBinding(CalendarItem.BorderThicknessProperty, new Binding(CalendarItem.BorderThicknessProperty.Name)
                    {
                        Path = new PropertyPath(Control.BorderThicknessProperty),
                        RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
                    });

                    calendar.SetBinding(CalendarItem.StyleProperty, new Binding(CalendarItem.StyleProperty.Name)
                    {
                        Path = new PropertyPath(Calendar.CalendarItemStyleProperty),
                        RelativeSource = new RelativeSource(RelativeSourceMode.TemplatedParent)
                    });

                    stackPanel.AppendChild(calendar);

                    var btn = new FrameworkElementFactory(typeof(Button));
                    btn.SetValue(Button.ContentProperty, GetShowTodayButtonContent(d));

                    var selectToday = new RoutedCommand("Today", typeof(DatePickerEx));

                    d.CommandBindings.Add(new CommandBinding(selectToday,
                      (s, ea) =>
                      {
                          (s as DatePicker).SelectedDate = DateTime.Now.Date;
                          (s as DatePicker).IsDropDownOpen = false;
                      },
                      (s, ea) => { ea.CanExecute = true; }));

                    btn.SetValue(Button.CommandProperty, selectToday);

                    stackPanel.AppendChild(btn);

                    template.VisualTree = stackPanel;
                    setter.Value = template;

                    Style customStyle = new Style(typeof(Calendar));
                    customStyle.Setters.Add(setter);
                    d.CalendarStyle = customStyle;
                }
            }
        }

        private static Calendar GetDatePickerCalendar(object sender)
        {
            var datePicker = (DatePicker)sender;
            var popup = (Popup)datePicker.Template.FindName("PART_Popup", datePicker);

            return (Calendar)popup.Child;
        }

        private void DatePicker_CalendarOpen(object sender, RoutedEventArgs e)
        {
            this._calendar = GetDatePickerCalendar(sender);

            if (this._calendar != null)
            {                
                CalendarItem calItem = (CalendarItem)this._calendar.Template.FindName("PART_CalendarItem", this._calendar);

                if (calItem != null)
                {
                    this._prevBtn = (Button)calItem.Template.FindName("PART_PreviousButton", calItem);
                    this._nextBtn = (Button)calItem.Template.FindName("PART_NextButton", calItem);

                    WeakEventManager<Button, RoutedEventArgs>.AddHandler(this._prevBtn, "Click", this.OnCalendarPrevButtonClick);
                    WeakEventManager<Button, RoutedEventArgs>.AddHandler(this._nextBtn, "Click", this.OnCalendarNextButtonClick);
                }
            }

            DateTime watermarkDefaultDate;
            if (DateTime.TryParseExact(this.WatermarkDefaultDate, DateFormats, Thread.CurrentThread.CurrentCulture, System.Globalization.DateTimeStyles.None, out watermarkDefaultDate))
            {
                if (this.SelectedDate == null || this.SelectedDate == watermarkDefaultDate)
                {
                    this._calendar.DisplayDate = DateTime.Now;
                }
            }
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            this._calendar = null;

            if (this._prevBtn != null)
            {
                WeakEventManager<Button, RoutedEventArgs>.RemoveHandler(this._prevBtn, "Click", this.OnCalendarPrevButtonClick);
                this._prevBtn = null;
            }

            if (this._nextBtn != null)
            {
                WeakEventManager<Button, RoutedEventArgs>.RemoveHandler(this._nextBtn, "Click", this.OnCalendarNextButtonClick);
                this._nextBtn = null;
            }
        }

        private void OnCalendarPrevButtonClick(object sender, RoutedEventArgs e)
        {
            if (e != null &&
                this._calendar != null &&
                this._calendar.SelectedDate != null)
            {
                DateTime selectedDate = (DateTime)this._calendar.SelectedDate;

                if (this._calendar.DisplayMode == CalendarMode.Month)
                {
                    this._datePickerTextBox.Text = selectedDate.AddMonths(-1).ToShortDateString();
                }
                else if (this._calendar.DisplayMode == CalendarMode.Year)
                {
                    this._datePickerTextBox.Text = selectedDate.AddYears(-1).ToShortDateString();
                }
                else if (this._calendar.DisplayMode == CalendarMode.Decade)
                {
                    this._datePickerTextBox.Text = selectedDate.AddYears(-10).ToShortDateString();
                }

                e.Handled = true;
            }
        }

        private void OnCalendarNextButtonClick(object sender, RoutedEventArgs e)
        {
            if (e != null &&
                this._calendar != null &&
                this._calendar.SelectedDate != null)
            {
                DateTime selectedDate = (DateTime)this._calendar.SelectedDate;

                if (this._calendar.DisplayMode == CalendarMode.Month)
                {
                    this._datePickerTextBox.Text = selectedDate.AddMonths(1).ToShortDateString();
                }
                else if (this._calendar.DisplayMode == CalendarMode.Year)
                {
                    this._datePickerTextBox.Text = selectedDate.AddYears(1).ToShortDateString();
                }
                else if (this._calendar.DisplayMode == CalendarMode.Decade)
                {
                    this._datePickerTextBox.Text = selectedDate.AddYears(10).ToShortDateString();
                }

                e.Handled = true;
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == null || ((DatePicker)sender).SelectedDate == null || ((DatePicker)sender).IsDropDownOpen)
            {
                return;
            }

            if (this.IsReadOnly == false)
            {
                if (this._datePickerTextBox.Text.Length > 7)
                {
                    if (e.Key == Key.Up)
                    {
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            ((DatePicker)sender).SelectedDate = ((DatePicker)sender).SelectedDate.GetValueOrDefault().AddMonths(1);
                        }
                        else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                        {
                            ((DatePicker)sender).SelectedDate = ((DatePicker)sender).SelectedDate.GetValueOrDefault().AddYears(1);
                        }
                        else
                        {
                            ((DatePicker)sender).SelectedDate = ((DatePicker)sender).SelectedDate.GetValueOrDefault().AddDays(1);
                        }
                    }

                    if (e.Key == Key.Down)
                    {
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            ((DatePicker)sender).SelectedDate = ((DatePicker)sender).SelectedDate.GetValueOrDefault().AddMonths(-1);
                        }
                        else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                        {
                            ((DatePicker)sender).SelectedDate = ((DatePicker)sender).SelectedDate.GetValueOrDefault().AddYears(-1);
                        }
                        else
                        {
                            ((DatePicker)sender).SelectedDate = ((DatePicker)sender).SelectedDate.GetValueOrDefault().AddDays(-1);
                        }
                    }

                    if (e.Key == Key.D && Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        ((DatePicker)sender).IsDropDownOpen = true;
                        if (this._calendar != null)
                        {
                            this._calendar.DisplayMode = CalendarMode.Month;
                        }
                    }

                    if (e.Key == Key.M && Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        ((DatePicker)sender).IsDropDownOpen = true;
                        if (this._calendar != null)
                        {
                            this._calendar.DisplayMode = CalendarMode.Year;
                        }
                    }

                    if (e.Key == Key.Y && Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        ((DatePicker)sender).IsDropDownOpen = true;
                        if (this._calendar != null)
                        {
                            this._calendar.DisplayMode = CalendarMode.Decade;
                        }
                    }

                    if (e.Key == Key.H && Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        DatePickerEx elementUI = sender as DatePickerEx;
                        Popup codePopup = new Popup();

                        TextBlock popupText = new TextBlock();
                        popupText.Text = "Popup Text";
                        popupText.Background = Brushes.LightBlue;
                        popupText.Foreground = Brushes.Blue;
                        codePopup.Child = popupText;
                        codePopup.PlacementTarget = elementUI;
                        codePopup.IsOpen = true;
                    }
                }
                else
                {
                    if (e.Key == Key.Up)
                    {
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            ((DatePicker)sender).SelectedDate = ((DatePicker)sender).SelectedDate.GetValueOrDefault().AddMonths(1);
                        }
                        else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                        {
                            ((DatePicker)sender).SelectedDate = ((DatePicker)sender).SelectedDate.GetValueOrDefault().AddYears(1);
                        }
                    }

                    if (e.Key == Key.Down)
                    {
                        if (Keyboard.IsKeyDown(Key.LeftShift))
                        {
                            ((DatePicker)sender).SelectedDate = ((DatePicker)sender).SelectedDate.GetValueOrDefault().AddMonths(-1);
                        }
                        else if (Keyboard.IsKeyDown(Key.LeftCtrl))
                        {
                            ((DatePicker)sender).SelectedDate = ((DatePicker)sender).SelectedDate.GetValueOrDefault().AddYears(-1);
                        }
                    }

                    if (e.Key == Key.D && Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        ((DatePicker)sender).IsDropDownOpen = true;
                        if (this._calendar != null)
                        {
                            this._calendar.DisplayMode = CalendarMode.Month;
                        }
                    }

                    if (e.Key == Key.M && Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        ((DatePicker)sender).IsDropDownOpen = true;
                        if (this._calendar != null)
                        {
                            this._calendar.DisplayMode = CalendarMode.Year;
                        }
                    }

                    if (e.Key == Key.Y && Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        ((DatePicker)sender).IsDropDownOpen = true;
                        if (this._calendar != null)
                        {
                            this._calendar.DisplayMode = CalendarMode.Decade;
                        }
                    }
                }
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length  == 1)
            {
                char character = Convert.ToChar(e.Text);

                if (char.IsNumber(character) || character == '.' || character == '/')
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this._datePickerTextBox.Text) == true && this.SelectedDate == null)
            {
                this.SelectedDate = null;
                return;
            }

            DateTime convertedDate;
            if (DateTime.TryParseExact(this._datePickerTextBox.Text, DateFormats, Thread.CurrentThread.CurrentCulture, System.Globalization.DateTimeStyles.None, out convertedDate))
            {
                if (this.SelectedDate != convertedDate)
                {
                    this.SelectedDate = convertedDate;
                    this.SetWatermark();
                }
            }
            else if (string.IsNullOrEmpty(this._datePickerTextBox.Text) == true && this.SelectedDate == null)
            {
                this.SelectedDate = null;
            }
        }

        private void SetWatermark()
        {
            FieldInfo fieldInfoTextBox = typeof(DatePicker).GetField("_textBox", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfoTextBox != null)
            {
                DatePickerTextBox dateTextBox = (DatePickerTextBox)fieldInfoTextBox.GetValue(this);
                if (dateTextBox != null)
                {
                    if (string.IsNullOrWhiteSpace(this.WatermarkText))
                    {
                        this.WatermarkText = WaterMarkDefaultText;
                    }

                    DateTime watermarkDefaultDate;
                    if (DateTime.TryParseExact(this.WatermarkDefaultDate, DateFormats, Thread.CurrentThread.CurrentCulture, System.Globalization.DateTimeStyles.None, out watermarkDefaultDate))
                    {
                        if (this.SelectedDate != null && this.SelectedDate == watermarkDefaultDate)
                        {
                            this.SelectedDate = null;
                        }
                    }

                    var partWatermark = dateTextBox.Template.FindName("PART_Watermark", dateTextBox) as ContentControl;
                    if (partWatermark != null)
                    {
                        partWatermark.Foreground = new SolidColorBrush(Colors.Gray);
                        partWatermark.Content = this.WatermarkText;
                    }
                }
            }
        }

        private static void OnIsReadOnly(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DatePickerEx dp = (DatePickerEx)d;

            if (dp != null)
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new Action<DatePickerEx>(ApplyIsReadOnly), dp);
            }
        }

        private static void ApplyIsReadOnly(DatePickerEx datePicker)
        {
            DatePickerTextBox textBox = GetTemplateDatePickerTextBox(datePicker);
            Button button = GetTemplateCalendarButton(datePicker);

            if(textBox != null && button != null)
            {
                if (datePicker.IsReadOnly)
                {
                    textBox.Background = datePicker.ReadOnlyBackgroundColor;
                    textBox.IsReadOnly = true;
                    button.IsEnabled = false;
                }
                else
                {
                    textBox.Background = Brushes.WhiteSmoke;
                    textBox.IsReadOnly = false;
                    button.IsEnabled = true;
                }
            }
        }
        private static DatePickerTextBox GetTemplateDatePickerTextBox(Control control)
        {
            control.ApplyTemplate();
            string clrlName = control.Name;
            var ctrl = (DatePickerTextBox)control.Template.FindName("PART_TextBox", control);
            return ctrl;
        }

        private static Button GetTemplateCalendarButton(Control control)
        {
            control.ApplyTemplate();
            string clrlName = control.Name;
            var ctrl = (Button)control.Template.FindName("PART_Button", control);
            ctrl.Background = Brushes.WhiteSmoke;
            return ctrl;
        }

        private Style SetTriggerFunction()
        {
            Style inputControlStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = DatePickerTextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = DatePickerTextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = DatePickerTextBox.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = DatePickerTextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsFocused);

            /* Trigger für IsFocused = True */
            Trigger triggerIsReadOnly = new Trigger();
            triggerIsReadOnly.Property = DatePickerTextBox.IsReadOnlyProperty;
            triggerIsReadOnly.Value = true;
            triggerIsReadOnly.Setters.Add(new Setter() { Property = DatePickerTextBox.BackgroundProperty, Value = Brushes.LightYellow });
            inputControlStyle.Triggers.Add(triggerIsReadOnly);

            return inputControlStyle;
        }

        /// <summary>
        /// Spezifisches Kontextmenü erstellen
        /// </summary>
        /// <returns></returns>
        private ContextMenu BuildContextMenu()
        {
            ContextMenu textBoxContextMenu = new ContextMenu();
            MenuItem copyMenu = new MenuItem();
            copyMenu.Header = "Kopiere";
            copyMenu.Icon = Icons.GetPathGeometry(Icons.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopyMenu);
            textBoxContextMenu.Items.Add(copyMenu);

            if (this.IsReadOnly == false)
            {
                MenuItem pasteMenu = new MenuItem();
                pasteMenu.Header = "Einfügen";
                pasteMenu.Icon = Icons.GetPathGeometry(Icons.IconPaste);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(pasteMenu, "Click", this.OnPasteMenu);
                textBoxContextMenu.Items.Add(pasteMenu);

                MenuItem deleteMenu = new MenuItem();
                deleteMenu.Header = "Ausschneiden";
                deleteMenu.Icon = Icons.GetPathGeometry(Icons.IconDelete);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(deleteMenu, "Click", this.OnDeleteMenu);
                textBoxContextMenu.Items.Add(deleteMenu);
            }

            return textBoxContextMenu;
        }

        private void OnCopyMenu(object sender, RoutedEventArgs e)
        {
            this._datePickerTextBox.SelectAll();
            Clipboard.SetText(this._datePickerTextBox.Text);
        }

        private void OnPasteMenu(object sender, RoutedEventArgs e)
        {
            this._datePickerTextBox.Text = Clipboard.GetText();
        }

        private void OnDeleteMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this._datePickerTextBox.Text);
            this._datePickerTextBox.Text = string.Empty;
        }
    }
}