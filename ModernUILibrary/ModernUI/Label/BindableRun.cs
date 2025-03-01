namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Documents;

    public class BindableRun : Run
    {
        public static readonly DependencyProperty BoundTextProperty =
            DependencyProperty.Register("BoundText", typeof(string), 
            typeof(BindableRun), 
            new PropertyMetadata(
                new PropertyChangedCallback(BindableRun.OnBoundTextChanged)));

        private static void OnBoundTextChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            ((Run)d).Text = e.NewValue as string;
        }

        public string BoundText
        {
            get { return GetValue(BoundTextProperty) as string; }
            set { SetValue(BoundTextProperty, value); }
        }
    }
}
