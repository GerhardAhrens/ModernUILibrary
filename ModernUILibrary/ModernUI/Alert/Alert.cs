namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class Alert : ContentControl
    {
        #region Constructors
        static Alert()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Alert), new FrameworkPropertyMetadata(typeof(Alert)));
        }
        #endregion
    }
}
