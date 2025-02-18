//-----------------------------------------------------------------------
// <copyright file="ComboBoxItemHighlightBehavior.cs" company="Lifeprojects.de">
//     Class: ComboBoxItemHighlightBehavior
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>20.09.2022</date>
//
// <summary>
// UI Control Class with ComboBoxItemHighlightBehavior for ComboBox-Control
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public static class ComboBoxItemHighlightBehavior
    {
        public static readonly DependencyProperty HighlightedItemProperty = DependencyProperty.RegisterAttached(
            "HighlightedItem",
            typeof(object),
            typeof(ComboBoxItemHighlightBehavior));

        public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached(
            "Enabled",
            typeof(bool),
            typeof(ComboBoxItemHighlightBehavior),
            new PropertyMetadata(false, OnEnabledChange));

        public static void SetHighlightedItem(UIElement element, object value)
        {
            element.SetValue(HighlightedItemProperty, value);
        }

        public static object GetHighlightedItem(UIElement element)
        {
            return element.GetValue(HighlightedItemProperty);
        }

        public static void SetEnabled(UIElement element, bool value)
        {
            element.SetValue(EnabledProperty, value);
        }

        public static bool EnabledItem(UIElement element)
        {
            return (bool)element.GetValue(EnabledProperty);
        }

        private static void OnEnabledChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBoxItem comboBoxItem)
            {
            }
            else
            {
                return;
            }

            comboBoxItem.MouseMove -= OnMouseMove;
            if (e.NewValue is true)
            {
                comboBoxItem.MouseMove += OnMouseMove;
            }
        }

        private static void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is ComboBoxItem comboBoxItem)
            {
            }
            else
            {
                return;
            }

            SetHighlightedItem(comboBoxItem, comboBoxItem.DataContext);
        }
    }
}
