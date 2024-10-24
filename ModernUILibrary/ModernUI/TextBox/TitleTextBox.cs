namespace ModernIU.Controls
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using ModernIU.Base;
    using static ModernIU.Controls.PopupEx;

    public enum TitleOrientationEnum
    {
        Horizontal,
        Vertical,
    }

    /// <summary>
    /// TextBox UserControl
    /// </summary>
    /// <remarks>
    /// https://www.shujaat.net/2010/08/spell-checking-for-wpf-text-entry.html
    /// </remarks>
    [TemplatePart(Name = "PART_ClearBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_ClearText", Type = typeof(Path))]
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_Counter", Type = typeof(TextBlock))]
    public class TitleTextBox : TextBox
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title" , typeof(string), typeof(TitleTextBox));
        public static readonly DependencyProperty IsShowTitleProperty = DependencyProperty.Register("IsShowTitle", typeof(bool), typeof(TitleTextBox), new PropertyMetadata(true));
        public static readonly DependencyProperty IsShowCounterProperty = DependencyProperty.Register("IsShowCounter", typeof(bool), typeof(TitleTextBox), new PropertyMetadata(true));
        public static readonly DependencyProperty CanClearTextProperty = DependencyProperty.Register("CanClearText", typeof(bool), typeof(TitleTextBox));
        public static readonly DependencyProperty CanSpellCheckProperty = DependencyProperty.Register("CanSpellCheck", typeof(bool), typeof(TitleTextBox), new PropertyMetadata(false, OnCanSpellCheckChanged));
        public static readonly DependencyProperty TitleOrientationProperty = DependencyProperty.Register("TitleOrientation", typeof(TitleOrientationEnum), typeof(TitleTextBox));
        public static readonly DependencyProperty ReadOnlyColorProperty = DependencyProperty.Register("ReadOnlyColor", typeof(Brush), typeof(TitleTextBox), new PropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(TitleTextBox), new PropertyMetadata(true, OnSetBorderChanged));

        private Path PART_ClearText;
        private ScrollViewer PART_ScrollViewer;
        private TextBlock PART_TextBlock;

        static TitleTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleTextBox), new FrameworkPropertyMetadata(typeof(TitleTextBox)));
        }

        public TitleTextBox()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.Margin = ControlBase.DefaultMargin;
            this.Height = ControlBase.DefaultHeight;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Top;
            this.MinHeight = 23;
            this.IsReadOnly = false;
            this.Focusable = true;
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public bool IsShowTitle
        {
            get { return (bool)GetValue(IsShowTitleProperty); }
            set { SetValue(IsShowTitleProperty, value); }
        }

        public bool IsShowCounter
        {
            get { return (bool)GetValue(IsShowCounterProperty); }
            set { SetValue(IsShowCounterProperty, value); }
        }

        public bool CanClearText
        {
            get { return (bool)GetValue(CanClearTextProperty); }
            set { SetValue(CanClearTextProperty, value); }
        }

        public bool CanSpellCheck
        {
            get { return (bool)GetValue(CanSpellCheckProperty); }
            set { SetValue(CanSpellCheckProperty, value); }
        }

        public TitleOrientationEnum TitleOrientation
        {
            get { return (TitleOrientationEnum)GetValue(TitleOrientationProperty); }
            set { SetValue(TitleOrientationProperty, value); }
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.MaxLength == 0)
            {
                this.MaxLength = 1000;
            }

            this.PART_ClearText = VisualHelper.FindVisualElement<Path>(this, "PART_ClearText");
            if(this.PART_ClearText != null)
            {
                if (this.IsReadOnly == false)
                {
                    this.PART_ClearText.MouseLeftButtonDown += PART_ClearText_MouseLeftButtonDown;
                    this.PART_ClearText.IsEnabled = true;
                    this.PART_ClearText.Visibility = Visibility.Visible;
                }
                else
                {
                    this.PART_ClearText.Visibility = Visibility.Collapsed;
                    this.PART_ClearText.IsEnabled = false;
                }
            }

            this.PART_ScrollViewer = VisualHelper.FindVisualElement<ScrollViewer>(this, "PART_ContentHost");
            this.PART_TextBlock = VisualHelper.FindVisualElement<TextBlock>(this, "PART_Counter");
            if (this.PART_TextBlock != null)
            {
                this.PART_TextBlock.Text = $"Restzeichen: {this.MaxLength}";
                this.PART_TextBlock.Foreground = Brushes.Green;
            }

            this.PreviewMouseWheel += TitleTextBox_PreviewMouseWheel;
            this.TextChanged += TitleTextBox_TextChanged;

            /* Rahmen für Control festlegen */
            if (this.SetBorder == true)
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

            if (this.CanSpellCheck == false)
            {
                /* Spezifisches Kontextmenü für Control übergeben */
                this.ContextMenu = this.BuildContextMenu();
                this.SpellCheck.IsEnabled = false;
            }
            else
            {
                this.SpellCheck.IsEnabled = true;
                this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
            }
        }

        private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.PART_TextBlock != null)
            {
                double minRest = this.MaxLength * 0.1D;
                int restCount = this.MaxLength - this.Text.Length;

                this.PART_TextBlock.Text = $"Restzeichen: {restCount}";
                if (restCount > (this.MaxLength - minRest))
                {
                    this.PART_TextBlock.Foreground = Brushes.Green;
                }
                else if (restCount < minRest)
                {
                    this.PART_TextBlock.Foreground = Brushes.Red;
                }
            }
        }

        private void TitleTextBox_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if(this.TitleOrientation == TitleOrientationEnum.Vertical && this.PART_ScrollViewer != null)
            {
                this.PART_ScrollViewer.ScrollToVerticalOffset(this.PART_ScrollViewer.VerticalOffset - e.Delta);
            }
        }

        private void PART_ClearText_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Text = string.Empty;
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

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                TitleTextBox control = (TitleTextBox)d;

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

        private static void OnCanSpellCheckChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                TitleTextBox control = (TitleTextBox)d;

                if (e.NewValue.GetType() == typeof(bool))
                {
                    if ((bool)e.NewValue == true)
                    {
                        control.SpellCheck.IsEnabled = true;
                        control.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
                    }
                    else
                    {
                        control.SpellCheck.IsEnabled = false;
                        /* Spezifisches Kontextmenü für Control übergeben */
                        control.ContextMenu = control.BuildContextMenu();
                    }
                }
            }
        }
    }
}
