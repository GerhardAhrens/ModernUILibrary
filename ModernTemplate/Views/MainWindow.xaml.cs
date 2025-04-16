namespace ModernTemplate
{
    using System.ComponentModel;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Extension;

    using ModernIU.Base;
    using ModernIU.Controls;

    using ModernTemplate.Core;
    using ModernTemplate.Views;
    using ModernTemplate.Views.ContentControls;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class MainWindow : WindowBase, IDialogClosing
    {
        private const string DateFormat = "dd.MM.yyyy HH:mm";
        private INotificationService notificationService = new NotificationService();
        private DispatcherTimer statusBarDate = null;

        public MainWindow()
        {
            this.InitializeComponent();
            this.InitCommands();
            this.InitTimer();
            base.CenterWindow(this);
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.DataContext = this;
        }

        public override void InitCommands()
        {
            base.CmdAgg.AddOrSetCommand(CommandButtons.Help, new RelayCommand(p1 => this.HelpHandler(p1), p2 => true));
            base.CmdAgg.AddOrSetCommand(CommandButtons.AppAbout, new RelayCommand(p1 => this.AppAboutHandler(p1), p2 => true));
        }

        #region Properties
        public string DialogDescription
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public UserControl WorkContent
        {
            get { return base.GetValue<UserControl>(); }
            set { base.SetValue(value); }
        }

        private CommandButtons CurrentCommandName { get; set; }
        #endregion Properties

        #region WindowEventHandler
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.DialogDescription = "Modern Template - Projekt";
            this.Focus();
            this.InitTimer();
            Keyboard.Focus(this);

            using (ApplicationSettings settings = new ApplicationSettings())
            {
                settings.Load();
                App.ExitApplicationQuestion = settings.ExitApplicationQuestion;
                App.SaveLastWindowsPosition = settings.SaveLastWindowsPosition;
                App.IsLogging = settings.IsLogging;
                App.RunEnvironment = settings.RunEnvironment;
            }

            /* Letzte Windows Positionn landen*/
            using (UserPreferences userPrefs = new UserPreferences(this))
            {
                userPrefs.Load(App.SaveLastWindowsPosition);
            }

            NotificationService.RegisterDialog<QuestionYesNo>();
            NotificationService.RegisterDialog<MessageOk>();

            base.EventAgg.Subscribe<ChangeViewEventArgs>(this.ChangeControl);

            ChangeViewEventArgs arg = new ChangeViewEventArgs();
            arg.MenuButton = CommandButtons.Home;
            this.ChangeControl(arg);

            StatusbarMain.Statusbar.SetDatabaeInfo();
        }

        public override void OnViewIsClosing(CancelEventArgs e)
        {
            if (App.ExitApplicationQuestion == true)
            {
                NotificationBoxButton result = this.notificationService.ApplicationExit();
                if (result == NotificationBoxButton.Yes)
                {
                    this.ExitApplication(e);
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                this.ExitApplication(e);
            }

        }

        private void ExitApplication(CancelEventArgs e)
        {
            Window window = Application.Current.MainWindow;
            if (window != null)
            {
                e.Cancel = false;
                this.statusBarDate.Stop();

                using (UserPreferences userPrefs = new UserPreferences(this))
                {
                    userPrefs.Save(App.SaveLastWindowsPosition);
                }

                Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                Application.Current.Shutdown();
            }
        }
        #endregion WindowHandler

        #region CommandHandler
        private void HelpHandler(object commandArgs)
        {
            this.notificationService.FeaturesNotFound($"{this.CurrentCommandName}-{commandArgs.ToString()}");
        }

        private void AppAboutHandler(object commandArgs)
        {
            IEnumerable<IAssemblyInfo> metaInfo = null;
            using (AssemblyMetaService ams = new AssemblyMetaService())
            {
                metaInfo = ams.GetMetaInfo();
            }

            List<string> assemblyList = new List<string>();
            foreach (IAssemblyInfo assembly in metaInfo)
            {
                assemblyList.Add($"{assembly.AssemblyName}; {assembly.AssemblyVersion}");
            }

            string headLineText = "Versionen zur Modern Template.";

            NotificationResult dlgResult = NotificationListBox.Show("Application", headLineText, assemblyList, MessageBoxButton.OK, NotificationIcon.Information, NotificationResult.No);
        }
        #endregion CommandHandler

        #region Dialognavigation
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (this.WorkContent != null)
            {
                if (this.WorkContent.GetType() == typeof(MainWindow))
                {
                    this.WorkContent.Width = (this.ActualWidth -20);
                }
            }
        }

        private void ChangeControl(ChangeViewEventArgs e)
        {
            if (e.MenuButton == CommandButtons.CloseApp)
            {
                this.Close();
                return;
            }

            FactoryResult view = DialogFactory.Get(e.MenuButton, e);
            if (view is FactoryResult menuWorkArea)
            {
                using (ObjectRuntime objectRuntime = new ObjectRuntime())
                {
                    StatusbarMain.Statusbar.SetNotification();
                    this.CurrentCommandName = e.MenuButton;
                    string titelUC = e.MenuButton.ToDescription();
                    this.WorkContent = menuWorkArea.WorkContent;
                    this.WorkContent.VerticalAlignment = VerticalAlignment.Stretch;
                    this.WorkContent.HorizontalAlignment = HorizontalAlignment.Stretch;
                    this.WorkContent.Focusable = true;

                    TextBlock textBlock = VisualHelper.FindByName<TextBlock>(this.WorkContent, "TbTitelUC");
                    if (textBlock != null)
                    {
                        textBlock.Text = titelUC;
                    }

                    StatusbarMain.Statusbar.SetNotification($"Bereit: {objectRuntime.ResultMilliseconds()}ms");
                }
            }
            else
            {
                throw new NotSupportedException($"Der Dialog '{e.MenuButton}|{e.MenuButton.ToString()}' kann nicht gefunden werden.");
            }
        }
        #endregion Dialognavigation

        #region Private Methodes
        private void InitTimer()
        {
            this.statusBarDate = new DispatcherTimer();
            this.statusBarDate.Interval = new TimeSpan(0, 0, 1);
            this.statusBarDate.Start();
            this.statusBarDate.Tick += new EventHandler(
                delegate (object s, EventArgs a)
                {
                    this.dtStatusBarDate.Text = DateTime.Now.ToString(DateFormat);
                });
        }
        #endregion Private Methodes
    }
}
