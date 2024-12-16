namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für TimelineControlsUC.xaml
    /// </summary>
    public partial class TimelineControlsUC : UserControl, INotifyPropertyChanged
    {
        public TimelineControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.InitTimeline2();
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            switch (btn.Tag.ToString())
            {
                case "First":
                    this.timeline.Items.Insert(0, new MTimelineItem() { Content = "Add First" });
                    break;
                case "Middle":
                    this.timeline.Items.Insert(1, new MTimelineItem() { Content = "Add Middle" });
                    break;
                case "Last":
                    this.timeline.Items.Add(new MTimelineItem() { Content = "Add Last" });
                    break;
                case "RemoveFirst":
                    this.timeline.Items.RemoveAt(0);
                    break;
                case "RemoveLast":
                    this.timeline.Items.RemoveAt(this.timeline.Items.Count - 1);
                    break;
                case "RemoveMiddle":
                    this.timeline.Items.RemoveAt(1);
                    break;
                default:
                    break;
            }
        }

        private void InitTimeline2()
        {
            ObservableCollection<Tuple<int, string, string>> list = new ObservableCollection<Tuple<int, string, string>>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(new Tuple<int, string, string>(i, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "TimeLine"));
            }

            this.timeline2.ItemsSource = list;
            this.timeline3.ItemsSource = list;
            this.timeline4.ItemsSource = list;
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
