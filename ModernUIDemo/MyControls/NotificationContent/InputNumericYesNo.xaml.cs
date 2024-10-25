namespace ModernUIDemo
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für InputNumericYesNo.xaml
    /// </summary>
    public partial class InputNumericYesNo : UserControl, INotificationServiceMessage
    {
        private static readonly Regex regexIntPattern = new Regex("[^0-9.-]+");

        public InputNumericYesNo()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<TextBox, TextChangedEventArgs>.AddHandler(this.TxtInput, "TextChanged", this.OnTextChanged);
            WeakEventManager<TextBox, TextCompositionEventArgs>.AddHandler(this.TxtInput, "PreviewTextInput", this.OnPreviewTextInput);
        }

        public int CountDown { get; set; }

        private char CurrencyDecimalSeparator { get; set; }
        private char NegativSymbol { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (string InfoText, string CustomText, int MaxLength, double FontSize) textOption = ((string InfoText, string CustomText, int MaxLength, double FontSize))this.Tag;

            this.CurrencyDecimalSeparator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            this.NegativSymbol = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NegativeSign);


            this.TbHeader.Text = textOption.InfoText;
            this.TxtInput.Text = textOption.CustomText;
            this.TxtInput.MaxLength = textOption.MaxLength;

            if (string.IsNullOrEmpty(this.TxtInput.Text) == true)
            {
                this.BtnYes.IsEnabled = false;
            }
            else
            {
                this.BtnYes.IsEnabled = true;
            }

            if (Convert.ToInt32(this.TxtInput.Text) == 0)
            {
                this.TxtInput.Text = string.Empty;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.TxtInput.Text) == true)
            {
                this.BtnYes.IsEnabled = false;
            }
            else
            {
                this.BtnYes.IsEnabled = true;
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = this.ValidInputChar(e.Text.Last());
        }

        private bool ValidInputChar(char inputChar)
        {
            bool result = true;

            List<char> validChars = new List<char>();
            validChars.Add('0');
            validChars.Add('1');
            validChars.Add('2');
            validChars.Add('3');
            validChars.Add('4');
            validChars.Add('5');
            validChars.Add('6');
            validChars.Add('7');
            validChars.Add('8');
            validChars.Add('9');
            validChars.Add(this.CurrencyDecimalSeparator);
            validChars.Add(this.NegativSymbol);

            result = validChars.Any(c => c == inputChar);

            return !result;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                if (this.TxtInput.Text.Count(c => c == this.NegativSymbol) >= 1)
                {
                    e.Handled = true;
                }
                else
                {
                    InputNumericYesNo ctrl = (InputNumericYesNo)e.Source;
                    int cursorPos = ctrl.TxtInput.CaretIndex;
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

            e.Handled = false;

            if (e.Key == Key.OemComma)
            {
                if (this.TxtInput.Text.Count(c => c == this.CurrencyDecimalSeparator) >= 1)
                {
                    e.Handled = true;
                }

                return;
            }

        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.Yes, this.TxtInput.Text);
            window.DialogResult = true;
            window.Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.No, null);
            window.DialogResult = false;
            window.Close();
        }

        private static bool IsTextAllowed(string text)
        {
            return !regexIntPattern.IsMatch(text);
        }
    }
}
