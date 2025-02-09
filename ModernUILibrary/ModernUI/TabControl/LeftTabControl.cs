namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    public class LeftTabControl : TabControl
    {
        static LeftTabControl()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(LeftTabControl), new FrameworkPropertyMetadata(typeof(LeftTabControl)));
        }

        public LeftTabControl()
        {
            //this.FontSize = ControlBase.FontSize;
            //this.FontFamily = ControlBase.FontFamily;
            //this.Focusable = true;
            //this.TabStripPlacement = Dock.Top;
            //this.SetValue(Grid.IsSharedSizeScopeProperty, true);
        }
    }
}
