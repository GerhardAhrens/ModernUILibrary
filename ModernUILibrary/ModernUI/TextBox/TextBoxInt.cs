namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernIU.Base;

    public class TextBoxInt : TextBox
    {
        public static readonly DependencyProperty IsNegativeProperty = DependencyProperty.Register("IsNegative", typeof(bool), typeof(TextBoxInt), new PropertyMetadata(false));
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(int), typeof(TextBoxInt), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxInt), new PropertyMetadata(true, OnSetBorderChanged));

        public TextBoxInt()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.BorderBrush = Brushes.Green;
            this.HorizontalContentAlignment = HorizontalAlignment.Right;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 23;
            this.ClipToBounds = false;
            this.Focusable = true;

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();
        }

        public bool IsNegative
        {
            get { return (bool)GetValue(IsNegativeProperty); }
            set { SetValue(IsNegativeProperty, value); }
        }

        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public bool SetBorder
        {
            get { return (bool)GetValue(SetBorderProperty); }
            set { SetValue(SetBorderProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.CaretIndex = this.Text.Length;
            this.SelectAll();

            /* Spezifisches Kontextmenü für Control übergeben */
            this.ContextMenu = this.BuildContextMenu();

            /* Rahmen für Control festlegen */
            if (SetBorder == true)
            {
                this.BorderBrush = Brushes.Green;
                this.BorderThickness = new Thickness(1);
            }
            else
            {
                this.BorderBrush = Brushes.Transparent;
                this.BorderThickness = new Thickness(0);
            }
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            if (string.IsNullOrEmpty(this.Text) == true)
            {
                this.Text = string.Empty;
                this.Number = 0;
            }
            else
            {
                if (int.TryParse(this.Text, out int result))
                {
                    this.Number = Convert.ToInt32(string.IsNullOrEmpty(this.Text) == true ? "0" : this.Text);
                }
                else
                {
                    this.Text = string.Empty;
                    this.Number = 0;
                }
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            this.SelectAll();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            this.Number = Convert.ToInt32(string.IsNullOrEmpty(this.Text) == true ? "0" : this.Text);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (e.ClickCount == 2)
            {
                this.SelectAll();
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (this.IsNegative == true)
            {
                if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
                {
                    if (this.Text.Count(c => c == '-') >= 1)
                    {
                        e.Handled = true;
                    }
                    else
                    {
                        int cursorPos = ((TextBox)e.Source).CaretIndex;
                        if (cursorPos == 0)
                        {
                            e.Handled = false;
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }

                    return;
                }
            }

            int key = (int)e.Key;            
            e.Handled = !(key >= 34 && key <= 43 || key == 2 || key == 32 || key == 21 || key == 22 || key == 23 || key == 25 || key == 9);

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

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextBoxInt)d;

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
            copyMenu.Header = "Kopiere";
            copyMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopyMenu);
            textBoxContextMenu.Items.Add(copyMenu);

            if (this.IsReadOnly == false)
            {
                MenuItem pasteMenu = new MenuItem();
                pasteMenu.Header = "Einfügen";
                pasteMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconPaste);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(pasteMenu, "Click", this.OnPasteMenu);
                textBoxContextMenu.Items.Add(pasteMenu);

                MenuItem deleteMenu = new MenuItem();
                deleteMenu.Header = "Ausschneiden";
                deleteMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconDelete);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(deleteMenu, "Click", this.OnDeleteMenu);
                textBoxContextMenu.Items.Add(deleteMenu);
            }

            return textBoxContextMenu;
        }

        private void OnCopyMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.Text);
        }

        private void OnPasteMenu(object sender, RoutedEventArgs e)
        {
            this.Text = Clipboard.GetText();
        }

        private void OnDeleteMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.Text);
            this.Text = string.Empty;
        }
    }
}
