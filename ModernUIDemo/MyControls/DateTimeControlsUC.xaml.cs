namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    using ModernUIDemo.Core;

    /// <summary>
    /// Interaktionslogik für DateTimeControlsUC.xaml
    /// </summary>
    public partial class DateTimeControlsUC : UserControl, INotifyPropertyChanged
    {
        public DateTimeControlsUC()
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
#if DEBUG
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
#endif
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

        private void MCalendar_SelectedDateChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            if (e.NewValue is DateTime datum)
            {
                MessageBox.Show(datum.ToString());
            }
        }

        private void FlatDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is FlatDatePicker datum)
            {
                MessageBox.Show(datum.SelectedDate.ToString());
            }
        }
    }
}
