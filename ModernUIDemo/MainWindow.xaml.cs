namespace ModernUIDemo
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using ModernUIDemo.Core;
    using ModernUIDemo.Model;
    using ModernUIDemo.MyControls;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            this.InitializeComponent();

            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.TabControlSource.Value = new ObservableCollection<TabControlItem>();
            this.TabControlSource.Value.Add(new TabControlItem("Darstellung", true));
            this.TabControlSource.Value.Add(new TabControlItem("Icon (PathGeometry)", new IconsControlsUC()));
            this.TabControlSource.Value.Add(new TabControlItem("Farben", new ColorControlsUC()));
            this.TabControlSource.Value.Add(new TabControlItem("Farben", new ColorControlsUC()));
            this.TabControlSource.Value.Add(new TabControlItem("Eingabe", true));
            this.TabControlSource.Value.Add(new TabControlItem("TextBox (String) Controls", new TextBoxStringControlsUC()));

            this.DataContext = this;
        }

        public XamlProperty<ObservableCollection<TabControlItem>> TabControlSource { get; set; } = XamlProperty.Set<ObservableCollection<TabControlItem>>();
        public XamlProperty<List<string>> MComboBoxSource { get; set; } = XamlProperty.Set<List<string>>();
        public XamlProperty<string> MComboBoxSourceSelectedItem { get; set; } = XamlProperty.Set<string>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.MComboBoxSource.Value = new List<string> { "Affe", "Bär", "Elefant", "Hund", "Zebra" };
        }

        #region PropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
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

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}