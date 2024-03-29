namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;

    public class FlatToggleButton : ToggleButton
    {
        static FlatToggleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatToggleButton), new FrameworkPropertyMetadata(typeof(FlatToggleButton)));
        }
    }
}
