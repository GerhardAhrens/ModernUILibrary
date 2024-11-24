namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für TagControlsUC.xaml
    /// </summary>
    public partial class TagControlsUC : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<string> list = new ObservableCollection<string>();

        public TagControlsUC()
        {
            this.InitializeComponent();

            list.Add("Tag 1");
            list.Add("Tag 2");

            this.TagBox.ItemsSource = list;
            this.TagInputBox.ItemsSource = list;

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
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

        private void TagA_Closed(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MessageBox.Show("Klicken Sie auf die Schaltfläche 'Schließen'");
        }

        private void AddOneClick(object sender, RoutedEventArgs e)
        {
            Tag tag = new Tag()
            {
                CornerRadius = new CornerRadius(3),
                Content = "Neuer Tag",
                Margin = new Thickness(2, 2, 2, 2)
            };

            this.list.Add("Neuer Tag");
        }
    }
}
