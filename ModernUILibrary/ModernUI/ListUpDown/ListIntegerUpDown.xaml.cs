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
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(ListIntegerUpDown), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnValuePropertyChanged)));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(ListIntegerUpDown), new PropertyMetadata(true, OnSetBorderChanged));
        public static readonly DependencyProperty WidthContentProperty = DependencyProperty.Register("WidthContent", typeof(double), typeof(ListIntegerUpDown), new PropertyMetadata(100.0, OnWidthContentPropertyChanged));

        private ICollectionView ItemSource { get; set; }
        private readonly Regex _numMatch;
        private int minimum = -1;
        private int maximum = -1;

        public ListIntegerUpDown()
        {
            this.InitializeComponent();

            _numMatch = new Regex(@"^-?\d+$");
            this.Focusable = true;

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
            WeakEventManager<TextBox, RoutedEventArgs>.AddHandler(this.TxtIntegerUpDown, "LostFocus", this.OnLostFocus);
        }


        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public string Value
        {
            get { return (string)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
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

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = d as ListIntegerUpDown;
                if (control != null)
                {
                    control.ItemSource = CollectionViewSource.GetDefaultView(e.NewValue);
                    control.ItemSource.MoveCurrentToFirst();
                    control.minimum = Convert.ToInt32(control.ItemSource.MoveCurrentToFirst());
                    control.maximum = Convert.ToInt32(control.ItemSource.MoveCurrentToLast());
                }
            }
        }

        private static void OnValuePropertyChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ListIntegerUpDown control = target as ListIntegerUpDown;
                control.TxtIntegerUpDown.Text = e.NewValue.ToString();
            }
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

        private void OnClickUp(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Value) == false)
            {
                this.ItemSource.MoveCurrentTo(Convert.ToInt32(this.Value));
            }

            this.ItemSource.MoveCurrentToPrevious();
            if (this.ItemSource.IsCurrentBeforeFirst == false)
            {
                this.TxtIntegerUpDown.Text = this.ItemSource.CurrentItem.ToString();
                this.Value = this.ItemSource.CurrentItem.ToString();
            }
        }

        private void OnClickDown(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Value) == false)
            {
                this.ItemSource.MoveCurrentTo(Convert.ToInt32(this.Value));
            }

            this.ItemSource.MoveCurrentToNext();
            if (this.ItemSource.IsCurrentAfterLast == false)
            {
                this.TxtIntegerUpDown.Text = this.ItemSource.CurrentItem.ToString();
                this.Value = this.ItemSource.CurrentItem.ToString();
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = (TextBox)sender;
            var text = tb.Text.Insert(tb.CaretIndex, e.Text);

            e.Handled = !this._numMatch.IsMatch(text);

            if (this.ItemSource.IsCurrentAfterLast == true || this.ItemSource.IsCurrentBeforeFirst == true)
            {
                e.Handled = false;
            }

        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb != null)
            {
                if (this._numMatch.IsMatch(tb.Text) == false)
                {
                    this.ResetText(tb);
                }

                if (this.ItemSource != null)
                {
                    var found = this.ItemSource.Cast<int>().ToList().FindAll(f => f.Equals(Convert.ToInt32(tb.Text)));
                    if (found != null && found.Count > 0)
                    {
                        tb.Text = found[0].ToString();
                        this.Value = found[0].ToString();
                    }
                }
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb != null)
            {
                this.ItemSource.MoveCurrentToFirst();
                tb.Text = this.ItemSource.CurrentItem.ToString();
                this.Value = this.ItemSource.CurrentItem.ToString();
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
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
            tb.Text = 0 < this.minimum ? this.minimum.ToString() : "0";
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
