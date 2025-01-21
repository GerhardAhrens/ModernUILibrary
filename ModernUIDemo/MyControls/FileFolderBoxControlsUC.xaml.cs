namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für FileFolderBoxControlsUC.xaml
    /// </summary>
    public partial class FileFolderBoxControlsUC : UserControl
    {
        public FileFolderBoxControlsUC()
        {
            this.InitializeComponent();
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
            string[] fileFilter = new string[] { "Dateityp A |*.eff", "Alle Dateien|*.txt" }; 

            FileTargetFolderSettings settings = new FileTargetFolderSettings(typeof(FileFolderBoxControlsUC),"c:\\Temp");
            settings.Owner = Application.Current.MainWindow;
            settings.HeaderText = "Auswahl Datenbank ...";
            settings.InstructionText = "Öffnen einer PERT Datenbank mit einer Aufwandsschätzungen...";
            settings.DescriptionText = "Wählen Sie ein Verzeichnis aus der Liste oder ein neues Verzeichnis über den Button unten.";
            settings.SelectFolderText = "Wählen sie einen anderen Ordner...";
            settings.FileFilter = fileFilter;
            settings.InitialFile = "Test.txt";
            settings.FolderAction = FileTargetFolderModus.OpenFile;

            var result = FileTargetFolderView.Execute(settings);
            if (result != null)
            {
                MessageBox.Show(result.SelectFolder, "Ausgewählt zum öffnen", MessageBoxButton.OK);
            }
        }

        private void OnSelectFolderSave(object sender, RoutedEventArgs e)
        {
            string outPathFile = string.Empty;
            string[] fileFilter = new string[] { "Dateityp A |*.eff", "Alle Dateien|*.txt" };

            FileTargetFolderSettings settings = new FileTargetFolderSettings(typeof(FileFolderBoxControlsUC), "c:\\Temp");
            settings.Owner = Application.Current.MainWindow;
            settings.HeaderText = "Speichern einer Datei ...";
            settings.InstructionText = "Speichern einer Datei in ein Verzeichnis...";
            settings.DescriptionText = "Wählen Sie ein Verzeichnis aus der Liste oder ein neues Verzeichnis über den Button unten.";
            settings.SelectFolderText = "Wählen sie einen anderen Ordner...";
            settings.FileFilter = fileFilter;
            settings.InitialFile = "SaverDefaultTest.txt";
            settings.FolderAction = FileTargetFolderModus.UsedFolder;

            var result = FileTargetFolderView.Execute(settings);
            if (result != null)
            {
                MessageBox.Show(result.SelectFolder, "Ausgewählt zum speichern", MessageBoxButton.OK);
            }
        }


    }
}
