namespace ModernIU.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;

    using ModernIU.Base;
    using ModernIU.Controls;

    public class MaskLayerBehavior
    {
        #region Owner
        public static UIElement GetOwner(DependencyObject obj)
        {
            return (UIElement)obj.GetValue(OwnerProperty);
        }

        public static void SetOwner(DependencyObject obj, UIElement value)
        {
            obj.SetValue(OwnerProperty, value);
        }

        public static readonly DependencyProperty OwnerProperty =
            DependencyProperty.RegisterAttached("Owner", typeof(UIElement), typeof(MaskLayerBehavior), new PropertyMetadata(null, OwnerChangedCallback));

        private static void OwnerChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }
        #endregion

        public static bool GetIsOpen(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsOpenProperty);
        }

        public static void SetIsOpen(DependencyObject obj, bool value)
        {
            obj.SetValue(IsOpenProperty, value);
        }
        
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.RegisterAttached("IsOpen", typeof(bool), typeof(MaskLayerBehavior), new PropertyMetadata(false, IsOpenChangedCallback));

        private static void IsOpenChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool isOpen = (bool)e.NewValue;
            ContentControl owner = MaskLayerBehavior.GetOwner(d) as ContentControl;
            PopupEx layerContent = d as PopupEx;

            if (owner != null)
            {
                if((bool)e.NewValue)
                {
                    Grid layer = new Grid() { Background = new SolidColorBrush(Color.FromArgb(160, 0, 0, 0)) };

                    layer.PreviewMouseLeftButtonUp += delegate
                    {
                        MaskLayerBehavior.SetIsOpen(d, false);
                    };

                    UIElement original = owner.Content as UIElement;
                    owner.Content = null;

                    Grid container = new Grid();
                    container.Children.Add(original);
                    container.Children.Add(layer);
                    owner.Content = container;

                    layerContent.AllowsTransparency = true;
                    layerContent.StaysOpen = true;
                    layerContent.SetValue(PopopHelper.PopupPlacementTargetProperty, owner);
                    layerContent.PlacementTarget = owner;
                    layerContent.Placement = PlacementMode.Center;
                    layerContent.IsOpen = true;
                }
                else
                {
                    Grid grid = owner.Content as Grid;
                    UIElement original = VisualTreeHelper.GetChild(grid, 0) as UIElement;
                    grid.Children.Remove(original);
                    owner.Content = original;
                }
            }
        }
    }
}
