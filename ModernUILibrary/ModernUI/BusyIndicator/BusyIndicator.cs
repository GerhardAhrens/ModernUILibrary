namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class BusyIndicator : Control
    {
        public static readonly DependencyProperty IsBusyProperty;
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty LoadingColorProperty;

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Brush LoadingColor
        {
            get { return (Brush)GetValue(LoadingColorProperty); }
            set { SetValue(LoadingColorProperty, value); }
        }

        private static void OnIsBusyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BusyIndicator indicator = d as BusyIndicator;
            indicator.Visibility = indicator.IsBusy ? Visibility.Visible : Visibility.Collapsed;
        }

        static BusyIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BusyIndicator), new FrameworkPropertyMetadata(typeof(BusyIndicator)));
            BusyIndicator.IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(BusyIndicator), new PropertyMetadata(false, OnIsBusyChangedCallback));
            BusyIndicator.TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BusyIndicator), new PropertyMetadata("Erstellen.."));
            BusyIndicator.LoadingColorProperty = DependencyProperty.Register("LoadingColor", typeof(Brush), typeof(BusyIndicator), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 122, 204))));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Visibility = this.IsBusy ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
