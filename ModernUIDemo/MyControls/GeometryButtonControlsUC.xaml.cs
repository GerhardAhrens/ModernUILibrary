namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für GeometryButtonControlsUC.xaml
    /// </summary>
    public partial class GeometryButtonControlsUC : UserControl, INotifyPropertyChanged
    {
        public GeometryButtonControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            var colors = typeof(Colors).GetProperties().Select(x => x.Name);
            this.BackgroundColorPicker.ItemsSource = colors;
            this.ForegroundColorPicker.ItemsSource = colors;

            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void GeometryButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.Text.Text, "GeometryButton");
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
