//-----------------------------------------------------------------------
// <copyright file="TextBoxEx.cs" company="Lifeprojects.de">
//     Class: TextBoxEx
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>14.08.2025</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernBaseLibrary.Extension;

    using ModernIU.Base;
    using ModernIU.Behaviors;

    public class TextBoxEx : TextBox
    {
        public static readonly DependencyProperty IsNegativeProperty = DependencyProperty.Register("IsNegative", typeof(bool), typeof(TextBoxEx), new PropertyMetadata(false));
        public static readonly DependencyProperty DecimalPlaceProperty = DependencyProperty.Register("DecimalPlace", typeof(int), typeof(TextBoxEx), new FrameworkPropertyMetadata(2));
        public static readonly DependencyProperty ReadOnlyColorProperty = DependencyProperty.Register("ReadOnlyColor", typeof(Brush), typeof(TextBoxEx), new PropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxEx), new PropertyMetadata(true, OnSetBorderChanged));
        public static readonly DependencyProperty EscapeClearsTextProperty = DependencyProperty.RegisterAttached("EscapeClearsText", typeof(bool), typeof(TextBoxEx), new FrameworkPropertyMetadata(false));

        private readonly char decimalSeparator;
        private readonly char numberGroupSeparator;
        private readonly CultureInfo cultureInfo = null;
        private readonly char negativeSign;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxEx"/> class.
        /// </summary>
        public TextBoxEx()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.BorderBrush = Brushes.Green;
            this.BorderThickness = new Thickness(1);
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.AcceptsReturn = false;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 23;
            this.ClipToBounds = false;
            this.Focusable = true;

            this.cultureInfo = Thread.CurrentThread.CurrentUICulture;
            this.decimalSeparator = Convert.ToChar(this.cultureInfo.NumberFormat.NumberDecimalSeparator);
            this.numberGroupSeparator = Convert.ToChar(this.cultureInfo.NumberFormat.NumberGroupSeparator);
            this.negativeSign = Convert.ToChar(this.cultureInfo.NumberFormat.NegativeSign);

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();
        }

        public TextBoxInputMode InputMode { get; set; } = TextBoxInputMode.None;

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

        public bool EscapeClearsText
        {
            get { return (bool)this.GetValue(EscapeClearsTextProperty); }
            set { this.SetValue(EscapeClearsTextProperty, value); }
        }

        public bool IsNegative
        {
            get { return (bool)GetValue(IsNegativeProperty); }
            set { SetValue(IsNegativeProperty, value); }
        }

        public int DecimalPlace
        {
            get { return (int)this.GetValue(DecimalPlaceProperty); }
            set { this.SetValue(DecimalPlaceProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.InputMode.In(TextBoxInputMode.DigitInput, TextBoxInputMode.DecimalInput,TextBoxInputMode.PercentInput, TextBoxInputMode.CurrencyInput) == true)
            {
                this.HorizontalContentAlignment = HorizontalAlignment.Right;
            }
            else
            {
                this.HorizontalContentAlignment = HorizontalAlignment.Left;
            }

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

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            this.Focus();
            this.Select(0, this.Text.Length);
            this.SelectAll();
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (this.InputMode == TextBoxInputMode.DecimalInput)
            {
                bool approvedDecimalPoint = false;


                if (e.Text == this.decimalSeparator.ToString())
                {
                    if ((this.Text.Contains(this.decimalSeparator) == false))
                    {
                        approvedDecimalPoint = true;
                    }
                }

                if (this.IsNegative == true)
                {
                    int countKomma = this.Text.ToCharArray().Where(w => w == this.decimalSeparator).Count();
                    if (countKomma < 1 && approvedDecimalPoint == true)
                    {
                        if ((char.IsPunctuation(e.Text, e.Text.Length - 1)))
                        {
                            e.Handled = false;
                            return;
                        }
                    }

                    int countNeg = this.Text.ToCharArray().Where(w => w == this.negativeSign).Count();
                    if (countNeg < 1 && e.Text == this.negativeSign.ToString())
                    {
                        if ((char.IsPunctuation(e.Text, e.Text.Length - 1)))
                        {
                            e.Handled = false;
                            return;
                        }
                    }
                }

                if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                {
                    e.Handled = true;
                }

                string fullText = $"{this.Text}{e.Text}";
                if (fullText.Contains(this.decimalSeparator.ToString()) == true)
                {
                    int countDec = fullText.Split(this.decimalSeparator)[1].Length;
                    if (countDec > this.DecimalPlace)
                    {
                        e.Handled = true;
                    }
                }
            }
            else if (this.InputMode == TextBoxInputMode.CurrencyInput)
            {
                bool approvedDecimalPoint = false;


                if (e.Text == this.decimalSeparator.ToString())
                {
                    if ((this.Text.Contains(this.decimalSeparator) == false))
                    {
                        approvedDecimalPoint = true;
                    }
                }

                if (this.IsNegative == true)
                {
                    int countKomma = this.Text.ToCharArray().Where(w => w == this.decimalSeparator).Count();
                    if (countKomma < 1 && approvedDecimalPoint == true)
                    {
                        if ((char.IsPunctuation(e.Text, e.Text.Length - 1)))
                        {
                            e.Handled = false;
                            return;
                        }
                    }

                    int countNeg = this.Text.ToCharArray().Where(w => w == this.negativeSign).Count();
                    if (countNeg < 1 && e.Text == this.negativeSign.ToString())
                    {
                        if ((char.IsPunctuation(e.Text, e.Text.Length - 1)))
                        {
                            e.Handled = false;
                            return;
                        }
                    }
                }

                if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                {
                    e.Handled = true;
                }

                string fullText = $"{this.Text}{e.Text}";
                if (fullText.Contains(this.decimalSeparator.ToString()) == true)
                {
                    int countDec = fullText.Split(this.decimalSeparator)[1].Length;
                    if (countDec > 2)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Escape && this.EscapeClearsText == true)
            {
                if (this.InputMode.In(TextBoxInputMode.DigitInput, TextBoxInputMode.DecimalInput, TextBoxInputMode.CurrencyInput, TextBoxInputMode.PercentInput))
                {
                    this.Text = "0";
                }
            }

            if (this.InputMode == TextBoxInputMode.DigitInput)
            {
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

                if (this.IsNegative == false)
                {
                    if (e.Key == Key.OemMinus)
                    {
                        e.Handled = true;
                        return;
                    }
                }

                string keyText = e.Key.ToString().Replace("D", string.Empty).Replace("Oem", string.Empty);
                char outTemp;
                if (char.TryParse(keyText, out outTemp))
                {
                    if (char.IsDigit(outTemp) == true)
                    {
                        e.Handled = false;
                        return;
                    }
                    else
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
            else if (this.InputMode == TextBoxInputMode.DecimalInput)
            {
            }
            else if (this.InputMode == TextBoxInputMode.CurrencyInput)
            {
            }
            else if (this.InputMode == TextBoxInputMode.Letter)
            {
                string keyText = e.Key.ToString().Replace("D", string.Empty).Replace("Oem", string.Empty);
                char outTemp;
                if (char.TryParse(keyText, out outTemp))
                {
                    if (char.IsLetter(outTemp) == true)
                    {
                        e.Handled = false;
                        return;
                    }
                    else
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
            else if (this.InputMode == TextBoxInputMode.LetterOrDigit)
            {
                string keyText = e.Key.ToString().Replace("D", string.Empty).Replace("Oem", string.Empty);
                char outTemp;
                if (char.TryParse(keyText, out outTemp))
                {
                    if (char.IsLetterOrDigit(outTemp) == true)
                    {
                        e.Handled = false;
                        return;
                    }
                    else
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
            else if (this.InputMode == TextBoxInputMode.None)
            {
            }

            switch (e.Key)
            {
                case Key.Up:
                    this.MoveFocus(FocusNavigationDirection.Previous);
                    break;
                case Key.Down:
                    this.MoveFocus(FocusNavigationDirection.Next);
                    break;
                case Key.Return:
                    if (this.AcceptsReturn == true)
                    {
                        this.MoveFocus(FocusNavigationDirection.Next);
                    }
                    break;
                case Key.Tab:
                    break;
            }
        }

        private void MoveFocus(FocusNavigationDirection direction)
        {
            UIElement focusedElement = Keyboard.FocusedElement as UIElement;

            if (focusedElement != null)
            {
                if (focusedElement is TextBoxEx txt)
                {
                    focusedElement.MoveFocus(new TraversalRequest(direction));
                    txt.Select(0, txt.Text.Length);
                    txt.SelectAll();
                }
            }
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextBoxEx)d;

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
            copyMenu.Icon = Icons.GetPathGeometry(Icons.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopyMenu);
            textBoxContextMenu.Items.Add(copyMenu);

            if (this.IsReadOnly == false)
            {
                MenuItem pasteMenu = new MenuItem();
                pasteMenu.Header = "Einfügen";
                pasteMenu.Icon = Icons.GetPathGeometry(Icons.IconPaste);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(pasteMenu, "Click", this.OnPasteMenu);
                textBoxContextMenu.Items.Add(pasteMenu);

                MenuItem deleteMenu = new MenuItem();
                deleteMenu.Header = "Ausschneiden";
                deleteMenu.Icon = Icons.GetPathGeometry(Icons.IconDelete);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(deleteMenu, "Click", this.OnDeleteMenu);
                textBoxContextMenu.Items.Add(deleteMenu);

                if (this.InputMode.NotIn(TextBoxInputMode.DigitInput, TextBoxInputMode.DecimalInput, TextBoxInputMode.CurrencyInput, TextBoxInputMode.PercentInput, TextBoxInputMode.LetterOrDigit))
                {
                    MenuItem setDateMenu = new MenuItem();
                    setDateMenu.Header = "Setze Datum";
                    setDateMenu.Icon = Icons.GetPathGeometry(Icons.IconClock);
                    WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(setDateMenu, "Click", this.OnSetDateMenu);
                    textBoxContextMenu.Items.Add(setDateMenu);
                }
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
    }
}
