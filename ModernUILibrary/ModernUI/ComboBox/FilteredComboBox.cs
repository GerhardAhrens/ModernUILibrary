//-----------------------------------------------------------------------
// <copyright file="ComboBoxEx.cs" company="Lifeprojects.de">
//     Class: ComboBoxEx
//     Copyright © Gerhard Ahrens, 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>27.07.2018</date>
//
// <summary>Class for UI Control ComboBoxEx</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using ModernIU.Base;

    using static System.Net.Mime.MediaTypeNames;

    /// <summary>
    /// Editable combo box which uses the text in its editable textbox to perform a lookup
    /// in its data source.
    /// </summary>
    public class FilteredComboBox : ComboBox
    {
        public static readonly DependencyProperty ReadOnlyBackgroundColorProperty = DependencyProperty.Register("ReadOnlyBackgroundColor", typeof(Brush), typeof(FilteredComboBox), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(222, 222, 222))));

        public static readonly DependencyProperty MinimumSearchLengthProperty = DependencyProperty.Register("MinimumSearchLength", typeof(int), typeof(FilteredComboBox), new UIPropertyMetadata(0));

        private string oldFilter = string.Empty;
        private string currentFilter = string.Empty;

        protected TextBox EditableTextBox => GetTemplateChild("PART_EditableTextBox") as TextBox;

        public FilteredComboBox()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.BorderBrush = ControlBase.BorderBrush;
            this.BorderThickness = ControlBase.BorderThickness;
            this.Height = ControlBase.DefaultHeight;
            this.Margin = ControlBase.DefaultMargin;
            this.ReadOnlyBackgroundColor = Brushes.LightYellow;
            this.SnapsToDevicePixels = true;
            this.IsReadOnly = false;
            this.IsEditable = true;
            this.IsTextSearchEnabled = false;
        }

        [Description("Length of the search string that triggers filtering.")]
        [Category("Filtered ComboBox")]
        public int MinimumSearchLength
        {
            [System.Diagnostics.DebuggerStepThrough]
            get
            {
                return (int)this.GetValue(MinimumSearchLengthProperty);
            }

            [System.Diagnostics.DebuggerStepThrough]
            set
            {
                this.SetValue(MinimumSearchLengthProperty, value);
            }
        }

        public Brush ReadOnlyBackgroundColor
        {
            get { return GetValue(ReadOnlyBackgroundColorProperty) as Brush; }
            set { this.SetValue(ReadOnlyBackgroundColorProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (newValue != null)
            {
                var view = CollectionViewSource.GetDefaultView(newValue);
                view.Filter += FilterItem;
            }

            if (oldValue != null)
            {
                var view = CollectionViewSource.GetDefaultView(oldValue);
                if (view != null) view.Filter -= FilterItem;
            }

            base.OnItemsSourceChanged(oldValue, newValue);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab:
                case Key.Enter:
                    this.IsDropDownOpen = false;
                    break;
                case Key.Escape:
                    this.IsDropDownOpen = false;
                    this.SelectedIndex = -1;
                    this.Text = currentFilter;
                    break;
                default:
                    if (e.Key == Key.Down)
                    {
                        this.IsDropDownOpen = true;
                    }

                    base.OnPreviewKeyDown(e);
                    break;
            }

            // Cache text
            this.oldFilter = this.Text;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.Down:
                    break;
                case Key.Tab:
                case Key.Enter:
                    this.ClearFilter();
                    break;
                default:
                    if (this.Text != this.oldFilter)
                    {
                        this.RefreshFilter();
                        this.IsDropDownOpen = true;
                        this.EditableTextBox.SelectionStart = int.MaxValue;
                    }

                    base.OnKeyUp(e);
                    this.currentFilter = this.Text;
                    break;
            }
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            this.ClearFilter();
            var temp = this.SelectedIndex;
            this.SelectedIndex = -1;
            this.Text = string.Empty;
            this.SelectedIndex = temp;
            base.OnPreviewLostKeyboardFocus(e);
        }

        private void RefreshFilter()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            var view = CollectionViewSource.GetDefaultView(this.ItemsSource);
            view.Refresh();
        }

        private void ClearFilter()
        {
            this.currentFilter = string.Empty;
            RefreshFilter();
        }

        private bool FilterItem(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (this.Text.Length == 0)
            {
                return true;
            }
            else if (this.Text.Length > MinimumSearchLength)
            {
                return value.ToString().ToLower().Contains(this.Text.ToLower());
            }
            else
            {
                return false;
            }
        }

        private Style SetTriggerFunction()
        {
            Style inputControlStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = Border.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = Border.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = Brushes.LightGray });
            inputControlStyle.Triggers.Add(triggerIsFocused);

            /* Trigger für IsReadOnlyProperty = True */
            Trigger triggerIsReadOnly = new Trigger();
            triggerIsReadOnly.Property = TextBox.IsReadOnlyProperty;
            triggerIsReadOnly.Value = true;
            triggerIsReadOnly.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = this.ReadOnlyBackgroundColor });
            inputControlStyle.Triggers.Add(triggerIsReadOnly);

            /* Trigger für IsEnabledProperty = True */
            Trigger triggerIsEnabled = new Trigger();
            triggerIsReadOnly.Property = TextBox.IsEnabledProperty;
            triggerIsReadOnly.Value = true;
            triggerIsReadOnly.Setters.Add(new Setter() { Property = TextBox.BackgroundProperty, Value = this.ReadOnlyBackgroundColor });
            inputControlStyle.Triggers.Add(triggerIsReadOnly);

            return inputControlStyle;
        }
    }
}
