namespace ModernIU.Controls
{
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernIU.Base;

    public enum EnumIpBoxType
    {
        IpAddress,
        SubnetMask,
    }

    public class IpTextBox : Control
    {
        private const string ipRegex = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
        private const string ZeroTo255Tip = "{0} ist kein gültiger Eintrag. Geben Sie einen Wert zwischen 0 und 255 an.";

        private TextBox PART_BOX1;
        private TextBox PART_BOX2;
        private TextBox PART_BOX3;
        private TextBox PART_BOX4;

        #region DependencyProperty
        public static readonly DependencyProperty ReadOnlyColorProperty = DependencyProperty.Register("ReadOnlyColor", typeof(Brush), typeof(IpTextBox), new PropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(IpTextBox), new PropertyMetadata(true, OnSetBorderChanged));

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

        #region Type

        public EnumIpBoxType Type
        {
            get { return (EnumIpBoxType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(EnumIpBoxType), typeof(IpTextBox), new PropertyMetadata(EnumIpBoxType.IpAddress));

        #endregion

        #region IsHasError

        public bool IsHasError
        {
            get { return (bool)GetValue(IsHasErrorProperty); }
            private set { SetValue(IsHasErrorProperty, value); }
        }

        public static readonly DependencyProperty IsHasErrorProperty =
            DependencyProperty.Register("IsHasError", typeof(bool), typeof(IpTextBox), new PropertyMetadata(false));

        #endregion

        #region ErrorContent

        public string ErrorContent
        {
            get { return (string)GetValue(ErrorContentProperty); }
            private set { SetValue(ErrorContentProperty, value); }
        }

        public static readonly DependencyProperty ErrorContentProperty =
            DependencyProperty.Register("ErrorContent", typeof(string), typeof(IpTextBox), new PropertyMetadata(string.Empty));

        #endregion

        #region IsKeyboardFocused

        public new bool IsKeyboardFocused
        {
            get { return (bool)GetValue(IsKeyboardFocusedProperty); }
            private set { SetValue(IsKeyboardFocusedProperty, value); }
        }

        public new static readonly DependencyProperty IsKeyboardFocusedProperty =
            DependencyProperty.Register("IsKeyboardFocused", typeof(bool), typeof(IpTextBox), new PropertyMetadata(false));

        #endregion

        #region Text

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(IpTextBox), new PropertyMetadata(string.Empty));

        #endregion

        #endregion

        static IpTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IpTextBox), new FrameworkPropertyMetadata(typeof(IpTextBox)));
        }

        public IpTextBox()
        {
            this.Height = ControlBase.DefaultHeight;
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.BorderBrush = ControlBase.BorderBrush;
            this.BorderThickness = ControlBase.BorderThickness;
            this.Margin = new Thickness(2);
            this.IsEnabled = true;
            this.Focusable = true;
        }

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.PART_BOX1 = this.GetTemplateChild("PART_BOX1") as TextBox;
            this.PART_BOX2 = this.GetTemplateChild("PART_BOX2") as TextBox;
            this.PART_BOX3 = this.GetTemplateChild("PART_BOX3") as TextBox;
            this.PART_BOX4 = this.GetTemplateChild("PART_BOX4") as TextBox;

            if (this.PART_BOX1 != null)
            {
                this.PART_BOX1.PreviewTextInput += PART_BOX1_PreviewTextInput;
                this.PART_BOX1.TextChanged += PART_BOX1_TextChanged;
                this.PART_BOX1.GotFocus += (o, e) => { this.IsKeyboardFocused = true; };
                this.PART_BOX1.LostFocus += (o, e) => { this.IsKeyboardFocused = false; };
                this.PART_BOX1.PreviewKeyDown += PART_BOX1_PreviewKeyDown;
            }

            if(this.PART_BOX2 != null)
            {
                this.PART_BOX2.PreviewTextInput += PART_BOX2_PreviewTextInput;
                this.PART_BOX2.TextChanged += PART_BOX2_TextChanged;
                this.PART_BOX2.GotFocus += (o, e) => { this.IsKeyboardFocused = true; };
                this.PART_BOX2.LostFocus += (o, e) => { this.IsKeyboardFocused = false; };
                this.PART_BOX2.PreviewKeyDown += PART_BOX1_PreviewKeyDown;
            }

            if(this.PART_BOX3 != null)
            {
                this.PART_BOX3.PreviewTextInput += PART_BOX3_PreviewTextInput;
                this.PART_BOX3.TextChanged += PART_BOX3_TextChanged;
                this.PART_BOX3.GotFocus += (o, e) => { this.IsKeyboardFocused = true; };
                this.PART_BOX3.LostFocus += (o, e) => { this.IsKeyboardFocused = false; };
                this.PART_BOX3.PreviewKeyDown += PART_BOX1_PreviewKeyDown;
            }

            if(this.PART_BOX4 != null)
            {
                this.PART_BOX4.PreviewTextInput += PART_BOX4_PreviewTextInput;
                this.PART_BOX4.GotFocus += (o, e) => { this.IsKeyboardFocused = true; };
                this.PART_BOX4.LostFocus += (o, e) => { this.IsKeyboardFocused = false; };
                this.PART_BOX4.PreviewKeyDown += PART_BOX1_PreviewKeyDown;
            }

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();
        }

        void PART_BOX1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.V)
            {
                IDataObject data = Clipboard.GetDataObject();
                if(data.GetDataPresent(DataFormats.Text))
                {
                    string text = (string)data.GetData(DataFormats.UnicodeText);
                    Regex regex = new Regex(ipRegex);
                    if(regex.IsMatch(text))
                    {
                        string[] strs = text.Split(new char[] { '.' });
                        this.PART_BOX1.Text = strs[0];
                        this.PART_BOX2.Text = strs[1];
                        this.PART_BOX3.Text = strs[2];
                        this.PART_BOX4.Text = strs[3];
                        
                        this.PART_BOX1.Focus();
                        this.PART_BOX1.SelectionStart = 0;
                    }
                    else
                    {
                        this.IsHasError = true;
                        this.ErrorContent = "IP-Adresse ist falsch formatiert";
                        this.PART_BOX1.Text = string.Empty;
                        this.PART_BOX2.Text = string.Empty;
                        this.PART_BOX3.Text = string.Empty;
                        this.PART_BOX4.Text = string.Empty;
                    }
                }
                e.Handled = true;
            }
        }

        #endregion

        #region private function
        private bool IsNumber(string input)
        {
            Regex regex = new Regex("^[0-9]*$");
            return regex.IsMatch(input);
        }

        private bool IsNumberRange(int number, int start, int end)
        {
            return number >= start && number <= end;
        }

        private bool CheckNumberIsLegal(string text1, string text2)
        {
            if (!this.IsNumber(text2)) return true;

            int text = Convert.ToInt32(text1);

            return !this.IsNumberRange(text, 0, 255);
        }
        #endregion

        #region Event Implement Function
        private void PART_BOX1_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string input = this.PART_BOX1.Text + e.Text;
            if (!string.IsNullOrWhiteSpace(input) && ".".Equals(e.Text))
            {
                e.Handled = true;
                this.PART_BOX2.Focus();
            }
            else
            {
                if (!this.IsNumber(e.Text))
                {
                    e.Handled = true;
                    return;
                }

                if(!string.IsNullOrWhiteSpace(this.PART_BOX1.SelectedText))
                {
                    input = this.PART_BOX1.Text.Remove(this.PART_BOX1.SelectionStart, this.PART_BOX1.SelectionLength) + e.Text;
                }

                int text = Convert.ToInt32(input);
                switch (this.Type)
                {
                    case EnumIpBoxType.IpAddress:
                        if (!this.IsNumberRange(text, 0, 223))
                        {
                            e.Handled = true;
                            this.IsHasError = true;
                            this.ErrorContent = string.Format("{0} Kein gültiger Eintrag. Geben Sie einen Wert zwischen 1 und 223 an.", input);
                            this.PART_BOX1.Text = "223";
                        }
                        else
                        {
                            IsHasError = false;
                        }
                        break;
                    case EnumIpBoxType.SubnetMask:
                        if (!this.IsNumberRange(text, 0, 255))
                        {
                            e.Handled = true;
                            this.IsHasError = true;
                            this.ErrorContent = string.Format(ZeroTo255Tip, input);
                            this.PART_BOX1.Text = "255";
                        }
                        else
                        {
                            IsHasError = false;
                        }
                        break;
                }
            }
        }

        private void PART_BOX2_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string input = this.PART_BOX2.Text + e.Text;

            if (!string.IsNullOrWhiteSpace(this.PART_BOX2.SelectedText))
            {
                input = this.PART_BOX2.Text.Remove(this.PART_BOX2.SelectionStart, this.PART_BOX2.SelectionLength) + e.Text;
            }

            if (!string.IsNullOrWhiteSpace(input) && ".".Equals(e.Text)) 
            {
                e.Handled = true;
                this.PART_BOX3.Focus();
            }
            else
            {
                e.Handled = this.CheckNumberIsLegal(input, e.Text);
                if (e.Handled)
                {
                    this.PART_BOX2.Text = "255";
                    this.PART_BOX2.SelectionStart = this.PART_BOX2.Text.Length + 1;
                    this.IsHasError = true;
                    this.ErrorContent = string.Format(ZeroTo255Tip, input);
                }
                else
                {
                    this.IsHasError = false;
                }
            }
        }

        private void PART_BOX3_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string input = this.PART_BOX3.Text + e.Text;

            if (!string.IsNullOrWhiteSpace(this.PART_BOX3.SelectedText))
            {
                input = this.PART_BOX3.Text.Remove(this.PART_BOX3.SelectionStart, this.PART_BOX3.SelectionLength) + e.Text;
            }

            if (!string.IsNullOrWhiteSpace(input) && ".".Equals(e.Text))
            {
                e.Handled = true;
                this.PART_BOX4.Focus();
            }
            else
            {
                e.Handled = this.CheckNumberIsLegal(input, e.Text);
                if (e.Handled)
                {
                    this.PART_BOX3.Text = "255";
                    this.PART_BOX3.SelectionStart = this.PART_BOX3.Text.Length + 1;
                    this.IsHasError = true;
                    this.ErrorContent = string.Format(ZeroTo255Tip, input);
                }
                else
                {
                    this.IsHasError = false;
                }
            }
        }

        private void PART_BOX4_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string input = this.PART_BOX4.Text + e.Text;

            if (!string.IsNullOrWhiteSpace(this.PART_BOX4.SelectedText))
            {
                input = this.PART_BOX4.Text.Remove(this.PART_BOX4.SelectionStart, this.PART_BOX4.SelectionLength) + e.Text;
            }

            e.Handled = this.CheckNumberIsLegal(input, e.Text);
            if (e.Handled)
            {
                this.PART_BOX4.Text = "255";
                this.PART_BOX4.SelectionStart = this.PART_BOX4.Text.Length + 1;
                this.IsHasError = true;
                this.ErrorContent = string.Format(ZeroTo255Tip, input);
            }
            else
            {
                this.IsHasError = false;
            }
        }

        private void PART_BOX1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.PART_BOX1.Text.Length == 3)
            {
                int number = 1;
                if (Int32.TryParse(this.PART_BOX1.Text, out number))
                {
                    switch (this.Type)
                    {
                        case EnumIpBoxType.IpAddress:
                            if (number < 1)
                            {
                                this.PART_BOX1.Text = "1";
                            }
                            break;
                        case EnumIpBoxType.SubnetMask:
                            break;
                    }
                }
                this.PART_BOX2.Focus();
            }
        }

        private void PART_BOX2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.PART_BOX2.Text.Length == 3)
            {
                this.PART_BOX3.Focus();
            }
        }

        private void PART_BOX3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.PART_BOX3.Text.Length == 3)
            {
                this.PART_BOX4.Focus();
            }
        }
        #endregion

        private Style SetTriggerFunction()
        {
            Style inputControlStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = IpTextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = IpTextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = IpTextBox.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = IpTextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsFocused);

            /* Trigger für IsReadOnly = True */
            Trigger triggerIsReadOnly = new Trigger();
            triggerIsReadOnly.Property = TextBox.IsReadOnlyProperty;
            triggerIsReadOnly.Value = true;
            triggerIsReadOnly.Setters.Add(new Setter() { Property = IpTextBox.BackgroundProperty, Value = Brushes.LightYellow });
            inputControlStyle.Triggers.Add(triggerIsReadOnly);

            /* Trigger für IsHasError = True */
            Trigger triggerIsHasError = new Trigger();
            triggerIsHasError.Property = IpTextBox.IsHasErrorProperty;
            triggerIsHasError.Value = true;
            triggerIsHasError.Setters.Add(new Setter() { Property = IpTextBox.BackgroundProperty, Value = Brushes.Red });
            inputControlStyle.Triggers.Add(triggerIsHasError);

            return inputControlStyle;
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (IpTextBox)d;

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
    }
}
