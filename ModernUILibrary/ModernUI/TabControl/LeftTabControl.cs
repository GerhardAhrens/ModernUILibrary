namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernIU.Base;

    public class LeftTabControl : TabControl
    {
        public static readonly DependencyProperty HeaderContentProperty;
        public static readonly DependencyProperty SelectionChangedCommandProperty;

        static LeftTabControl()
        {
            LeftTabControl.HeaderContentProperty = DependencyProperty.Register(nameof(HeaderContent), typeof(object), typeof(LeftTabControl));
            LeftTabControl.SelectionChangedCommandProperty = DependencyProperty.Register(nameof(SelectionChangedCommand), typeof(ICommand), typeof(LeftTabControl), new PropertyMetadata(null));
        }

        public LeftTabControl()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.Focusable = true;
            this.TabStripPlacement = Dock.Left;
            this.SetValue(Grid.IsSharedSizeScopeProperty, true);

            this.SelectionChanged += this.OnSelectionChanged;
        }

        ~LeftTabControl()
        {
            this.SelectionChanged -= this.OnSelectionChanged;
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
