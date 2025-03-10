namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class DataGridDateColumn : DataGridBoundColumn
    {
        private static Style _defaultEditingElementStyle;
        private static Style _defaultElementStyle;

        /// <summary>
        /// FieldName Dependency Property.
        /// </summary>
        public static readonly DependencyProperty FieldNameProperty =
            DependencyProperty.Register("FieldName", typeof(string), typeof(DataGridDateColumn), new PropertyMetadata(""));

        /// <summary>
        /// IsColumnFiltered Dependency Property.
        /// </summary>
        public static readonly DependencyProperty IsColumnFilteredProperty =
                    DependencyProperty.Register("IsColumnFiltered", typeof(bool), typeof(DataGridDateColumn), new PropertyMetadata(false));

        static DataGridDateColumn()
        {
            DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridDateColumn), new FrameworkPropertyMetadata(DefaultElementStyle));
            DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridDateColumn), new FrameworkPropertyMetadata(DefaultEditingElementStyle));
        }

        public string FieldName
        {
            get => (string)GetValue(FieldNameProperty);
            set => SetValue(FieldNameProperty, value);
        }

        public bool IsColumnFiltered
        {
            get => (bool)GetValue(IsColumnFilteredProperty);
            set => SetValue(IsColumnFilteredProperty, value);
        }

        protected override System.Windows.FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            DatePicker dp = new DatePicker();
            this.ApplyStyle(true, false, dp);
            this.ApplyBinding(dp, DatePicker.SelectedDateProperty);
            return dp;
        }

        private void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
        {
            Style style = this.PickStyle(isEditing, defaultToElementStyle);
            if (style != null)
            {
                element.Style = style;
            }
        }

        private Style PickStyle(bool isEditing, bool defaultToElementStyle)
        {
            Style elementStyle = isEditing ? this.EditingElementStyle : this.ElementStyle;
            if ((isEditing && defaultToElementStyle) && (elementStyle == null))
            {
                elementStyle = this.ElementStyle;
            }
            return elementStyle;
        }

        private void ApplyBinding(DependencyObject target, DependencyProperty property)
        {
            BindingBase binding = this.Binding;
            if (binding != null)
            {
                BindingOperations.SetBinding(target, property, binding);
            }
            else
            {
                BindingOperations.ClearBinding(target, property);
            }
        }

        protected override System.Windows.FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            TextBlock tb = new TextBlock();
            this.ApplyStyle(false, false, tb);
            this.ApplyBinding(tb, TextBlock.TextProperty);
            return tb;
        }

        public static Style DefaultEditingElementStyle
        {
            get
            {
                if (_defaultEditingElementStyle == null)
                {
                    Style style = new Style(typeof(DatePicker));
                    style.Setters.Add(new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center));
                    style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
                    style.Seal();
                    _defaultEditingElementStyle = style;
                }
                return _defaultEditingElementStyle;
            }
        }

        public static Style DefaultElementStyle
        {
            get
            {
                if (_defaultElementStyle == null)
                {
                    Style style = new Style(typeof(TextBlock));
                    style.Setters.Add(new Setter(FrameworkElement.MarginProperty, new Thickness(2.0, 0.0, 2.0, 0.0)));
                    style.Seal();
                    _defaultElementStyle = style;
                }
                return _defaultElementStyle;
            }
        }
    }
}
