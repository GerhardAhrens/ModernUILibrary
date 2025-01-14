namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using DemoDataGeneratorLib.Base;

    using ModernIU.Controls;
    using ModernIU.WPF.Base;

    using ModernUIDemo.Model;

    /// <summary>
    /// Interaktionslogik für ListBoxControlsUC.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class ListBoxControlsUC : UserControl, INotifyPropertyChanged
    {
        public ListBoxControlsUC()
        {
            this.InitializeComponent();

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.DataContext = this;
        }

        public XamlProperty<List<string>> MComboBoxSource { get; set; } = XamlProperty.Set<List<string>>();
        public XamlProperty<IEnumerable<CheckComboBoxTest>> ListBoxSource { get; set; } = XamlProperty.Set<IEnumerable<CheckComboBoxTest>>();
        public XamlProperty<string> MComboBoxSourceSelectedItem { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<Brush> SelectedColorItem { get; set; } = XamlProperty.Set<Brush>();

        private IEnumerable<CheckComboBoxTest> _ListBoxSourceSelectedItem;
        public IEnumerable<CheckComboBoxTest> ListBoxSourceSelectedItem
        {
            get { return _ListBoxSourceSelectedItem; }
            set
            {
                SetField(ref _ListBoxSourceSelectedItem, value);
            }
        }

        public XamlProperty<List<string>> FilterdComboBoxSource { get; set; } = XamlProperty.Set<List<string>>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            /*

            List<CheckComboBoxTest> data = new List<CheckComboBoxTest>();
            data.Add(new CheckComboBoxTest(1, "C#"));
            data.Add(new CheckComboBoxTest(2, "C++"));
            data.Add(new CheckComboBoxTest(3, "VB.Net"));
            data.Add(new CheckComboBoxTest(4, "Javascript"));
            data.Add(new CheckComboBoxTest(5, "Object C"));
            data.Add(new CheckComboBoxTest(6, "Java"));

            //this.CheckComboBox.ItemsSource = data;
            this.CheckComboBox.Items.Add(data[0]);
            this.CheckComboBox.Items.Add(data[1]);
            this.CheckComboBox.Items.Add(data[2]);
            this.CheckComboBox.DisplayMemberPath = "Content";
            */

            this.MComboBoxSource.Value = new List<string> { "Affe", "Bär", "Elefant", "Hund", "Zebra" };
            this.FilterdComboBoxSource.Value = new List<string> { "Affe", "Bär", "Ameise","Igel", "Elefant", "Hund","Pferd","Pinguin", "Zebra" , "2001", "2010", "2024","2030"};
            this.SelectedColorItem.Value = Brushes.Transparent;
            this.FillData();
        }

        private void FillData()
        {
            IEnumerable<CheckComboBoxTest> listBoxSorce = BuildDemoData<CheckComboBoxTest>.CreateForList<CheckComboBoxTest>(ConfigMultiSelectListbox, 1_000);
            listBoxSorce = new ObservableCollection<CheckComboBoxTest>(listBoxSorce.AsParallel().OrderBy(o => o.ID));
            this.ListBoxSource.Value = listBoxSorce;
        }

        private CheckComboBoxTest ConfigMultiSelectListbox(CheckComboBoxTest listBoxDemoData)
        {
            listBoxDemoData.ID = BuildDemoData.Integer(1, 10_000);
            listBoxDemoData.Content = BuildDemoData.LastName();

            return listBoxDemoData;
        }

        private void btnGetContent_Click(object sender, RoutedEventArgs e)
        {
            MMessageBox.Show(this.CheckComboBox.Content.ToString(), "Auswahl", MessageBoxButton.OK);
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
    }

    public class CheckComboBoxTest
    {
        public int ID { get; set; }
        public string Content { get; set; }
    }
}
