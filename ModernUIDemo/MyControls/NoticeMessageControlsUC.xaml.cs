namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;

    using ModernIU.Controls;

    using ModernUIDemo.Model;

    /// <summary>
    /// Interaktionslogik für NoticeMessageControlsUC.xaml
    /// </summary>
    public partial class NoticeMessageControlsUC : UserControl
    {
        Notifiaction notifiaction = new Notifiaction();
        public NoticeMessageControlsUC()
        {
            this.InitializeComponent();

            this.NoticeList = new ObservableCollection<NoticeInfo>();
            this.NoticeList.Add(new NoticeInfo()
            {
                Title = "Info",
                Content = "Information",
                Type = "Info",
            });
        }

        public ObservableCollection<NoticeInfo> NoticeList { get; set; }

        private void BtnInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
