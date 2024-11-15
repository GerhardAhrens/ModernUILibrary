namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class ContentNavigation : ContentControl
    {
        static ContentNavigation()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentNavigation), new FrameworkPropertyMetadata(typeof(ContentNavigation)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}