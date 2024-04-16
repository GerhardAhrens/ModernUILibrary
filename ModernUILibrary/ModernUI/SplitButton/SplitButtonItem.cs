namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class SplitButtonItem : ContentControl
    {
        static SplitButtonItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButtonItem), new FrameworkPropertyMetadata(typeof(SplitButtonItem)));
        }

        public SplitButtonItem()
        {
            this.MouseEnter += DropdownButtonItem_MouseEnter;
            this.MouseLeave += DropdownButtonItem_MouseLeave;
            this.MouseLeftButtonUp += DropdownButtonItem_MouseLeftButtonUp;
        }

        private SplitButton ParentListBox
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this) as SplitButton;
            }
        }


        private void DropdownButtonItem_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SplitButtonItem item = sender as SplitButtonItem;
            this.ParentListBox.OnItemClick(item.Content, item.Content);
            this.ParentListBox.IsDropDownOpen = false;
            e.Handled = true;
        }

        private void DropdownButtonItem_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "Normal", false);
        }

        private void DropdownButtonItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "MouseOver", false);
        }
    }
}
