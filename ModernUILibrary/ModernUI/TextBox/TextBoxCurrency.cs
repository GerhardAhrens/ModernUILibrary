/*
 * <copyright file="TextBoxCurrency.cs" company="Lifeprojects.de">
 *     Class: TextBoxCurrency
 *     Copyright � Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>28.03.2024 09:00:36</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Control zur Eingabe von W�hrungen
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
 * --------------------------------------------------------------------
 * Fork from mtusk CurrencyTextBox
 * https://github.com/mtusk/wpf-currency-textbox
 * Fork 2016-2019 by Derek Tremblay (Abbaye) 
 * derektremblay666@gmail.com
*/

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    using ModernIU.Base;

    [ContentProperty("Number")]
    public class TextBoxCurrency : TextBox
    {
        private readonly List<decimal> _undoList = new List<decimal>();
        private readonly List<decimal> _redoList = new List<decimal>();
        private Popup _popup;
        private Label _popupLabel;
        private decimal _numberBeforePopup;

        /*Events */
        public event EventHandler PopupClosed;
        public event EventHandler NumberChanged;

        #region Constructor
        static TextBoxCurrency() =>
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TextBoxCurrency),
                    new FrameworkPropertyMetadata(typeof(TextBoxCurrency)));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.Margin = ControlBase.DefaultMargin;
            this.Height = ControlBase.DefaultHeight;
            this.HorizontalContentAlignment = HorizontalAlignment.Right;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.IsReadOnly = false;
            this.Focusable = true;

            /* Trigger an Style �bergeben */
            this.Style = this.SetTriggerFunction();

            /* Rahmen f�r Control festlegen */
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

            // Bind Text to Number with the specified StringFormat
            var textBinding = new Binding
            {
                Path = new PropertyPath(nameof(Number)),
                RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                StringFormat = StringFormat,
                ConverterCulture = Culture
            };

            BindingOperations.SetBinding(this, TextProperty, textBinding);

            /* Disable copy/paste */
            DataObject.AddCopyingHandler(this, CopyPasteEventHandler);
            DataObject.AddPastingHandler(this, CopyPasteEventHandler);

            /*Events */
            CaretIndex = Text.LastIndexOfAny(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) + 1;

            PreviewKeyDown += TextBox_PreviewKeyDown;
            PreviewMouseDown += TextBox_PreviewMouseDown;
            PreviewMouseUp += TextBox_PreviewMouseUp;
            TextChanged += TextBox_TextChanged;

            /* Disable contextmenu */
            ContextMenu = null;

            InputScope scope = new InputScope();
            scope.Names.Add(new InputScopeName
            {
                NameValue = InputScopeNameValue.Number
            });

            InputScope = scope;
        }
        #endregion Constructor

        #region Dependency Properties

        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(
            nameof(Culture), typeof(CultureInfo), typeof(TextBoxCurrency),
                new PropertyMetadata(CultureInfo.CurrentCulture, CulturePropertyChanged));

        private static void CulturePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBinding = new Binding
            {
                Path = new PropertyPath("Number"),
                RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                StringFormat = (string)d.GetValue(StringFormatProperty),
                ConverterCulture = (CultureInfo)e.NewValue
            };

            BindingOperations.SetBinding(d, TextProperty, textBinding);
        }

        public CultureInfo Culture
        {
            get => (CultureInfo)GetValue(CultureProperty);
            set => SetValue(CultureProperty, value);
        }

        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(
            nameof(Number), typeof(decimal), typeof(TextBoxCurrency),
                new FrameworkPropertyMetadata(0M, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    NumberPropertyChanged, NumberPropertyCoerceValue), NumberPropertyValidated);

        private static bool NumberPropertyValidated(object value) => value is decimal;

        private static object NumberPropertyCoerceValue(DependencyObject d, object baseValue)
        {
            if (!(d is TextBoxCurrency ctb)) return baseValue;

            var value = (decimal)baseValue;

            /*Check maximum value */
            if (value > ctb.MaximumValue && ctb.MaximumValue > 0)
                return ctb.MaximumValue;

            if (value < ctb.MinimumValue && ctb.MinimumValue < 0)
                return ctb.MinimumValue;

            return value;
        }

        private static void NumberPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBoxCurrency ctb)) return;

            /*Update IsNegative */
            ctb.SetValue(IsNegativeProperty, ctb.Number < 0);

            /*Launch event */
            ctb.NumberChanged?.Invoke(ctb, new EventArgs());
        }

        public decimal Number
        {
            get => (decimal)GetValue(NumberProperty);
            set => SetValue(NumberProperty, value);
        }

        public static readonly DependencyProperty IsNegativeProperty =
            DependencyProperty.Register(nameof(IsNegative), typeof(bool), typeof(TextBoxCurrency), new PropertyMetadata(false));

        public bool IsNegative => (bool)GetValue(IsNegativeProperty);

        public bool IsCalculPanelMode
        {
            get => (bool)GetValue(IsCalculPanelModeProperty);
            set => SetValue(IsCalculPanelModeProperty, value);
        }

        /* Using a DependencyProperty as the backing store for IsCalculPanelMode.  This enables animation, styling, binding, etc...*/
        public static readonly DependencyProperty IsCalculPanelModeProperty =
            DependencyProperty.Register(nameof(IsCalculPanelMode), typeof(bool), typeof(TextBoxCurrency), new PropertyMetadata(false));

        public bool CanShowAddPanel
        {
            get => (bool)GetValue(CanShowAddPanelProperty);
            set => SetValue(CanShowAddPanelProperty, value);
        }

        /// <summary>
        /// Set for enabling the calcul panel
        /// </summary>
        public static readonly DependencyProperty CanShowAddPanelProperty =
            DependencyProperty.Register(nameof(CanShowAddPanel), typeof(bool), typeof(TextBoxCurrency), new PropertyMetadata(false));

        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register(nameof(MaximumValue), typeof(decimal), typeof(TextBoxCurrency),
                new FrameworkPropertyMetadata(0M, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    MaximumValuePropertyChanged, MaximumCoerceValue), MaximumValidateValue);

        private static bool MaximumValidateValue(object value) => (decimal)value <= decimal.MaxValue / 2;

        private static object MaximumCoerceValue(DependencyObject d, object baseValue)
        {
            var ctb = d as TextBoxCurrency;

            if (ctb.MaximumValue > decimal.MaxValue / 2)
                return decimal.MaxValue / 2;

            return baseValue;
        }

        private static void MaximumValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctb = d as TextBoxCurrency;

            if (ctb.Number > (decimal)e.NewValue)
                ctb.Number = (decimal)e.NewValue;
        }

        public decimal MaximumValue
        {
            get => (decimal)GetValue(MaximumValueProperty);
            set => SetValue(MaximumValueProperty, value);
        }

        public decimal MinimumValue
        {
            get => (decimal)GetValue(MinimumValueProperty);
            set => SetValue(MinimumValueProperty, value);
        }

        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register(nameof(MinimumValue), typeof(decimal), typeof(TextBoxCurrency),
                new FrameworkPropertyMetadata(0M, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    MinimumValuePropertyChanged, MinimumCoerceValue), MinimumValidateValue);

        private static bool MinimumValidateValue(object value) =>
            (decimal)value >= decimal.MinValue / 2; //&& (decimal)value <= 0;

        private static object MinimumCoerceValue(DependencyObject d, object baseValue)
        {
            var ctb = d as TextBoxCurrency;

            if (ctb.MinimumValue < decimal.MinValue / 2)
                return decimal.MinValue / 2;

            return baseValue;
        }

        private static void MinimumValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctb = d as TextBoxCurrency;

            if (ctb.Number < (decimal)e.NewValue)
                ctb.Number = (decimal)e.NewValue;
        }

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(
            nameof(StringFormat), typeof(string), typeof(TextBoxCurrency),
            new FrameworkPropertyMetadata("C2", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                StringFormatPropertyChanged, StringFormatCoerceValue), StringFormatValidateValue);

        private static object StringFormatCoerceValue(DependencyObject d, object baseValue) =>
            ((string)baseValue).ToUpper();

        /// <summary>
        /// Validate the StringFormat
        /// </summary>
        private static bool StringFormatValidateValue(object value)
        {
            var val = value.ToString().ToUpper();

            return val == "C0" || val == "C" || val == "C1" || val == "C2" || val == "C3" || val == "C4" || val == "C5" || val == "C6" ||
                val == "N0" || val == "N" || val == "N1" || val == "N2" || val == "N3" || val == "N4" || val == "N5" || val == "N6" ||
                val == "P0" || val == "P" || val == "P1" || val == "P2" || val == "P3" || val == "P4" || val == "P5" || val == "P6";
        }

        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }

        private static void StringFormatPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            /* Update the Text binding with the new StringFormat */
            var textBinding = new Binding
            {
                Path = new PropertyPath("Number"),
                RelativeSource = new RelativeSource(RelativeSourceMode.Self),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                StringFormat = (string)e.NewValue,
                ConverterCulture = (CultureInfo)obj.GetValue(CultureProperty)
            };

            BindingOperations.SetBinding(obj, TextProperty, textBinding);
        }

        public int UpDownRepeat
        {
            get => (int)GetValue(UpDownRepeatProperty);
            set => SetValue(UpDownRepeatProperty, value);
        }

        /// <summary>
        /// Set the Up/down value when key repeated
        /// </summary>
        public static readonly DependencyProperty UpDownRepeatProperty =
            DependencyProperty.Register(nameof(UpDownRepeat), typeof(int), typeof(TextBoxCurrency), new PropertyMetadata(10));

        public static readonly DependencyProperty ReadOnlyColorProperty = DependencyProperty.Register("ReadOnlyColor", typeof(Brush), typeof(TextBoxCurrency), new PropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxCurrency), new PropertyMetadata(true, OnSetBorderChanged));

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

        #endregion Dependency Properties

        public void Insert(Key K)
        {
            AddUndoInList(Number);
            InsertKey(K);
        }


        #region Events
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            tb.CaretIndex = tb.Text.LastIndexOfAny(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }) + 1;
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            /* Prevent changing the caret index */
            e.Handled = true;
            (sender as TextBox).Focus();
        }

        private void TextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            /* Prevent changing the caret index */
            e.Handled = true;
            (sender as TextBox).Focus();
        }

        /// <summary>
        /// Action when is key pressed
        /// </summary>
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (IsReadOnly)
            {
                e.Handled = true;
                return;
            }

            if (KeyValidator.IsNumericKey(e.Key))
            {
                e.Handled = true;

                AddUndoInList(Number);
                InsertKey(e.Key);
            }
            else if (KeyValidator.IsBackspaceKey(e.Key))
            {
                e.Handled = true;

                AddUndoInList(Number);
                RemoveRightMostDigit();
            }
            else if (KeyValidator.IsUpKey(e.Key))
            {
                e.Handled = true;

                AddUndoInList(Number);
                AddOneDigit();

                /*if the key is repeated add more digit */
                if (e.IsRepeat)
                {
                    AddUndoInList(Number);
                    AddOneDigit(UpDownRepeat);
                }
            }
            else if (KeyValidator.IsDownKey(e.Key))
            {
                e.Handled = true;

                AddUndoInList(Number);
                SubstractOneDigit();

                /*if the key is repeated substract more digit */
                if (e.IsRepeat)
                {
                    AddUndoInList(Number);
                    SubstractOneDigit(UpDownRepeat);
                }
            }
            else if (KeyValidator.IsCtrlZKey(e.Key))
            {
                e.Handled = true;

                Undo();
            }
            else if (KeyValidator.IsCtrlYKey(e.Key))
            {
                e.Handled = true;

                Redo();
            }
            else if (KeyValidator.IsEnterKey(e.Key))
            {
                if (!IsCalculPanelMode)
                {
                    AddUndoInList(Number);
                    ShowAddPopup();
                }
                else
                {
                    ((Popup)((Grid)Parent).Parent).IsOpen = false;

                    if (PopupClosed != null)
                    {
                        e.Handled = true;
                        PopupClosed(this, new EventArgs());
                    }
                }
            }
            else if (KeyValidator.IsDeleteKey(e.Key))
            {
                e.Handled = true;

                AddUndoInList(Number);
                Clear();
            }
            else if (KeyValidator.IsSubstractKey(e.Key))
            {
                e.Handled = true;

                AddUndoInList(Number);
                InvertValue();
            }
            else if (KeyValidator.IsIgnoredKey(e.Key))
            {
                e.Handled = false;
            }
            else if (KeyValidator.IsCtrlCKey(e.Key))
            {
                e.Handled = true;

                CopyToClipBoard();
            }
            else if (KeyValidator.IsCtrlVKey(e.Key))
            {
                e.Handled = true;

                AddUndoInList(Number);
                PasteFromClipBoard();
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// cancel copy and paste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyPasteEventHandler(object sender, DataObjectEventArgs e) => e.CancelCommand();

        #endregion

        #region Private Methods       
        private void Ctb_NumberChanged(object sender, EventArgs e)
        {
            var ctb = sender as TextBoxCurrency;

            Number = _numberBeforePopup + ctb.Number;

            _popupLabel.Content = ctb.Number >= 0 ? "+" : "-";
        }

        /// <summary>
        /// Insert number from key
        /// </summary>
        private void InsertKey(Key key)
        {
            /*Max length fix */
            if (MaxLength != 0 && Number.ToString(Culture).Length > MaxLength)
                return;

            try
            {
                /* Push the new number from the right */
                if (KeyValidator.IsNumericKey(key))
                    Number = Number < 0
                        ? Number * 10M - GetDigitFromKey(key) / GetDivider()
                        : Number * 10M + GetDigitFromKey(key) / GetDivider();
            }
            catch (OverflowException)
            {
                Number = Number < 0 ? decimal.MinValue : decimal.MaxValue;
            }

        }

        /// <summary>
        /// Get the digit from key
        /// </summary>        
        private static decimal GetDigitFromKey(Key key)
        {
            switch (key)
            {
                case Key.D0:
                case Key.NumPad0: return 0M;
                case Key.D1:
                case Key.NumPad1: return 1M;
                case Key.D2:
                case Key.NumPad2: return 2M;
                case Key.D3:
                case Key.NumPad3: return 3M;
                case Key.D4:
                case Key.NumPad4: return 4M;
                case Key.D5:
                case Key.NumPad5: return 5M;
                case Key.D6:
                case Key.NumPad6: return 6M;
                case Key.D7:
                case Key.NumPad7: return 7M;
                case Key.D8:
                case Key.NumPad8: return 8M;
                case Key.D9:
                case Key.NumPad9: return 9M;
                default: throw new ArgumentOutOfRangeException($"Invalid key: {key}");
            }
        }

        /// <summary>
        /// Get a decimal for adjust digit when a key was inserted
        /// </summary>
        private decimal GetDivider()
        {
            switch (GetBindingExpression(TextProperty).ParentBinding.StringFormat)
            {
                case "N0":
                case "C0": return 1M;
                case "N":
                case "C": return 100M;
                case "N1":
                case "C1": return 10M;
                case "N2":
                case "C2": return 100M;
                case "N3":
                case "C3": return 1000M;
                case "N4":
                case "C4": return 10000M;
                case "N5":
                case "C5": return 100000M;
                case "N6":
                case "C6": return 1000000M;
                case "P0": return 100M;
                case "P": return 10000M;
                case "P1": return 1000M;
                case "P2": return 10000M;
                case "P3": return 100000M;
                case "P4": return 1000000M;
                case "P5": return 10000000M;
                case "P6": return 100000000M;
                default: return 1M;
            }
        }

        /// <summary>
        /// Get the number of digit .
        /// </summary>
        private int GetDigitCount()
        {
            if (string.IsNullOrEmpty(StringFormat)) return 1;
            if (string.Equals("N", StringFormat, StringComparison.OrdinalIgnoreCase)) return 2;
            if (string.Equals("C", StringFormat, StringComparison.OrdinalIgnoreCase)) return 2;

            var s = StringFormat.Substring(StringFormat.Length - 1, 1);
            int resp;
            if (int.TryParse(s, NumberStyles.Integer, Culture, out resp)) return resp;

            return 1;
        }

        /// <summary>
        /// Delete the right digit of number property
        /// </summary>
        private void RemoveRightMostDigit()
        {
            try
            {
                bool isNegative = Number < 0;
                var digitCount = GetDigitCount();
                string decimalSeparator = !string.IsNullOrEmpty(StringFormat) && StringFormat.StartsWith("C", StringComparison.OrdinalIgnoreCase)
                                           ? Culture.NumberFormat.CurrencyDecimalSeparator : Culture.NumberFormat.NumberDecimalSeparator;

                string numberString = Math.Abs(Number).ToString("#.###########", Culture);
                numberString = numberString.Substring(0, numberString.Length - 1);
                numberString = numberString.Replace(decimalSeparator, string.Empty);
                numberString = numberString.PadLeft(digitCount + 1, '0');

                numberString = (isNegative ? Culture.NumberFormat.NegativeSign : string.Empty) +
                               numberString.Substring(0, numberString.Length - digitCount) +
                               Culture.NumberFormat.NumberDecimalSeparator +
                               numberString.Substring(numberString.Length - digitCount);

                Number = Convert.ToDecimal(numberString, Culture);
            }
            catch
            {
                Clear();
            }
        }
        #endregion Privates methodes

        #region Undo/Redo 

        /// <summary>
        /// Add undo to the list
        /// </summary>
        private void AddUndoInList(decimal number, bool clearRedo = true)
        {
            //Clear first item when undolimit is reach
            if (_undoList.Count == UndoLimit)
                _undoList.RemoveRange(0, 1);

            //Add item to undo list
            _undoList.Add(number);

            //Clear redo when needed
            if (clearRedo)
                _redoList.Clear();
        }

        /// <summary>
        /// Undo the to the previous value
        /// </summary>
        public new void Undo()
        {
            if (!CanUndo()) return;

            Number = _undoList[_undoList.Count - 1];

            _redoList.Add(_undoList[_undoList.Count - 1]);
            _undoList.RemoveAt(_undoList.Count - 1);
        }

        /// <summary>
        /// Redo to the value previously undone. The list is clear when key is handled
        /// </summary>
        public new void Redo()
        {
            if (_redoList.Count <= 0) return;

            AddUndoInList(Number, false);
            Number = _redoList[_redoList.Count - 1];
            _redoList.RemoveAt(_redoList.Count - 1);
        }

        /// <summary>
        /// Get or set for indicate if control CanUndo
        /// </summary>
        public new bool IsUndoEnabled { get; set; } = true;

        /// <summary>
        /// Clear the undo list
        /// </summary>
        public void ClearUndoList() => _undoList.Clear();

        /// <summary>
        /// Check if the control can undone to a previous value
        /// </summary>
        /// <returns></returns>
        public new bool CanUndo() => IsUndoEnabled && _undoList.Count > 0;

        #endregion Undo/Redo

        #region Public Methods

        /// <summary>
        /// Reset the number to zero.
        /// </summary>
        public new void Clear() => Number = 0M;

        /// <summary>
        /// Set number to positive
        /// </summary>
        public void SetPositive() { if (Number < 0) Number *= -1; }

        /// <summary>
        /// Set number to negative
        /// </summary>
        public void SetNegative() { if (Number > 0) Number *= -1; }

        /// <summary>
        /// Alternate value to Negative-Positive and Positive-Negative
        /// </summary>
        public void InvertValue() => Number *= -1;

        /// <summary>
        /// Add one digit to the property number
        /// </summary>
        /// <param name="repeat">Repeat add</param>
        public void AddOneDigit(int repeat = 1)
        {
            for (var i = 0; i < repeat; i++)
                switch (GetBindingExpression(TextProperty).ParentBinding.StringFormat)
                {
                    case "P0":
                        Number = decimal.Add(Number, 0.01M);
                        break;
                    case "N0":
                    case "C0":
                        Number = decimal.Add(Number, 1M);
                        break;
                    case "P":
                        Number = decimal.Add(Number, 0.0001M);
                        break;
                    case "N":
                    case "C":
                        Number = decimal.Add(Number, 0.01M);
                        break;
                    case "P1":
                        Number = decimal.Add(Number, 0.001M);
                        break;
                    case "N1":
                    case "C1":
                        Number = decimal.Add(Number, 0.1M);
                        break;
                    case "P2":
                        Number = decimal.Add(Number, 0.0001M);
                        break;
                    case "N2":
                    case "C2":
                        Number = decimal.Add(Number, 0.01M);
                        break;
                    case "P3":
                        Number = decimal.Add(Number, 0.00001M);
                        break;
                    case "N3":
                    case "C3":
                        Number = decimal.Add(Number, 0.001M);
                        break;
                    case "P4":
                        Number = decimal.Add(Number, 0.000001M);
                        break;
                    case "N4":
                    case "C4":
                        Number = decimal.Add(Number, 0.0001M);
                        break;
                    case "P5":
                        Number = decimal.Add(Number, 0.0000001M);
                        break;
                    case "N5":
                    case "C5":
                        Number = decimal.Add(Number, 0.00001M);
                        break;
                    case "P6":
                        Number = decimal.Add(Number, 0.00000001M);
                        break;
                    case "N6":
                    case "C6":
                        Number = decimal.Add(Number, 0.000001M);
                        break;
                }
        }

        /// <summary>
        /// Substract one digit to the property number
        /// </summary>
        /// <param name="repeat">Repeat substract</param>
        public void SubstractOneDigit(int repeat = 1)
        {
            for (var i = 0; i < repeat; i++)
                switch (GetBindingExpression(TextProperty).ParentBinding.StringFormat)
                {
                    case "P0":
                        Number = decimal.Subtract(Number, 0.01M);
                        break;
                    case "N0":
                    case "C0":
                        Number = decimal.Subtract(Number, 1M);
                        break;
                    case "P":
                        Number = decimal.Subtract(Number, 0.0001M);
                        break;
                    case "N":
                    case "C":
                        Number = decimal.Subtract(Number, 0.01M);
                        break;
                    case "P1":
                        Number = decimal.Subtract(Number, 0.001M);
                        break;
                    case "N1":
                    case "C1":
                        Number = decimal.Subtract(Number, 0.1M);
                        break;
                    case "P2":
                        Number = decimal.Subtract(Number, 0.0001M);
                        break;
                    case "N2":
                    case "C2":
                        Number = decimal.Subtract(Number, 0.01M);
                        break;
                    case "P3":
                        Number = decimal.Subtract(Number, 0.00001M);
                        break;
                    case "N3":
                    case "C3":
                        Number = decimal.Subtract(Number, 0.001M);
                        break;
                    case "P4":
                        Number = decimal.Subtract(Number, 0.000001M);
                        break;
                    case "N4":
                    case "C4":
                        Number = decimal.Subtract(Number, 0.0001M);
                        break;
                    case "P5":
                        Number = decimal.Subtract(Number, 0.0000001M);
                        break;
                    case "N5":
                    case "C5":
                        Number = decimal.Subtract(Number, 0.00001M);
                        break;
                    case "P6":
                        Number = decimal.Subtract(Number, 0.00000001M);
                        break;
                    case "N6":
                    case "C6":
                        Number = decimal.Subtract(Number, 0.000001M);
                        break;
                }
        }

        #endregion Other function

        #region Clipboard
        /// <summary>
        /// Paste if is a number on clipboard
        /// </summary>
        private void PasteFromClipBoard()
        {
            try
            {
                switch (GetBindingExpression(TextProperty).ParentBinding.StringFormat)
                {
                    case "P0":
                    case "P":
                    case "P1":
                    case "P2":
                    case "P3":
                    case "P4":
                    case "P5":
                    case "P6":
                        Number = decimal.Parse(Clipboard.GetText());
                        break;
                    default:
                        Number = Math.Round(decimal.Parse(Clipboard.GetText()), GetDigitCount());
                        break;
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Copy the property Number to Control
        /// </summary>
        private void CopyToClipBoard()
        {
            Clipboard.Clear();
            Clipboard.SetText(Number.ToString(Culture));
        }
        #endregion Clipboard

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextBoxCurrency)d;

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

            /* Trigger f�r IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = TextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger f�r IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = TextBox.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsFocused);

            /* Trigger f�r IsFocused = True */
            Trigger triggerIsReadOnly = new Trigger();
            triggerIsReadOnly.Property = TextBox.IsReadOnlyProperty;
            triggerIsReadOnly.Value = true;
            triggerIsReadOnly.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightYellow });
            inputControlStyle.Triggers.Add(triggerIsReadOnly);

            return inputControlStyle;
        }

        #region Add/remove value Popup 
        /// <summary>
        /// Show popup for add/remove value
        /// </summary>
        private void ShowAddPopup()
        {
            if (!CanShowAddPanel) return;

            //Initialize somes Child object
            var grid = new Grid { Background = Brushes.White };

            var ctbPopup = new TextBoxCurrency
            {
                CanShowAddPanel = false,
                IsCalculPanelMode = true,
                StringFormat = StringFormat,
                Background = Brushes.WhiteSmoke
            };

            _popup = new Popup
            {
                Width = ActualWidth,
                Height = 32,
                PopupAnimation = PopupAnimation.Fade,
                Placement = PlacementMode.Bottom,
                PlacementTarget = this,
                StaysOpen = false,
                Child = grid,
                IsOpen = true
            };

            //Set object properties                                         
            ctbPopup.NumberChanged += Ctb_NumberChanged;
            ctbPopup.PopupClosed += CtbPopup_PopupClosed;

            _numberBeforePopup = Number;
            _popupLabel = new Label { Content = "+" };

            //ColumnDefinition
            var c1 = new ColumnDefinition { Width = new GridLength(20, GridUnitType.Auto) };
            var c2 = new ColumnDefinition { Width = new GridLength(80, GridUnitType.Star) };
            grid.ColumnDefinitions.Add(c1);
            grid.ColumnDefinitions.Add(c2);
            Grid.SetColumn(_popupLabel, 0);
            Grid.SetColumn(ctbPopup, 1);

            //Add object 
            grid.Children.Add(_popupLabel);
            grid.Children.Add(ctbPopup);

            ctbPopup.Focus();
        }

        private void CtbPopup_PopupClosed(object sender, EventArgs e) => Focus();
        #endregion Add/remove value Popup
    }
}
