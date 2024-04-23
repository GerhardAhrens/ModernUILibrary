namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    using ModernUIDemo.Core;

    /// <summary>
    /// Interaktionslogik für ListBoxControlsUC.xaml
    /// </summary>
    public partial class ListBoxControlsUC : UserControl
    {
        public ListBoxControlsUC()
        {
            this.InitializeComponent();

            List<CheckComboBoxTest> data = new List<CheckComboBoxTest>();
            data.Add(new CheckComboBoxTest(1, "C#"));
            data.Add(new CheckComboBoxTest(2, "C++"));
            data.Add(new CheckComboBoxTest(3, "VB.Net"));
            data.Add(new CheckComboBoxTest(4, "Javascript"));
            data.Add(new CheckComboBoxTest(5, "Object C"));
            data.Add(new CheckComboBoxTest(6, "Java"));

            this.CheckComboBox.ItemsSource = data;
            this.CheckComboBox.DisplayMemberPath = "Content";

            this.MComboBoxSource.Value = new List<string> { "Affe", "Bär", "Elefant", "Hund", "Zebra" };

            this.DataContext = this;
        }

        public XamlProperty<List<string>> MComboBoxSource { get; set; } = XamlProperty.Set<List<string>>();
        public XamlProperty<string> MComboBoxSourceSelectedItem { get; set; } = XamlProperty.Set<string>();


        internal class CheckComboBoxTest
        {
            public int ID { get; set; }
            public string Content { get; set; }

            public CheckComboBoxTest(int id, string content)
            {
                this.ID = id;
                this.Content = content;
            }
        }

        private void btnGetContent_Click(object sender, RoutedEventArgs e)
        {
            MMessageBox.Show(this.CheckComboBox.Content.ToString(), "", MessageBoxButton.YesNoCancel);
        }
    }
}
