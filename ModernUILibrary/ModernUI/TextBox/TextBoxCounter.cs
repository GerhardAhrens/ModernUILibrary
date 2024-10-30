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
        public static DependencyProperty CharactersRemainingProperty;
        public static DependencyProperty RemainingTextProperty;
        public static DependencyProperty MaxCharactersAllowedProperty;
        public static DependencyProperty NotiftyLimitProperty;
        public static DependencyProperty NotificationStyleNameProperty;
        public static DependencyProperty NotificationStyleProperty;
        public static DependencyProperty DefaultNotificationStyleNameProperty;
        public static DependencyProperty IsValidProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        static TextBoxCounter()
        {
            CharactersRemainingProperty = DependencyProperty.Register("CharactersRemaining", typeof(int), typeof(TextBoxCounter));
            RemainingTextProperty = DependencyProperty.Register("RemainingText", typeof(string), typeof(TextBoxCounter), new PropertyMetadata("Restzeichen:", RemainingTextChanged));
            MaxCharactersAllowedProperty = DependencyProperty.Register("MaxCharactersAllowed", typeof(int),
                                                                       typeof(TextBoxCounter), new PropertyMetadata(0, MaxCharactersAllowedPropertyChanged));

            NotiftyLimitProperty = DependencyProperty.Register("NotifyLimit", typeof(int), typeof(TextBoxCounter));
            NotificationStyleNameProperty = DependencyProperty.Register("NotificationStyleName", typeof(String), typeof(TextBoxCounter));
            NotificationStyleProperty = DependencyProperty.Register("NotificationStyle", typeof(Style), typeof(TextBoxCounter));
            DefaultNotificationStyleNameProperty = DependencyProperty.Register("DefaultNotificationStyleName",
                                                                               typeof(String),
                                                                               typeof(TextBoxCounter), new PropertyMetadata(null, DefaultNotificationStyleNamePropertyChanged));

            IsValidProperty = DependencyProperty.Register("IsValid", typeof(bool), typeof(TextBoxCounter));
        }

        public TextBoxCounter()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 23;
            this.IsReadOnly = false;
            this.Focusable = true;

            PreviewKeyUp += new System.Windows.Input.KeyEventHandler(SharpRichTextBox_PreviewKeyUp);
            PreviewKeyDown += new System.Windows.Input.KeyEventHandler(SharpRichTextBox_PreviewKeyDown);

            // need to handle the paste command 
            var binding = new CommandBinding();
            binding.Command = ApplicationCommands.Paste;
            binding.PreviewCanExecute += binding_PreviewCanExecute;
            binding.PreviewExecuted += binding_PreviewExecuted;

            CommandBindings.Add(binding);

            this.SetUpBindingForBackground();
        }

        /// <summary>
        /// Used to indicate if the RichTextBox is in valid state
        /// </summary>
        public bool IsValid
        {
            get { return CharactersRemaining <= MaxCharactersAllowed; }

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
        public int CharactersRemaining
        {
            get
            {
                return (int)GetValue(CharactersRemainingProperty);
            }

            set
            {
                SetValue(CharactersRemainingProperty, value);
                this.OnPropertyChanged();
            }
        }

        public string RemainingText
        {
            get
            {
                return (string)GetValue(RemainingTextProperty);
            }

            set
            {
                SetValue(RemainingTextProperty, value);
                this.OnPropertyChanged();
            }
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

            sharpRichTextBox.CharactersRemaining = sharpRichTextBox.MaxCharactersAllowed;
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
                sharpRichTextBox.RemainingText = (string)e.NewValue;
                sharpRichTextBox.SetUpBindingForBackground();
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
                Text = this.RemainingText,
                Opacity = 0.3,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 35,
                FontSize = 22d,
                Margin = new Thickness(0,0,5,0),
            };

            var textBlock = new TextBlock()
            {
                Text = this.CharactersRemaining.ToString(),
                Opacity = 0.3,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 35,
                Width = 120,
                FontSize = 22d
            };

            var styleBinding = new System.Windows.Data.Binding("NotificationStyle");
            styleBinding.Source = this;

            var binding = new System.Windows.Data.Binding("CharactersRemaining");
            binding.Source = this;

            textBlock.SetBinding(TextBlock.TextProperty, binding);
            textBlockDesc.SetBinding(TextBlock.StyleProperty, styleBinding);
            textBlock.SetBinding(TextBlock.StyleProperty, styleBinding);

            stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
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

            if (clipboardText.Length <= CharactersRemaining)
            {
                this.AppendText(clipboardText);
            }
            else
            {
                this.AppendText(clipboardText.Substring(0, CharactersRemaining));
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

            this.CharactersRemaining = this.MaxCharactersAllowed - noOfCharactersEntered;
        }

        protected void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
