namespace ModernIU.Controls
{
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernIU.Base;

    /// <summary>
    /// Interaktionslogik für ListStringUpDown.xaml
    /// </summary>
    public partial class ListStringUpDown : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ListStringUpDown), new PropertyMetadata(null, OnItemsSourceChanged));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(ListStringUpDown), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnValuePropertyChanged)));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(ListStringUpDown), new PropertyMetadata(true, OnSetBorderChanged));
        public static readonly DependencyProperty WidthContentProperty = DependencyProperty.Register("WidthContent", typeof(double), typeof(ListStringUpDown), new PropertyMetadata(100.0, OnWidthContentPropertyChanged));

        private ICollectionView ItemSource { get; set; }

        public ListStringUpDown()
        {
            this.InitializeComponent();
            this.TxTBoxStringUpDown.BorderBrush = Brushes.Green;
            this.TxTBoxStringUpDown.VerticalContentAlignment = VerticalAlignment.Center;
            this.TxTBoxStringUpDown.VerticalAlignment = VerticalAlignment.Center;
            this.TxTBoxStringUpDown.VerticalContentAlignment = VerticalAlignment.Center;
            this.TxTBoxStringUpDown.FontSize = ControlBase.FontSize;
            this.TxTBoxStringUpDown.FontFamily = ControlBase.FontFamily;
            this.TxTBoxStringUpDown.BorderBrush = ControlBase.BorderBrush;
            this.TxTBoxStringUpDown.Padding = new Thickness(0);
            this.TxTBoxStringUpDown.Margin = ControlBase.DefaultMargin;
            this.TxTBoxStringUpDown.IsReadOnly = false;
            this.TxTBoxStringUpDown.Focusable = true;

            /* Trigger an Style übergeben */
            this.TxTBoxStringUpDown.Style = this.SetTriggerFunction();

            /* Spezifisches Kontextmenü für Control übergeben */
            this.TxTBoxStringUpDown.ContextMenu = this.BuildContextMenu();

            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnUp, "Click", this.OnClickUp);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnDown, "Click", this.OnClickDown);
            WeakEventManager<TextBox, KeyEventArgs>.AddHandler(this.TxTBoxStringUpDown, "PreviewKeyDown", this.OnPreviewKeyDown);
            WeakEventManager<TextBox, TextChangedEventArgs>.AddHandler(this.TxTBoxStringUpDown, "TextChanged", this.OnTextChanged);
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set {SetValue(ValueProperty, value); }
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
                ListStringUpDown control = d as ListStringUpDown;
                if (control != null)
                {
                    control.ItemSource = CollectionViewSource.GetDefaultView(e.NewValue);
                    control.ItemSource.MoveCurrentToFirst();
                    control.TxTBoxStringUpDown.Text = control.ItemSource.CurrentItem.ToString();
                }
            }
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ListStringUpDown control = d as ListStringUpDown;
                control.TxTBoxStringUpDown.Text = e.NewValue.ToString();
            }
        }

        private static void OnWidthContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ListStringUpDown control = d as ListStringUpDown;
                double resultValue = 0;
                if (double.TryParse(e.NewValue.ToString(), out resultValue) == true)
                {
                    control.TxTBoxStringUpDown.Width = resultValue;
                }
                else
                {
                    control.TxTBoxStringUpDown.Width = 100;
                }
            }
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (ListStringUpDown)d;

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

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb != null && this.ItemSource != null)
            {
                tb.SelectAll();

                var found = this.ItemSource.Cast<string>().ToList().FindAll(f => f.Equals(tb.Text));
                if (found != null && found.Count > 0)
                {
                    tb.Text = found[0].ToString();
                    this.Value = found[0].ToString();
                }
            }
        }

        private void ResetText(TextBox tb)
        {
            this.ItemSource.MoveCurrentToFirst();
            tb.Text = this.ItemSource.SourceCollection.Cast<string>().First();
            tb.SelectAll();
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

        private void OnClickUp(object sender, RoutedEventArgs e)
        {
            this.ItemSource.MoveCurrentToPrevious();
            if (this.ItemSource.IsCurrentBeforeFirst == false)
            {
                this.TxTBoxStringUpDown.Text = this.ItemSource.CurrentItem.ToString();
            }
        }

        private void OnClickDown(object sender, RoutedEventArgs e)
        {
            this.ItemSource.MoveCurrentToNext();
            if (this.ItemSource.IsCurrentAfterLast == false)
            {
                this.TxTBoxStringUpDown.Text = this.ItemSource.CurrentItem.ToString();
            }
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
            Clipboard.SetText(this.TxTBoxStringUpDown.Text);
        }
    }
}
