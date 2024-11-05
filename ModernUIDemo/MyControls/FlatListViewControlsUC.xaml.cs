namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für FlatListViewControlsUC.xaml
    /// </summary>
    public partial class FlatListViewControlsUC : UserControl
    {
        public FlatListViewControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

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
