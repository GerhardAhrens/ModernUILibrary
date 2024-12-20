namespace ModernIU.Base
{
    using System.Windows;
    using System.Windows.Controls.Primitives;

    public class PopopHelper
    {
        #region PopupPlacementTarget
        public static DependencyObject GetPopupPlacementTarget(DependencyObject obj)
        {
            return (DependencyObject)obj.GetValue(PopupPlacementTargetProperty);
        }

        public static void SetPopupPlacementTarget(DependencyObject obj, DependencyObject value)
        {
            obj.SetValue(PopupPlacementTargetProperty, value);
        }

        public static readonly DependencyProperty PopupPlacementTargetProperty =
            DependencyProperty.RegisterAttached("PopupPlacementTarget", typeof(DependencyObject), typeof(PopopHelper), new PropertyMetadata(null, OnPopupPlacementTargetChanged));

        private static void OnPopupPlacementTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                DependencyObject popupPopupPlacementTarget = e.NewValue as DependencyObject;
                Popup pop = d as Popup;

                Window w = Window.GetWindow(popupPopupPlacementTarget);
                if (null != w)
                {
                    w.LocationChanged += delegate
                    {
                        UpdatePosition(pop);
                    };

                    w.SizeChanged += delegate
                    {
                        UpdatePosition(pop);
                    };
                }
            }
        }
        #endregion

        private static void UpdatePosition(Popup pop)
        {
            if(pop == null)
            {
                return;
            }

            var offset = pop.HorizontalOffset;
            pop.HorizontalOffset = offset + 1;
            pop.HorizontalOffset = offset;
        }
    }
}
