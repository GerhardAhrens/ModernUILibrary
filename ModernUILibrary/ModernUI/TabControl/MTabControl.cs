namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    public class MTabControl : TabControl
    {
        public static readonly DependencyProperty TypeProperty;

        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register("HeaderContent", typeof(object), typeof(MTabControl));

        static MTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTabControl), new FrameworkPropertyMetadata(typeof(MTabControl)));
            MTabControl.TypeProperty = DependencyProperty.Register("Type", typeof(EnumTabControlType), typeof(MTabControl), new PropertyMetadata(EnumTabControlType.Line));
        }

        public MTabControl()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.Focusable = true;
        }

        public EnumTabControlType Type
        {
            get { return (EnumTabControlType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public object HeaderContent
        {
            get { return (object)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabItem();
        }
    }
}
