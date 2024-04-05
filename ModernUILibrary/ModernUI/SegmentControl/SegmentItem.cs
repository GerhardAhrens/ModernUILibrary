namespace ModernIU.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;

    public class SegmentItem : ListBoxItem
    {
        public static readonly DependencyProperty IsFirstItemProperty =
            DependencyProperty.Register("IsFirstItem", typeof(bool), typeof(SegmentItem), new PropertyMetadata(false));

        public static readonly DependencyProperty IsMiddleItemProperty =
            DependencyProperty.Register("IsMiddleItem", typeof(bool), typeof(SegmentItem), new PropertyMetadata(false));

        public static readonly DependencyProperty IsLastItemProperty =
            DependencyProperty.Register("IsLastItem", typeof(bool), typeof(SegmentItem), new PropertyMetadata(false));

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(SegmentItem));

        static SegmentItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SegmentItem), new FrameworkPropertyMetadata(typeof(SegmentItem)));
        }

        private SegmentControl ParentItemsControl
        {
            get { return this.ParentSelector as SegmentControl; }
        }

        internal ItemsControl ParentSelector
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as ItemsControl; }
        }
        [Bindable(true), Description("Liest oder setzt, ob der Eintrag der erste in der Liste ist.")]
        public bool IsFirstItem
        {
            get { return (bool)GetValue(IsFirstItemProperty); }
            set { SetValue(IsFirstItemProperty, value); }
        }

        [Bindable(true), Description("Liest oder setzt, ob der Eintrag einer der mittleren in der Liste ist.")]
        public bool IsMiddleItem
        {
            get { return (bool)GetValue(IsMiddleItemProperty); }
            set { SetValue(IsMiddleItemProperty, value); }
        }


        [Bindable(true), Description("Liest oder setzt, ob der Eintrag der letzte in der Liste ist.")]
        public bool IsLastItem
        {
            get { return (bool)GetValue(IsLastItemProperty); }
            set { SetValue(IsLastItemProperty, value); }
        }

        [Bindable(true), Description("Radius")]
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        
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
