namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class MScrollViewer : ScrollViewer
    {
        #region Constructors
        static MScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MScrollViewer), new FrameworkPropertyMetadata(typeof(MScrollViewer)));
        }
        #endregion

        public double VerticalOffsetEx
        {
            get { return (double)GetValue(VerticalOffsetExProperty); }
            set { SetValue(VerticalOffsetExProperty, value); }
        }
        
        public static readonly DependencyProperty VerticalOffsetExProperty =
            DependencyProperty.Register("VerticalOffsetEx", typeof(double), typeof(MScrollViewer), new PropertyMetadata(0d, VerticalOffsetExChangedCallback));

        private static void VerticalOffsetExChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MScrollViewer scrollViewer = d as MScrollViewer;
            scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
        }
    }
}
