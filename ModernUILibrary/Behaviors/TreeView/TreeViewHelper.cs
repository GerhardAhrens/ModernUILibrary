//-----------------------------------------------------------------------
// <copyright file="TreeViewHelper.cs" company="Lifeprojects.de">
//     Class: TreeViewHelper
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>21.05.2025 14:05:43</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Behaviors
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Microsoft.Xaml.Behaviors;
    using ModernIU.Controls;

    public class TreeViewHelper : Behavior<TreeView>
    {
        #region BoundSelectedItem
        public static readonly DependencyProperty BoundSelectedItemProperty =
            DependencyProperty.Register("BoundSelectedItem", typeof(object), typeof(TreeViewHelper), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBoundSelectedItemChanged));

        public object BoundSelectedItem
        {
            get => GetValue(BoundSelectedItemProperty);
            set => SetValue(BoundSelectedItemProperty, value);
        }

        private static void OnBoundSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (args.NewValue is ITreeViewItem item)
            {
                item.IsSelected = true;
            }
        }

        private void OnTreeViewSelectedItemChanged(object obj, RoutedPropertyChangedEventArgs<object> args)
        {
            BoundSelectedItem = args.NewValue;
        }
        #endregion BoundSelectedItem

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            base.OnDetaching();
        }
    }
}
