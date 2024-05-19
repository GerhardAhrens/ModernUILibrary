namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using ModernUIDemo.Core;

    /// <summary>
    /// Interaktionslogik für TextBoxNumericControlsUC.xaml
    /// </summary>
    public partial class TextBoxNumericControlsUC : UserControl, INotifyPropertyChanged
    {
        public TextBoxNumericControlsUC()
        {
            this.InitializeComponent();

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.DataContext = this;
        }

        private DateTime? _TextBoxDateValue;

        public DateTime? TextBoxDateValue
        {
            get { return this._TextBoxDateValue; }
            set 
            { 
                this._TextBoxDateValue = value;
                this.SetField(ref _TextBoxDateValue, value);
            }
        }

        public XamlProperty<DateTime?> DateTimePickerValue { get; set; } = XamlProperty.Set<DateTime?>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.TextBoxDateValue = DateTime.Now;
            this.DateTimePickerValue.Value = DateTime.Now;
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
