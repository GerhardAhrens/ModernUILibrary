namespace ModernUIDemo.MyControls
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using DemoDataGeneratorLib.Base;

    using ModernIU.Controls;
    using ModernIU.WPF.Base;

    using ModernUI.MVVM.Base;

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

        private IEnumerable<CheckComboBoxTest> _ListBoxASource;
        public IEnumerable<CheckComboBoxTest> ListBoxASource
        {
            get { return _ListBoxASource; }
            set
            {
                SetField(ref _ListBoxASource, value);
            }
        }

        private IEnumerable<CheckComboBoxTest> _ListBoxSourceSelectedItem;
        public IEnumerable<CheckComboBoxTest> ListBoxSourceSelectedItem
        {
            get { return _ListBoxSourceSelectedItem; }
            set
            {
                SetField(ref _ListBoxSourceSelectedItem, value);
            }
        }

        private IEnumerable<CheckComboBoxTest> _ListBoxBSource;
        public IEnumerable<CheckComboBoxTest> ListBoxBSource
        {
            get { return _ListBoxBSource; }
            set
            {
                SetField(ref _ListBoxBSource, value);
            }
        }

        private IEnumerable<string> _ListBoxCSource;
        public IEnumerable<string> ListBoxCSource
        {
            get { return _ListBoxCSource; }
            set
            {
                SetField(ref _ListBoxCSource, value);
            }
        }

        public XamlProperty<List<string>> FilterdComboBoxSource { get; set; } = XamlProperty.Set<List<string>>();

        public ICommand SelectFirstItemCommand { get; }

        public ICommand SelectLastItemCommand { get; }

        private CheckComboBoxTest _selectedItem;

        public CheckComboBoxTest SelectedItem
        {
            get => _selectedItem;
            set => SetField(ref _selectedItem, value);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnSelectFirst, "Click", this.OnSelectFirst);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnSelectLast, "Click", this.OnSelectLast);

            this.MComboBoxSource.Value = new List<string> { "Affe", "Bär", "Elefant", "Hund", "Zebra" };
            this.FilterdComboBoxSource.Value = new List<string> { "Affe", "Bär", "Ameise","Igel", "Elefant", "Hund","Pferd","Pinguin", "Zebra" , "2001", "2010", "2024","2030"};
            this.SelectedColorItem.Value = Brushes.Transparent;
            this.FillDataListBoxA();
            this.FillDataListBoxB();
            this.FillDataListBoxC();
        }

        private void FillDataListBoxA()
        {
            IEnumerable<CheckComboBoxTest> listBoxSorce = BuildDemoData<CheckComboBoxTest>.CreateForList<CheckComboBoxTest>(ConfigMultiSelectListbox, 1_000);
            listBoxSorce = new ObservableCollection<CheckComboBoxTest>(listBoxSorce.AsParallel().OrderBy(o => o.ID));
            this.ListBoxASource = listBoxSorce;
        }

        private void FillDataListBoxB()
        {
            IEnumerable<CheckComboBoxTest> listBoxSorce = BuildDemoData<CheckComboBoxTest>.CreateForList<CheckComboBoxTest>(ConfigListboxEx, 1_000);
            listBoxSorce = new ObservableCollection<CheckComboBoxTest>(listBoxSorce.AsParallel().OrderBy(o => o.ID));
            this.ListBoxBSource = listBoxSorce;
        }

        private void FillDataListBoxC()
        {
            IEnumerable<string> listBoxSorce = BuildDemoData<string>.CreateForList<string>(CheckComboBoxData, 20);
            this.ListBoxCSource = listBoxSorce;
        }

        private CheckComboBoxTest ConfigMultiSelectListbox(CheckComboBoxTest listBoxDemoData, int counter)
        {
            listBoxDemoData.ID = BuildDemoData.Integer(1, 10_000);
            listBoxDemoData.Content = BuildDemoData.LastName();

            return listBoxDemoData;
        }

        private CheckComboBoxTest ConfigListboxEx(CheckComboBoxTest listBoxDemoData, int counter)
        {
            listBoxDemoData.ID = BuildDemoData.Integer(1, 10_000);
            listBoxDemoData.Content = BuildDemoData.LastName();

            return listBoxDemoData;
        }

        private string CheckComboBoxData(string text, int counter)
        {
            return BuildDemoData.ProgrammingLanguage();
        }

        private void btnGetContent_Click(object sender, RoutedEventArgs e)
        {
            MMessageBox.Show(this.CheckComboBox.Content.ToString(), "Auswahl", MessageBoxButton.OK);
        }

        private void OnSelectFirst(object sender, RoutedEventArgs e)
        {
            this.SelectedItem = ListBoxBSource.First();
        }

        private void OnSelectLast(object sender, RoutedEventArgs e)
        {
            this.SelectedItem = ListBoxBSource.Last();
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

    [DebuggerDisplay("ID={this.ID};Content={this.Content}")]
    public class CheckComboBoxTest
    {
        public int ID { get; set; }
        public string Content { get; set; }

        public override string ToString()
        {
            return $"ID={this.ID};Content={this.Content}";
        }
    }
}
