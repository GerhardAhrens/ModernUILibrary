//-----------------------------------------------------------------------
// <copyright file="TextBoxMath.cs" company="Lifeprojects.de">
//     Class: TextBoxMath
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>02.02.2024</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------


namespace ModernIU.Controls
{
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernIU.Base;

    public class TextBoxMath : TextBox
    {
        private static Regex regexDisallowedInteger = new Regex(@"[^0-9-]+");  // matches disallowed text
        private static Regex regexDisallowedFloat = new Regex(@"[^0-9-+.,e]+");  // matches disallowed text

        public static readonly DependencyProperty ReadOnlyColorProperty =
            DependencyProperty.Register("ReadOnlyColor", typeof(Brush), typeof(TextBoxMath), new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty SetBorderProperty =
            DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxMath), new PropertyMetadata(true, OnSetBorderChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxMath"/> class.
        /// </summary>
        public TextBoxMath()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 23;
            this.FontSize = 14;
            this.IsReadOnly = false;
            this.Focusable = true;

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();

            WeakEventManager<TextBoxMath, TextCompositionEventArgs>.AddHandler(this,"PreviewTextInput", this.OnPreviewTextInput);
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
            this.CaretIndex = this.Text.Length;
            this.SelectAll();

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

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Text.Length > 0 && OpenMathMenu(e.Text))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = regexDisallowedFloat.IsMatch(e.Text);  // or regexDisallowedInteger
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
            {
                if (e.Key == Key.Tab)
                {
                    return;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Up:
                        this.MoveFocus(FocusNavigationDirection.Previous);
                        break;
                    case Key.Down:
                        this.MoveFocus(FocusNavigationDirection.Next);
                        break;
                    case Key.Left:
                        return;
                    case Key.Right:
                        return;
                    case Key.Pa1:
                        return;
                    case Key.End:
                        return;
                    case Key.Delete:
                        return;
                    case Key.Return:
                        this.MoveFocus(FocusNavigationDirection.Next);
                        break;
                    case Key.Tab:
                        this.MoveFocus(FocusNavigationDirection.Next);
                        break;
                }
            }
        }

