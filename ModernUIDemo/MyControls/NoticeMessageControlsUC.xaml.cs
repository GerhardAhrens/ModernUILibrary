namespace ModernUIDemo.MyControls
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;
    using ModernIU.Controls;

    using ModernUIDemo.Model;

    /// <summary>
    /// Interaktionslogik für NoticeMessageControlsUC.xaml
    /// </summary>
    public partial class NoticeMessageControlsUC : UserControl
    {
        private ObservableCollection<NoticeInfo> _NoticeList;
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

        public ObservableCollection<NoticeInfo> NoticeList
        {
            get { return _NoticeList; }
            set { _NoticeList = value; }
        }


        private void BtnInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            notifiaction.AddNotifiaction(new NotifiactionModel()
            {
                Title = "Dies ist der Titel der Informationsschrift",
                Content = "Diese Meldung wird nicht automatisch geschlossen"
            });
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.NoticeList.Add(new NoticeInfo()
            {
                Title = "Dies ist die Rubrik 4 der Bekanntmachung",
                Content = "Diese Meldung wird nicht automatisch geschlossen",
                Type = "Error",
            });
        }

        private void btnSuccess_Click(object sender, RoutedEventArgs e)
        {
            notifiaction.AddNotifiaction(new NotifiactionModel()
            {
                Title = "Dies ist der Button der Erfolgsmeldung.",
                Content = "Diese Meldung wird nicht automatisch geschlossen, Sie müssen auf die Schaltfläche Schließen klicken",
                NotifiactionType = EnumPromptType.Success
            });
        }

        private void btnError_Click(object sender, RoutedEventArgs e)
        {
            notifiaction.AddNotifiaction(new NotifiactionModel()
            {
                Title = "Dies ist der Button der Fehlermeldung",
                Content = "Diese Benachrichtigung wird nicht automatisch geschlossen, Sie müssen auf die Schaltfläche \"Schließen\" klicken.",
                NotifiactionType = EnumPromptType.Error
            });
        }

        private void btnWarn_Click(object sender, RoutedEventArgs e)
        {
            notifiaction.AddNotifiaction(new NotifiactionModel()
            {
                Title = "Dies ist eine Warnung, die den Titel",
                Content = "Diese Benachrichtigung wird nicht automatisch geschlossen, Sie müssen auf die Schaltfläche \"Schließen\" klicken.",
                NotifiactionType = EnumPromptType.Warn
            });
        }
    }
}
