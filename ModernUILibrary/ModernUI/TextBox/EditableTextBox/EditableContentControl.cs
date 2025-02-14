namespace ModernIU.Controls
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    [ContentProperty("Content")]
    public class EditableContentControl : EditableControl
    {
        static EditableContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableContentControl), new FrameworkPropertyMetadata(typeof(EditableContentControl)));
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(EditableContentControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, TextChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(EditableContentControl), new PropertyMetadata(null));

        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Button btnView = GetTemplateChild("btnSwitch") as Button;
            //ContentControl contentCtl = GetTemplateChild("ContentCtl") as ContentControl;
            TextBlock txtBlock = GetTemplateChild("txtBlock") as TextBlock;
            if (btnView != null)
            {
                btnView.Click += BtnView_Click;
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

        private void txtBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetValue(IsInEditModeProperty, true);
            ContentControl ContentCtl = GetTemplateChild("ContentCtl") as ContentControl;

            ContentCtl.Focus();
            SetValue(ShowHighlightsProperty, true);
            if ((HighlightActivator)GetValue(HighlightActivationProperty) == HighlightActivator.OnIconClick)
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


        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            SetValue(IsInEditModeProperty, !(bool)GetValue(IsInEditModeProperty));
            ContentControl txtBox = GetTemplateChild("ContentCtl") as ContentControl;
            if ((bool)GetValue(IsInEditModeProperty))
            {
                txtBox.Focus();
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

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("TextChanged: " + e.NewValue);
        }
    }
}
