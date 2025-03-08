namespace ModernIU.Controls
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for AboutThisWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {

        private string _uri;
        private bool _ForceClose;

        public HelpWindow()
        {
            InitializeComponent();
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.CloseWindow, "Click", this.OnClose);

            this._uri = "about:blank";
            this.Title = "Just About Nothing";
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.ForceClose();
        }

        public void ForceClose()
        {
            this._ForceClose = true;
            this.Close();
        }

        public void Navigate(string title, string uri)
        {
            this.Title = title;
            this.Uri = uri;
            this.Visibility = Visibility.Visible;
            this.Activate();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (this._ForceClose == false)
            {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            }
        }

        private string Uri
        {
            get
            {
                return this._uri;
            }
            set
            {
                this._uri = value;
                try
                {
                    string htmlContent = LoadResourceText(value);

                    this.HelpFrame.NavigateToString(htmlContent.ToString());
                }
                catch (Exception ex)
                {
                    string errmsg;
                    if (ex is UriFormatException)
                    {
                        errmsg = string.Format("{0}\r\n\r\nUri: {1}", string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace), Uri);
                    }
                    else
                    {
                        errmsg = string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
                    }

                    this.HelpFrame.NavigateToString(errmsg);
                    this.Title = string.Format("Error: {0}", ex.GetType());
                }
            }
        }

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
                result = string.Empty;
            }

            return result;
        }
    }
}
