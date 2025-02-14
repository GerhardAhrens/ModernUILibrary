namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using ModernBaseLibrary.Extension;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für TextBoxDesignControlsUC.xaml
    /// </summary>
    public partial class TextBoxDesignControlsUC : UserControl, INotifyPropertyChanged
    {
        public TextBoxDesignControlsUC()
        {
            this.InitializeComponent();
            this.CurrentWindow = Application.Current.Windows.LastActiveWindow();

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.CmdAgg.AddOrSetCommand("EditableTextBoxSave", new RelayCommand(p1 => this.EditableTextBoxSave(), p2 => true));

            this.DataContext = this;
        }

        private string _Text;

        public string Text
        {
            get { return this._Text; }
            set 
            {
                this._Text = value;
                this.OnPropertyChanged();
            }
        }


        public ICommandAggregator CmdAgg { get; } = new CommandAggregator();

        private Window CurrentWindow { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void EditableTextBoxSave()
        {
            MessageBox.Show(this.CurrentWindow, "Änderung wurde gespeichert!", "Änderung Speichern", MessageBoxButton.OK, MessageBoxImage.Information);
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
