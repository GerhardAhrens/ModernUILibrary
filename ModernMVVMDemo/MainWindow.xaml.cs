namespace ModernMVVMDemo
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Versioning;
    using System.Windows;
    using ModernIU.MVVM.Base;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class MainWindow : WindowBase, IDialogClosing
    {
        public MainWindow() : base(typeof(MainWindow))
        {
            this.InitializeComponent();
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Window, CancelEventArgs>.AddHandler(this, "Closing", this.OnClosing);
            this.DataContext = this;
        }

        public string DialogDescription
        {
            get => this.GetValue<string>();
            set => this.SetValue(value);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.DialogDescription = "MVVM Demo Programm";
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }

        public void OnViewIsClosing(CancelEventArgs eventArgs)
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