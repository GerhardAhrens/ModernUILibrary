namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;
    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für FunctionControlsUC.xaml
    /// </summary>
    public partial class FunctionControlsUC : UserControl, INotifyPropertyChanged
    {
        public FunctionControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
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

        private void BtnPasswordgenerator_Click(object sender, RoutedEventArgs e)
        {
            PasswordGeneratorResult result = PasswordGeneratorView.Execute();
            if (result != null && result.Error == null)
            {
                if (result.Cancelled == false)
                {
                    MMessageBox.Show($"Gewähltes Passwort: {result.Result}", EnumPromptType.Info);
                }
            }
        }
    }
}
