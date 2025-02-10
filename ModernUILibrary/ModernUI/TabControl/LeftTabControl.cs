namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    public class LeftTabControl : TabControl
    {
        public static readonly DependencyProperty HeaderContentProperty;

        static LeftTabControl()
        {
        }

        public LeftTabControl()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.Focusable = true;
            this.TabStripPlacement = Dock.Left;
            this.SetValue(Grid.IsSharedSizeScopeProperty, true);
        }

        public object HeaderContent
        {
            get { return (object)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabItem();
        }
    }
}
