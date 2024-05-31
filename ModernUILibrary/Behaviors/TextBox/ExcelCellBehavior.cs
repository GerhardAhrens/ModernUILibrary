/*
 * <copyright file="ExcelCellBehavior.cs" company="Lifeprojects.de">
 *     Class: ExcelCellBehavior
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>01.02.2023 15:53:50</date>
 * <Project>CurrentProject</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernIU.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    public static class ExcelCellBehavior
    {
        private static Brush _selectionBrush;
        private static Brush _caretBrush;

        #region Active Dependency Property

        /// <summary>
        /// Gets the active.
        /// </summary>
        /// <param name="object">The @object.</param>
        [AttachedPropertyBrowsableForChildrenAttribute(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetActive(DependencyObject @object)
        {
            return (bool)@object.GetValue(ActiveProperty);
        }

        public static void SetActive(DependencyObject @object, bool value)
        {
            @object.SetValue(ActiveProperty, value);
        }

        public static readonly DependencyProperty ActiveProperty = DependencyProperty.RegisterAttached(
            "Active", typeof(bool), typeof(ExcelCellBehavior), new PropertyMetadata(false, ActivePropertyChanged));

        private static void ActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = d as TextBox;

            if (textBox != null)
            {
                // Save the initial state of textbox
                _caretBrush = textBox.CaretBrush;
                _selectionBrush = textBox.SelectionBrush;

                if ((e.NewValue as bool?).GetValueOrDefault(false))
                {
                    textBox.PreviewKeyDown += OnPreviewKeyDown;
                    textBox.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
                    textBox.PreviewMouseDoubleClick += OnMouseDoubleClick;

                    textBox.GotFocus += HandleGotFocus;
                }
                else
                {
                    textBox.PreviewKeyDown -= OnPreviewKeyDown;
                    textBox.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
                    textBox.PreviewMouseDoubleClick -= OnMouseDoubleClick;

                    textBox.GotFocus -= HandleGotFocus;
                }
            }
        }

        #endregion

        #region EventHandlers

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // No need to start editing if any modifier key(ctrl, Alt, Shift) Or 
            // navigation keys(Up/Down) is pressed Or it is a repeated key down event notification
            if (e.KeyboardDevice.Modifiers != ModifierKeys.None || e.Key == Key.Down || e.Key == Key.Up || e.IsRepeat)
            {
                return;
            }

            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                if (e.Key == Key.Enter && IsTextBoxInExcelMode(textBox))
                {
                    // No need to change the mode if Enter key is pressed while excel mode is active.
                    return;
                }

                if (e.Key == Key.Escape)
                {
                    // change the state to initial editing state
                    SetSelectedState(textBox);
                }
                else
                {
                    SetDefaultState(textBox);
                }
            }
        }

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null && textBox.IsKeyboardFocusWithin == false)
            {
                textBox.Focus();
                SetSelectedState(textBox);
                e.Handled = true;
            }
            else if (IsTextBoxInExcelMode(textBox))
            {
                // If textbox is in excel mode and left mouse button is pressed then nothing should happen
                e.Handled = true;
            }
        }

        private static void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                SetDefaultState(textBox);

                textBox.Focus();
                textBox.SelectAll();
                e.Handled = true;
            }
        }

        private static void HandleGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.Focus();
                SetSelectedState(textBox);
                e.Handled = true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the default state to a TextBox.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        private static void SetDefaultState(TextBox textBox)
        {
            textBox.SelectionBrush = _selectionBrush;
            textBox.CaretBrush = _caretBrush;
        }

        /// <summary>
        /// Set the selected state to a TextBox.
        /// </summary>
        /// <param name="textBox">The text box.</param>
        private static void SetSelectedState(TextBox textBox)
        {
            textBox.CaretBrush = Brushes.Transparent;
            textBox.SelectionBrush = Brushes.Transparent;
            textBox.SelectAll();
        }

        /// <summary>
        /// Determines whether the specified text box is in excel mode or not .
        /// </summary>
        /// <param name="textBox">The text box.</param>
        /// <returns><c>true</c> if [the specified text box] [is in excel mode] ; otherwise, <c>false</c>.</returns>
        public static bool IsTextBoxInExcelMode(TextBoxBase textBox)
        {
            return textBox.CaretBrush == Brushes.Transparent && textBox.SelectionBrush == Brushes.Transparent;
        }
        #endregion
    }
}
