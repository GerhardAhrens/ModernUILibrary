namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für CheckControlsUC.xaml
    /// </summary>
    public partial class RadioButtonControlsUC : UserControl, INotifyPropertyChanged
    {
        public RadioButtonControlsUC()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private bool _IsErstelleLabel;

        public bool IsErstelleLabel
        {
            get { return _IsErstelleLabel; }
            set
            {
                _IsErstelleLabel = value;
                this.OnPropertyChanged();
            }
        }

        private bool _IsRBAnimiert;

        public bool IsRBAnimiert
        {
            get { return _IsRBAnimiert; }
            set
            {
                _IsRBAnimiert = value;
                this.OnPropertyChanged();
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
}
