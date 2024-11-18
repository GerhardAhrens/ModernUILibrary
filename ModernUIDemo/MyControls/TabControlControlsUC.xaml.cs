namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für TabControlControlsUC.xaml
    /// </summary>
    public partial class TabControlControlsUC : UserControl, INotifyPropertyChanged
    {
        ObservableCollection<TabInfo> list = new ObservableCollection<TabInfo>();

        public TabControlControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            list.Add(new TabInfo() { Title = "Windows", Type = 1 });
            list.Add(new TabInfo() { Title = "MacOS", Type = 2 });
            list.Add(new TabInfo() { Title = "Linux", Type = 3 });
            this.tabControl3.ItemsSource = list;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        protected class TabInfo
        {
            public string Title { get; set; }
            public int Type { get; set; }
        }

        private void btnTab3AddItem_Click(object sender, RoutedEventArgs e)
        {
            this.list.Add(new TabInfo() { Title = "Android", Type = 4 });
            this.btnTab3AddItem.IsEnabled = false;
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
