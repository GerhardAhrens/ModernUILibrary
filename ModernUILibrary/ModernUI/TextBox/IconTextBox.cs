namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernIU.Base;

    public class IconTextBox : IconTextBoxBase
    {
        public static readonly RoutedEvent EnterKeyClickEvent = EventManager.RegisterRoutedEvent("EnterKeyClick", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<object>), typeof(IconTextBox));
        public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register("IconPlacement", typeof(IconPlacementEnum), typeof(IconTextBox));
        public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register("IconColor", typeof(Brush), typeof(IconTextBox));
        public static readonly DependencyProperty ReadOnlyColorProperty = DependencyProperty.Register("ReadOnlyColor", typeof(Brush), typeof(IconTextBox), new PropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(IconTextBox), new PropertyMetadata(true, OnSetBorderChanged));

        public enum IconPlacementEnum
        {
            Left,
            Right,
        }

        static IconTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconTextBox), new FrameworkPropertyMetadata(typeof(IconTextBox)));
        }

        public IconTextBox() : base()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.Margin = ControlBase.DefaultMargin;
            this.Height = ControlBase.DefaultHeight;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.IsReadOnly = false;
            this.Focusable = true;

            this.KeyUp += IconTextBox_KeyUp;
        }

        ~IconTextBox()
        {
            this.KeyUp -= IconTextBox_KeyUp;
        }

        public event RoutedPropertyChangedEventHandler<object> EnterKeyClick
        {
            add
            {
                this.AddHandler(EnterKeyClickEvent, value);
            }
            remove
            {
                this.RemoveHandler(EnterKeyClickEvent, value);
            }
        }

        public IconPlacementEnum IconPlacement
        {
            get { return (IconPlacementEnum)GetValue(IconPlacementProperty); }
            set { SetValue(IconPlacementProperty, value); }
        }

        public Brush IconColor
        {
            get { return (Brush)GetValue(IconColorProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public bool SetBorder
        {
            get { return (bool)GetValue(SetBorderProperty); }
            set { SetValue(SetBorderProperty, value); }
        }

        public Brush ReadOnlyColor
        {
            get { return (Brush)GetValue(ReadOnlyColorProperty); }
            set { SetValue(ReadOnlyColorProperty, value); }
        }

        protected virtual void OnEnterKeyClick(Key keyValue, object newValue)
        {
            RoutedPropertyChangedEventArgs<object> arg = new RoutedPropertyChangedEventArgs<object>(keyValue, newValue, EnterKeyClickEvent);
            this.RaiseEvent(arg);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            /* Rahmen für Control festlegen */
            if (SetBorder == true)
            {
                this.BorderBrush = ControlBase.BorderBrush;
                this.BorderThickness = ControlBase.BorderThickness;
            }
            else
            {
                this.BorderBrush = Brushes.Transparent;
                this.BorderThickness = new Thickness(0);
            }

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();

            /* Spezifisches Kontextmenü für Control übergeben */
            this.ContextMenu = this.BuildContextMenu();
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

                MenuItem setDateMenu = new MenuItem();
                setDateMenu.Header = "Setze Datum";
                setDateMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconClock);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(setDateMenu, "Click", this.OnSetDateMenu);
                textBoxContextMenu.Items.Add(setDateMenu);
            }

            return textBoxContextMenu;
        }

        private void OnCopyMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.Text);
        }

        private void OnPasteMenu(object sender, RoutedEventArgs e)
        {
            this.InsertText(Clipboard.GetText());
        }

        private void OnDeleteMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.Text);
            this.Text = string.Empty;
        }

        private void OnSetDateMenu(object sender, RoutedEventArgs e)
        {
            this.InsertText(DateTime.Now.ToShortDateString());
        }

        private void IconTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                this.OnEnterKeyClick(e.Key, null);
            }
            else if (e.Key == Key.Tab)
            {
                this.OnEnterKeyClick(e.Key, null);
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

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TitleTextBox)d;

                if (e.NewValue.GetType() == typeof(bool))
                {
                    if ((bool)e.NewValue == true)
                    {
                        control.BorderBrush = ControlBase.BorderBrush;
                        control.BorderThickness = ControlBase.BorderThickness;
                    }
                    else
                    {
                        control.BorderBrush = Brushes.Transparent;
                        control.BorderThickness = new Thickness(0);
                    }
                }
            }
        }

        private void InsertText(string value)
        {
            // maxLength of insertedValue
            var valueLength = this.MaxLength > 0 ? (this.MaxLength - this.Text.Length + this.SelectionLength) : value.Length;
            if (valueLength <= 0)
            {
                // the value length is 0 - no need to insert anything
                return;
            }

            // save the caretIndex and create trimmed text
            var index = this.CaretIndex;
            var trimmedValue = value.Length > valueLength ? value.Substring(0, valueLength) : value;

            // if some text is selected, replace this text
            if (this.SelectionLength > 0)
            {
                index = this.SelectionStart;
                this.SelectedText = trimmedValue;
            }
            // insert the text to caret index position
            else
            {
                var text = this.Text.Substring(0, index) + trimmedValue + this.Text.Substring(index);
                this.Text = text;
            }

            // move caret to the end of inserted text
            this.CaretIndex = index + valueLength;
        }
    }
}
