/*
 * <copyright file="TextBoxInputBehavior.cs" company="Lifeprojects.de">
 *     Class: TextBoxInputBehavior
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>04.12.2022 12:24:59</date>
 * <Project>CurrentProject</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * < Website >
 * https://blindmeis.wordpress.com/2015/01/20/wpf-textbox-input-behavior/
 * </Website>
 *
 *This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernIU.Behaviors
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Media;
    using System.Runtime.Versioning;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Microsoft.Xaml.Behaviors;

    public enum TextBoxInputMode
    {
        None,
        Decimal,
        Integer,
        Percent,
        Letter,
        LetterOrDigit,
        Money,
        Date
    }

    [SupportedOSPlatform("windows")]
    public class TextBoxInputBehavior : Behavior<TextBox>
    {
        private const NumberStyles validNumberStyles = NumberStyles.AllowDecimalPoint |
                                              NumberStyles.AllowThousands |
                                              NumberStyles.AllowLeadingSign;

        public static readonly DependencyProperty JustPositivDecimalInputProperty =
            DependencyProperty.Register("JustPositivDecimalInput", typeof(bool), typeof(TextBoxInputBehavior), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty DecimalPlaceProperty = 
            DependencyProperty.Register("DecimalPlace", typeof(int), typeof(TextBoxInputBehavior), new FrameworkPropertyMetadata(2));

        public static readonly DependencyProperty EscapeClearsTextProperty = 
            DependencyProperty.RegisterAttached("EscapeClearsText", typeof(bool), typeof(TextBoxInputBehavior), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty BeepProperty =
            DependencyProperty.RegisterAttached("Beep", typeof(bool), typeof(TextBoxInputBehavior), new FrameworkPropertyMetadata(true));

        private static readonly string[] DateFormats = new string[] { "d.M.yyyy", "dd.MM.yyyy" };

        private bool changeIntern = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxInputBehavior"/> class.
        /// </summary>
        public TextBoxInputBehavior()
        {
            this.InputMode = TextBoxInputMode.None;
            this.JustPositivDecimalInput = false;
            this.MaxVorkommastellen = null;
        }

        public TextBoxInputMode InputMode { get; set; }

        public ushort? MaxVorkommastellen { get; set; }


        public bool JustPositivDecimalInput
        {
            get { return (bool)GetValue(JustPositivDecimalInputProperty); }
            set { SetValue(JustPositivDecimalInputProperty, value); }
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

        public bool Beep
        {
            get { return (bool)this.GetValue(BeepProperty); }
            set { this.SetValue(BeepProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (InputMode == TextBoxInputMode.Integer || InputMode == TextBoxInputMode.Decimal || InputMode == TextBoxInputMode.Money || InputMode == TextBoxInputMode.Percent)
            {
                AssociatedObject.HorizontalContentAlignment = HorizontalAlignment.Right;
            }
            else
            {
                AssociatedObject.HorizontalContentAlignment = HorizontalAlignment.Left;
            }

            AssociatedObject.VerticalContentAlignment = VerticalAlignment.Center;

            AssociatedObject.PreviewTextInput += AssociatedObjectPreviewTextInput;
            AssociatedObject.PreviewKeyDown += AssociatedObjectPreviewKeyDown;
            AssociatedObject.TextChanged += this.AssociatedObjectTextChanged;

            DataObject.AddPastingHandler(AssociatedObject, Pasting);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewTextInput -= AssociatedObjectPreviewTextInput;
            AssociatedObject.PreviewKeyDown -= AssociatedObjectPreviewKeyDown;
            AssociatedObject.TextChanged -= this.AssociatedObjectTextChanged;

            DataObject.RemovePastingHandler(AssociatedObject, Pasting);
        }

        private void Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var pastedText = (string)e.DataObject.GetData(typeof(string));

                if (this.IsValidInput(GetText(pastedText)) == false)
                {
                    if (this.Beep == true)
                    {
                        SystemSounds.Beep.Play();
                    }

                    e.CancelCommand();
                }
            }
            else
            {
                if (this.Beep == true)
                {
                    SystemSounds.Beep.Play();
                }

                e.CancelCommand();
            }
        }

        private void AssociatedObjectTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                string originalSource = ((TextBox)e.Source).Text;
                if (this.changeIntern == true)
                {
                    this.changeIntern = false;
                    return;
                }

                if (InputMode == TextBoxInputMode.Date)
                {
                    if (string.IsNullOrEmpty(txt.Text) == true)
                    {
                        return;
                    }

                    if (txt.Text.Length > 10)
                    {
                        if (this.CheckIsDate(originalSource.Split(' ')[0]).Item1 == true)
                        {
                            this.changeIntern = true;
                            if (this.CheckIsDate(originalSource.Split(' ')[0]).Item2 != null)
                            {
                                txt.Text = Convert.ToDateTime(originalSource.Split(' ')[0]).ToShortDateString();
                            }
                        }
                    }
                    else
                    {
                        if (this.CheckIsDate(originalSource).Item1 == true)
                        {
                            this.changeIntern = true;
                            if (this.CheckIsDate(originalSource).Item2 != null)
                            {
                                txt.Text = Convert.ToDateTime(originalSource).ToShortDateString();
                            }
                        }
                    }
                }
            }
        }

        private void AssociatedObjectPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && this.EscapeClearsText == true)
            {
                this.AssociatedObject.Text = string.Empty;
            }

            if (e.Key == Key.Space)
            {
                if (this.IsValidInput(GetText(" ")) == false)
                {
                    if (this.Beep == true)
                    {
                        SystemSounds.Beep.Play();
                    }

                    e.Handled = true;
                }
            }

            if (e.Key == Key.Back)
            {
                //wenn was selektiert wird dann wird nur das gelöscht mit BACK
                if (AssociatedObject.SelectionLength > 0)
                {
                    if (this.IsValidInput(GetText(string.Empty)) == false)
                    {
                        if (this.Beep == true)
                        {
                            SystemSounds.Beep.Play();
                        }

                        e.Handled = true;
                    }
                }
                else if (AssociatedObject.CaretIndex > 0)
                {
                    //selber löschen
                    var txt = AssociatedObject.Text;
                    var backspace = txt.Remove(AssociatedObject.CaretIndex - 1, 1);

                    if (this.IsValidInput(backspace) == false)
                    {
                        SystemSounds.Beep.Play();
                        e.Handled = true;
                    }
                }
            }

            if (e.Key == Key.Delete)
            {
                //wenn was selektiert wird dann wird nur das gelöscht mit ENTF
                if (AssociatedObject.SelectionLength > 0)
                {
                    if (this.IsValidInput(GetText(string.Empty)) == false)
                    {
                        if (this.Beep == true)
                        {
                            SystemSounds.Beep.Play();
                        }

                        e.Handled = true;
                    }
                }
                else if (AssociatedObject.CaretIndex < AssociatedObject.Text.Length)
                {
                    //selber löschen
                    var txt = AssociatedObject.Text;
                    var entf = txt.Remove(AssociatedObject.CaretIndex, 1);

                    if (this.IsValidInput(entf) == false)
                    {
                        if (this.Beep == true)
                        {
                            SystemSounds.Beep.Play();
                        }

                        e.Handled = true;
                    }
                }
            }
        }

        private void AssociatedObjectPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.IsValidInput(GetText(e.Text)) == false)
            {
                if (this.Beep == true)
                {
                    SystemSounds.Beep.Play();
                }

                e.Handled = false;
            }
        }

        private string GetText(string input)
        {
            var txt = AssociatedObject;

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

        private bool IsValidInput(string input)
        {
            char decimalSeparator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            char negativeSign = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NegativeSign);
            char dateSeparator = Convert.ToChar(CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator);

            if (input.Length == 0)
            {
                return true;
            }

            switch (InputMode)
            {
                case TextBoxInputMode.None:
                    return true;

                case TextBoxInputMode.Integer:
                    return CheckIsDigit(input);

                case TextBoxInputMode.Decimal:
                    if (CheckIsDecimal(input) == false)
                    {
                        return false;
                    }

                    decimal decimalValue = 0.00M;
                    bool isDecimalValue = decimal.TryParse(input, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimalValue);
                    if (isDecimalValue == false || GetDecimalPlaces(decimalValue) > DecimalPlace)
                    {
                        return false;
                    }

                    return true;

                case TextBoxInputMode.Percent: //99,999 is zulässig und  nur positiv ohne 1000er Trennzeichen
                    float percent;

                    if (input.Contains(negativeSign))
                    {
                        return false;
                    }

                    //wen mehr als ein Komma
                    if (input.ToCharArray().Where(x => x == decimalSeparator).Count() > 1)
                    {
                        return false;
                    }

                    bool percentResult = float.TryParse(input, NumberStyles.Float, CultureInfo.CurrentCulture, out percent);

                    if (MaxVorkommastellen.HasValue)
                    {
                        var vorkomma = Math.Truncate(percent);
                        if (vorkomma.ToString(CultureInfo.CurrentCulture).Length > MaxVorkommastellen.Value)
                        {
                            return false;
                        }
                    }

                    return percentResult;

                case TextBoxInputMode.Letter:
                    if (input.ToCharArray().Any(x => char.IsLetter(x) == false) == true)
                    {
                        return false;
                    }

                    return true;

                case TextBoxInputMode.LetterOrDigit:
                    if (input.ToCharArray().Any(x => char.IsLetterOrDigit(x) == false) == true)
                    {
                        return false;
                    }

                    return true;

                case TextBoxInputMode.Money:
                    decimal money = 0.00M;

                    bool isMoney = decimal.TryParse(input, NumberStyles.Currency, CultureInfo.CurrentCulture, out money);
                    if (isMoney == false || GetDecimalPlaces(money) > 2)
                    {
                        return false;
                    }

                    return true;

                case TextBoxInputMode.Date:
                    {
                        if (CheckIsDigit(input.Last().ToString()) == false)
                        {
                            if (CheckPunctuation(input.Last().ToString()) == false)
                            {
                                return false;
                            }
                            else
                            {
                                if (input.Last() != dateSeparator)
                                {
                                    return false;
                                }
                            }
                        }

                        return this.CheckIsDate(input, dateSeparator).Item1;
                    }

                default:
                    throw new ArgumentException("Unknown TextBoxInputMode");

            }
        }

        private bool CheckIsDigit(string wert)
        {
            return wert.ToCharArray().All(char.IsDigit);
        }

        private bool CheckPunctuation(string wert)
        {
            return wert.ToCharArray().All(char.IsPunctuation);
        }

        private bool CheckIsDecimal(string input)
        {
            char decimalSeparator = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
            char negativeSign = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NegativeSign);

            decimal d;
            //wen mehr als ein Komma
            if (input.ToCharArray().Where(x => x == decimalSeparator).Count() > 1)
            {
                return false;
            }

            if (input.Contains(negativeSign))
            {
                if (JustPositivDecimalInput)
                {
                    return false;
                }


                if (input.IndexOf(negativeSign, StringComparison.Ordinal) > 0)
                {
                    return false;
                }

                if (input.ToCharArray().Count(x => x == negativeSign) > 1)
                {
                    return false;
                }

                //minus einmal am anfang zulässig
                if (input.Length == 1)
                {
                    return true;
                }
            }

            return decimal.TryParse(input, validNumberStyles, CultureInfo.CurrentCulture, out d);
        }

        private int GetDecimalPlaces(decimal d)
        {
            return BitConverter.GetBytes(decimal.GetBits(d)[3])[2];
        }

        private (bool,DateOnly?) CheckIsDate(string input, char dateSeparator = '.')
        {
            bool result = false;

            char negativeSign = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NegativeSign);

            if (input.ToCharArray().Where(x => x == dateSeparator).Count() > 2)
            {
                return (false,null);
            }

            if (input.ToCharArray().Where(x => x == negativeSign).Count() > 0)
            {
                return (false, null);
            }

            /*
            bool isLengthOk = false;
            foreach (string dateFormat in DateFormats)
            {
                if (dateFormat.Length == input.Length)
                {
                    isLengthOk = true;
                    break;
                }
            }

            if (isLengthOk ==  false)
            {
                return (true,null);
            }
            */

            DateOnly date;
            if (DateOnly.TryParseExact(input, DateFormats, Thread.CurrentThread.CurrentCulture, DateTimeStyles.None, out date) == true)
            {
                result = true;
            }

            return (result, date);
        }
    }
}
