namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.WPF.Base;

    /// <summary>
    /// Interaktionslogik für ComboBoxControlsUC.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class ComboBoxControlsUC : UserControl, INotifyPropertyChanged
    {
        private Dictionary<string, object> _itemsMC;
        private Dictionary<string, object> _selectedItemsMC;

        public ComboBoxControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.DataContext = this;
        }

        public XamlProperty<List<string>> FilterdComboBoxSource { get; set; } = XamlProperty.Set<List<string>>();

        public XamlProperty<Dictionary<int,string>> MComboBoxSource { get; set; } = XamlProperty.Set<Dictionary<int, string>>();

        private TestEnum1 _TestEnumProperty;

        public TestEnum1 TestEnumProperty
        {
            get { return this._TestEnumProperty; }
            set
            {
                this._TestEnumProperty = value;
                this.OnPropertyChanged();
            }
        }

        public Dictionary<string, object> ItemsMC
        {
            get { return this._itemsMC; }
            set
            {
                this._itemsMC = value;
                this.OnPropertyChanged();
            }
        }

        public Dictionary<string, object> SelectedItemsMC
        {
            get { return this._selectedItemsMC; }
            set
            {
                this._selectedItemsMC = value;
                this.OnPropertyChanged();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Dictionary<int, string> mdataSource = new Dictionary<int, string>();
            mdataSource.Add(1, "C#");
            mdataSource.Add(2, "C++");
            mdataSource.Add(3, "VB.Net");
            mdataSource.Add(4, "Javascript");
            mdataSource.Add(5, "Object C");
            mdataSource.Add(6, "Java");
            MComboBoxSource.Value = mdataSource;

            this.FilterdComboBoxSource.Value = new List<string> { "Affe", "Bär", "Ameise", "Igel", "Elefant", "Hund", "Pferd", "Pinguin", "Zebra", "2001", "2010", "2024", "2030" };

            Dictionary<string, object> itemsMC = new Dictionary<string, object>();
            itemsMC.Add("Chennai", "MAS");
            itemsMC.Add("Trichy", "TPJ");
            itemsMC.Add("Bangalore", "SBC");
            itemsMC.Add("Coimbatore", "CBE");
            this.ItemsMC = itemsMC;
        }
        private void Submit()
        {
            foreach (KeyValuePair<string, object> s in this.SelectedItemsMC)
            {
                MessageBox.Show(s.Key);
            }
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

    public enum TestEnum1
    {
        A,
        B,
        C,
        X,
        Y,
        Z,
    }

}
