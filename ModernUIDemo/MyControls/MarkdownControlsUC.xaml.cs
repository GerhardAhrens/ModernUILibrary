namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für MarkdownControlsUC.xaml
    /// </summary>
    public partial class MarkdownControlsUC : UserControl, INotifyPropertyChanged
    {
        public MarkdownControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private string mdDemoText;

        public string MDDemoText
        {
            get { return mdDemoText; }
            set 
            { 
                mdDemoText = value;
                this.OnPropertyChanged();
            }
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.MDDemoText = this.LoadResourceText("DemoText.md");
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

        private string LoadResourceText(string filename)
        {
            string result = string.Empty;
            Assembly executingAssembly = Assembly.GetEntryAssembly();
            string[] allResourceNames = executingAssembly.GetManifestResourceNames();
            var pathForHelp = allResourceNames.FirstOrDefault<string>(p => p.Contains(filename) == true);
            if (pathForHelp != null)
            {
                using (var stream = executingAssembly.GetManifestResourceStream(pathForHelp))
                {
                    if (stream != null)
                    {
                        using (var r = new StreamReader(stream))
                        {
                            result = r.ReadToEnd();
                        }
                    }
                }
            }
            else
            {
                result = NoHelpContent();
            }

            return result;
        }

        private static string NoHelpContent()
        {
            string result = string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"##Markdown Demotext **");
            sb.AppendLine("***");
            sb.AppendLine("Es wurde kein Markdown Demotext gefunden.");

            result = sb.ToString();

            return result;
        }
    }
}
