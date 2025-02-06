namespace ModernIU.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    public class ButtonGroupItem : ContentControl
    {
        static ButtonGroupItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonGroupItem), new FrameworkPropertyMetadata(typeof(ButtonGroupItem)));
        }

        private ButtonGroup ParentItemsControl
        {
            get { return this.ParentSelector as ButtonGroup; }
        }

        internal ItemsControl ParentSelector
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as ItemsControl; }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ButtonGroupItem));

        [Bindable(true), Description("Liest oder setzt, ob der Eintrag der erste in der Liste ist.")]
        public bool IsFirstItem
        {
            get { return (bool)GetValue(IsFirstItemProperty); }
            set { SetValue(IsFirstItemProperty, value); }
        }

        public static readonly DependencyProperty IsFirstItemProperty = DependencyProperty.Register("IsFirstItem", typeof(bool), typeof(ButtonGroupItem), new PropertyMetadata(false));

        [Bindable(true), Description("Liest oder setzt, ob der Eintrag einer der mittleren in der Liste ist.")]
        public bool IsMiddleItem
        {
            get { return (bool)GetValue(IsMiddleItemProperty); }
            set { SetValue(IsMiddleItemProperty, value); }
        }

        public static readonly DependencyProperty IsMiddleItemProperty = DependencyProperty.Register("IsMiddleItem", typeof(bool), typeof(ButtonGroupItem), new PropertyMetadata(false));

        [Bindable(true), Description("Liest oder setzt, ob der Eintrag der letzte in der Liste ist.")]
        public bool IsLastItem
        {
            get { return (bool)GetValue(IsLastItemProperty); }
            set { SetValue(IsLastItemProperty, value); }
        }

        public static readonly DependencyProperty IsLastItemProperty = DependencyProperty.Register("IsLastItem", typeof(bool), typeof(ButtonGroupItem), new PropertyMetadata(false));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.MouseLeftButtonUp += ButtonGroupItem_MouseLeftButtonUp;
        }

        #region Event Implement Function

        private void ButtonGroupItem_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.ParentItemsControl.OnItemClick(this, this);
        }

        #endregion
    }
}
