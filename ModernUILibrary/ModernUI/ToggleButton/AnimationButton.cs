namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;

    public class AnimationButton : ToggleButton
    {
        static AnimationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimationButton), new FrameworkPropertyMetadata(typeof(AnimationButton)));
        }
    }
}
