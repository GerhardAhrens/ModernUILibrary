// <copyright file="TextBoxReadOnly.cs" company="Lifeprojects.de">
//     Class: TextBoxReadOnly
//     Copyright © Gerhard Ahrens, 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>27.07.2018</date>
//
// <summary>Class for UI Control TextBox</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernIU.Base;

    public class TextBoxReadOnly : TextBox
    {
        public static readonly DependencyProperty ReadOnlyColorProperty = DependencyProperty.Register("ReadOnlyColor", typeof(Brush), typeof(TextBoxReadOnly), new PropertyMetadata(Brushes.LightYellow));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxReadOnly), new PropertyMetadata(true));

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxReadOnly"/> class.
        /// </summary>
        public TextBoxReadOnly()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 23;
            this.FontSize = 14;
            this.IsReadOnly = true;
            this.Focusable = true;

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();
        }

        public Brush ReadOnlyColor
        {
            get { return (Brush)GetValue(ReadOnlyColorProperty); }
            set { SetValue(ReadOnlyColorProperty, value); }
        }

        public bool SetBorder
        {
            get { return (bool)GetValue(SetBorderProperty); }
            set { SetValue(SetBorderProperty, value); }
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

        private Style SetTriggerFunction()
        {
            Style textBoxStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = TextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = this.ReadOnlyColor });
            textBoxStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsMouseOver = false */
            triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = TextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = false;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = this.ReadOnlyColor });

            /* Trigger zum Style hinzufügen */
            textBoxStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = TextBox.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = this.ReadOnlyColor });
            textBoxStyle.Triggers.Add(triggerIsFocused);

            /* Trigger für IsFocused = false */
            triggerIsFocused = new Trigger();
            triggerIsFocused.Property = TextBox.IsFocusedProperty;
            triggerIsFocused.Value = false;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = this.ReadOnlyColor });
            textBoxStyle.Triggers.Add(triggerIsFocused);

            return textBoxStyle;
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
            copyMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopyMenu);
            textBoxContextMenu.Items.Add(copyMenu);

            return textBoxContextMenu;
        }

        private void OnCopyMenu(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.Text);
        }
    }
}
