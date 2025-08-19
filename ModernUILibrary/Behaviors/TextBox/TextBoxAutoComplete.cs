//-----------------------------------------------------------------------
// <copyright file="TextBoxAutoComplete.cs" company="Lifeprojects.de">
//     Class: TextBoxAutoComplete
//     Copyright © Gerhard Ahrens, 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>19.12.2022</date>
//
// <summary>
// Autocomplete Erwiterung für eine TextBox
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Microsoft.Xaml.Behaviors;

    public class TextBoxAutoComplete
    {
        private static TextChangedEventHandler onTextChanged = new TextChangedEventHandler(OnTextChanged);
        private static KeyEventHandler onKeyDown = new KeyEventHandler(OnPreviewKeyDown);

        /// <summary>
        /// The collection to search for matches from.
        /// </summary>
        public static readonly DependencyProperty AutoCompleteItemsSourceProperty =
            DependencyProperty.RegisterAttached
            (
                "AutoCompleteItemsSource",
                typeof(IEnumerable<string>),
                typeof(TextBoxAutoComplete),
                new UIPropertyMetadata(null, OnAutoCompleteItemsSource)
            );
        /// <summary>
        /// Whether or not to ignore case when searching for matches.
        /// </summary>
        public static readonly DependencyProperty AutoCompleteStringComparisonProperty =
            DependencyProperty.RegisterAttached
            (
                "AutoCompleteStringComparison",
                typeof(StringComparison),
                typeof(TextBoxAutoComplete),
                new UIPropertyMetadata(StringComparison.Ordinal)
            );

        /// <summary>
        /// What string should indicate that we should start giving auto-completion suggestions.  For example: @
        /// If this is null or empty, auto-completion suggestions will begin at the beginning of the textbox's text.
        /// </summary>
        public static readonly DependencyProperty AutoCompleteIndicatorProperty =
            DependencyProperty.RegisterAttached
            (
                "AutoCompleteIndicator",
                typeof(string),
                typeof(TextBoxAutoComplete),
                new UIPropertyMetadata(string.Empty)
            );

        #region Items Source
        public static IEnumerable<string> GetAutoCompleteItemsSource(DependencyObject obj)
        {
            if (obj != null)
            {
                object objRtn = obj.GetValue(AutoCompleteItemsSourceProperty);
                if (objRtn is IEnumerable<string>)
                {
                    return objRtn as IEnumerable<string>;
                }
            }

            return null;
        }

        public static void SetAutoCompleteItemsSource(DependencyObject obj, IEnumerable<string> value)
        {
            obj.SetValue(AutoCompleteItemsSourceProperty, value);
        }

        private static void OnAutoCompleteItemsSource(object sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (sender == null)
            {
                return;
            }

            //If we're being removed, remove the callbacks
            //Remove our old handler, regardless of if we have a new one.
            tb.TextChanged -= onTextChanged;
            tb.PreviewKeyDown -= onKeyDown;
            if (e.NewValue != null)
            {
                //New source.  Add the callbacks
                tb.TextChanged += onTextChanged;
                tb.PreviewKeyDown += onKeyDown;
            }
        }
        #endregion

        #region String Comparison
        public static StringComparison GetAutoCompleteStringComparison(DependencyObject obj)
        {
            return (StringComparison)obj.GetValue(AutoCompleteStringComparisonProperty);
        }

        public static void SetAutoCompleteStringComparison(DependencyObject obj, StringComparison value)
        {
            obj.SetValue(AutoCompleteStringComparisonProperty, value);
        }
        #endregion

        #region Indicator
        public static string GetAutoCompleteIndicator(DependencyObject obj)
        {
            return (string)obj.GetValue(AutoCompleteIndicatorProperty);
        }

        public static void SetAutoCompleteIndicator(DependencyObject obj, string value)
        {
            obj.SetValue(AutoCompleteIndicatorProperty, value);
        }
        #endregion

        /// <summary>
        /// Used for moving the caret to the end of the suggested auto-completion text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            TextBox tb = e.OriginalSource as TextBox;
            if (tb == null)
            {
                return;
            }

            //If we pressed enter and if the selected text goes all the way to the end, move our caret position to the end
            if (tb.SelectionLength > 0 && tb.SelectionStart + tb.SelectionLength == tb.Text.Length)
            {
                tb.SelectionStart = tb.CaretIndex = tb.Text.Length;
                tb.SelectionLength = 0;
            }
        }

        /// <summary>
        /// Search for auto-completion suggestions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if ((from change in e.Changes where change.RemovedLength > 0 select change).Any() && (from change in e.Changes where change.AddedLength > 0 select change).Any() == false)
            {
                return;
            }

            TextBox tb = e.OriginalSource as TextBox;
            if (sender == null)
            {
                return;
            }

            IEnumerable<string> values = GetAutoCompleteItemsSource(tb);
            //No reason to search if we don't have any values.
            if (values == null)
            {
                return;
            }

            //No reason to search if there's nothing there.
            if (string.IsNullOrEmpty(tb.Text))
            {
                return;
            }

            string indicator = GetAutoCompleteIndicator(tb);
            int startIndex = 0; //Start from the beginning of the line.
            string matchingString = tb.Text.ToUpper();
            //If we have a trigger string, make sure that it has been typed before
            //giving auto-completion suggestions.
            if (string.IsNullOrEmpty(indicator) == false)
            {
                startIndex = tb.Text.LastIndexOf(indicator);
                //If we haven't typed the trigger string, then don't do anything.
                if (startIndex == -1)
                {
                    return;
                }

                startIndex += indicator.Length;
                matchingString = tb.Text.Substring(startIndex, tb.Text.Length - startIndex).ToUpper();
            }

            //If we don't have anything after the trigger string, return.
            if (string.IsNullOrEmpty(matchingString) == true)
            {
                return;
            }

            int textLength = matchingString.Length;

            StringComparison comparer = GetAutoCompleteStringComparison(tb);
            //Do search and changes here.
            string match =
            (
                from value in
                
                    from subvalue in values
                    where subvalue != null && subvalue.Length >= textLength
                    select subvalue
                
                where value.ToUpper().Substring(0, textLength).Equals(matchingString.ToUpper(), comparer)
                select value.Substring(textLength, value.Length - textLength)
            ).FirstOrDefault();

            //Nothing.  Leave 'em alone
            if (string.IsNullOrEmpty(match) == true)
            {
                return;
            }

            int matchStart = startIndex + matchingString.Length;
            tb.TextChanged -= onTextChanged;
            tb.Text = $"{tb.Text.ToUpper()}{match.ToUpper()}";
            tb.CaretIndex = matchStart;
            tb.SelectionStart = matchStart;
            tb.SelectionLength = tb.Text.Length - startIndex;
            tb.TextChanged += onTextChanged;
        }
    }
}
