namespace ModernUIDemo.MyControls
{
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using ModernIU.Base;

    using ModernUIDemo.Windows;

    using ModernUILibrary.ModernUIBase;

    /// <summary>
    /// Interaktionslogik für WindowControlsUC.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class WindowControlsUC : UserControl
    {
        public WindowControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void BtnMWindows_Click(object sender, RoutedEventArgs e)
        {
            MWindowTest window = new MWindowTest();
            window.Title = "Custom Window";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }

        private void BtnMWindowsBase_Click(object sender, RoutedEventArgs e)
        {
            BaseWindowTest window = new BaseWindowTest();
            DialogHelper.ShowDialog(window, this);

        }

        private void BtnAutoWindowsBase_Click(object sender, RoutedEventArgs e)
        {
            ACWindow window = new ACWindow();
            window.CloseButtonType = CloseBoxTypeEnum.Close;
            window.AutoClose = true;
            window.AutoCloseInterval = 5;
            DialogHelper.ShowDialog(window, this);
        }
    }
}
