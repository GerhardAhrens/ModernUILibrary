namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    using ModernIU.Base;

    public class BusyIndicatorAdorner : Adorner
    {
        private VisualCollection _visuals;
        private BusyIndicator busyIndicator;

        public static bool GetIsOpen(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsOpenProperty);
        }

        public static void SetIsOpen(DependencyObject obj, bool value)
        {
            obj.SetValue(IsOpenProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.RegisterAttached("IsOpen", typeof(bool), typeof(BusyIndicatorAdorner)
                , new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, BusyIndicatorAdorner.IsOpenCallback, CoerceIsOpen, true, System.Windows.Data.UpdateSourceTrigger.PropertyChanged));

        private static void IsOpenCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BusyIndicatorAdorner adorner = UIElementEx.GetAdorner<BusyIndicatorAdorner>(d);
            if(adorner == null)
            {
                return;
            }
        }

        private static object CoerceIsOpen(DependencyObject d, object baseValue)
        {
            if (baseValue == null)
                return false;
            return baseValue;
        }

        public BusyIndicatorAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }
    }
}
