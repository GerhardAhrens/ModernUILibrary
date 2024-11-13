namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using ModernUIDemo.Core;

    using static ModernUIDemo.MyControls.ListBoxControlsUC;

    /// <summary>
    /// Interaktionslogik für ComboBoxControlsUC.xaml
    /// </summary>
    public partial class ComboBoxControlsUC : UserControl, INotifyPropertyChanged
    {
        public ComboBoxControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
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

            this.FilterdComboBoxSource.Value = new List<string> { "Affe", "Bär", "Ameise", "Igel", "Elefant", "Hund", "Pferd", "Pinguin", "Zebra", "2001", "2010", "2024", "2030" };

            this.DataContext = this;
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
