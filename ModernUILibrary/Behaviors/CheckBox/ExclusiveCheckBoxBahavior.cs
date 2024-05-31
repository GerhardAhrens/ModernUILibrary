
namespace ModernIU.Behaviors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class ExclusiveCheckBoxBahavior : IDisposable
    {
        #region static part
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(ExclusiveCheckBoxBahavior),
            new PropertyMetadata(false, OnIsEnabledChanged));

        public static void SetIsEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GetInstance(d)?.Dispose();
            if ((bool)e.NewValue)
            {
                SetInstance(d, new ExclusiveCheckBoxBahavior(d));
            }
        }


        private static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached("Instance",
            typeof(ExclusiveCheckBoxBahavior), typeof(ExclusiveCheckBoxBahavior), new PropertyMetadata(null));

        private static void SetInstance(DependencyObject element, ExclusiveCheckBoxBahavior value)
        {
            element.SetValue(InstanceProperty, value);
        }

        private static ExclusiveCheckBoxBahavior GetInstance(DependencyObject element)
        {
            return (ExclusiveCheckBoxBahavior)element.GetValue(InstanceProperty);
        }
        #endregion

        #region instance part

        private List<CheckBox> checkBoxes;
        public ExclusiveCheckBoxBahavior(DependencyObject d)
        {
            ((FrameworkElement)d).Initialized += (sender, args) =>
            {
                checkBoxes = FindVisualChildren<CheckBox>(d).ToList();
                checkBoxes.ForEach(i => i.Checked += CheckBoxChecked);
            };
        }

        private void CheckBoxChecked(object sender, RoutedEventArgs e)
        {
            checkBoxes.ForEach(i =>
            {
                if (!Equals(i, sender))
                {
                    i.IsChecked = false;
                }
            });
        }

        public void Dispose()
        {
            checkBoxes.ForEach(i => i.Checked -= CheckBoxChecked);
        }
        #endregion

        #region private static
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject root) where T : DependencyObject
        {
            if (root != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(root, i);
                    if (child is T) yield return (T)child;
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        #endregion
    }
}
