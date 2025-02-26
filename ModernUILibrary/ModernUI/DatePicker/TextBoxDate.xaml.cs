namespace ModernIU.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernIU.Base;

    /// <summary>
    /// Interaktionslogik für TextBoxDate.xaml
    /// </summary>
    public partial class TextBoxDate : UserControl
    {
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(TextBoxDate), new PropertyMetadata(false, OnIsReadOnly));
        public static readonly DependencyProperty ReadOnlyBackgroundColorProperty = DependencyProperty.Register(nameof(ReadOnlyBackgroundColor), typeof(Brush), typeof(TextBoxDate), new PropertyMetadata(Brushes.LightYellow));
        public static readonly DependencyProperty ShowTodayButtonProperty = DependencyProperty.RegisterAttached(nameof(ShowTodayButton), typeof(bool?), typeof(TextBoxDate), new PropertyMetadata(null, OnShowTodayButtonChanged));
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime?), typeof(TextBoxDate), new FrameworkPropertyMetadata(new DateTime(1900,1,1), OnSelectedDateChanged));
        private static readonly string[] DateFormats = new string[] { "d.M.yyyy", "dd.MM.yyyy", "yyyy.MM", "yyyy.M", "MM.yyyy", "M.yyyy", "yyyy.MM" };
        private string day = string.Empty;
        private string month = string.Empty;
        private string year = string.Empty;

        public TextBoxDate()
        {
            this.InitializeComponent();

            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(2);
            this.Focusable = true;

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<ComboBoxEx, SelectionChangedEventArgs>.AddHandler(this.cbDay, "SelectionChanged", this.OnDateSelectionChanged);
            WeakEventManager<ComboBoxEx, SelectionChangedEventArgs>.AddHandler(this.cbMonth, "SelectionChanged", this.OnDateSelectionChanged);
            WeakEventManager<ComboBoxEx, SelectionChangedEventArgs>.AddHandler(this.cbYear, "SelectionChanged", this.OnDateSelectionChanged);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.btnToday, "Click", this.OnSetCurrentDate);
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

        public DateTime? SelectedDate
        {
            get { return GetValue(SelectedDateProperty) as DateTime?; }
            set { this.SetValue(SelectedDateProperty, value); }
        }

        public bool? ShowTodayButton
        {
            get { return (bool?)GetValue(ShowTodayButtonProperty); }
            set { SetValue(ShowTodayButtonProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.cbDay.ItemsSource = Enumerable.Range(1, 31).Select(x => (x - 1) + 1);
            this.cbDay.IsEnabledContextMenu = false;
            this.cbMonth.ItemsSource = Enumerable.Range(1, 12).Select(x => (x - 1) + 1);
            this.cbMonth.IsEnabledContextMenu = false;
            this.cbYear.ItemsSource = Enumerable.Range(1900, 200).Select(x => (x - 1) + 1);
            this.cbYear.IsEnabledContextMenu = false;
        }

        private void MoveFocus(FocusNavigationDirection direction)
        {
            UIElement focusedElement = Keyboard.FocusedElement as UIElement;

            if (focusedElement != null)
            {
                if (focusedElement is TextBox)
                {
                    focusedElement.MoveFocus(new TraversalRequest(direction));
                }
            }
        }

        private void OnDateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxEx cb = sender as ComboBoxEx;
            if (sender != null)
            {
                if (cb.Name == "cbDay")
                {
                    if (e.AddedItems.Count > 0)
                    {
                        this.day = ((object[])e.AddedItems)[0].ToString();
                    }
                    else
                    {
                        this.day = DateTime.Now.Day.ToString();
                    }
                }
                else if (cb.Name == "cbMonth")
                {
                    if (e.AddedItems.Count > 0)
                    {
                        this.month = ((object[])e.AddedItems)[0].ToString();
                    }
                    else
                    {
                        this.month = DateTime.Now.Month.ToString();
                    }
                }
                else if (cb.Name == "cbYear")
                {
                    if (e.AddedItems.Count > 0)
                    {
                        this.year = ((object[])e.AddedItems)[0].ToString();
                    }
                    else
                    {
                        this.year = DateTime.Now.Year.ToString();
                    }
                }

                if (string.IsNullOrEmpty(day) == false && string.IsNullOrEmpty(month) == false && string.IsNullOrEmpty(year) == false)
                {
                    string dateSeparator = CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator;
                    string fullDate = $"{day}{dateSeparator}{month}{dateSeparator}{year}";
                    DateTime date;
                    if (DateTime.TryParseExact(fullDate, DateFormats, Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out date) == true)
                    {
                        this.SelectedDate = date;
                    }
                }
            }
        }

        private static void OnIsReadOnly(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxDate datePicker = (TextBoxDate)d;

            if (datePicker != null)
            {
                if (datePicker.IsReadOnly == true)
                {
                    datePicker.FontWeight = FontWeights.Bold;
                    datePicker.cbDay.IsReadOnly = datePicker.IsReadOnly;
                    datePicker.cbMonth.IsReadOnly = datePicker.IsReadOnly;
                    datePicker.cbYear.IsReadOnly = datePicker.IsReadOnly;
                }
                else
                {
                    datePicker.FontWeight = FontWeights.Normal;
                    datePicker.cbDay.IsReadOnly = datePicker.IsReadOnly;
                    datePicker.cbMonth.IsReadOnly = datePicker.IsReadOnly;
                    datePicker.cbYear.IsReadOnly = datePicker.IsReadOnly;
                }
            }
        }

        private static void OnShowTodayButtonChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBoxDate control)
            {
                bool showButton = (bool)(e.NewValue);
                if (showButton == true)
                {
                    control.btnToday.Visibility = Visibility.Visible;
                }
                else
                {
                    control.btnToday.Visibility = Visibility.Collapsed;
                }
            }
        }

        private static void OnSelectedDateChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBoxDate control)
            {
                DateTime? selected = (DateTime?)(e.NewValue);
                if (selected != null)
                {
                    control.cbDay.SelectedValue = selected.HasValue == true ? selected.Value.Day : -1;
                    control.cbMonth.SelectedValue = selected.HasValue == true? selected.Value.Month : -1;
                    control.cbYear.SelectedValue = selected.HasValue == true? selected.Value.Year : -1;
                }
            }
        }

        private void OnSetCurrentDate(object sender, RoutedEventArgs e)
        {
            if (sender is Button control)
            {
                this.cbDay.SelectedValue = DateTime.Now.Day;
                this.cbMonth.SelectedValue = DateTime.Now.Month;
                this.cbYear.SelectedValue = DateTime.Now.Year;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key == 2 || key == 32 || key == 21 || key == 22 || key == 23 || key == 25 || key == 3);

            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                if (e.Key == Key.Tab)
                {
                    return;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Up:
                        break;
                    case Key.Down:
                        break;
                    case Key.Left:
                        return;
                    case Key.Right:
                        return;
                    case Key.Pa1:
                        return;
                    case Key.End:
                        return;
                    case Key.Delete:
                        return;
                    case Key.Return:
                        this.MoveFocus(FocusNavigationDirection.Next);
                        break;
                    case Key.Tab:
                        break;
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.BorderBrush = ControlBase.BorderBrush;
            this.BorderThickness = ControlBase.BorderThickness;

            /* Spezifisches Kontextmenü für Control übergeben */
            this.cbDay.ContextMenu = this.BuildContextMenu();
            this.cbMonth.ContextMenu = this.BuildContextMenu();
            this.cbYear.ContextMenu = this.BuildContextMenu();
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
            string dtFull = $"{this.cbDay.Text}.{this.cbMonth.Text}.{this.cbYear.Text}";
            Clipboard.SetText(dtFull);
        }

        private void OnPasteMenu(object sender, RoutedEventArgs e)
        {
            string dtFull = Clipboard.GetText();
            if (dtFull.Split('.').Length == 3)
            {
                this.day = dtFull.Split('.')[0];
                this.month = dtFull.Split('.')[1];
                this.year = dtFull.Split('.')[2];
            }

        }

        private void OnDeleteMenu(object sender, RoutedEventArgs e)
        {
            string dtFull = $"{this.cbDay.Text}.{this.cbMonth.Text}.{this.cbYear.Text}";
            Clipboard.SetText(dtFull);
            this.day = string.Empty;
            this.month = string.Empty;
            this.year = string.Empty;
        }
    }
}
