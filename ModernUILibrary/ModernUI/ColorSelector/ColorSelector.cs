namespace ModernIU.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Media;

    public class ColorSelector : Selector
    {
        static ColorSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorSelector), new FrameworkPropertyMetadata(typeof(ColorSelector)));
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            
            if(!(item is ColorSelectorItem))
            {
                ColorSelectorItem colorItem = element as ColorSelectorItem;
                if (!string.IsNullOrEmpty(this.DisplayMemberPath))
                {
                    Binding binding = new Binding(this.DisplayMemberPath);
                    colorItem.SetBinding(ColorSelectorItem.BackgroundProperty, binding);
                }
                else
                {
                    Color color;
                    try
                    {
                        color = (Color)ColorConverter.ConvertFromString(Convert.ToString(item));
                    }
                    catch (Exception ex)
                    {
                        string errorText = ex.Message;
                        color = Color.FromRgb(255, 255, 255);
                    }
                    colorItem.SetValue(ColorSelectorItem.ColorProperty, new SolidColorBrush(color));
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ColorSelectorItem();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public void SetItemSelected(ColorSelectorItem selectedItem)
        {
            if(this.Items == null)
            {
                return;
            }

            for (int i = 0; i < this.Items.Count; i++)
            {
                ColorSelectorItem colorItem = this.ItemContainerGenerator.ContainerFromIndex(i) as ColorSelectorItem;
                if (colorItem == selectedItem)
                {
                    colorItem.SetCurrentValue(ColorSelectorItem.IsSelectedProperty, true);
                }
                else
                {
                    colorItem.SetCurrentValue(ColorSelectorItem.IsSelectedProperty, false);
                }
            }
        }
    }
}
