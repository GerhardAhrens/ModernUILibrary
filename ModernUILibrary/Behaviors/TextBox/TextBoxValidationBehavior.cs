//-----------------------------------------------------------------------
// <copyright file="TextBoxValidationBehavior.cs" company="Lifeprojects.de">
//     Class: TextBoxValidationBehavior
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>10.06.2020</date>
//
// <summary>
// Definition of Class for Validation Behavior
// </summary>
//-----------------------------------------------------------------------
/*
    <TextBox>
       <i:Interaction.Behaviors>
          <behaviors:TextBoxValidationBehavior RegexString="[a-z]" />
       </i:Interaction.Behaviors>
    </TextBox>
 */

namespace ModernIU.Behaviors
{
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using Microsoft.Xaml.Behaviors;

    public class TextBoxValidationBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty RegexStringProperty = DependencyProperty.Register("RegexString", typeof(string), typeof(TextBoxValidationBehavior), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CanBeEmptyProperty = DependencyProperty.Register("CanBeEmpty", typeof(bool), typeof(TextBoxValidationBehavior), new PropertyMetadata(false));

        public string RegexString
        {
            get { return this.GetValue(RegexStringProperty) as string; }
            set { this.SetValue(RegexStringProperty, value); }
        }

        public bool CanBeEmpty
        {
            get { return (bool)this.GetValue(CanBeEmptyProperty); }
            set { this.SetValue(CanBeEmptyProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            var tb = this.AssociatedObject;
            if (tb != null)
            {
                tb.TextChanged += new TextChangedEventHandler(this.OnTextBoxTextChanged);
            }

            this.Validate();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            var tb = this.AssociatedObject;
            if (tb != null)
            {
                tb.TextChanged -= new TextChangedEventHandler(this.OnTextBoxTextChanged);
            }
        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Validate();
        }

        private void Validate()
        {
            TextBox tb = this.AssociatedObject;
            string text = tb.Text;

            bool emptyValid = this.CanBeEmpty ? true : !string.IsNullOrEmpty(text);
            bool regexValid = string.IsNullOrEmpty(this.RegexString) ? true : Regex.IsMatch(text, this.RegexString);

            bool valid = emptyValid && regexValid;

            if (valid == true)
            {
                VisualStateManager.GoToState(tb, "Valid", false);
                tb.Foreground = Brushes.Black;
                tb.FontStyle = FontStyles.Normal;
            }
            else
            {
                if (tb.Focus() == true)
                {
                    VisualStateManager.GoToState(tb, "InvalidFocused", true);
                    tb.Foreground = Brushes.Red;
                    tb.FontStyle = FontStyles.Italic;
                }
                else
                {
                    VisualStateManager.GoToState(tb, "InvalidUnfocused", true);
                    tb.Foreground = Brushes.Red;
                    tb.FontStyle = FontStyles.Italic;
                }
            }
        }
    }
}
