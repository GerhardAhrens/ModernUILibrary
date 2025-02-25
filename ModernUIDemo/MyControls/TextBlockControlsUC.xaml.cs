namespace ModernUIDemo.MyControls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernIU.Controls;
    using ModernIU.Controls.Chart;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für TextBlockControlsUC.xaml
    /// </summary>
    public partial class TextBlockControlsUC : UserControl, INotifyPropertyChanged
    {
        private ICommand requestNavigateCommand;

        public TextBlockControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        public ICommand RequestNavigateCommand => requestNavigateCommand ??= new RelayCommand<UriEventArgs>(OnRequestNavigateHandler);

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.tbFormattedTextBlock.Text = "<b>Test</b>; <u>Hallo</u>; <fs20>Hallo</fs>; <fg=green>Hallo</fg>";
        }

        private void OnRequestNavigateHandler(UriEventArgs item)
        {
            MessageBox.Show(item.TextNavigate, "URL anueigen");
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
