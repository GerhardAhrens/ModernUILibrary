namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interop;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Extension;

    /// <summary>
    /// Interaktionslogik für ColorControlsUC.xaml
    /// </summary>
    public partial class ColorControlsUC : UserControl
    {
        public ColorControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnCallHotKey, "Click", this.OnBtnCallHotKeyClick);
        }

        private HotKeyHost HotKeys { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.HotKeys = new HotKeyHost((HwndSource)HwndSource.FromVisual(App.Current.MainWindow));
            this.HotKeys.AddHotKey(new HotKeyToMessageBox("ShowMessageBox", Key.C, ModifierKeys.Alt, "Show MessageBox"));

        }

        private void OnBtnCallHotKeyClick(object sender, RoutedEventArgs e)
        {
            List<string> hotKeys = this.HotKeys.HotkeysToList(" - ");
            string hotkeyList = hotKeys.ToStringAll<string>();

            MessageBox.Show(hotkeyList, "Liste Hotkeys");
        }
    }
}
