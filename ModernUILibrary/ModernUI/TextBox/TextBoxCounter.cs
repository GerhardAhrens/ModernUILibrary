namespace ModernIU.Controls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernIU.Base;

    public class TextBoxCounter : RichTextBox, INotifyPropertyChanged
    {
        public static readonly DependencyProperty CharactersCounterProperty;
        public static readonly DependencyProperty CounterTextProperty;
        public static readonly DependencyProperty CounterFontSizeProperty;
        public static readonly DependencyProperty MaxCharactersAllowedProperty;
        public static readonly DependencyProperty NotiftyLimitProperty;
        public static readonly DependencyProperty NotificationStyleNameProperty;
        public static readonly DependencyProperty NotificationStyleProperty;
        public static readonly DependencyProperty DefaultNotificationStyleNameProperty;
        public static readonly DependencyProperty IsValidProperty;
        public static readonly DependencyProperty SetBorderProperty;
        public static readonly DependencyProperty ReadOnlyColorProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        static TextBoxCounter()
        {
            CharactersCounterProperty = DependencyProperty.Register(nameof(CharactersCounter), typeof(int), typeof(TextBoxCounter));
            CounterTextProperty = DependencyProperty.Register(nameof(CounterText), typeof(string), typeof(TextBoxCounter), new PropertyMetadata("Restzeichen:", RemainingTextChanged));
            CounterFontSizeProperty = DependencyProperty.Register(nameof(CounterFontSize), typeof(double), typeof(TextBoxCounter), new PropertyMetadata(18d, RemainingFontSizeChanged));
            MaxCharactersAllowedProperty = DependencyProperty.Register("MaxCharactersAllowed", typeof(int),
                                                                       typeof(TextBoxCounter), new PropertyMetadata(0, MaxCharactersAllowedPropertyChanged));

            NotiftyLimitProperty = DependencyProperty.Register("NotifyLimit", typeof(int), typeof(TextBoxCounter));
            NotificationStyleNameProperty = DependencyProperty.Register("NotificationStyleName", typeof(String), typeof(TextBoxCounter));
            NotificationStyleProperty = DependencyProperty.Register("NotificationStyle", typeof(Style), typeof(TextBoxCounter));
            DefaultNotificationStyleNameProperty = DependencyProperty.Register("DefaultNotificationStyleName",
                                                                               typeof(String),
                                                                               typeof(TextBoxCounter), new PropertyMetadata(null, DefaultNotificationStyleNamePropertyChanged));

            IsValidProperty = DependencyProperty.Register("IsValid", typeof(bool), typeof(TextBoxCounter));
            SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxCounter), new PropertyMetadata(true, OnSetBorderChanged));
            ReadOnlyColorProperty = DependencyProperty.Register("ReadOnlyColor", typeof(Brush), typeof(TextBoxCounter), new PropertyMetadata(Brushes.LightYellow));
    }

    public TextBoxCounter()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(2);
            this.BorderBrush = Brushes.Green;
            this.BorderThickness = new Thickness(1);
            this.Background = Brushes.Transparent;
            this.MinHeight = 18;
            this.Height = 23;
            this.IsReadOnly = false;
            this.Focusable = true;

            this.PreviewKeyUp += new System.Windows.Input.KeyEventHandler(SharpRichTextBox_PreviewKeyUp);
            this.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(SharpRichTextBox_PreviewKeyDown);

            // need to handle the paste command 
            var binding = new CommandBinding();
            binding.Command = ApplicationCommands.Paste;
            binding.PreviewCanExecute += binding_PreviewCanExecute;
            binding.PreviewExecuted += binding_PreviewExecuted;

            this.CommandBindings.Add(binding);

            this.SetUpBindingForBackground();
        }

        /// <summary>
        /// Used to indicate if the RichTextBox is in valid state
        /// </summary>
        public bool IsValid
        {
            get { return CharactersCounter <= MaxCharactersAllowed; }

        }

        /// <summary>
        /// The default style of the control used to display the counter value
        /// </summary>
        public string DefaultNotificationStyleName
        {
            get { return (String)GetValue(DefaultNotificationStyleNameProperty); }
            set { SetValue(DefaultNotificationStyleNameProperty, value); }
        }

        /// <summary>
        /// style used to notify the user about few characters remaining
        /// </summary>
        public Style NotificationStyle
        {
            get { return (Style)GetValue(NotificationStyleProperty); }
            set
            {
                SetValue(NotificationStyleProperty, value);
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// the name of the style used to notify the user that few characters are renaming 
        /// </summary>
        public String NotificationStyleName
        {
            get { return (String)GetValue(NotificationStyleNameProperty); }
            set { SetValue(NotificationStyleNameProperty, value); }
        }

        /// <summary>
        /// this is used to indicate that after how many remaining characters apply the notificationstyle
        /// </summary>
        public int NotifyLimit
        {
            get { return (int)GetValue(NotiftyLimitProperty); }
            set { SetValue(NotiftyLimitProperty, value); }
        }

        /// <summary>
        /// no of characters allowed in the textbox 
        /// </summary>
        public int MaxCharactersAllowed
        {
            get { return (int)GetValue(MaxCharactersAllowedProperty); }
            set { SetValue(MaxCharactersAllowedProperty, value); }
        }

        /// <summary>
        /// number of characters that can be typed by the user in the textbox
        /// </summary>
        public int CharactersCounter
        {
            get
            {
                return (int)GetValue(CharactersCounterProperty);
            }

            set
            {
                SetValue(CharactersCounterProperty, value);
                this.OnPropertyChanged();
            }
        }

        public string CounterText
        {
            get
            {
                return (string)GetValue(CounterTextProperty);
            }

            set
            {
                SetValue(CounterTextProperty, value);
                this.OnPropertyChanged();
            }
        }

        public double CounterFontSize
        {
            get
            {
                return (double)GetValue(CounterFontSizeProperty);
            }

            set
            {
                SetValue(CounterFontSizeProperty, value);
                this.OnPropertyChanged();
            }
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
            this.SelectAll();

            /* Rahmen für Control festlegen */
            if (this.SetBorder == true)
            {
                this.BorderBrush = Brushes.Green;
                this.BorderThickness = new Thickness(1);
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
        /// Fired when the style name property is assigned from xaml 
        /// </summary>
        /// <param name="d">Dependency Object</param>
        /// <param name="e">Event Args</param>
        private static void DefaultNotificationStyleNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sharpRichTextBox = d as TextBoxCounter;

            if (sharpRichTextBox == null)
            {
                return;
            }

            sharpRichTextBox.NotificationStyle = (Style)sharpRichTextBox.FindResource(sharpRichTextBox.DefaultNotificationStyleName);
        }

        /// <summary>
        /// Fired when the max characters property is changed from the xaml view 
        /// </summary>
        /// <param name="d">Dependency object</param>
        /// <param name="e">event args</param>
        private static void MaxCharactersAllowedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxCounter sharpRichTextBox = d as TextBoxCounter;

            if (sharpRichTextBox == null)
            {
                return;
            }

            sharpRichTextBox.CharactersCounter = sharpRichTextBox.MaxCharactersAllowed;
        }

        private static void RemainingTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxCounter sharpRichTextBox = d as TextBoxCounter;

            if (sharpRichTextBox == null)
            {
                return;
            }

            if (e.NewValue != null)
            {
                sharpRichTextBox.CounterText = (string)e.NewValue;
                sharpRichTextBox.SetUpBindingForBackground();
            }
        }

        private static void RemainingFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxCounter sharpRichTextBox = d as TextBoxCounter;

            if (sharpRichTextBox == null)
            {
                return;
            }

            if (e.NewValue != null)
            {
                sharpRichTextBox.CounterFontSize = (double)e.NewValue;
                sharpRichTextBox.SetUpBindingForBackground();
            }
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                TextBoxCounter control = (TextBoxCounter)d;

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

        /// <summary>
        /// This method is used to setup the watermark background for the RichTextBox 
        /// </summary>
        private void SetUpBindingForBackground()
        {
            var visualBrush = new VisualBrush();
            var stackPanel = new StackPanel();

            var textBlockDesc = new TextBlock()
            {
                Text = this.CounterText,
                Opacity = 0.4,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 35,
                FontSize = this.CounterFontSize,
                Margin = new Thickness(0,0,5,2),
            };

            var textBlock = new TextBlock()
            {
                Text = this.CharactersCounter.ToString(),
                Opacity = 0.3,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 35,
                Width = 120,
                FontSize = this.CounterFontSize,
                Margin = new Thickness(0, 0, 2, 2)
            };

            var styleBinding = new System.Windows.Data.Binding("NotificationStyle");
            styleBinding.Source = this;

            var binding = new System.Windows.Data.Binding(nameof(CharactersCounter));
            binding.Source = this;

            textBlock.SetBinding(TextBlock.TextProperty, binding);
            textBlockDesc.SetBinding(TextBlock.StyleProperty, styleBinding);
            textBlock.SetBinding(TextBlock.StyleProperty, styleBinding);

            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Children.Add(textBlockDesc);
            stackPanel.Children.Add(textBlock);

            visualBrush.Visual = stackPanel;
            visualBrush.AlignmentX = AlignmentX.Right;
            visualBrush.AlignmentY = AlignmentY.Bottom;
            visualBrush.Stretch = Stretch.None;

            this.Background = visualBrush;
        }

        /// <summary>
        /// Make sure that the pasted text is correct
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void binding_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!System.Windows.Clipboard.ContainsText()) return;

            var clipboardText = System.Windows.Clipboard.GetText();

            if (clipboardText.Length <= CharactersCounter)
            {
                this.AppendText(clipboardText);
            }
            else
            {
                this.AppendText(clipboardText.Substring(0, CharactersCounter));
            }

            e.Handled = true;

            this.UpdateRemainingCharactersDisplay();
        }

        /// <summary>
        /// Make sure that the user can paste text. If limit has reached then this operation is cancelled. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void binding_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (GetNumberOfCharactersRemaining() <= 0)
            {
                e.CanExecute = false;
                e.Handled = true;
                return;
            }

            e.CanExecute = true;
            e.Handled = false;
        }

        private void SharpRichTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.IsArrowKey() || e.Key.IsBackKey() || e.Key.IsDeleteKey())
            {
                e.Handled = false;
                return;
            }

            if (this.GetNumberOfCharactersRemaining() <= 0)
            {
                e.Handled = true;
            }
        }

        private void SharpRichTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            this.UpdateRemainingCharactersDisplay();
            e.Handled = true;
        }

        /// <summary>
        /// gets the number of characters that can be typed. 
        /// </summary>
        /// <returns></returns>
        private int GetNumberOfCharactersRemaining()
        {
            return MaxCharactersAllowed - this.GetTextLengthWithoutNewLine();
        }

        /// <summary>
        /// get the length of the characters without the line
        /// </summary>
        /// <returns></returns>
        private int GetTextLengthWithoutNewLine()
        {
            var textRange = new TextRange(Document.ContentStart, Document.ContentEnd);
            var text = textRange.Text.Replace("\r\n", String.Empty);
            return text.Length;
        }

        /// <summary>
        /// update the remaining character display
        /// </summary>
        private void UpdateRemainingCharactersDisplay()
        {
            var noOfCharactersEntered = GetTextLengthWithoutNewLine();

            var noOfCharactersRemaining = GetNumberOfCharactersRemaining();

            if (noOfCharactersRemaining < 0)
            {
                return;
            }

            if ((MaxCharactersAllowed - noOfCharactersEntered) <= NotifyLimit)
            {
                NotificationStyle = this.FindResource(NotificationStyleName) as Style;
            }
            else
            {
                this.NotificationStyle = this.FindResource(DefaultNotificationStyleName) as Style;
            }

            this.CharactersCounter = this.MaxCharactersAllowed - noOfCharactersEntered;
        }

        protected void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private Style SetTriggerFunction()
        {
            Style inputControlStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = RichTextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = RichTextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = RichTextBox.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = RichTextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsFocused);

            /* Trigger für IsFocused = True */
            Trigger triggerIsReadOnly = new Trigger();
            triggerIsReadOnly.Property = RichTextBox.IsReadOnlyProperty;
            triggerIsReadOnly.Value = true;
            triggerIsReadOnly.Setters.Add(new Setter() { Property = RichTextBox.BackgroundProperty, Value = Brushes.LightYellow });
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
            this.Copy();
        }

        private void OnPasteMenu(object sender, RoutedEventArgs e)
        {
            this.Paste();
            var textRange = new TextRange(Document.ContentStart, Document.ContentEnd);
            this.CharactersCounter = this.MaxCharactersAllowed - textRange.Text.Replace("\r\n",string.Empty).Length;
        }

        private void OnDeleteMenu(object sender, RoutedEventArgs e)
        {            
            this.SelectAll();
            this.Copy();
            this.Selection.Text = string.Empty;
            this.CharactersCounter = this.MaxCharactersAllowed;
        }

        private void OnSetDateMenu(object sender, RoutedEventArgs e)
        {
            this.InsertText(DateTime.Now.ToShortDateString());
        }

        private void InsertText(string text)
        {
            this.CaretPosition = this.CaretPosition.GetPositionAtOffset(0, LogicalDirection.Forward);
            this.CaretPosition.InsertTextInRun(text);
        }
    }
}
