namespace ModernIU.Controls
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    [ContentProperty("Text")]
    public class EditableTextBox : EditableControl
    {
        public static readonly RoutedEvent NewValueSavedEvent;

        static EditableTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableTextBox), new FrameworkPropertyMetadata(typeof(EditableTextBox)));
            NewValueSavedEvent = EventManager.RegisterRoutedEvent("NavigationItemsChanged", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(EditableTextBox));
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(EditableTextBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, TextChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty SaveNewValueCommandProperty =
            DependencyProperty.Register("SaveNewValueCommand", typeof(ICommand), typeof(EditableTextBox), new PropertyMetadata(null));

        public ICommand SaveNewValueCommand
        {
            get { return (ICommand)GetValue(SaveNewValueCommandProperty); }
            set { SetValue(SaveNewValueCommandProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Button btnView = GetTemplateChild("btnSwitch") as Button;
            TextBox txtBox = GetTemplateChild("txtBox") as TextBox;
            TextBlock txtBlock = GetTemplateChild("txtBlock") as TextBlock;
            if (btnView != null)
            {
                btnView.Click += BtnView_Click;
            }
            if (txtBox != null)
            {
                txtBox.KeyDown += txtBox_KeyDown;
                txtBox.LostFocus += txtBox_LostFocus;
            }
            if (txtBlock != null)
            {
                txtBlock.MouseEnter += txtBlock_MouseEnter;
                txtBlock.MouseLeave += txtBlock_MouseLeave;
                txtBlock.MouseLeftButtonDown += txtBlock_MouseLeftButtonDown;
            }
            
            if ((HighlightActivator)GetValue(HighlightActivationProperty) == HighlightActivator.Allways)
            {
                SetValue(ShowHighlightsProperty, true);
            }
        }

        private void txtBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            if ((HighlightActivator)GetValue(HighlightActivationProperty) == HighlightActivator.OnHover)
            {
                SetValue(ShowHighlightsProperty, false);
            }
        }

        private void txtBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            if ((HighlightActivator)GetValue(HighlightActivationProperty) == HighlightActivator.OnHover)
            {
                SetValue(ShowHighlightsProperty, true);
            }

        }

        private void txtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SetValue(IsInEditModeProperty, false);
            SetValue(TextProperty, ((TextBox)sender).Text);
            if ((HighlightActivator)GetValue(HighlightActivationProperty) == HighlightActivator.OnIconClick)
            {
                SetValue(ShowHighlightsProperty, false);
            }

            RoutedEventArgs newEventArgs = new RoutedEventArgs(NewValueSavedEvent);
            ((TextBox)sender).RaiseEvent(newEventArgs);
            SaveNewValueCommand?.Execute(((TextBox)sender).Text);

        }

        private void txtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { SetValue(IsInEditModeProperty, !(bool)GetValue(IsInEditModeProperty)); }
            if (e.Key == Key.Escape) { SetValue(IsInEditModeProperty, !(bool)GetValue(IsInEditModeProperty)); }
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            SetValue(IsInEditModeProperty, !(bool)GetValue(IsInEditModeProperty));
            TextBox txtBox = GetTemplateChild("txtBox") as TextBox;
            if ((bool)GetValue(IsInEditModeProperty))
            {
                txtBox.Focus();
                txtBox.SelectAll();
                SetValue(ShowHighlightsProperty, true);
                if ((HighlightActivator)GetValue(HighlightActivationProperty) == HighlightActivator.OnIconClick)
                {
                    SetValue(ShowHighlightsProperty, true);
                }
            }
            else
            {
                if ((HighlightActivator)GetValue(HighlightActivationProperty) == HighlightActivator.OnIconClick)
                {
                    SetValue(ShowHighlightsProperty, false);
                }
            }
        }

        private void txtBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetValue(IsInEditModeProperty, !(bool)GetValue(IsInEditModeProperty));
            TextBox txtBox = GetTemplateChild("txtBox") as TextBox;

            txtBox.Focus();
            txtBox.SelectAll();
            SetValue(ShowHighlightsProperty, true);
            if ((HighlightActivator)GetValue(HighlightActivationProperty) == HighlightActivator.OnIconClick)
            {
                SetValue(ShowHighlightsProperty, true);
            }
        }

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("TextChanged: " + e.NewValue);
        }
    }
}
