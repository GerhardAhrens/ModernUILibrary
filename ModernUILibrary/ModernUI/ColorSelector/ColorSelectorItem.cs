
namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;

    public class ColorSelectorItem : ContentControl
    {
        public static readonly DependencyProperty IsSelectedProperty =
            Selector.IsSelectedProperty.AddOwner(typeof(ColorSelectorItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(ColorSelectorItem.OnIsSelectedChanged)));

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(ColorSelectorItem));

        static ColorSelectorItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorSelectorItem), new FrameworkPropertyMetadata(typeof(ColorSelectorItem)));
        }

        private ColorSelector ParentColorSelector
        {
            get { return this.ParentSelector as ColorSelector; }
        }

        internal Selector ParentSelector
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as Selector; }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.MouseLeftButtonUp += ColorItem_MouseLeftButtonUp;
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorSelectorItem colorItem = d as ColorSelectorItem;
            bool flag = (bool)e.NewValue;
            if (flag)
            {
                colorItem.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, colorItem));
            }
            else
            {
                colorItem.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, colorItem));
            }

            colorItem.UpdateVisualState(true);
        }

        private void UpdateVisualState(bool useTransitions)
        {
            if (base.IsEnabled == false)
            {
                VisualStateManager.GoToState(this, (base.Content is Control) ? "Normal" : "Disabled", useTransitions);
            }
            else if (base.IsMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
            if (this.IsSelected)
            {
                if (Selector.GetIsSelectionActive(this))
                {
                    VisualStateManager.GoToState(this, "Selected", useTransitions);
                }
                else
                {
                    //VisualStates.GoToState(this, useTransitions, new string[]
                    //{
                    //    "SelectedUnfocused",
                    //    "Selected"
                    //});
                }
            }
            else
            {
                VisualStateManager.GoToState(this, "Unselected", useTransitions);
            }
            if (base.IsKeyboardFocused)
            {
                VisualStateManager.GoToState(this, "Focused", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unfocused", useTransitions);
            }
        }

        private void OnUnselected(RoutedEventArgs routedEventArgs)
        {
            this.HandleIsSelectedChanged(false, routedEventArgs);
        }

        private void OnSelected(RoutedEventArgs routedEventArgs)
        {
            this.HandleIsSelectedChanged(true, routedEventArgs);
        }

        private void HandleIsSelectedChanged(bool newValue, RoutedEventArgs e)
        {
            base.RaiseEvent(e);
        }

        private void ColorItem_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.ParentColorSelector != null)
            {
                this.ParentColorSelector.SetItemSelected(this);
            }
        }
    }
}
