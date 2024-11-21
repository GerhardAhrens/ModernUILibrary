namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für CarouselControlsUC.xaml
    /// </summary>
    public partial class CarouselControlsUC : UserControl, INotifyPropertyChanged
    {
        public CarouselControlsUC()
        {
            this.InitializeComponent();

            this.CarouseASource = new ObservableCollection<string>();
            for (int i = 0; i < 5; i++)
            {
                CarouseASource.Add(i.ToString());
            }

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private ObservableCollection<string> _CarouseASource;

        public ObservableCollection<string> CarouseASource
        {
            get { return _CarouseASource; }
            set 
            {
                _CarouseASource = value;
                this.OnPropertyChanged();
            }
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
    }

    public class CarouselModel
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
