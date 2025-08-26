namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class SelectableTextBlock : Control
    {
        public TextAlignment TextAlignment { get; set; }
        public TextDecorationCollection TextDecorations { get; set; }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SelectableTextBlock), new FrameworkPropertyMetadata(string.Empty));

        public string Text { get { return (string)GetValue(TextProperty); } set { SetValue(TextProperty, value); } }

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(SelectableTextBlock), new FrameworkPropertyMetadata(TextWrapping.NoWrap));

        public TextWrapping TextWrapping { get { return (TextWrapping)GetValue(TextWrappingProperty); } set { SetValue(TextWrappingProperty, value); } }

        static SelectableTextBlock()
        {
            IsTabStopProperty.OverrideMetadata(typeof(SelectableTextBlock), new FrameworkPropertyMetadata(false));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectableTextBlock), new FrameworkPropertyMetadata(typeof(SelectableTextBlock)));
        }

        public SelectableTextBlock()
        {
            TextDecorations = new TextDecorationCollection();
        }
    }
}
