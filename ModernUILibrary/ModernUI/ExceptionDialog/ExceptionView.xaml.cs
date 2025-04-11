//-----------------------------------------------------------------------
// <copyright file="ExceptionView.xaml.cs" company="Lifeprojects.de">
//     Class: ExceptionView
//     Copyright © company="Lifeprojects.de" 2019
// </copyright>
//
// <author>Gerhard Ahrens - company="Lifeprojects.de"</author>
// <email>developer@lifeprojects.de</email>
// <date>15.03.2019</date>
//
// <summary>
// Exception Viewer
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using ModernBaseLibrary.Core;

    using ModernIU.Base;

    /// <summary>
    /// A WPF window for viewing Exceptions and inner Exceptions, including all their properties.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class ExceptionView : Window
    {
        private static string _defaultTitle;
        private static string _product;

        private readonly double _small;
        private readonly double _med;
        private readonly double _large;
        private double _chromeWidth;
        private string internalMessage = string.Empty;
        private Exception internalException = null;

        public ExceptionView(string headerMessage, Exception e) : this(headerMessage, e, null)
        {
            this.Cursor = System.Windows.Input.Cursors.Arrow;
        }

        public ExceptionView(string headerMessage, Exception e, Window owner)
        {
            this.InitializeComponent();
            this.Cursor = System.Windows.Input.Cursors.Arrow;

            if (owner != null)
            {
                this.Style = owner.Style;
                this.Owner = owner;
                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                DefaultPaneBrush = Brushes.WhiteSmoke;
                this.Topmost = true;
                this.Owner = Application.Current.MainWindow;
            }

            this.ShowInTaskbar = false;

            if (DefaultPaneBrush != null)
            {
                TreeView.Background = DefaultPaneBrush;
            }

            this.docViewer.Background = TreeView.Background;

            this._small = TreeView.FontSize;
            this._med = this._small * 1.1;
            this._large = this._small * 1.2;

            this.Title = DefaultTitle;

            if (string.IsNullOrEmpty(headerMessage) == true)
            {
                headerMessage = "Sorry, das in der Anwendung ein Fehler aufgetreten ist!";
            }

            this.BuildTree(e, headerMessage);
        }

        public static string DefaultTitle
        {
            get
            {
                if (_defaultTitle == null)
                {
                    if (string.IsNullOrEmpty(Product))
                    {
                        _defaultTitle = "Error";
                    }
                    else
                    {
                        _defaultTitle = "Error - {Product}";
                    }
                }

                return _defaultTitle;
            }

            set
            {
                _defaultTitle = value;
            }
        }

        public static Brush DefaultPaneBrush
        {
            get;
            set;
        }

        public static Action<ErrorLogArgs> CallBackMethodes { get; private set; }

        public static string Product
        {
            get
            {
                if (_product == null)
                {
                    _product = GetProductName();
                }

                return _product;
            }
        }

        public static bool Show(Exception e, string headerMessage, Action<ErrorLogArgs> method)
        {
            CallBackMethodes = method;

            return Create(headerMessage, e, null).Display();
        }

        public static bool Show(Exception e, string headerMessage)
        {
            return Create(headerMessage, e, null).Display();
        }

        public static bool Show(Exception e)
        {
            return Create(string.Empty, e, null).Display();
        }

        private static string RenderEnumerable(IEnumerable data)
        {
            StringBuilder result = new StringBuilder();

            foreach (object obj in data)
            {
                result.AppendFormat("{0}\n", obj);
            }

            if (result.Length > 0)
            {
                result.Length = result.Length - 1;
            }

            return result.ToString();
        }

        private static string RenderDictionary(IDictionary data)
        {
            StringBuilder result = new StringBuilder();

            foreach (object key in data.Keys)
            {
                if (key != null && data[key] != null)
                {
                    result.AppendLine(key.ToString() + " = " + data[key].ToString());
                }
            }

            if (result.Length > 0)
            {
                result.Length = result.Length - 1;
            }

            return result.ToString();
        }

        private static string GetProductName()
        {
            string result = string.Empty;

            try
            {
                Assembly _appAssembly = GetAppAssembly();

                object[] customAttributes = _appAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    result = ((AssemblyProductAttribute)customAttributes[0]).Product;
                }
            }
            catch
            {
            }

            return result;
        }

        private static Assembly GetAppAssembly()
        {
            Assembly _appAssembly = null;

            try
            {
                _appAssembly = Application.Current.MainWindow.GetType().Assembly;
            }
            catch
            {
            }

            if (_appAssembly == null)
            {
                _appAssembly = Assembly.GetEntryAssembly();
            }

            if (_appAssembly == null)
            {
                _appAssembly = Assembly.GetExecutingAssembly();
            }

            return _appAssembly;
        }

        private static ExceptionView Create(string headerMessage, Exception e, Window owner)
        {
            if (owner == null)
            {
                owner = Application.Current.Windows.Cast<Window>().FirstOrDefault(f => f.IsActive == true);
            }

            Mouse.OverrideCursor = null;

            return new ExceptionView(headerMessage, e, owner);
        }

        private bool Display()
        {
            this.ShowDialog();
            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            treeCol.Width = new GridLength(treeCol.ActualWidth, GridUnitType.Pixel);
            this._chromeWidth = this.ActualWidth - mainGrid.ActualWidth;
            this.CalcMaxTreeWidth();
        }

        private void BuildTree(Exception e, string summaryMessage)
        {
            this.internalMessage = e.Message;
            this.internalException = e;

            List<Inline> inlines = new List<Inline>();
            var firstItem = new TreeViewItem();
            firstItem.Header = "Alle Meldungen";
            TreeView.Items.Add(firstItem);

            Bold inline = new Bold(new Run(summaryMessage));
            inline.FontSize = this._large;
            inlines.Add(inline);

            while (e != null)
            {
                inlines.Add(new LineBreak());
                inlines.Add(new LineBreak());
                this.AddLines(inlines, e.Message);

                this.AddException(e);
                e = e.InnerException;
            }

            firstItem.Tag = inlines;
            firstItem.IsSelected = true;
        }

        private void AddProperty(List<Inline> inlines, string propName, object propVal)
        {
            inlines.Add(new LineBreak());
            inlines.Add(new LineBreak());
            var inline = new Bold(new Run(propName + ":"));
            inline.FontSize = this._med;
            inlines.Add(inline);
            inlines.Add(new LineBreak());

            if (propVal is string)
            {
                this.AddLines(inlines, propVal as string);
            }
            else
            {
                if (propVal != null)
                {
                    inlines.Add(new Run(propVal.ToString()));
                }
            }
        }

        private void AddLines(List<Inline> inlines, string str)
        {
            string[] lines = str.Split('\n');

            inlines.Add(new Run(lines[0].Trim('\r')));

            foreach (string line in lines.Skip(1))
            {
                inlines.Add(new LineBreak());
                inlines.Add(new Run(line.Trim('\r')));
            }
        }

        private void AddException(Exception e)
        {
            var exceptionItem = new TreeViewItem();
            var inlines = new List<Inline>();
            System.Reflection.PropertyInfo[] properties = e.GetType().GetProperties();

            exceptionItem.Header = e.GetType();
            exceptionItem.Tag = inlines;
            TreeView.Items.Add(exceptionItem);

            Inline inline = new Bold(new Run(e.GetType().ToString()));
            inline.FontSize = this._large;
            inlines.Add(inline);

            this.AddProperty(inlines, "Message", e.Message);
            this.AddProperty(inlines, "Stack Trace", e.StackTrace);

            foreach (PropertyInfo info in properties)
            {
                if (info.Name != "InnerException")
                {
                    var value = info.GetValue(e, null);

                    if (value != null)
                    {
                        if (value is string)
                        {
                            if (string.IsNullOrEmpty(value as string))
                            {
                                continue;
                            }
                        }
                        else if (value is IDictionary)
                        {
                            value = RenderDictionary(value as IDictionary);
                            if (string.IsNullOrEmpty(value as string))
                            {
                                continue;
                            }
                        }
                        else if (value is IEnumerable && !(value is string))
                        {
                            value = RenderEnumerable(value as IEnumerable);
                            if (string.IsNullOrEmpty(value as string))
                            {
                                continue;
                            }
                        }

                        if (info.Name != "Message" && info.Name != "StackTrace")
                        {
                            this.AddProperty(inlines, info.Name, value);
                        }

                        // Create a TreeViewItem for the individual property.
                        var propertyItem = new TreeViewItem();
                        var propertyInlines = new List<Inline>();

                        propertyItem.Header = info.Name;
                        propertyItem.Tag = propertyInlines;
                        exceptionItem.Items.Add(propertyItem);
                        this.AddProperty(propertyInlines, info.Name, value);
                    }
                }
            }
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.ShowCurrentItem();
        }

        private void ShowCurrentItem()
        {
            if (TreeView.SelectedItem != null)
            {
                var inlines = (TreeView.SelectedItem as TreeViewItem).Tag as List<Inline>;
                var doc = new FlowDocument();

                doc.FontSize = this._small;
                doc.FontFamily = TreeView.FontFamily;
                doc.TextAlignment = TextAlignment.Left;
                doc.Background = this.docViewer.Background;

                Paragraph para = new Paragraph();
                DrawingImage drawImage = ResourceReader.Instance.ReadAs<DrawingImage>("big-data-compute-filled-small", "Resources/DrawingImageIcons.xaml");
                if (drawImage != null)
                {
                    Image image = new Image();
                    image.Width = 32;
                    image.Height = 32;
                    image.Source = drawImage;
                    para.Inlines.Add(image);
                }

                para.Inlines.AddRange(inlines);
                doc.Blocks.Add(para);
                this.docViewer.Document = doc;
            }
        }

        private double CalcNoWrapWidth(IEnumerable<Inline> inlines)
        {
            double pageWidth = 0;
            var tb = new TextBlock();
            var size = new Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (Inline inline in inlines)
            {
                tb.Inlines.Clear();
                tb.Inlines.Add(inline);
                tb.Measure(size);

                if (tb.DesiredSize.Width > pageWidth)
                {
                    pageWidth = tb.DesiredSize.Width;
                }
            }

            return pageWidth;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.WriteErrorInTable();

            this.Close();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.WriteErrorInTable(true);

            this.Close();
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            TextRange range = this.CreateExceptionText();
            DataObject data = new DataObject();

            using (Stream stream = new MemoryStream())
            {
                range.Save(stream, DataFormats.Rtf);
                data.SetData(DataFormats.Rtf, Encoding.UTF8.GetString((stream as MemoryStream).ToArray()));
            }

            data.SetData(DataFormats.StringFormat, range.Text);
            Clipboard.SetDataObject(data);

            this.ShowCurrentItem();
        }

        private void ExpressionViewerWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                this.CalcMaxTreeWidth();
            }
        }

        private void CalcMaxTreeWidth()
        {
            mainGrid.MaxWidth = this.ActualWidth - this._chromeWidth;
            treeCol.MaxWidth = mainGrid.MaxWidth - textCol.MinWidth;
        }

        private TextRange CreateExceptionText()
        {
            Assembly assembly = System.Windows.Application.Current.MainWindow.GetType().Assembly;

            var inlines = new List<Inline>();
            var doc = new FlowDocument();
            var para = new Paragraph();

            doc.FontSize = this._small;
            doc.FontFamily = TreeView.FontFamily;
            doc.TextAlignment = TextAlignment.Left;

            inlines.Add(new Run("Application Information"));
            inlines.Add(new LineBreak());
            inlines.Add(new Run("____________________________________________________"));
            inlines.Add(new LineBreak());
            inlines.Add(new Run(string.Format("Application \t: {0}", assembly.GetName().Name)));
            inlines.Add(new LineBreak());
            inlines.Add(new Run(string.Format("Version     \t: {0}", assembly.GetName().Version)));
            inlines.Add(new LineBreak());
            inlines.Add(new Run(string.Format("Date        \t: {0}", DateTime.Now)));
            inlines.Add(new LineBreak());
            inlines.Add(new Run(string.Format("FW Version  \t: {0}", assembly.ImageRuntimeVersion)));
            inlines.Add(new LineBreak());
            inlines.Add(new Run(string.Format("Computername\t: {0}", Environment.MachineName)));
            inlines.Add(new LineBreak());
            inlines.Add(new Run(string.Format("Username    \t: {0}", Environment.UserName)));
            inlines.Add(new LineBreak());
            inlines.Add(new Run(string.Format("OS          \t: {0}", Environment.OSVersion.ToString())));
            inlines.Add(new LineBreak());
            inlines.Add(new Run(string.Format("Culture     \t: {0}", CultureInfo.CurrentCulture.Name)));
            inlines.Add(new LineBreak());
            inlines.Add(new Run(string.Format("App up time \t: {0}", (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString())));
            inlines.Add(new LineBreak());
            inlines.Add(new Run("===================================================="));
            inlines.Add(new LineBreak());
            inlines.Add(new LineBreak());
            inlines.Add(new LineBreak());
            inlines.Add(new Run("Exception Information"));
            foreach (TreeViewItem treeItem in TreeView.Items)
            {
                if (inlines.Any())
                {
                    inlines.Add(new LineBreak());
                    inlines.Add(new Run("____________________________________________________"));
                    inlines.Add(new LineBreak());
                }

                inlines.AddRange(treeItem.Tag as List<Inline>);
            }

            para.Inlines.AddRange(inlines);
            doc.Blocks.Add(para);

            TextRange range = new TextRange(doc.ContentStart, doc.ContentEnd);

            return range;
        }

        private void WriteErrorInTable(bool applicationExit = false)
        {
            TextRange range = this.CreateExceptionText();

            string assmVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();

            try
            {
                if (CallBackMethodes != null)
                {
                    ErrorLogArgs logArgs = new ErrorLogArgs();
                    logArgs.Title = this.TruncateLeft(this.internalMessage,200);
                    logArgs.ErrorText = range.Text;
                    logArgs.ErrorLevel = "Error";
                    logArgs.UserAction = string.Empty;
                    logArgs.Version = assmVersion;
                    logArgs.Exception = this.internalException;
                    logArgs.ApplicationExit = applicationExit;
                    CallBackMethodes(logArgs);
                }
                else
                {
                    ExceptionToFile appLog = new ExceptionToFile();
                    appLog.ErrorText = range.Text;
                    appLog.Save();
                }
            }
            catch (Exception)
            {
            }
        }

        private string TruncateLeft(string @this, int maxChars, string addText = "")
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(@this) == true)
            {
                return result;
            }

            if ((maxChars - addText.Length) < 1)
            {
                return result;
            }

            if (@this.Length > (maxChars - addText.Length))
            {
                result = @this.Substring(@this.Length - (maxChars - addText.Length));
            }
            else
            {
                result = @this;
            }

            if (string.IsNullOrEmpty(addText) == false)
            {
                return $"{addText}{result}";
            }


            return result;
        }
    }
}