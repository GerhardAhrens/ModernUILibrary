namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using DemoDataGeneratorLib.Base;

    using ModernUI.MVVM.Base;

    using ModernUIDemo.Core;
    using ModernUIDemo.Model;

    /// <summary>
    /// Interaktionslogik für FlatListViewControlsUC.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class FlatListViewControlsUC : UserControl
    {
        public FlatListViewControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.CmdAgg.AddOrSetCommand("SelectedRowCommand", new RelayCommand(p1 => this.SelectedRowClick(p1), p2 => true));
            this.CmdAgg.AddOrSetCommand("SelectionChangedCommand", new RelayCommand(p1 => this.SelectionChangedClick(p1), p2 => true));
            this.CmdAgg.AddOrSetCommand("MouseDoubleClickCommand", new RelayCommand(p1 => this.SelectionChangedClick(p1), p2 => true));
            this.DataContext = this;
        }

        public ObservableCollection<Employe> Employes { get; set; }

        private void SelectionChangedClick(object p1)
        {
            this.SelectionChanged.Text = ((Employe)p1).LastName;
        }

        private void SelectedRowClick(object p1)
        {
            string item = ((Employe)p1).LastName; ;
            MessageBox.Show(item);
        }

        public ICommandAggregator CmdAgg { get; } = new CommandAggregator();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Employes = BuildDemoData<Employe>.CreateForObservableCollection<Employe>(ConfigObject, 1_000);
            this.FlatListView.ItemsSource = this.Employes;
        }

        private Employe ConfigObject(Employe employe, int counter)
        {

            var timeStamp = BuildDemoData.SetTimeStamp();
            employe.Id = counter;
            employe.FirstName = BuildDemoData.FirstName();
            employe.LastName = BuildDemoData.LastName();
            employe.Salary = BuildDemoData.Integer(1_000, 10_000);
            employe.Symbol = BuildDemoData.Symbols();
            employe.StartDate = BuildDemoData.Dates(new DateTime(2000, 1, 1), DateTime.Now);
            employe.Manager = BuildDemoData.Boolean();
            employe.CreateOn = timeStamp.CreateOn;
            employe.CreateBy = timeStamp.CreateBy;
            employe.ModifiedOn = timeStamp.ModifiedOn;
            employe.ModifiedBy = timeStamp.ModifiedBy;

            return employe;
        }
    }
}
