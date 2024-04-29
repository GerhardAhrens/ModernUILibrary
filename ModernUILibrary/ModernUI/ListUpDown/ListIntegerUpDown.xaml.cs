namespace ModernIU.Controls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernIU.Base;

    /// <summary>
    /// Interaktionslogik für ListIntegerUpDown.xaml
    /// </summary>
    public partial class ListIntegerUpDown : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ListIntegerUpDown), new PropertyMetadata(null, OnItemsSourceChanged));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(ListIntegerUpDown), new PropertyMetadata(0, new PropertyChangedCallback(OnValuePropertyChanged)));
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(int), typeof(ListIntegerUpDown), new UIPropertyMetadata(100));
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(int), typeof(ListIntegerUpDown), new UIPropertyMetadata(0));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(ListIntegerUpDown), new PropertyMetadata(true, OnSetBorderChanged));
        public static readonly DependencyProperty WidthContentProperty = DependencyProperty.Register("WidthContent", typeof(double), typeof(ListIntegerUpDown), new PropertyMetadata(100.0, OnWidthContentPropertyChanged));
        private static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListIntegerUpDown));
        private static readonly RoutedEvent IncreaseClickedEvent = EventManager.RegisterRoutedEvent("IncreaseClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListIntegerUpDown));
        private static readonly RoutedEvent DecreaseClickedEvent = EventManager.RegisterRoutedEvent("DecreaseClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ListIntegerUpDown));

        private ICollectionView ItemSource { get; set; }
        private readonly Regex _numMatch;

        public ListIntegerUpDown()
        {
            this.InitializeComponent();

            _numMatch = new Regex(@"^-?\d+$");
            this.Focusable = true;

            this.Maximum = int.MaxValue;
            this.Minimum = 0;

            this.TxtIntegerUpDown.Text = "0";
            this.TxtIntegerUpDown.HorizontalContentAlignment = HorizontalAlignment.Right;
            this.TxtIntegerUpDown.VerticalAlignment = VerticalAlignment.Center;
            this.TxtIntegerUpDown.VerticalContentAlignment = VerticalAlignment.Center;
            this.TxtIntegerUpDown.FontSize = ControlBase.FontSize;
            this.TxtIntegerUpDown.FontFamily = ControlBase.FontFamily;
            this.TxtIntegerUpDown.BorderBrush = ControlBase.BorderBrush;
            this.TxtIntegerUpDown.Margin = ControlBase.DefaultMargin;
            this.TxtIntegerUpDown.Padding = new Thickness(0);
            this.TxtIntegerUpDown.IsReadOnly = false;
            this.TxtIntegerUpDown.Focusable = true;

            /* Trigger an Style übergeben */
            this.TxtIntegerUpDown.Style = this.SetTriggerFunction();

            /* Spezifisches Kontextmenü für Control übergeben */
            this.TxtIntegerUpDown.ContextMenu = this.BuildContextMenu();

            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnUp, "Click", this.OnClickUp);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnDown, "Click", this.OnClickDown);
            WeakEventManager<TextBox, TextCompositionEventArgs>.AddHandler(this.TxtIntegerUpDown, "PreviewTextInput", this.OnPreviewTextInput);
            WeakEventManager<TextBox, TextChangedEventArgs>.AddHandler(this.TxtIntegerUpDown, "TextChanged", this.OnTextChanged);
            WeakEventManager<TextBox, KeyEventArgs>.AddHandler(this.TxtIntegerUpDown, "PreviewKeyDown", this.OnPreviewKeyDown);
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public int Value
        {
            get
            {
                return (int)this.GetValue(ValueProperty);
            }
            set
            {
                this.TxtIntegerUpDown.Text = value.ToString();
                this.SetValue(ValueProperty, value);
            }
        }

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public bool SetBorder
        {
            get { return (bool)GetValue(SetBorderProperty); }
            set { SetValue(SetBorderProperty, value); }
        }

        public double WidthContent
        {
            get { return (double)GetValue(WidthContentProperty); }
            set { SetValue(WidthContentProperty, value); }
        }

        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        public event RoutedEventHandler IncreaseClicked
        {
            add { AddHandler(IncreaseClickedEvent, value); }
            remove { RemoveHandler(IncreaseClickedEvent, value); }
        }

        public event RoutedEventHandler DecreaseClicked
        {
            add { AddHandler(DecreaseClickedEvent, value); }
            remove { RemoveHandler(DecreaseClickedEvent, value); }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = d as ListIntegerUpDown;
                if (control != null)
                {
                    control.ItemSource = CollectionViewSource.GetDefaultView(e.NewValue);
                    control.ItemSource.MoveCurrentToFirst();
                }
            }
        }

        private static void OnValuePropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ListIntegerUpDown control = target as ListIntegerUpDown;
            control.TxtIntegerUpDown.Text = e.NewValue.ToString();
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (ListIntegerUpDown)d;

                if (e.NewValue.GetType() == typeof(bool))
                {
                    if ((bool)e.NewValue == true)
                    {
                        control.BorderBrush = Brushes.Green;
                        control.BorderThickness = new Thickness(1);
                    }
                    else
                    {
                        control.BorderBrush = Brushes.Transparent;
                        control.BorderThickness = new Thickness(0);
                    }
                }
            }
        }

        private static void OnWidthContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ListIntegerUpDown control = d as ListIntegerUpDown;
                double resultValue = 0;
                if (double.TryParse(e.NewValue.ToString(), out resultValue) == true)
                {
                    control.TxtIntegerUpDown.Width = resultValue;
                }
                else
                {
                    control.TxtIntegerUpDown.Width = 100;
                }
            }
        }

        private void OnClickDown(object sender, RoutedEventArgs e)
        {
            if (this.Value < this.Maximum)
            {
                this.Value++;
                RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
        }

        private void OnClickUp(object sender, RoutedEventArgs e)
        {
            if (this.Value > this.Minimum)
            {
                this.Value--;
                RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = (TextBox)sender;
            var text = tb.Text.Insert(tb.CaretIndex, e.Text);

            e.Handled = !_numMatch.IsMatch(text);

            if (this.Value > this.Maximum || this.Value < this.Minimum)
            {
                e.Handled = false;
            }

        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (!_numMatch.IsMatch(tb.Text))
            {
                ResetText(tb);
            }

            this.Value = Convert.ToInt32(tb.Text);
            if (this.Value < this.Minimum)
            {
                this.Value = this.Minimum;
            }

            if (this.Value > this.Maximum)
            {
                this.Value = this.Maximum;
            }

            RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown && e.Key == Key.Up && Value < Maximum)
            {
                this.Value++;
                RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
            else if (e.IsDown && e.Key == Key.Down && Value > Minimum)
            {
                this.Value--;
                RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));

            }

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
                        this.MoveFocus(FocusNavigationDirection.Previous);
                        break;
                    case Key.Down:
                        this.MoveFocus(FocusNavigationDirection.Next);
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
                        this.MoveFocus(FocusNavigationDirection.Next);
                        break;
                }
            }
        }

        private void ResetText(TextBox tb)
        {
            tb.Text = 0 < Minimum ? Minimum.ToString() : "0";
            tb.SelectAll();
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

        private Style SetTriggerFunction()
        {
            Style inputControlStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = TextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = TextBox.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsFocused);

            /* Trigger für IsFocused = True */
            Trigger triggerIsReadOnly = new Trigger();
            triggerIsReadOnly.Property = TextBox.IsReadOnlyProperty;
            triggerIsReadOnly.Value = true;
            triggerIsReadOnly.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightYellow });
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
            copyMenu.Header = "Kopiere Inhalt";
            copyMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopyMenu);
            textBoxContextMenu.Items.Add(copyMenu);

            return textBoxContextMenu;
        }

        private void OnCopyMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.TxtIntegerUpDown.Text);
        }
    }
}
