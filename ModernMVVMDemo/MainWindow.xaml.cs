namespace ModernMVVMDemo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernIU.Controls;
    using ModernIU.MVVM.Base;
    using ModernMVVMDemo.Message;
    using ModernMVVMDemo.View;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class MainWindow : WindowBase, IDialogClosing
    {
        public ICommand CloseWindow2Command => new RelayCommand(p1 => this.CloseWindowHandler(p1), p2 => true);
        private INotificationService _notificationService = new NotificationService();

        public MainWindow() : base(typeof(MainWindow))
        {
            this.InitializeComponent();
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.InitCommands();
            this.DataContext = this;
        }

        public string DialogDescription
        {
            get => this.GetValue<string>();
            set => this.SetValue(value);
        }

        public UserControl WorkContent
        {
            get { return base.GetValue<UserControl>(); }
            set { base.SetValue(value); }
        }

        public override void InitCommands()
        {
            base.CmdAgg.AddOrSetCommand("CloseWindowCommand", new RelayCommand(p1 => this.CloseWindowHandler(p1), p2 => true));
        }

        private void CloseWindowHandler(object p1)
        {
            NotificationBoxButton result = this._notificationService.ApplicationExit();
            if (result == NotificationBoxButton.Yes)
            {
                this.Close();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.DialogDescription = "MVVM Demo Programm";
            NotificationService.RegisterDialog<QuestionYesNo>();
            NotificationService.RegisterDialog<QuestionHtmlYesNo>();

            this.WorkContent = new ContentA_UC();
        }

        public override void OnViewIsClosing(CancelEventArgs e)
        {
            Window window = Application.Current.MainWindow;
            if (window != null)
            {
                NotificationBoxButton result = this._notificationService.ApplicationExit2();
                if (result == NotificationBoxButton.Yes)
                {
                    e.Cancel = false;
                    Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                    Application.Current.Shutdown();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}