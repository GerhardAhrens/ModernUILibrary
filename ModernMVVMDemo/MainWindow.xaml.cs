namespace ModernMVVMDemo
{
    using System;
    using System.Runtime.Versioning;
    using System.Windows;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class MainWindow : WindowBase
    {
        public MainWindow() : base(typeof(MainWindow))
        {
            this.InitializeComponent();
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        public string DialogDescription
        {
            get => this.GetValue<string>();
            set => this.SetValue(value);
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            this.DialogDescription = "MVVM Demo Programm";
        }
    }
}