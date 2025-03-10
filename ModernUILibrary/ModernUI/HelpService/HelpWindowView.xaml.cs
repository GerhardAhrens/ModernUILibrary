namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;

    using ModernIU.Base;

    /// <summary>
    /// Interaction logic for AboutThisWindow.xaml
    /// </summary>
    public partial class HelpWindowView : Window, INotifyPropertyChanged
    {
        public ICommand CancelButtonCommand => new UIButtonCommand(p => this.OnCancelButtonClick(), p => true);

        public event PropertyChangedEventHandler PropertyChanged;

        public HelpWindowView()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void OnCancelButtonClick()
        {
            this.DialogResult = true;
            this.Close();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        public static HelpWindowResult Execute(Window owner, string headerText, string htmlPage)
        {
            return ExecuteInternal(owner, headerText, htmlPage);
        }

        public static HelpWindowResult Execute(string headerText, string htmlPage)
        {
            Window actualWindow = Application.Current.Windows.Cast<Window>().Last(l => l.IsActive == true);
            return ExecuteInternal(actualWindow, headerText, htmlPage);
        }

        public HelpWindowResult Execute()
        {
            HelpWindowResult result = null;
            HelpWindowEventArgs args = null;
            bool? resultDialog = false;

            try
            {
                resultDialog = this.ShowDialog();
                args = new HelpWindowEventArgs();
                if (resultDialog == true)
                {
                    args.Cancel = false;
                    args.Result = true;
                }
                else
                {
                    args.Cancel = true;
                    args.Result = true;
                }
            }
            catch (Exception ex)
            {
                args.Error = ex;
            }
            finally
            {
                result = new HelpWindowResult(args);
            }

            return result;
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

        internal static HelpWindowResult ExecuteInternal(Window owner, string headerText, string htmlPage)
        {
            HelpWindowView dialog = new HelpWindowView();
            dialog.Owner = owner;

            if (headerText != null)
            {
                dialog.txtHeaderText.Text = headerText;
            }
            else
            {
                dialog.txtHeaderText.Text = "Hilfe zu ... anzeigen";
            }

            try
            {
                string htmlContent = dialog.LoadResourceText(htmlPage);
                if (string.IsNullOrEmpty(htmlContent) == false)
                {
                    dialog.HelpFrame.NavigateToString(htmlContent.ToString());
                }
                else
                {
                    StringBuilder pageNotFound = new StringBuilder();
                    pageNotFound.Append("<html>");
                    pageNotFound.Append("<body scroll=\"no\">");
                    pageNotFound.Append("<h2>Es konnte eine Seite nicht gefunden werden.</h2>");
                    pageNotFound.Append($"<h3 style=\"color:red;\">Pr&uuml;fen Sie in den Resourcen nach: {htmlPage}</h3>");
                    pageNotFound.Append("</body>");
                    pageNotFound.Append("</html>");

                    dialog.HelpFrame.NavigateToString(pageNotFound.ToString());
                }
            }
            catch (Exception ex)
            {
                string errmsg;
                if (ex is UriFormatException)
                {
                    errmsg = string.Format("{0}\r\n\r\nUri: {1}", string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace), htmlPage);
                }
                else
                {
                    errmsg = string.Format("{0}\r\n\r\n{1}", ex.Message, ex.StackTrace);
                }

                dialog.HelpFrame.NavigateToString(errmsg);
            }

            return dialog.Execute();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
    }
}
