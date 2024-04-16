namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using ModernIU.Base;

    public class Icon : Control
    {
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(PathFigureCollection), typeof(Icon));

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(EnumIconType), typeof(Icon));

        #region Constructors
        static Icon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Icon), new FrameworkPropertyMetadata(typeof(Icon)));
        }
        #endregion

        public EnumIconType Type
        {
            get { return (EnumIconType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public PathFigureCollection Data
        {
            get { return (PathFigureCollection)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
    }
}
