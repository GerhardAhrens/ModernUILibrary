namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class Flyout : HeaderedContentControl
    {
        #region Constructors
        static Flyout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Flyout), new FrameworkPropertyMetadata(typeof(Flyout)));
        }
        #endregion
    }
}
