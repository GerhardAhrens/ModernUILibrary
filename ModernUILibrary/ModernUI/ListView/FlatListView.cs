namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    using ModernIU.Base;

    public class FlatListView : ListView
    {
        public static readonly DependencyProperty ItemCountProperty = 
            DependencyProperty.Register("ItemCount", typeof(int), typeof(FlatListView), new FrameworkPropertyMetadata(0));

        public int ItemCount
        {
            get { return (int)GetValue(ItemCountProperty); }
            set { SetValue(ItemCountProperty, value); }
        }

        public static readonly DependencyProperty SelectedRowCommandProperty =
            DependencyProperty.Register(nameof(SelectedRowCommand), typeof(ICommand), typeof(FlatListView), new PropertyMetadata(null));

        public ICommand SelectedRowCommand
        {
            get { return (ICommand)GetValue(SelectedRowCommandProperty); }
            set { SetValue(SelectedRowCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register(nameof(SelectionChangedCommand), typeof(ICommand), typeof(FlatListView), new PropertyMetadata(null));

        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        static FlatListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatListView), new FrameworkPropertyMetadata(typeof(FlatListView)));
        }

        public FlatListView()
        {
            this.Loaded += this.OnLoaded;
            this.MouseDoubleClick += this.OnMouseDoubleClick;
            this.KeyDown += this.OnKeyDown;
            this.SelectionChanged += this.OnSelectionChanged;
            this.SetValue(KeyboardNavigation.IsTabStopProperty, false);
            this.SetValue(ScrollViewer.IsDeferredScrollingEnabledProperty, false);
            this.SetValue(VirtualizingPanel.IsVirtualizingProperty, true);
            this.SetValue(VirtualizingPanel.VirtualizationModeProperty, VirtualizationMode.Recycling);
        }

        ~FlatListView()
        {
            this.Loaded -= this.OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ItemCount = this.Items.Count;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            DependencyObject originalSource = (DependencyObject)e.OriginalSource;
            while ((originalSource != null) && !(originalSource is ListViewItem))
            {
                originalSource = VisualTreeHelper.GetParent(originalSource);
                if (originalSource != null && (originalSource.GetType() == typeof(Thumb) || originalSource.GetType() == typeof(ScrollViewer)))
                {
                    e.Handled = true;
                    return;
                }
            }

            if (this.SelectedRowCommand != null && this.SelectedRowCommand.CanExecute(this.SelectedItem) == true)
            {
                this.SelectedRowCommand.Execute(this.SelectedItem);
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectionChangedCommand != null && this.SelectionChangedCommand.CanExecute(this.SelectedItem) == true)
            {
                this.SelectionChangedCommand.Execute(this.SelectedItem);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (this.SelectedRowCommand != null && this.SelectedRowCommand.CanExecute(this.SelectedItem) == true)
                {
                    this.SelectedRowCommand.Execute(this.SelectedItem);
                }
            }
        }
    }
}
