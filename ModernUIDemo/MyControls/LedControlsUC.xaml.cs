namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für LedControlsUC.xaml
    /// </summary>
    public partial class LedControlsUC : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<Color> colors = new List<Color>
        {
            System.Windows.Media.Colors.DodgerBlue,
            System.Windows.Media.Colors.BlueViolet,
            System.Windows.Media.Colors.DarkSlateBlue,
            System.Windows.Media.Colors.Chocolate,
            System.Windows.Media.Colors.Yellow
        };

        private int activeLed = 0;
        private string setLED = "0";


        public LedControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        public List<Color> Colors
        {
            get { return colors; }
            set
            {
                colors = value;
                this.OnPropertyChanged();
            }
        }

        public int ActiveLed
        {
            get { return activeLed; }
            set
            {
                activeLed = value;
                this.OnPropertyChanged();
            }
        }

        public string SetLED
        {
            get { return setLED; }
            set
            {
                setLED = value;
                if (string.IsNullOrEmpty(setLED) == false)
                {
                    this.ActiveLed = Convert.ToInt32(setLED);
                    this.OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
