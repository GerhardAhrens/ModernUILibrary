namespace ModernIU.Controls
{
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Xml;

    /// <summary>
    /// Interaktionslogik für XMLViewer.xaml
    /// </summary>
    public partial class XMLViewer : UserControl
    {
        public static readonly DependencyProperty XMLFileProperty =
            DependencyProperty.Register(nameof(XMLFile), typeof(string), typeof(XMLViewer), new PropertyMetadata(LoadXMLChanged));

        public XMLViewer()
        {
            this.InitializeComponent();
        }

        public string XMLFile
        {
            get { return (string)GetValue(XMLFileProperty); }
            set { SetValue(XMLFileProperty, value); }
        }

        private string GetXmlFile()
        {
            return this.Dispatcher.Invoke(() => { return this.XMLFile; });
        }

        private static void LoadXMLChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is XMLViewer)
            {
                XMLViewer xv = (XMLViewer)d;
                if (string.IsNullOrEmpty(xv.GetXmlFile()) == false)
                {
                    try
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        xv.BindXMLDocument();
                        Mouse.OverrideCursor = Cursors.Arrow;
                    }
                    catch (XmlException ex)
                    {
                        throw new NotSupportedException("The XML file is invalid", ex);
                    }
                    finally
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                    }
                }
            }
        }


        private void BindXMLDocument()
        {
            if (string.IsNullOrEmpty(this.GetXmlFile()) == false)
            {
                XmlDocument xmlDoc = new XmlDocument();
                if (xmlDoc != null)
                {
                    xmlDoc.Load(this.GetXmlFile());
                }
                else
                {
                    xmlTree.ItemsSource = null;
                    return;
                }

                XmlDataProvider provider = new XmlDataProvider();
                provider.Document = xmlDoc;
                Binding binding = new Binding();
                binding.Source = provider;
                binding.XPath = "child::node()";
                xmlTree.SetBinding(TreeView.ItemsSourceProperty, binding);
            }
        }
    }
}