        private void MoveFocus(FocusNavigationDirection direction)
        {
            UIElement focusedElement = Keyboard.FocusedElement as UIElement;

            if (focusedElement != null)
            {
                if (focusedElement is TextBox)
                {
                    focusedElement.MoveFocus(new TraversalRequest(direction));
                }
            }
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextBoxMath)d;

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
            }

            return textBoxContextMenu;
        }

        private void OnCopyMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.Text);
        }

        private void OnPasteMenu(object sender, RoutedEventArgs e)
        {
            this.Text = Clipboard.GetText();
        }

        private void OnDeleteMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.Text);
            this.Text = string.Empty;
        }

        #region Math
        private enum EOperation { Add, Subtract, Multiply, Divide, Percent }
        private EOperation operation = EOperation.Add;
        private ContextMenu menuMath = null;
        private MenuItem miOperand = null;
        private MenuItem miResult = null;

        private bool OpenMathMenu(string text)
        {
            if (menuMath != null)
                return false;

            if (text == "+" || text == "-" || text == "*" || text == "/" || text == "%")
            {
                if (text == "-" && CaretIndex == 0)  // negative sign in front of number
                    return false;

                miOperand = new MenuItem();
                miOperand.Header = string.Empty;
                miOperand.FontSize = 18;
                miOperand.FontWeight = FontWeights.Medium;
                miOperand.IsEnabled = false;

                if (text == "+")
                {
                    operation = EOperation.Add;
                    miOperand.Icon = Icons.GetPathGeometry(Icons.IconAdd);
                }
                else if (text == "-")
                {
                    operation = EOperation.Subtract;
                    miOperand.Icon = Icons.GetPathGeometry(Icons.IconMinus);
                }
                else if (text == "*")
                {
                    operation = EOperation.Multiply;
                    miOperand.Icon = Icons.GetPathGeometry(Icons.IconMult);
                }
                else if (text == "/")
                {
                    operation = EOperation.Divide;
                    miOperand.Icon = Icons.GetPathGeometry(Icons.IconDiv);
                }
                else if (text == "%")
                {
                    operation = EOperation.Percent;
                    miOperand.Icon = Icons.GetPathGeometry(Icons.IconPercent);
                }

                miResult = new MenuItem();
                miResult.Header = string.Empty;
                miResult.Icon = Icons.GetPathGeometry(Icons.IconEquals);
                miResult.FontSize = 18;
                miResult.FontWeight = FontWeights.Medium;
                miResult.IsEnabled = false;
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(miResult, "Click", this.OnResultClick);

                menuMath = new ContextMenu();
                menuMath.Items.Add(miOperand);
                menuMath.Items.Add(new Separator());
                menuMath.Items.Add(miResult);
                menuMath.PlacementTarget = this;
                menuMath.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

                WeakEventManager<ContextMenu, KeyEventArgs>.AddHandler(menuMath, "PreviewKeyDown", this.OnMathPreviewKeyDown);
                WeakEventManager<ContextMenu, RoutedEventArgs>.AddHandler(menuMath, "Closed", this.OnMathClosed);
                menuMath.IsOpen = true;
                return true;
            }

            return false;
        }

        private void OnMathPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                return;
            }

            string operand = miOperand.Header.ToString();
            switch (e.Key)
            {
                case Key.Cancel:
                case Key.Clear:
                case Key.Escape:
                case Key.OemClear:
                    miResult.Header = "";
                    OnResultClick(this, null);
                    break;

                case Key.Back:
                case Key.Delete:
                    if (operand.Length > 0)
                    {
                        UpdateResult(operand.Substring(0, operand.Length - 1));
                    }
                    else
                    {
                        OnResultClick(this, null);
                    }
                    break;

                case Key.LineFeed:
                case Key.Enter:
                    OnResultClick(this, null);
                    break;

                case Key.OemPlus:  // '='
                    OnResultClick(this, null);
                    break;

                case Key.Subtract:
                case Key.OemMinus:
                    if (operand.Length == 0)
                        miOperand.Header = "-";
                    break;

                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                    char key1 = (char)((e.Key - Key.D0) + '0');
                    if (char.IsDigit(key1))
                        UpdateResult(operand + key1);
                    break;

                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    char key2 = (char)((e.Key - Key.NumPad0) + '0');
                    if (char.IsDigit(key2))
                        UpdateResult(operand + key2);
                    break;

                case Key.OemComma:
                    if (!operand.Contains(","))
                        miOperand.Header = operand + ",";
                    break;

                case Key.Decimal:
                case Key.OemPeriod:
                    if (!operand.Contains("."))
                        miOperand.Header = operand + ".";
                    break;

                    //case Key.Multiply:
                    //case Key.Add:
                    //case Key.Divide:
            }
        }

        private void UpdateResult(string operand)
        {
            miOperand.Header = operand;
            double dop1, dop2, result;
            if (double.TryParse(Text, out dop1) && double.TryParse(operand, out dop2))
            {
                switch (operation)
                {
                    case EOperation.Add:
                        result = dop1 + dop2;
                        miResult.Header = result.ToString();
                        miResult.IsEnabled = true;
                        break;

                    case EOperation.Subtract:
                        result = dop1 - dop2;
                        miResult.Header = result.ToString();
                        miResult.IsEnabled = true;
                        break;

                    case EOperation.Multiply:
                        result = dop1 * dop2;
                        miResult.Header = result.ToString();
                        miResult.IsEnabled = true;
                        break;

                    case EOperation.Divide:
                        if (dop2 == 0.0)
                        {
                            miResult.Header = "";
                            miResult.IsEnabled = false;
                        }
                        else
                        {
                            result = dop1 / dop2;
                            if (double.IsInfinity(result) || double.IsNaN(result))
                            {
                                miResult.Header = "";
                                miResult.IsEnabled = false;
                            }
                            else
                            {
                                miResult.Header = result.ToString();
                                miResult.IsEnabled = true;
                            }
                        }
                        break;
                    case EOperation.Percent:
                        result = (dop1 / dop2) * 100;
                        miResult.Header = result.ToString();
                        miResult.IsEnabled = true;
                        break;
                }
            }
            else
            {
                miResult.Header = "";
                miResult.IsEnabled = false;
            }
        }

        private void OnResultClick(object sender, RoutedEventArgs e)
        {
            string result = miResult.Header.ToString();
            if (result.Length > 0)
            {
                Text = result;
                CaretIndex = Text.Length;
            }

            menuMath.IsOpen = false;
        }

        private void OnMathClosed(object sender, RoutedEventArgs e)
        {
            WeakEventManager<ContextMenu, KeyEventArgs>.RemoveHandler(menuMath, "PreviewKeyDown", this.OnMathPreviewKeyDown);
            WeakEventManager<ContextMenu, RoutedEventArgs>.RemoveHandler(menuMath, "Closed", this.OnMathClosed);
            WeakEventManager<MenuItem, RoutedEventArgs>.RemoveHandler(miResult, "Click", this.OnResultClick);

            menuMath = null;
            miOperand = null;
            miResult = null;
        }
        #endregion Math
    }
}
