namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class MRadionButton : RadioButton
    {
        static MRadionButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MRadionButton), new FrameworkPropertyMetadata(typeof(MRadionButton)));
        }
    }
}
