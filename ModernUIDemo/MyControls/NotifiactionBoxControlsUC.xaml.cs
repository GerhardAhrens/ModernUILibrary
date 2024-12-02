namespace ModernUIDemo.MyControls
{
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    using ModernIU.Controls;

    using ModernUIDemo.Cryptography;

    /// <summary>
    /// Interaktionslogik für NotifiactionBoxControlsUC.xaml
    /// </summary>
    public partial class NotifiactionBoxControlsUC : UserControl
    {
        public NotifiactionBoxControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxA, "Click", this.ONotifiactionBoxAClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxB, "Click", this.ONotifiactionBoxBClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxC, "Click", this.ONotifiactionBoxCClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxD, "Click", this.ONotifiactionBoxDClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxE, "Click", this.ONotifiactionBoxEClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxF, "Click", this.ONotifiactionBoxFClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxG, "Click", this.ONotifiactionBoxGClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxH, "Click", this.ONotifiactionBoxHClick);
            WeakEventManager<FlatButton, RoutedEventArgs>.AddHandler(this.BtnNotificationBoxI, "Click", this.ONotifiactionBoxIClick);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        #region Default NotifiactionBox
        private void ONotifiactionBoxAClick(object sender, RoutedEventArgs e)
        {
            NotificationResult dlgResult = NotificationBox.Show("Application", "NotifiactionBox.Show() ", "Für : OK\nNotificationResult.No", MessageBoxButton.OK, NotificationIcon.Information, NotificationResult.No);
            if (dlgResult != NotificationResult.None)
            {
                System.Windows.MessageBox.Show(owner: Application.Current.MainWindow, $"Gwählt: {dlgResult}");

            }
        }

        private void ONotifiactionBoxBClick(object sender, RoutedEventArgs e)
        {
            NotificationResult dlgResult = NotificationBox.Show("Application", "NotifiactionBox.Show() ", "Für : Ja/Nein\nNotificationResult.No", MessageBoxButton.YesNo, NotificationIcon.Information, NotificationResult.No);
            if (dlgResult != NotificationResult.None)
            {
                System.Windows.MessageBox.Show(owner: Application.Current.MainWindow, $"Gwählt: {dlgResult}");
            }
        }

        private void ONotifiactionBoxCClick(object sender, RoutedEventArgs e)
        {
            NotificationResult dlgResult = NotificationBox.Show("Application", "NotifiactionBox.Show() ", "Für : Yes/No\nNotificationResult.No", MessageBoxButton.YesNo, NotificationIcon.Information, NotificationResult.No,"EN");
            if (dlgResult != NotificationResult.None)
            {
                System.Windows.MessageBox.Show(owner: Application.Current.MainWindow, $"Gwählt: {dlgResult}");
            }
        }

        private void ONotifiactionBoxDClick(object sender, RoutedEventArgs e)
        {
           NotificationResult dlgResult = NotificationBox.Show("Application", "Instruction", "InstructionText");
            if (dlgResult != NotificationResult.None)
            {
                System.Windows.MessageBox.Show(owner: Application.Current.MainWindow, $"Gwählt: {dlgResult}");
            }
        }

        private void ONotifiactionBoxEClick(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(p => p.IsActive == true);
            NotificationResult dlgResult = NotificationBox.ShowWithOwner(currentWindow, "Application", "NotifiactionBox.Show() ", "Für : OK\nDialogResultsEx.Ok", MessageBoxButton.OK, NotificationIcon.Information, NotificationResult.Ok);
            if (dlgResult != NotificationResult.None)
            {
                System.Windows.MessageBox.Show(owner: Application.Current.MainWindow, $"Gwählt: {dlgResult}");
            }
        }
        #endregion Default NotifiactionBox

        #region Custom NotifiactionBox
        private void ONotifiactionBoxFClick(object sender, RoutedEventArgs e)
        {
            NotificationBoxOption msgOpt = new NotificationBoxOption();
            msgOpt.DialogWidth = 700;
            msgOpt.Caption = "Applikation";
            msgOpt.InstructionHeading = "InstructionHeading mit NotificationBoxOption Class";
            msgOpt.InstructionHeadingFontSize = 18;
            msgOpt.InstructionHeadingFontSize = 14;
            msgOpt.InstructionText = "InstructionText.";
            msgOpt.MessageBoxButton = MessageBoxButton.YesNo;
            msgOpt.InstructionIcon = NotificationIcon.Information;
            msgOpt.NotificationResult = NotificationResult.ButtonRight;
            msgOpt.ButtonRight = "Nein";
            msgOpt.ButtonMiddle = "Ja";
            NotificationResult dlgResult = NotificationBox.ShowCustom(msgOpt);

            if (dlgResult != NotificationResult.None)
            {
                System.Windows.MessageBox.Show(owner: Application.Current.MainWindow, $"Gwählt: {dlgResult}");
            }
        }

        private void ONotifiactionBoxGClick(object sender, RoutedEventArgs e)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string iconPath = $"{assemblyPath}\\Resources\\Picture\\app.ico";

            NotificationBoxOption msgOpt = new NotificationBoxOption();
            msgOpt.DialogWidth = 700;
            msgOpt.Caption = "Applikation";
            msgOpt.InstructionHeading = "InstructionHeading mit NotificationBoxOption Class und Custom Icon";
            msgOpt.InstructionHeadingFontSize = 18;
            msgOpt.InstructionHeadingFontSize = 14;
            msgOpt.InstructionText = "InstructionText.";
            msgOpt.MessageBoxButton = MessageBoxButton.YesNo;
            msgOpt.InstructionIcon = NotificationIcon.None;
            msgOpt.NotificationResult = NotificationResult.ButtonRight;
            msgOpt.ButtonRight = "Nein";
            msgOpt.ButtonMiddle = "Ja";
            msgOpt.Icon = new BitmapImage(new Uri(iconPath));
            NotificationResult dlgResult = NotificationBox.ShowCustom(msgOpt);

            if (dlgResult != NotificationResult.None)
            {
                System.Windows.MessageBox.Show(owner: Application.Current.MainWindow, $"Gwählt: {dlgResult}");
            }
        }
        #endregion Custom NotifiactionBox

        #region NotifiactionBox with ListBox
        private void ONotifiactionBoxHClick(object sender, RoutedEventArgs e)
        {
            List<string> randomTextList = new List<string>();

            using (RandomDataContent rdn = new RandomDataContent())
            {
                for (int i = 0; i < 30; i++)
                {
                    string rdnText = $"{rdn.Letters(10)} : {rdn.AlphabetAndNumeric(10)} - {rdn.Letters(30)}  - {rdn.Letters(30)}";
                    randomTextList.Add(rdnText);
                }
            }

            string headLineText = "NotificationListBox.Show() mit OK, und Liste mit Sting-Daten\nals langer Text und einer Instruktion wie mit dem Listeninhalt umgegangen werden soll.";

            NotificationResult dlgResult = NotificationListBox.Show("Application", headLineText, randomTextList, MessageBoxButton.OK, NotificationIcon.Information, NotificationResult.No);
            if (dlgResult != NotificationResult.None)
            {
                System.Windows.MessageBox.Show(owner: Application.Current.MainWindow, $"Gwählt: {dlgResult}");

            }
        }

        private void ONotifiactionBoxIClick(object sender, RoutedEventArgs e)
        {
            List<string> randomTextList = new List<string>();

            using (RandomDataContent rdn = new RandomDataContent())
            {
                for (int i = 0; i < 30; i++)
                {
                    string rdnText = $"{rdn.Letters(10)} : {rdn.AlphabetAndNumeric(10)} - {rdn.Letters(30)}  - {rdn.Letters(30)}";
                    randomTextList.Add(rdnText);
                }
            }

            string headLineText = "NotificationListBox.Show() mit OK, und Liste mit Sting-Daten\nals langer Text und einer Instruktion wie mit dem Listeninhalt umgegangen werden soll.";

            NotificationResult dlgResult = NotificationListBox.Show("Application", headLineText, randomTextList, MessageBoxButton.YesNo, NotificationIcon.Information, NotificationResult.No);
            if (dlgResult != NotificationResult.None)
            {
                System.Windows.MessageBox.Show(owner: Application.Current.MainWindow, $"Gwählt: {dlgResult}");

            }
        }

        #endregion

    }
}
