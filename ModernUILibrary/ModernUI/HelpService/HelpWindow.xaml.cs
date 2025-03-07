namespace ModernIU.Controls
{
    using System;
    using System.Text;
    using System.Windows;

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
            this._uri = "about:blank";
            this.Title = "Just About Nothing";
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
            if (!this._ForceClose)
            {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            }

            base.OnClosing(e);
        }

        private string Uri
        {
            get
            {
                return this._uri;
            }
            set
            {
                if (this._uri == value)
                {
                    return;
                }

                this._uri = value;
                try
                {
                    StringBuilder htmlContent = new StringBuilder();
                    htmlContent.Append("<html>");
                    htmlContent.Append("<head>");
                    htmlContent.Append("<meta content=\"de-de\" http-equiv=\"Content-Language\">");
                    htmlContent.Append("<title>Hilfe Window Demo</title>");
                    htmlContent.Append("    <style>");
                    htmlContent.Append("        body\r\n{\r\n\tpadding: 10;\r\n\tbackground: #E8FFE8;\r\n\tfont-family: \"Gill Sans\", \"Gill Sans MT\", Calibri, \"Trebuchet MS\", sans-serif;\r\n\tcolor: #008000;\r\n}\r\n.content {\r\n\tborder-style: solid;\r\n\tborder-radius: 10px;\r\n\tmargin-left: auto;\r\n\tmargin-right: auto;\r\n\twidth: auto;\r\n\tmax-width: 600px;\r\n\tpadding: 10;\r\n}\r\n.code {\r\n\t\t\tfont-family: \"Courier New\", Courier, monospace;\r\n\t\t}\r\np {\r\n\ttext-align: left;\r\n\ttext-indent: 2em;\r\n\tmargin: 0px 0px 0.5em 0px;\r\n}");
                    htmlContent.Append("    </style>");
                    htmlContent.Append("</head>");
                    htmlContent.Append("<body scroll=\"no\">");
                    htmlContent.Append("<div class=\"content\">");
                    htmlContent.Append("Das ist ein <b><em>deutlicher</b></em> Hinweis <span class=\"code\">Window</span> ");
                    htmlContent.Append($"<h3 style=\"color:blue;\">Datum/Zeit: {DateTime.Now}</h3>");
                    htmlContent.Append("</div>");
                    htmlContent.Append("</body>");
                    htmlContent.Append("</html>");

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

                    //this.HelpFrame.Content = errmsg;
                    this.Title = string.Format("Error: {0}", ex.GetType());
                }
            }
        }
    }
}
