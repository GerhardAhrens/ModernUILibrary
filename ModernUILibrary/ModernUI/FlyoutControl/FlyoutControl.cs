namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class FlyoutControl : ItemsControl
    {
        #region Constructors
        static FlyoutControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlyoutControl), new FrameworkPropertyMetadata(typeof(FlyoutControl)));
        }
        #endregion

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new Flyout();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is Flyout;
        }
    }
}
