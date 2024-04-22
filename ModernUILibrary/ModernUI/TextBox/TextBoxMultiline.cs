//-----------------------------------------------------------------------
// <copyright file="TextBoxMultiline.cs" company="Lifeprojects.de">
//     Class: TextBoxMultiline
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

    public sealed class TextBoxMultiline : TextBox
    {
        public static readonly DependencyProperty LinesProperty = 
            DependencyProperty.Register("Lines",
                typeof(int),
                typeof(TextBoxMultiline),
                new FrameworkPropertyMetadata(2, OnLinesPropertyChanged));

        public static readonly DependencyProperty ReadOnlyColorProperty = DependencyProperty.Register("ReadOnlyColor", typeof(Brush), typeof(TextBoxMultiline), new PropertyMetadata(Brushes.Transparent));
        public static readonly DependencyProperty SetBorderProperty = DependencyProperty.Register("SetBorder", typeof(bool), typeof(TextBoxMultiline), new PropertyMetadata(true, OnSetBorderChanged));

        public TextBoxMultiline()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.VerticalContentAlignment = VerticalAlignment.Top;
            this.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            this.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            this.AcceptsReturn = true;
            this.TextWrapping = TextWrapping.Wrap;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 23;
            this.MaxLines = 5;
            this.ClipToBounds = false;
            this.IsReadOnly = false;
            this.Focusable = true;

            this.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton), true);
            this.AddHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText), true);
        }

        ~TextBoxMultiline()
        {
            this.RemoveHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton));
            this.RemoveHandler(MouseDoubleClickEvent, new RoutedEventHandler(SelectAllText));
        }

        public int Lines
        {
            get
            {
                return (int)GetValue(LinesProperty);
            }
            set
            {
                SetValue(LinesProperty, value);
            }
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

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();
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
                    case Key.Tab:
                        this.MoveFocus(FocusNavigationDirection.Next);
                        break;
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
            copyMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopyMenu);
            textBoxContextMenu.Items.Add(copyMenu);

            if (this.IsReadOnly == false)
            {
                MenuItem pasteMenu = new MenuItem();
                pasteMenu.Header = "Einfügen";
                pasteMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconPaste);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(pasteMenu, "Click", this.OnPasteMenu);
                textBoxContextMenu.Items.Add(pasteMenu);

                MenuItem deleteMenu = new MenuItem();
                deleteMenu.Header = "Ausschneiden";
                deleteMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconDelete);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(deleteMenu, "Click", this.OnDeleteMenu);
                textBoxContextMenu.Items.Add(deleteMenu);

                MenuItem setDateMenu = new MenuItem();
                setDateMenu.Header = "Setze Datum";
                setDateMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconClock);
                WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(setDateMenu, "Click", this.OnSetDateMenu);
                textBoxContextMenu.Items.Add(setDateMenu);
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

        private void OnSetDateMenu(object sender, RoutedEventArgs e)
        {
            this.Text = DateTime.Now.ToShortDateString();
        }

        private static void OnSetBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var control = (TextBoxAll)d;

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

        private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                var textBox = (TextBox)parent;
                if (textBox.IsKeyboardFocusWithin == false)
                {
                    textBox.Focus();
                    e.Handled = true;
                }
            }
        }

        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            var textBox = e.OriginalSource as TextBox;
            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }

        private static void OnLinesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBoxMultiline multilineTextBox = (TextBoxMultiline)d;
            multilineTextBox.MaxLines = (int)e.NewValue;
        }
    }
}