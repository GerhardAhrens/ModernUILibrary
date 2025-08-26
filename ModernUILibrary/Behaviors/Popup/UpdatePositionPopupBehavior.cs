//-----------------------------------------------------------------------
// <copyright file="UpdatePositionPopupBehavior.cs" company="Lifeprojects.de">
//     Class: UpdatePositionPopupBehavior
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>26.08.2025 07:42:21</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Behaviors
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Media3D;

    using Microsoft.Xaml.Behaviors;

    public class UpdatePositionPopupBehavior : Behavior<Popup>
    {
        private const int WM_MOVING = 0x0216;

        // should be moved to a helper class
        private DependencyObject GetTopmostParent(DependencyObject element)
        {
            var current = element;
            var result = element;

            while (current != null)
            {
                result = current;
                current = (current is Visual || current is Visual3D) ? VisualTreeHelper.GetParent(current) : LogicalTreeHelper.GetParent(current);
            }

            return result;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += (sender, e) =>
            {
                var root = GetTopmostParent(AssociatedObject.PlacementTarget) as Window;
                if (root != null)
                {
                    var helper = new WindowInteropHelper(root);
                    var hwndSource = HwndSource.FromHwnd(helper.Handle);
                    if (hwndSource != null)
                    {
                        hwndSource.AddHook(HwndMessageHook);
                    }
                }
            };
        }

        private IntPtr HwndMessageHook(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool bHandled)
        {
            if (msg == WM_MOVING)
            {
                Update();
            }

            return IntPtr.Zero;
        }

        public void Update()
        {
            // force the popup to update it's position
            var mode = AssociatedObject.Placement;
            AssociatedObject.Placement = PlacementMode.Relative;
            AssociatedObject.Placement = mode;
        }
    }
}
