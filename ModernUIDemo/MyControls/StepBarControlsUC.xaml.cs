namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für StepBarControlsUC.xaml
    /// </summary>
    public partial class StepBarControlsUC : UserControl, INotifyPropertyChanged
    {
        ObservableCollection<string> list = new ObservableCollection<string>();

        public StepBarControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            list.Add("Fertig");
            list.Add("Weiter");
            list.Add("Fertig");
            list.Add("Weiter");

            this.stepBar1.ItemsSource = list;
            this.text.DataContext = Step;

        }

        private int _Step;

        public int Step
        {
            get
            {
                int temp = this.stepBar.Progress;
                return ++temp;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            this.stepBar1.Progress++;
            this.stepBar.Progress++;
            this.text.DataContext = Step;
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            this.stepBar1.Progress--;
            this.stepBar.Progress--;
            this.text.DataContext = Step;
        }
        private void btn_AddItem(object sender, RoutedEventArgs e)
        {
            list.Add("Weiter");
        }

        private void btn_RemoveItem(object sender, RoutedEventArgs e)
        {
            list.RemoveAt(0);
            this.text.DataContext = Step;
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
