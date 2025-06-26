namespace ModernIU.Behaviors
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Threading;

    using Microsoft.Xaml.Behaviors;

    public class ListBoxHelper : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
            base.OnDetaching();
        }

        private static void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox?.SelectedItem == null)
            {
                return;
            }

            Action action = () =>
            {
                listBox.UpdateLayout();

                if (listBox.SelectedItem == null)
                {
                    return;
                }

                listBox.ScrollIntoView(listBox.SelectedItem);
            };

            listBox.Dispatcher.BeginInvoke(action, DispatcherPriority.ContextIdle);
        }
    }
}