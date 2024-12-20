namespace ModernUIDemo.MyControls
{
    using System.IO;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    using ModernBaseLibrary.Cryptography;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für NotifiactionBoxControlsUC.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
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

        #region ActionDialog

        public void OnActionDialogButtonClick(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Tag.ToString())
            {
                case "Default":
                    this.ActionDialogTestDefault();
                    break;
                case "WithSettings":
                    this.ActionDialogTestWithSettings();
                    break;
                case "WithSettingsAndStepA":
                    this.ActionDialogTestWithSettingsAndStepByStepA();
                    break;
                case "WithSettingsAndStepB":
                    this.ActionDialogTestWithSettingsAndStepByStepB();
                    break;
            }
        }

        private void ActionDialogTestDefault()
        {
            Action action = () => { Thread.Sleep(4000); };
            Window mainWindow = Application.Current.MainWindow;
            ActionDialogResult result = ActionDialog.Execute(mainWindow, "Loading data...", action);

            if (result.OperationFailed)
            {
                MessageBox.Show("ActionDialog failed.");
            }
            else
            {
                MessageBox.Show("ActionDialog successfully executed.");
            }
        }

        private void ActionDialogTestWithSettings()
        {
            Window mainWindow = Application.Current.MainWindow;
            int millisecondsTimeout = 250;
            ActionDialogResult result = ActionDialog.Execute(mainWindow, "Loading data...", () => {

                for (int i = 1; i <= 20; i++)
                {
                    ActionDialog.Current.ReportWithCancellation("Executing step {0}/20...", i);

                    Thread.Sleep(millisecondsTimeout);
                }

            }, new ActionDialogType(true, true, true));

            if (result.Cancelled)
            {
                MessageBox.Show("ActionDialog cancelled.");
            }
            else if (result.OperationFailed)
            {
                MessageBox.Show("ActionDialog failed.");
            }
            else
            {
                MessageBox.Show("ActionDialog successfully executed.");
            }
        }

        private void ActionDialogTestWithSettingsAndStepByStepA()
        {
            Window mainWindow = Application.Current.MainWindow;
            int millisecondsTimeout = 1000;
            ActionDialogResult result = ActionDialog.Execute(mainWindow, "Loading data...", () => {

                ActionDialog.Current.ReportWithCancellation("Bearbeiten Step 1");
                Thread.Sleep(millisecondsTimeout);

                ActionDialog.Current.ReportWithCancellation("Bearbeiten Step 2");
                Thread.Sleep(millisecondsTimeout);

                ActionDialog.Current.ReportWithCancellation("Bearbeiten Step 3");
                Thread.Sleep(millisecondsTimeout);

                ActionDialog.Current.ReportWithCancellation("Bearbeiten Step ...");
                Thread.Sleep(millisecondsTimeout);

            }, new ActionDialogType(true, true, true));

            if (result.Cancelled)
            {
                MessageBox.Show("ActionDialog cancelled.");
            }
            else if (result.OperationFailed)
            {
                MessageBox.Show("ActionDialog failed.");
            }
            else
            {
                MessageBox.Show("ActionDialog successfully executed.");
            }
        }

        private void ActionDialogTestWithSettingsAndStepByStepB()
        {
            Window mainWindow = Application.Current.MainWindow;
            ActionDialogResult result = ActionDialog.Execute(mainWindow, "Loading data...", () => {

                return this.ActionStepValue();

            }, new ActionDialogType(true, true, false));

            if (result.Cancelled)
            {
                MessageBox.Show("ActionDialog cancelled.");
            }
            else if (result.OperationFailed)
            {
                MessageBox.Show("ActionDialog failed.");
            }
            else
            {
                MessageBox.Show("ActionDialog successfully executed.");
            }
        }

        private string ActionStepValue()
        {
            int millisecondsTimeout = 2000;

            ActionDialog.Current.ReportWithCancellation("Lese Daten A\n- Adressen", "Bearbeiten Step 1");
            Thread.Sleep(millisecondsTimeout);

            ActionDialog.Current.ReportWithCancellation("Lese Daten B\n- Rechnungen\n-Rechnungspositionen", "Bearbeiten Step 2");
            Thread.Sleep(millisecondsTimeout);

            ActionDialog.Current.ReportWithCancellation("Lese Daten C", "Bearbeiten Step 3");
            Thread.Sleep(millisecondsTimeout);

            ActionDialog.Current.ReportWithCancellation("Schreibe alle Daten", "Bearbeiten Step ...");
            Thread.Sleep(millisecondsTimeout);

            return "Ok";
        }

        #endregion ActionDialog

        #region Progressbar Action
        public void OnProgressDialogButtonClick(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Tag.ToString())
            {
                case "Default":
                    this.ProgressDialogTestDefault();
                    break;
                case "WithSubLabel":
                    this.ProgressDialogTestWithSubLabel();
                    break;
                case "WithCancelButton":
                    this.ProgressDialogTestWithCancelButton();
                    break;
                case "WithCancelButtonAndProgressDisplay":
                    this.ProgressDialogTestWithCancelButtonAndProgressDisplay();
                    break;
            }
        }

        private void ProgressDialogTestDefault()
        {
            Window mainWindow = Application.Current.MainWindow;
            ProgressBarDialogResult result = ProgressDialog.Execute(mainWindow, "Loading data...", () => {

                Thread.Sleep(4000);

            });

            if (result.OperationFailed)
            {
                MessageBox.Show("ProgressDialog failed.");
            }
            else
            {
                MessageBox.Show("ProgressDialog successfully executed.");
            }
        }

        private void ProgressDialogTestWithSubLabel()
        {
            Window mainWindow = Application.Current.MainWindow;
            ProgressBarDialogResult result = ProgressDialog.Execute(mainWindow, "Loading data", () =>
            {
                return this.ProgressStepValue();
            }, ProgressBarDialogType.WithSubLabel);

            if (result.OperationFailed)
            {
                MessageBox.Show("ProgressDialog failed.");
            }
            else
            {
                MessageBox.Show("ProgressDialog successfully executed.");
            }
        }

        private int ProgressStepValue()
        {
            for (int i = 1; i <= 5; i++)
            {
                ProgressDialog.Current.Report("Executing step {0}/5... \nThat is a very long Text for the ProgressDialog ", i);

                Thread.Sleep(1500);
            }

            return 5;
        }

        private void ProgressDialogTestWithCancelButton()
        {
            int millisecondsTimeout = 1500;
            Window mainWindow = Application.Current.MainWindow;

            ProgressBarDialogResult result = ProgressDialog.Execute(mainWindow, "Loading data", () => {

                for (int i = 1; i <= 50; i++)
                {
                    ProgressDialog.Current.ReportWithCancellationCheck("Executing step {0}/50...", i);

                    Thread.Sleep(millisecondsTimeout);
                }

            }, ProgressBarDialogType.WithSubLabelAndCancel);

            if (result.Cancelled)
            {
                MessageBox.Show("ProgressDialog cancelled.");
            }
            else if (result.OperationFailed)
            {
                MessageBox.Show("ProgressDialog failed.");
            }
            else
            {
                MessageBox.Show("ProgressDialog successfully executed.");
            }
        }

        private void ProgressDialogTestWithCancelButtonAndProgressDisplay()
        {
            // Easy way to pass data to the async method
            int millisecondsTimeout = 250;
            Window mainWindow = Application.Current.MainWindow;

            ProgressBarDialogResult result = ProgressDialog.Execute(mainWindow, "Loading data", () =>
            {
                for (int i = 1; i <= 20; i++)
                {
                    ProgressDialog.Current.ReportWithCancellationCheck(i * 5, "Executing step {0}/20...", i);

                    Thread.Sleep(millisecondsTimeout);
                }

            }, new ProgressBarDialogType(true, true, false));

            if (result.Cancelled)
            {
                MessageBox.Show("ProgressDialog cancelled.");
            }
            else if (result.OperationFailed)
            {
                MessageBox.Show("ProgressDialog failed.");
            }
            else
            {
                MessageBox.Show("ProgressDialog successfully executed.");
            }
        }

        #endregion Progressbar Action
    }
}
