namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using ModernBaseLibrary.Graphics.SVG;

    /// <summary>
    /// Interaktionslogik für GraphicsControlsUC.xaml
    /// </summary>
    public partial class GraphicsControlsUC : UserControl, INotifyPropertyChanged
    {
        private readonly string FileName = "Demo_A.svg";

        public GraphicsControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            string fullPath = Path.Combine(CurrentAssemblyPath(), "Images\\SVG\\", FileName);
            if (File.Exists(fullPath))
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    this.SVGImage.Source = SvgReader.Load(stream);
                }
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
