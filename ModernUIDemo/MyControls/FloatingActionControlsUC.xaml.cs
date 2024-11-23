namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für FloatingActionControlsUC.xaml
    /// </summary>
    public partial class FloatingActionControlsUC : UserControl, INotifyPropertyChanged
    {
        public FloatingActionControlsUC()
        {
            this.InitializeComponent();

            ObservableCollection<Student> list = new ObservableCollection<Student>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(new Student() { Name = "Button:" + i, ID = i });
            }

            this.FloatingActionMenu.ItemsSource = list;
            this.FloatingActionMenu.DisplayMemberPath = "Name";
            this.FloatingActionMenu.DisplayTipContentMemberPath = "ID";

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void FloatingActionMenu_ItemClick(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MessageBox.Show(string.Format("OldValue={0}, NewValue={1}", e.OldValue, e.NewValue));
        }

        private void FloatingActionMenuA_ItemClick(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MessageBox.Show(string.Format("OldValue={0}, NewValue={1}", e.OldValue, e.NewValue));
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
