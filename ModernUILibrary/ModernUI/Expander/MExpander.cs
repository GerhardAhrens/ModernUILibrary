namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class MExpander : Expander
    {
        static MExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MExpander), new FrameworkPropertyMetadata(typeof(MExpander)));
        }

    }
}
