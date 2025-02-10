namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernIU.Base;

    public class MTabControl : TabControl
    {
        public static readonly DependencyProperty TypeProperty;
        public static readonly DependencyProperty HeaderContentProperty;
        public static readonly DependencyProperty SelectionChangedCommandProperty;


        static MTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTabControl), new FrameworkPropertyMetadata(typeof(MTabControl)));
            MTabControl.TypeProperty = DependencyProperty.Register(nameof(Type), typeof(EnumTabControlType), typeof(MTabControl), new PropertyMetadata(EnumTabControlType.Line));
            MTabControl.HeaderContentProperty = DependencyProperty.Register(nameof(HeaderContent), typeof(object), typeof(MTabControl));
            MTabControl.SelectionChangedCommandProperty = DependencyProperty.Register(nameof(SelectionChangedCommand), typeof(ICommand), typeof(MTabControl), new PropertyMetadata(null));
        }

        public MTabControl()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.Focusable = true;
        }

        ~MTabControl()
        {
            this.SelectionChanged -= this.OnSelectionChanged;
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

        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectionChangedCommand != null && this.SelectionChangedCommand.CanExecute(this.SelectedItem) == true)
            {
                this.SelectionChangedCommand.Execute(this.SelectedItem);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabItem();
        }
    }
}
