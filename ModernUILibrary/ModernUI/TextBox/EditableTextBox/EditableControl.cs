namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public abstract class EditableControl : Control
    {
        public static readonly DependencyProperty IsInEditModeProperty =
            DependencyProperty.Register("IsInEditMode", typeof(bool), typeof(EditableControl), new PropertyMetadata(false, IsInEditModelChanged));

        public bool IsInEditMode
        {
            get { return (bool)GetValue(IsInEditModeProperty); }
            set { SetValue(IsInEditModeProperty, value); }
        }

        public static readonly DependencyProperty HighlightBrushProperty =
            DependencyProperty.Register("HighlightBrush", typeof(Brush), typeof(EditableControl), new PropertyMetadata(new SolidColorBrush(Colors.AliceBlue)));

        public Brush HighlightBrush
        {
            get { return (Brush)GetValue(HighlightBrushProperty); }
            set { SetValue(HighlightBrushProperty, value); }
        }


        public static readonly DependencyProperty ShowHighlightsProperty =
            DependencyProperty.Register("ShowHighlights", typeof(bool), typeof(EditableTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ShowHighlightsChanged));

        public bool ShowHighlights
        {
            get { return (bool)GetValue(ShowHighlightsProperty); }
            set { SetValue(ShowHighlightsProperty, value); }
        }

        public static readonly DependencyProperty ShowEditIconProperty =
            DependencyProperty.Register("ShowEditIcon", typeof(bool), typeof(EditableControl), new PropertyMetadata(true));

        public bool ShowEditIcon
        {
            get { return (bool)GetValue(ShowEditIconProperty); }
            set { SetValue(ShowEditIconProperty, value); }
        }


        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(EditableTextBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        public static readonly DependencyProperty HighlightActivationProperty =
            DependencyProperty.Register("HighlightActivation", typeof(HighlightActivator), typeof(EditableTextBox), new FrameworkPropertyMetadata(HighlightActivator.Allways, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, HighlightActivationChanged));

        public HighlightActivator HighlightActivation
        {
            get { return (HighlightActivator)GetValue(HighlightActivationProperty); }
            set { SetValue(HighlightActivationProperty, value); }
        }

        private static void HighlightActivationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((HighlightActivator)e.NewValue == HighlightActivator.Allways)
            {
                d.SetValue(ShowHighlightsProperty, true);
            }
            if ((HighlightActivator)e.NewValue == HighlightActivator.OnHover)
            {
                d.SetValue(ShowHighlightsProperty, false);
            }
            if ((HighlightActivator)e.NewValue == HighlightActivator.OnIconClick)
            {
                d.SetValue(ShowHighlightsProperty, false);
            }
        }

        private static void IsInEditModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void ShowHighlightsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
    public enum HighlightActivator
    {
        Allways = 0,
        OnHover = 1,
        OnIconClick = 2
    }

}

