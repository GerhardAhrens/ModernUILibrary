namespace ModernUIDemo
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Data;

    using Microsoft.VisualBasic;

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

            List<TabControlItem> tabItemSource = new List<TabControlItem>();
            tabItemSource.Add(new TabControlItem("Darstellung", true));
            tabItemSource.Add(new TabControlItem("Icon (PathGeometry)", new IconsControlsUC()));
            tabItemSource.Add(new TabControlItem("Farben", new ColorControlsUC()));
            tabItemSource.Add(new TabControlItem("Farben", new ColorControlsUC()));
            tabItemSource.Add(new TabControlItem("Eingabe", true));
            tabItemSource.Add(new TabControlItem("TextBox (String) Controls", new TextBoxStringControlsUC()));
            tabItemSource.Add(new TabControlItem("TextBox (Numeric) Controls", new TextBoxNumericControlsUC()));
            tabItemSource.Add(new TabControlItem("ListBox Controls", new ListBoxControlsUC()));
            tabItemSource.Add(new TabControlItem("Button Controls", new ButtonControlsUC()));
            tabItemSource.Add(new TabControlItem("DropDownButton Controls", new DropDownButtonControlsUC()));
            tabItemSource.Add(new TabControlItem("RadioButton Controls", new RadioButtonControlsUC()));
            tabItemSource.Add(new TabControlItem("NumericUpDown Controls", new NumericUpDownControlsUC()));
            tabItemSource.Add(new TabControlItem("View, Loading", true));
            tabItemSource.Add(new TabControlItem("Badges Controls", new BadgesControlsUC()));
            tabItemSource.Add(new TabControlItem("BusyIndicator Controls", new BusyIndicatorControlsUC()));
            tabItemSource.Add(new TabControlItem("Loading Animation Controls", new LoadingControlsUC()));
            tabItemSource.Add(new TabControlItem($"MessageBox, NoticeMessage\nWindow", true));
            tabItemSource.Add(new TabControlItem("NoticeMessage Controls", new NoticeMessageControlsUC()));
            tabItemSource.Add(new TabControlItem("MessageBox Window", new MessageBoxControlsUC()));
            tabItemSource.Add(new TabControlItem("PopUp Window", new PopUpControlsUC()));
            tabItemSource.Add(new TabControlItem("Tooltip Controls", new PopUpControlsUC()));

            this.TabControlSource.Value = CollectionViewSource.GetDefaultView(tabItemSource);


            this.DataContext = this;
        }

        public XamlProperty<ICollectionView> TabControlSource { get; set; } = XamlProperty.Set<ICollectionView>();
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