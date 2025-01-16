namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für ChooseBoxControlsUC.xaml
    /// </summary>
    public partial class ChooseBoxControlsUC : UserControl
    {
        public ChooseBoxControlsUC()
        {
            this.InitializeComponent();
            /*UI.Dialoge*/
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.btnSelectFolderOpen, "Click", this.OnSelectFolderOpen);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.btnSelectFolderSave, "Click", this.OnSelectFolderSave);
        }

        private void OnSelectFolderOpen(object sender, RoutedEventArgs e)
        {

        }

        private void OnSelectFolderSave(object sender, RoutedEventArgs e)
        {

        }


    }
}
