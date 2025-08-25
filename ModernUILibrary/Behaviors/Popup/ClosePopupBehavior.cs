namespace ModernIU.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Threading;

    public static class ClosePopupBehavior
    {
        public static readonly DependencyProperty PopupContainerProperty =
            DependencyProperty.RegisterAttached("PopupContainer", typeof(ContentControl), typeof(ClosePopupBehavior), new PropertyMetadata(OnPopupContainerChanged));

        public static ContentControl GetPopupContainer(DependencyObject obj)
        {
            return (ContentControl)obj.GetValue(PopupContainerProperty);
        }

        public static void SetPopupContainer(DependencyObject obj, ContentControl value)
        {
            obj.SetValue(PopupContainerProperty, value);
        }

        private static readonly DependencyProperty WindowPopupProperty =
            DependencyProperty.RegisterAttached("WindowPopup", typeof(Popup), typeof(ClosePopupBehavior));

        private static Popup GetWindowPopup(DependencyObject obj)
        {
            return (Popup)obj.GetValue(WindowPopupProperty);
        }

        private static void SetWindowPopup(DependencyObject obj, Popup value)
        {
            obj.SetValue(WindowPopupProperty, value);
        }

        private static void OnPopupContainerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Popup popup = (Popup)d;
            if (popup == null)
            {
                return;
            }

            var contentControl = e.NewValue as ContentControl;

            popup.LostFocus += (sender, args) =>
            {
                var popup1 = (Popup)sender;
                popup.IsOpen = false;
                if (contentControl != null)
                {
                    contentControl.PreviewMouseDown -= ContainerOnPreviewMouseDown;
                }
            };

            popup.Opened += (sender, args) =>
            {
                var popup1 = (Popup)sender;
                popup.Focus();
                SetWindowPopup(contentControl, popup1);
                contentControl.PreviewMouseDown -= ContainerOnPreviewMouseDown;
                contentControl.PreviewMouseDown += ContainerOnPreviewMouseDown;
            };

            popup.PreviewMouseUp += (sender, args) =>
            {
                popup.IsOpen = false;
                if (contentControl != null)
                {
                    contentControl.PreviewMouseDown -= ContainerOnPreviewMouseDown;
                }
            };

            popup.MouseLeave += (sender, args) =>
            {
                popup.IsOpen = false;
                if (contentControl != null)
                {
                    contentControl.PreviewMouseDown -= ContainerOnPreviewMouseDown;
                }
            };

            popup.Unloaded += (sender, args) =>
            {
                popup.IsOpen = false;
                if (contentControl != null)
                {
                    contentControl.PreviewMouseDown -= ContainerOnPreviewMouseDown;
                }
            };
        }

        private static void ContainerOnPreviewMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var popup = GetWindowPopup((DependencyObject)sender);
            popup.IsOpen = false;
            ((FrameworkElement)sender).PreviewMouseUp -= ContainerOnPreviewMouseDown;
        }
    }
}
