namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    public class FlatToolTip : ToolTip
    {
        public static readonly DependencyProperty PlacementExProperty =
            DependencyProperty.Register("PlacementEx", typeof(EnumPlacement), typeof(FlatToolTip), new PropertyMetadata(EnumPlacement.RightCenter));

        #region Constructors

        static FlatToolTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatToolTip), new FrameworkPropertyMetadata(typeof(FlatToolTip)));
        }

        #endregion

        public EnumPlacement PlacementEx
        {
            get { return (EnumPlacement)GetValue(PlacementExProperty); }
            set { SetValue(PlacementExProperty, value); }
        }
        
    }
}
