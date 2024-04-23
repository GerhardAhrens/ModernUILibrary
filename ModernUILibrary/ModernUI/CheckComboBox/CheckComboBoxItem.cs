namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class CheckComboBoxItem : ListBoxItem
    {
        private CheckComboBox ParentCheckComboBox
        {
            get
            {
                return ItemsControl.ItemsControlFromItemContainer(this) as CheckComboBox;
            }
        }


        static CheckComboBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckComboBoxItem), new FrameworkPropertyMetadata(typeof(CheckComboBoxItem)));
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if(this.ParentCheckComboBox != null)
            {
                this.ParentCheckComboBox.NotifyCheckComboBoxItemClicked(this);
            }

            base.OnMouseLeftButtonDown(e);
        }
    }
}
