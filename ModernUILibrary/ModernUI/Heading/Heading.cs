
namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    public class Heading : TextBlock
    {

        #region HeaderType

        public EnumHeadingType HeaderType
        {
            get { return (EnumHeadingType)GetValue(HeaderTypeProperty); }
            set { SetValue(HeaderTypeProperty, value); }
        }
        
        public static readonly DependencyProperty HeaderTypeProperty =
            DependencyProperty.Register("HeaderType", typeof(EnumHeadingType), typeof(Heading));

        #endregion

        #region Constructors

        static Heading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Heading), new FrameworkPropertyMetadata(typeof(Heading)));
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion
    }
}
