//-----------------------------------------------------------------------
// <copyright file="ListBox.cs" company="Lifeprojects.de">
//     Class: ListBox
//     Copyright © Gerhard Ahrens, 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>27.07.2018</date>
//
// <summary>
// Class for UI Control ListBox
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using ModernIU.Base;

    public class ListBoxEx :ListBox
    {
        public ListBoxEx()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.BorderBrush = ControlBase.BorderBrush;
            this.BorderThickness = ControlBase.BorderThickness;
            this.SelectionMode = SelectionMode.Extended;

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();

            this.IsSynchronizedWithCurrentItem = true;
            this.VerticalContentAlignment = VerticalAlignment.Top;
            this.Padding = new Thickness(2);
            this.Margin = new Thickness(2);
            this.ClipToBounds = false;
            this.MinHeight = 24;
            this.MinWidth = 60;
            this.SnapsToDevicePixels = true;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.BorderBrush = Brushes.Green;
            this.BorderThickness = new Thickness(1);
            this.Margin = new Thickness(2);
            this.SetValue(KeyboardNavigation.IsTabStopProperty, false);
            this.SetValue(ScrollViewer.IsDeferredScrollingEnabledProperty, false);
            this.SetValue(VirtualizingPanel.IsVirtualizingProperty, true);
            this.SetValue(VirtualizingPanel.VirtualizationModeProperty, VirtualizationMode.Recycling);
            this.Resources.Add(SystemColors.WindowBrushKey, Brushes.WhiteSmoke);
            this.Resources.Add(SystemColors.WindowTextBrushKey, Colors.White);
            this.Resources.Add(SystemColors.HighlightColorKey, Colors.Gray);
            Validation.SetErrorTemplate(this, null);
        }

        private Style SetTriggerFunction()
        {
            Style inputControlStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = TextBox.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = TextBox.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsFocused);

            /* Trigger für IsFocused = True */
            Trigger triggerIsReadOnly = new Trigger();
            triggerIsReadOnly.Property = TextBox.IsReadOnlyProperty;
            triggerIsReadOnly.Value = true;
            triggerIsReadOnly.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightYellow });
            inputControlStyle.Triggers.Add(triggerIsReadOnly);

            return inputControlStyle;
        }
    }
}
