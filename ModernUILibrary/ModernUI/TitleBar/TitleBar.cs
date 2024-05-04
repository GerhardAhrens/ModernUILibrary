namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class TitleBar : ContentControl
    {
        static TitleBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleBar), new FrameworkPropertyMetadata(typeof(TitleBar)));
        }
    }
}
