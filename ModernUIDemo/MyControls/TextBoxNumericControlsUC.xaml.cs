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

        private DateTime? _ValueDate;

        public DateTime? ValueDate
        {
            get { return this._ValueDate; }
            set 
            { 
                this._ValueDate = value;
                this.SetField(ref _ValueDate, value);
            }
        }

        //public XamlProperty<DateTime?> ValueDate { get; set; } = XamlProperty.Set<DateTime?>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ValueDate = DateTime.Now;
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
