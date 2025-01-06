namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using DemoDataGeneratorLib.Base;

    using ModernBaseLibrary.Cryptography;

    using ModernUI.MVVM.Base;

    using ModernUIDemo.Model;

    /// <summary>
    /// Interaktionslogik für FilterDataGridControlsUC.xaml
    /// </summary>
    public partial class FilterDataGridControlsUC : UserControl, INotifyPropertyChanged
    {
        private ICollectionView collView;
        private string search;

        public ICommand RefreshCommand => new DelegateCommand(this.RefreshData);
        public ICommand LoadingRowCommand => new DelegateCommand(this.LoadingRowHandler);
        public ICommand SelectedRowCommand => new DelegateCommand(this.SelectedRowHandler);

        public ObservableCollection<Employe> Employes { get; set; }
        public ObservableCollection<Employe> FilteredList { get; set; }

        public string Search
        {
            get => this.search;
            set
            {
                this.search = value;

                if (this.collView != null)
                {
                    this.collView.Filter = e =>
                    {
                        var item = (Employe)e;
                        return item != null && ((item.LastName?.StartsWith(this.search, StringComparison.OrdinalIgnoreCase) ?? false)
                                                || (item.FirstName?.StartsWith(this.search, StringComparison.OrdinalIgnoreCase) ?? false));
                    };

                    this.collView.Refresh();

                    this.FilteredList = new ObservableCollection<Employe>(collView.OfType<Employe>().ToList());

                    this.OnPropertyChanged("Search");
                    this.OnPropertyChanged("FilteredList");
                }
            }
        }

        public FilterDataGridControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.FillData();
        }

        private void FillData()
        {
            this.Search = string.Empty;
            IEnumerable<Employe> employe = BuildDemoData<Employe>.CreateForList<Employe>(ConfigObject, 1_000);
            this.Employes = new ObservableCollection<Employe>(employe.AsParallel().OrderBy(o => o.LastName));

            this.FilteredList = new ObservableCollection<Employe>(this.Employes);
            this.collView = CollectionViewSource.GetDefaultView(this.FilteredList);

            this.OnPropertyChanged("Search");
            this.OnPropertyChanged("Employes");
            this.OnPropertyChanged("FilteredList");
        }

        private Employe ConfigObject(Employe employe)
        {

            var timeStamp = BuildDemoData.SetTimeStamp();
            employe.FirstName = BuildDemoData.FirstName();
            employe.LastName = BuildDemoData.LastName();
            employe.Salary = BuildDemoData.Integer(1_000,10_000);
            employe.StartDate = BuildDemoData.Dates(new DateTime(2000,1,1),DateTime.Now);
            employe.Manager = BuildDemoData.Boolean();
            employe.CreateOn = timeStamp.CreateOn;
            employe.CreateBy = timeStamp.CreateBy;
            employe.ModifiedOn = timeStamp.ModifiedOn;
            employe.ModifiedBy = timeStamp.ModifiedBy;

            return employe;
        }

        private void RefreshData(object obj)
        {
            this.FillData();
        }

        private void LoadingRowHandler(object obj)
        {
            DataGridRowEventArgs e = (DataGridRowEventArgs)obj;
            var index = e.Row.GetIndex() + 1;
            e.Row.Header = $"{index}";
        }

        private void SelectedRowHandler(object obj)
        {
        }

        #region PropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion PropertyChanged Implementierung
    }
}
