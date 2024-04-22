namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;

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
            this.OnPropertyChanged(propertyName);
            return true;
        }
        #endregion PropertyChanged Implementierung
    }
}
