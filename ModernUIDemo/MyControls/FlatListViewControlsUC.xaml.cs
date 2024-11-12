namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    using ModernUIDemo.Core;

    /// <summary>
    /// Interaktionslogik für FlatListViewControlsUC.xaml
    /// </summary>
    public partial class FlatListViewControlsUC : UserControl
    {
        public FlatListViewControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.CmdAgg.AddOrSetCommand("SelectedRowCommand", new RelayCommand(p1 => this.SelectedRowClick(p1), p2 => true));
            this.CmdAgg.AddOrSetCommand("SelectionChangedCommand", new RelayCommand(p1 => this.SelectionChangedClick(p1), p2 => true));
            this.DataContext = this;
        }

        private void SelectionChangedClick(object p1)
        {
            this.SelectionChanged.Text = (((System.Tuple<int, string, string>)p1).Item1).ToString();
        }

        private void SelectedRowClick(object p1)
        {
            string item = (((System.Tuple<int, string, string>)p1).Item1).ToString();
            MessageBox.Show(item);
        }

        public ICommandAggregator CmdAgg { get; } = new CommandAggregator();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Tuple<int, string, string>> list = new ObservableCollection<Tuple<int, string, string>>();

            for (int i = 0; i < 300; i++)
            {
                list.Add(new Tuple<int, string, string>(i, i.ToString(), i.ToString()));
            }

            this.FlatListView.ItemsSource = list;
        }
    }
}
