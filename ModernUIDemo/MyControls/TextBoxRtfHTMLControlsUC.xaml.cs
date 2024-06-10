namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für TextBoxRtfHTMLControlsUC.xaml
    /// </summary>
    public partial class TextBoxRtfHTMLControlsUC : UserControl
    {
        TextBoxRtfHTMLControlsVM vmRoot = null;


        public TextBoxRtfHTMLControlsUC()
        {
            this.InitializeComponent();

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            vmRoot = new TextBoxRtfHTMLControlsVM();
            this.DataContext = vmRoot;

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Formatting
            this.HtmlFormattingRichTextBox.TextFormatter = new HtmlFormatter();
            this.HtmlFormattingRichTextBox.Text =
                @"<u>Scissors</u> cuts <i>paper</i>, <i>paper</i> covers <span style='color:orange'>rock</span>, <span style='color:orange'>rock</span> crushes <b style='color:blue'>lizard</b>, " +
                "<b style='color:blue'>lizard</b> poisons <b>Spock</b>, <b>Spock</b> smashes <u>scissors</u>, <u>scissors</u> decapitates <b style='color:blue'>lizard</b>, " +
                "<b style='color:blue'>lizard</b> eats <i>paper</i>, <i>paper</i> disproves <b>Spock</b>, <b>Spock</b> vaporizes <span style='color:orange'>rock</span>, " +
                "and —as it <span style='font-size:24'><i>always</i></span> has— <span style='color:orange'>rock</span> crushes <u>scissors</u>";

            // Tables
            this.HtmlTableRichTextBox.Text = @"<table>" +
                                              "<tr><td><span style='color: blue'>&nbsp;top left</span></td><td>&nbsp;top right</td></tr>" +
                                              "<tr><td><a href='http://www.lifeprojects.de'>&nbsp;middle left</a></td><td>&nbsp;middle right</td></tr>" +
                                              "<tr><td><i>&nbsp;bottom left</i></td><td><span style='font-size:18'>&nbsp;bottom right</span></td></tr>" +
                                              "</table>";
            this.InputHTML.Text = "<span style='color:orange'>Hallo</span>";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.vmRoot.Culture = "en";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.vmRoot.Culture = "de";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.ResultHTML.Text = this.InputHTML.Text;
        }
    }
}
