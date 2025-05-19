namespace ModernTemplate.Core
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class WindowTitleMain
    {
        private static WindowTitleModel _WindowTitleModel = new WindowTitleModel();
        public static WindowTitleModel WindowTitleLine
        {
            get { return _WindowTitleModel; }
            set { _WindowTitleModel = value; }
        }
    }

    public class WindowTitleModel : INotifyPropertyChanged
    {
        public WindowTitleModel()
        {
            this.ProductVersion = string.Empty;
            this.WindowTitle = string.Empty;
        }

        private string _ProductVersion = string.Empty;
        public string ProductVersion
        {
            get { return _ProductVersion; }
            set
            {
                _ProductVersion = value;
                this.OnPropertyChanged();
            }
        }

        private string _WindowTitle = string.Empty;
        public string WindowTitle
        {
            get { return _WindowTitle; }
            set
            {
                _WindowTitle = value;
                this.OnPropertyChanged();
            }
        }

        public void SetWindowTitle(string windowTitle = null)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            string productName = fvi.ProductName;

            if (string.IsNullOrEmpty(windowTitle) == true)
            {
                this.WindowTitle = $"{productName} {version}";
            }
            else
            {
                this.WindowTitle = $"{productName} {version} [{windowTitle}]";
            }
        }

        #region INotifyPropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged Implementierung
    }
}
