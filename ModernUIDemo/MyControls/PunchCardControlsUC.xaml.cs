namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für PunchCardControlsUC.xaml
    /// </summary>
    public partial class PunchCardControlsUC : UserControl, INotifyPropertyChanged
    {
        private List<Tuple<string, List<int>>> data;
        private bool toolTips;

        public PunchCardControlsUC()
        {
            this.InitializeComponent();
            this.OnLoaded();
            this.DataContext = this;
        }

        public List<Tuple<string, List<int>>> Data
        {
            get { return this.data; }
            set
            {
                this.data = value;
                this.OnPropertyChanged();
            }
        }

        public bool ToolTips
        {
            get { return this.toolTips; }
            set
            {
                this.toolTips = value;
                this.OnPropertyChanged();
            }
        }

        private void OnLoaded()
        {
            this.ToolTips = true;

            this.Data = new List<Tuple<string, List<int>>>
            {
                new Tuple<string, List<int>>("1", new List<int> {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}),
                new Tuple<string, List<int>>("2", new List<int> {1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1, 1, 1}),
                new Tuple<string, List<int>>("3", new List<int> {1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}),
                new Tuple<string, List<int>>("4", new List<int> {1, 4, 1, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}),
                new Tuple<string, List<int>>("5", new List<int> {1, 5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 1, 1, 4, 1, 1, 1, 1, 1}),
                new Tuple<string, List<int>>("6", new List<int> {1, 6, 1, 1, 1, 2, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}),
                new Tuple<string, List<int>>("7", new List<int> {1, 7, 1, 1, 1, 1, 1, 1, 1, 6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1})
            };
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
