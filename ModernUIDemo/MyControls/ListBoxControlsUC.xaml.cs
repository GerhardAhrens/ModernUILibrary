namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using ModernIU.Controls;

    using ModernUIDemo.Core;

    /// <summary>
    /// Interaktionslogik für ListBoxControlsUC.xaml
    /// </summary>
    public partial class ListBoxControlsUC : UserControl, INotifyPropertyChanged
    {
        public ListBoxControlsUC()
        {
            this.InitializeComponent();

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.DataContext = this;
        }

        public XamlProperty<List<string>> MComboBoxSource { get; set; } = XamlProperty.Set<List<string>>();
        public XamlProperty<List<string>> ListBoxSource { get; set; } = XamlProperty.Set<List<string>>();
        public XamlProperty<string> MComboBoxSourceSelectedItem { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<Brush> SelectedColorItem { get; set; } = XamlProperty.Set<Brush>();

        private Dictionary<string, object> _ListBoxSourceSelectedItem;
        public Dictionary<string, object> ListBoxSourceSelectedItem
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

            this.MComboBoxSource.Value = new List<string> { "Affe", "Bär", "Elefant", "Hund", "Zebra" };
            this.ListBoxSource.Value = new List<string> { "Affe", "Bär", "Elefant", "Hund", "Zebra" };
            this.FilterdComboBoxSource.Value = new List<string> { "Affe", "Bär", "Ameise","Igel", "Elefant", "Hund","Pferd","Pinguin", "Zebra" , "2001", "2010", "2024","2030"};

            this.SelectedColorItem.Value = Brushes.Transparent;
        }

        internal class CheckComboBoxTest
        {
            public int ID { get; set; }
            public string Content { get; set; }

            public CheckComboBoxTest(int id, string content)
            {
                this.ID = id;
                this.Content = content;
            }
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
}
