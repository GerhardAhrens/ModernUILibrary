namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

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
            string outPathFile = string.Empty;
            string[] fileFilter = new string[] { "eff", "txt" }; 

            FileTargetFolderSettings settings = new FileTargetFolderSettings();
            settings.Owner = Application.Current.MainWindow;
            settings.HeaderText = "Auswahl Datenbank ...";
            settings.InstructionText = "Öffnen einer PERT Datenbank mit einer Aufwandsschätzungen...";
            settings.DescriptionText = "Wählen Sie ein Verzeichnis aus der Liste oder ein neues Verzeichnis über den Button unten.";
            settings.SelectFolderText = "Wählen sie einen anderen Ordner...";
            settings.FileFilter = fileFilter;
            settings.FolderAction = FileTargetFolderModus.OpenFile;

            var result = FileTargetFolderView.Execute(settings);
            if (result != null)
            {
                MessageBox.Show("Ausgewählt zum öffnen", result.SelectFolder, MessageBoxButton.OK);
            }

        }

        private void OnSelectFolderSave(object sender, RoutedEventArgs e)
        {

        }


    }
}
