namespace ModernMVVMDemo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Input;

    using ModernIU.MVVM.Base;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class MainWindow : WindowBase, IDialogClosing
    {
        public ICommand CloseWindow2Command => new RelayCommand(p1 => this.CloseWindowHandler(p1), p2 => true);

        public MainWindow() : base(typeof(MainWindow))
        {
            this.InitializeComponent();
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Window, CancelEventArgs>.AddHandler(this, "Closing", this.OnClosing);
            List<Window> list = new List<Window>();
            this.InitCommands();
            this.DataContext = this;
        }

        public string DialogDescription
        {
            get => this.GetValue<string>();
            set => this.SetValue(value);
        }

        public override void InitCommands()
        {
            base.CmdAgg.AddOrSetCommand("CloseWindowCommand", new RelayCommand(p1 => this.CloseWindowHandler(p1), p2 => true));
        }

        private void CloseWindowHandler(object p1)
        {
            this.Close();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.DialogDescription = "MVVM Demo Programm";
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }

        public override void OnViewIsClosing(CancelEventArgs eventArgs)
        {
            Window window = Application.Current.MainWindow;
            if (window != null)
            {
                Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                Application.Current.Shutdown();
            }
        }
    }
}