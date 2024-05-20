namespace ModernIU.Controls
{
    using System;
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
        public static readonly DependencyProperty ShowTodayButtonProperty = DependencyProperty.RegisterAttached(nameof(ShowTodayButton), typeof(bool), typeof(TextBoxDate), new PropertyMetadata(false, OnShowTodayButtonChanged));
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime?), typeof(TextBoxDate), new FrameworkPropertyMetadata(new DateTime(1900,1,1), OnSelectedDateChanged));

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

        public bool ShowTodayButton
        {
            get { return (bool)GetValue(ShowTodayButtonProperty); }
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
                }
                else if (cb.Name == "cbMonth")
                {
                }
                else if (cb.Name == "cbYear")
                {
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
                    control.cbDay.SelectedValue = selected.HasValue ? selected.Value.Day : -1;
                    control.cbMonth.SelectedValue = selected.HasValue ? selected.Value.Month : -1;
                    control.cbYear.SelectedValue = selected.HasValue ? selected.Value.Year : -1;
                }
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
        }
    }
}
