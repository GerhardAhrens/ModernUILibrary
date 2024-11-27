namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für ImageGIFControlsUC.xaml
    /// </summary>
    public partial class ImageGIFControlsUC : UserControl, INotifyPropertyChanged
    {
        private string _CurrentPath;

        public ImageGIFControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.DataContext = this;
        }

        public string CurrentPath
        {
            get { return _CurrentPath; }
            set
            {
                if (_CurrentPath != value)
                {
                    _CurrentPath = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Debugger.IsAttached == true)
            {
                DirectoryInfo di = new DirectoryInfo(this.CurrentAssemblyPath());
                string path = di.Parent.Parent.Parent.FullName;
                this.CurrentPath = Path.Combine(path, @"Images\", "DonaldDuck.gif");
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(this.CurrentAssemblyPath());
                string path = di.Parent.Parent.Parent.FullName;
                this.CurrentPath = Path.Combine(path, @"Images\", "DonaldDuck.gif");
            }

        }

        private string CurrentAssemblyPath()
        {
            string result = string.Empty;

            result = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            return result;
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
