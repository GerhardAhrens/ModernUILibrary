namespace ModernIU.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using ModernIU.Base;

    /// <summary>
    /// Interaktionslogik für TextBoxDate.xaml
    /// </summary>
    public partial class TextBoxDate : UserControl
    {
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(TextBoxDate), new PropertyMetadata(false, OnIsReadOnly));
        public static readonly DependencyProperty ReadOnlyBackgroundColorProperty = DependencyProperty.Register(nameof(ReadOnlyBackgroundColor), typeof(Brush), typeof(TextBoxDate), new PropertyMetadata(Brushes.LightYellow));
        public static readonly DependencyProperty ShowTodayButtonProperty = DependencyProperty.RegisterAttached(nameof(ShowTodayButton), typeof(bool), typeof(TextBoxDate), new FrameworkPropertyMetadata(false, ShowTodayButtonChanged));
        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(nameof(SelectedValue), typeof(string), typeof(TextBoxDate), new PropertyMetadata(string.Empty));

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

            this.DataContext = this;
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

        public string SelectedValue
        {
            get { return GetValue(SelectedValueProperty) as string; }
            set { this.SetValue(SelectedValueProperty, value); }
        }

        public bool ShowTodayButton
        {
            get { return (bool)GetValue(ShowTodayButtonProperty); }
            set { SetValue(ShowTodayButtonProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.cbDay.ItemsSource = Enumerable.Range(1, 31).Select(x => (x - 1) + 1);
            this.cbDay.SelectedValue = DateTime.Now.Day;
            this.cbDay.IsEnabledContextMenu = false;
            this.cbMonth.ItemsSource = Enumerable.Range(1, 12).Select(x => (x - 1) + 1);
            this.cbMonth.SelectedValue = DateTime.Now.Month;
            this.cbMonth.IsEnabledContextMenu = false;
            this.cbYear.ItemsSource = Enumerable.Range(1900, 200).Select(x => (x - 1) + 1);
            this.cbYear.SelectedValue = DateTime.Now.Year;
            this.cbYear.IsEnabledContextMenu = false;
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

        private static void ShowTodayButtonChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBoxDate)
            {
                var d = (TextBoxDate)sender;
                bool showButton = (bool)(e.NewValue);

                if (showButton == true)
                {
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
