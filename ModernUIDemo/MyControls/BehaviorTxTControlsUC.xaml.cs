namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.WPF.Base;

    using ModernUI.MVVM.Base;

    using ModernUIDemo.Core;

    /// <summary>
    /// Interaktionslogik für BehaviorTxTControlsUC.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class BehaviorTxTControlsUC : UserControl
    {
        public BehaviorTxTControlsUC()
        {
            this.InitializeComponent();

            this.txtIBAN.Text = "DE001111222233334444";

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnDateTime, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnPhone, "Click", this.OnButtonClick);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnIBAN, "Click", this.OnButtonClick);

            this.DataContext = this;
        }

        public DelegateCommand HyperLinkTextCommand => new DelegateCommand(this.HyperLinkTextHandler, this.CanHyperLinkTextHandler);

        public XamlProperty<string> ValueDate { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<string> HyperLinkEMailText { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<string> HyperLinkURLText { get; set; } = XamlProperty.Set<string>();
        public XamlProperty<ObservableCollection<string>> AutoCompleteList { get; set; } = XamlProperty.Set<ObservableCollection<string>>();
        public XamlProperty<string> AutoCompleteListTT { get; set; } = XamlProperty.Set<string>();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ValueDate.Value = DateOnly.FromDateTime(DateTime.Now).ToShortDateString();
            this.HyperLinkEMailText.Value = "developer@lifeprojects.de";
            this.HyperLinkURLText.Value = "www.lifeprojects.de";
            this.AutoCompleteList.Value = new ObservableCollection<string> { "Test", "Banane", "Pferd", "Hund", "Huhn", "Schlange", "Textur", "4711", "0815", "Gerhard", "Lifeprojects" };
            this.AutoCompleteListTT.Value = string.Join(';',this.AutoCompleteList.Value);
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if (btn != null)
            { 
                if (btn.Name == "BtnDateTime")
                {
                    MessageBox.Show(this.txtDateTime.Text, "Auswahl");
                }
                else if (btn.Name == "BtnPhone")
                {
                    MessageBox.Show($"{this.txtPhone.Text}");
                }
                else if (btn.Name == "BtnIBAN")
                {
                    MessageBox.Show($"{this.txtIBAN.Text.Trim(new char[] { ' ', '_' })}");
                }
            }
        }

        private bool CanHyperLinkTextHandler()
        {
            return true;
        }

        private void HyperLinkTextHandler(object p1)
        {
            MessageBox.Show($"Send EMail to '{p1}'.", "Send EMail");
        }
    }
}
