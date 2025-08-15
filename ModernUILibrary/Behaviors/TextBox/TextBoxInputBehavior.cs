//-----------------------------------------------------------------------
// <copyright file="TextBoxInputBehavior.cs" company="Lifeprojects.de">
//     Class: TextBoxInputBehavior
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>09.06.2020</date>
//
// <summary>Definition of Behavior Class for press Enter to next Control</summary>
//-----------------------------------------------------------------------
/*
    <TextBox>
       <i:Interaction.Behaviors>
          <behavior:TextBoxInputBehavior EscapeClearsText="True" InputMode="DigitInput" />
       </i:Interaction.Behaviors>
    </TextBox>
 */

namespace ModernIU.Behaviors
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Microsoft.Xaml.Behaviors;

    public class TextBoxInputBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty JustPositivDecimalInputProperty = DependencyProperty.Register("JustPositivDecimalInput", typeof(bool), typeof(TextBoxInputBehavior), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty DecimalPlaceProperty = DependencyProperty.Register("DecimalPlace", typeof(int), typeof(TextBoxInputBehavior), new FrameworkPropertyMetadata(2));
        public static readonly DependencyProperty EscapeClearsTextProperty = DependencyProperty.RegisterAttached("EscapeClearsText", typeof(bool), typeof(TextBoxInputBehavior), new FrameworkPropertyMetadata(false));

        private const NumberStyles VALIDNUMBERSTYLES = NumberStyles.AllowDecimalPoint |
                                           NumberStyles.AllowThousands |
                                           NumberStyles.AllowLeadingSign;

        private static readonly string[] DateFormats = new string[] { "d.M.yyyy", "dd.MM.yyyy", "d.M.yy" };

        private readonly char decimalSeparator;
        private readonly char numberGroupSeparator;
        private readonly CultureInfo cultureInfo = null;

        public TextBoxInputBehavior()
        {
            this.InputMode = TextBoxInputMode.None;
            this.JustPositivDecimalInput = false;
            this.MaxVorkommastellen = null;
            this.cultureInfo = Thread.CurrentThread.CurrentUICulture;
            this.decimalSeparator = Convert.ToChar(this.cultureInfo.NumberFormat.NumberDecimalSeparator);
            this.numberGroupSeparator = Convert.ToChar(this.cultureInfo.NumberFormat.NumberGroupSeparator);
            this.IsPasting = false;
        }

        public TextBoxInputMode InputMode { get; set; }

        public ushort? MaxVorkommastellen { get; set; }

        public bool JustPositivDecimalInput
        {
            get { return (bool)this.GetValue(JustPositivDecimalInputProperty); }
            set { this.SetValue(JustPositivDecimalInputProperty, value); }
        }

        public int DecimalPlace
        {
            get { return (int)this.GetValue(DecimalPlaceProperty); }
            set { this.SetValue(DecimalPlaceProperty, value); }
        }

        public bool EscapeClearsText
        {
            get { return (bool)this.GetValue(EscapeClearsTextProperty); }
            set { this.SetValue(EscapeClearsTextProperty, value); }
        }

        private bool IsPasting { get; set; }

        private string PastText { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewTextInput += this.AssociatedObjectPreviewTextInput;
            this.AssociatedObject.PreviewKeyDown += this.AssociatedObjectPreviewKeyDown;
            this.AssociatedObject.KeyDown += this.AssociatedObjectKeyDown;
            this.AssociatedObject.LostFocus += this.AssociatedObjectLostFocus;
            if (this.InputMode == TextBoxInputMode.Date)
            {
                this.AssociatedObject.HorizontalContentAlignment = HorizontalAlignment.Left;
            }
            else if (this.InputMode == TextBoxInputMode.CurrencyInput)
            {
                this.AssociatedObject.HorizontalContentAlignment = HorizontalAlignment.Right;
            }
            else if (this.InputMode == TextBoxInputMode.DecimalInput)
            {
                this.AssociatedObject.HorizontalContentAlignment = HorizontalAlignment.Right;
            }
            else if (this.InputMode == TextBoxInputMode.DigitInput)
            {
                this.AssociatedObject.HorizontalContentAlignment = HorizontalAlignment.Right;
            }
            else
            {
                this.AssociatedObject.HorizontalContentAlignment = HorizontalAlignment.Left;
            }

            DataObject.AddPastingHandler(this.AssociatedObject, this.Pasting);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewTextInput -= this.AssociatedObjectPreviewTextInput;
            this.AssociatedObject.PreviewKeyDown -= this.AssociatedObjectPreviewKeyDown;
            this.AssociatedObject.KeyDown -= this.AssociatedObjectKeyDown;
            this.AssociatedObject.LostFocus -= this.AssociatedObjectLostFocus;

            DataObject.RemovePastingHandler(this.AssociatedObject, this.Pasting);
        }

        private void AssociatedObjectKeyDown(object sender, KeyEventArgs e)
        {
            /*this.AssociatedObject.Text = Convert.ToDecimal(this.AssociatedObject.Text).ToString("C2");*/
        }

        private void AssociatedObjectLostFocus(object sender, RoutedEventArgs e)
        {
            if (this.InputMode == TextBoxInputMode.CurrencyInput)
            {
                if (this.AssociatedObject.Text.Contains(this.decimalSeparator) == false)
                {
                    decimal value = Convert.ToDecimal($"{this.AssociatedObject.Text}{this.decimalSeparator}00");
                    this.AssociatedObject.Text = value.ToString($"N{this.DecimalPlace}");
                }
                else
                {
                    decimal value = Convert.ToDecimal(this.AssociatedObject.Text.Replace(".",string.Empty));
                    this.AssociatedObject.Text = value.ToString($"N{this.DecimalPlace}");
                }
            }
            else if (this.InputMode == TextBoxInputMode.DecimalInput)
            {
                if (string.IsNullOrEmpty(this.AssociatedObject.Text) == true)
                {
                    decimal value = Convert.ToDecimal("0");
                    this.AssociatedObject.Text = value.ToString();
                }
                else
                {
                    if (this.AssociatedObject.Text.Contains(this.decimalSeparator) == false)
                    {
                        decimal value = Convert.ToDecimal(this.AssociatedObject.Text);
                        if (this.DecimalPlace > 0)
                        {
                            this.AssociatedObject.Text = value.ToString($"N{this.DecimalPlace}");
                        }
                        else
                        {
                            this.AssociatedObject.Text = value.ToString();
                        }
                    }
                    else
                    {
                        decimal value = Convert.ToDecimal(this.AssociatedObject.Text.Replace(".", string.Empty));
                        this.AssociatedObject.Text = value.ToString($"N{this.DecimalPlace}");
                    }
                }
            }
            else if (this.InputMode == TextBoxInputMode.DigitInput)
            {
                if (string.IsNullOrEmpty(this.AssociatedObject.Text) == false)
                {
                    int value = Convert.ToInt32(this.AssociatedObject.Text);
                    this.AssociatedObject.Text = value.ToString();
                }
            }
        }

        private void Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pastedText = (string)e.DataObject.GetData(typeof(string));

                string currentText = this.GetText(pastedText);
                this.IsPasting = true;
                if (this.IsValidInput(currentText) == false)
                {
                    System.Media.SystemSounds.Beep.Play();
                    e.CancelCommand();
                }

                this.IsPasting = false;
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
                e.CancelCommand();
            }
        }

        private void AssociatedObjectPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && this.EscapeClearsText == true)
            {                
                this.AssociatedObject.Text = string.Empty;
            }

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.V)
            {
                this.IsPasting = true;
                string pastedText = Clipboard.GetText();
                if (this.IsValidInput(pastedText) == true)
                {
                    this.AssociatedObject.Text = this.PastText;
                    this.IsPasting = false;
                    this.PastText = string.Empty;
                }
            }

            if (e.Key == Key.Space)
            {
                if (!this.IsValidInput(this.GetText(" ")))
                {
                    System.Media.SystemSounds.Beep.Play();
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Back)
            {
                /*wenn was selektiert wird dann wird nur das gelöscht mit BACK*/
                try
                {
                    if (this.AssociatedObject?.SelectionLength > 0)
                    {
                        if (this.IsValidInput(this.GetText(string.Empty)) == false)
                        {
                            System.Media.SystemSounds.Beep.Play();
                            e.Handled = true;
                        }
                    }
                    else if (this.AssociatedObject?.CaretIndex > 0)
                    {
                        //selber löschen
                        var txt = this.AssociatedObject.Text.Replace(".",string.Empty);
                        if (txt.Length != this.AssociatedObject.CaretIndex - 1)
                        {
                            string backspace = txt.Remove(this.AssociatedObject.CaretIndex - 1, 1);

                            if (!this.IsValidInput(backspace))
                            {
                                System.Media.SystemSounds.Beep.Play();
                                e.Handled = true;
                            }
                        }
                        else
                        {
                            if (!this.IsValidInput(txt))
                            {
                                System.Media.SystemSounds.Beep.Play();
                                e.Handled = true;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            if (e.Key == Key.Delete)
            {
                /*wenn was selektiert wird dann wird nur das gelöscht mit ENTF*/
                if (this.InputMode == TextBoxInputMode.DigitInput || this.InputMode == TextBoxInputMode.DecimalInput || this.InputMode == TextBoxInputMode.CurrencyInput)
                {
                    if (this.AssociatedObject.Text.Length <= 1)
                    {
                        this.AssociatedObject.Text = "0";
                        e.Handled = true;
                    }
                }
                else
                {
                    if (this.AssociatedObject?.SelectionLength > 0)
                    {
                        if (!this.IsValidInput(this.GetText(string.Empty)))
                        {
                            System.Media.SystemSounds.Beep.Play();
                            e.Handled = true;
                        }
                    }
                    else if (this.AssociatedObject?.CaretIndex < this.AssociatedObject.Text.Length)
                    {
                        /*selber löschen */
                        var txt = this.AssociatedObject.Text;
                        var entf = txt.Remove(this.AssociatedObject.CaretIndex, 1);

                        if (!this.IsValidInput(entf))
                        {
                            System.Media.SystemSounds.Beep.Play();
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void AssociatedObjectPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!this.IsValidInput(this.GetText(e.Text)))
            {
                System.Media.SystemSounds.Beep.Play();
                e.Handled = true;
            }
        }

        private string GetText(string input)
        {
            if (string.IsNullOrEmpty(input) == true)
            {
                return string.Empty;
            }

            try
            {
                var txt = this.AssociatedObject;

                int selectionStart = txt.SelectionStart;
                if (txt.Text.Length < selectionStart)
                {
                    selectionStart = txt.Text.Length;
                }

                int selectionLength = txt.SelectionLength;
                if (txt.Text.Length < selectionStart + selectionLength)
                {
                    selectionLength = txt.Text.Length - selectionStart;
                }

                var realtext = txt.Text.Remove(selectionStart, selectionLength);

                int caretIndex = txt.CaretIndex;
                if (realtext.Length < caretIndex)
                {
                    caretIndex = realtext.Length;
                }

                var newtext = realtext.Insert(caretIndex, input);

                return newtext;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsValidInput(string input)
        {
            if (string.IsNullOrEmpty(input) == true)
            {
                return true;
            }

            if (input.Length == 0)
            {
                return true;
            }

            switch (this.InputMode)
            {
                case TextBoxInputMode.None:
                    {
                        return true;
                    }

                case TextBoxInputMode.DigitInput:
                    {
                        return this.CheckIsDigit(input);
                    }

                case TextBoxInputMode.DecimalInput:
                    /* wen mehr als ein Komma */
                    if (input.ToCharArray().Where(x => x == this.decimalSeparator).Count() > 1)
                    {
                        return false;
                    }

                    if (input.ToCharArray().Where(x => x == this.numberGroupSeparator).Count() > 0)
                    {
                        decimal decimalOut;
                        bool isOk = decimal.TryParse(input, NumberStyles.Any, this.cultureInfo, out decimalOut);
                        if (isOk == false)
                        {
                            return false;
                        }
                    }

                    if (input.Contains(this.decimalSeparator.ToString()) == true)
                    {
                        if (input.Split(this.decimalSeparator)[1].Length > this.DecimalPlace)
                        {
                            if (this.IsPasting == true)
                            {
                                string decimalValue = input.Split(this.decimalSeparator)[1];
                                this.PastText = $"{input.Split(this.decimalSeparator)[0]}{this.decimalSeparator}{decimalValue.Substring(0, this.DecimalPlace)}";
                                this.IsPasting = false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                    if (input.Contains("-"))
                    {
                        if (this.JustPositivDecimalInput)
                        {
                            return false;
                        }


                        if (input.IndexOf("-", StringComparison.Ordinal) > 0)
                        {
                            return false;
                        }

                        if (input.ToCharArray().Count(x => x == '-') > 1)
                        {
                            return false;
                        }

                        /*minus einmal am anfang zulässig */
                        if (input.Length == 1)
                        {
                            return true;
                        }
                    }

                    decimal decimalValueOut;
                    var result = decimal.TryParse(input, VALIDNUMBERSTYLES, CultureInfo.CurrentCulture, out decimalValueOut);
                    return result;

                case TextBoxInputMode.CurrencyInput:
                    /* wen mehr als ein Komma */
                    if (input.ToCharArray().Where(x => x == this.decimalSeparator).Count() > 1)
                    {
                        return false;
                    }

                    if (input.ToCharArray().Where(x => x == this.numberGroupSeparator).Count() > 0)
                    {
                        decimal decimalOut;
                        bool isOk = decimal.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, this.cultureInfo, out decimalOut);
                        if (isOk == false)
                        {
                            return false;
                        }
                    }

                    if (input.Contains(this.decimalSeparator.ToString()) == true)
                    {
                        if (input.Split(this.decimalSeparator)[1].Length > this.DecimalPlace)
                        {
                            if (this.IsPasting == true)
                            {
                                string decimalValue = input.Split(this.decimalSeparator)[1];
                                this.PastText = $"{input.Split(this.decimalSeparator)[0]}{this.decimalSeparator}{decimalValue.Substring(0, this.DecimalPlace)}";
                                this.IsPasting = false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }

                    if (input.Contains("-"))
                    {
                        if (this.JustPositivDecimalInput)
                        {
                            return false;
                        }


                        if (input.IndexOf("-", StringComparison.Ordinal) > 0)
                        {
                            return false;
                        }

                        if (input.ToCharArray().Count(x => x == '-') > 1)
                        {
                            return false;
                        }

                        /*minus einmal am anfang zulässig */
                        if (input.Length == 1)
                        {
                            return true;
                        }
                    }

                    decimal currencyValueOut;
                    var currencyResult = decimal.TryParse(input, VALIDNUMBERSTYLES, CultureInfo.CurrentCulture, out currencyValueOut);
                    return currencyResult;

                case TextBoxInputMode.PercentInput: /*99,999 is zulässig und nur positiv ohne 1000er Trennzeichen*/
                    {
                        float f;

                        if (input.Contains("-"))
                        {
                            return false;
                        }

                        /*wen mehr als ein Komma */
                        if (input.ToCharArray().Where(x => x == this.decimalSeparator).Count() > 1)
                        {
                            return false;
                        }

                        if (input.ToCharArray().Where(x => x == this.numberGroupSeparator).Count() > 0)
                        {
                            return false;
                        }

                        if (input.Contains(this.decimalSeparator.ToString()) == true)
                        {
                            if (input.Split(this.decimalSeparator)[1].Length > this.DecimalPlace)
                            {
                                if (this.IsPasting == true)
                                {
                                    string decimalValue = input.Split(this.decimalSeparator)[1];
                                    this.PastText = $"{input.Split(this.decimalSeparator)[0]}{this.decimalSeparator}{decimalValue.Substring(0, this.DecimalPlace)}";
                                    this.IsPasting = false;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        var percentResult = float.TryParse(input, NumberStyles.Float, CultureInfo.CurrentCulture, out f);

                        if (this.MaxVorkommastellen.HasValue)
                        {
                            var vorkomma = Math.Truncate(f);
                            if (vorkomma.ToString(CultureInfo.CurrentCulture).Length > this.MaxVorkommastellen.Value)
                            {
                                return false;
                            }
                        }

                        return percentResult;
                    }

                case TextBoxInputMode.Letter:
                    {
                        return this.CheckIsLetter(input);
                    }

                case TextBoxInputMode.LetterOrDigit:
                    {
                        return this.CheckIsLetterOrDigit(input);
                    }

                case TextBoxInputMode.Date:
                    {
                        return this.CheckIsdate(input);
                    }

                default: throw new ArgumentException("Unknown TextBoxInputMode");
            }
        }

        private bool CheckIsDigit(string wert)
        {
            return wert.ToCharArray().All(char.IsDigit);
        }

        private bool CheckIsLetterOrDigit(string wert)
        {
            return wert.ToCharArray().All(char.IsLetterOrDigit);
        }

        private bool CheckIsLetter(string wert)
        {
            return wert.ToCharArray().All(char.IsLetter);
        }

        private bool CheckIsdate(string value)
        {
            bool result = false;
            DateTime date;
            if (DateTime.TryParseExact(value, DateFormats, Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out date) == true)
            {
                result = true;
            }

            return result;
        }
    }
}
