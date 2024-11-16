namespace ModernIU.BehaviorsBase
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    using ModernIU.Base;

    public static class ItemsControlExtensions
    {
        public static void AnimateScrollIntoView(this ItemsControl itemsControl, object item)
        {
            ScrollViewer scrollViewer = VisualHelper.FindVisualChild<ScrollViewer>(itemsControl);
            if(scrollViewer == null)
            {
                return;
            }

            UIElement container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as UIElement;

            if (container == null)
            {
                return;
            }

            int index = itemsControl.ItemContainerGenerator.IndexFromContainer(container);

            //double toValue = scrollViewer.ScrollableHeight * ((double)index / itemsControl.Items.Count);
            double toValue = VisualTreeHelper.GetOffset(container).Y;

            DoubleAnimation verticalAnimation = new DoubleAnimation();
            verticalAnimation.From = scrollViewer.VerticalOffset;
            verticalAnimation.To = toValue;
            verticalAnimation.DecelerationRatio = .2;
            verticalAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(verticalAnimation);
            Storyboard.SetTarget(verticalAnimation, scrollViewer);
            Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath(ScrollViewerBehavior.VerticalOffsetProperty));
            storyboard.Begin();
        }
    }
}
