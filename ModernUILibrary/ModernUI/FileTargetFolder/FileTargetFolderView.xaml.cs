//-----------------------------------------------------------------------
// <copyright file="FileTargetFolderView.xaml.cs" company="Lifeprojects.de">
//     Class: FileTargetFolderView
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.10.2021</date>
//
// <summary>
// Klasse (View DialogWindow) Auswahl möglicher Zielverzeichnise beim speichern von Dateien
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;

    using ModernBaseLibrary.Core.IO;

    using ModernUI.MVVM.Base;


    /// <summary>
    /// Interaktionslogik für FileTargetFolderView.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class FileTargetFolderView : Window, INotifyPropertyChanged
    {
        private string selectFolderValue;
        private ICollectionView dialogDataView = null;

        public FileTargetFolderView()
        {
            this.InitializeComponent();
            WeakEventManager<Window, CancelEventArgs>.AddHandler(this, "Closing", this.OnClosing);
            WeakEventManager<Window, MouseButtonEventArgs>.AddHandler(this, "MouseDown", this.OnMouseDown);

            this.CmdAgg.AddOrSetCommand("CancelButtonCommand", p1 => this.OnCancelButtonClick(), p2 => true);
            this.CmdAgg.AddOrSetCommand("CancelButtonItemCommand", p1 => this.OnCancelButtonItemClick(p1), p2 => true);
            this.CmdAgg.AddOrSetCommand("SelectFolderButtonCommand", p1 => this.OnSelectFolderButtonClick(), p2 => true);
            this.CmdAgg.AddOrSetCommand("UsedFolderCommand", p1 => this.OnUsedFolderHandle(p1), p2 => true);

            this.DataContext = this;
        }

        public ICollectionView DialogDataView
        {
            get { return this.dialogDataView; }
            set
            {
                if (this.dialogDataView != value)
                {
                    this.dialogDataView = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public ICommandAggregator CmdAgg { get; } = new CommandAggregator();

        public List<string> Folders { get; private set; }

        public string SelectFolderValue
        {
            get { return this.selectFolderValue; }
            set
            {
                if (this.selectFolderValue != value)
                {
                    this.selectFolderValue = value;
                    this.OnPropertyChanged();
                }
            }
        }

        private FileTargetFolderSettings Settings { get; set; }

        private FileTargetFolderModus FolderAction { get; set; }

        #region Public Execute (Dialog anzeigen)

        public static FileTargetFolderResult Execute(FileTargetFolderSettings settings)
        {
            FileTargetFolderView dialog = new FileTargetFolderView();
            dialog.Owner = settings.Owner;
            dialog.WindowState = WindowState.Normal;
            dialog.WindowStyle = WindowStyle.None;
            dialog.ResizeMode = ResizeMode.NoResize;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.txtHeaderText.Text = string.IsNullOrEmpty(settings.HeaderText) == true ? "Speichern unter ..." : settings.HeaderText;
            if (string.IsNullOrEmpty(settings.InstructionText) == true)
            {
                dialog.txtInstructionText.Visibility = Visibility.Hidden;
            }
            else
            {
                dialog.txtInstructionText.Text = settings.InstructionText;
            }

            if (string.IsNullOrEmpty(settings.DescriptionText) == true)
            {
                dialog.tbDescriptionText.Visibility = Visibility.Hidden;
            }
            else
            {
                dialog.tbDescriptionText.Text = settings.DescriptionText;
            }

            return dialog.InternalExecute(settings);
        }
        #endregion Public Execute (Dialog anzeigen)

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
        #endregion INotifyPropertyChanged

        #region Execute Internal
        private FileTargetFolderResult InternalExecute(FileTargetFolderSettings settings)
        {
            FileTargetFolderResult result = null;
            SelectFolderEventArgs args = null;
            bool? resultDialog = false;

            try
            {
                this.Settings = settings;

                this.folderList.Focus();
                if (settings.FolderTyp == null || string.IsNullOrEmpty(settings.Folder) == true)
                {
                    string defaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    this.SelectFolderValue = Path.GetDirectoryName(defaultFolder);
                    this.Folders.Add(defaultFolder);
                }
                else
                {
                    this.SelectFolderValue = LastSavedFolder.GetOrSet(settings.FolderTyp, settings.Folder);
                    LastSavedFolder.Save();
                    LastSavedFolder.Load();
                    this.Folders = LastSavedFolder.GetFolders();
                }

                if (this.Folders != null)
                {
                    this.DialogDataView = CollectionViewSource.GetDefaultView(this.Folders);
                    this.DialogDataView.MoveCurrentToFirst();
                }

                resultDialog = this.ShowDialog();
                args = new SelectFolderEventArgs();

                if (this.DialogResult.HasValue == true)
                {
                    args.Cancel = this.DialogResult.Value == false;
                    if (args.Cancel == false)
                    {
                        if (this.SelectFolderValue != null)
                        {
                            args.Result = this.SelectFolderValue.ToString();
                            args.FolderAction = this.FolderAction;
                        }
                        else
                        {
                            args.FolderAction = this.FolderAction;
                        }

                        result = new FileTargetFolderResult(args);
                    }
                    else
                    {
                        args.Result = null;
                    }
                }
            }
            catch (Exception ex)
            {
                args.Error = ex;
            }
            finally
            {
                result = new FileTargetFolderResult(args);
            }

            return result;
        }
        #endregion Execute

        #region Command Methode Handler
        private void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void OnCancelButtonClick()
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OnCancelButtonItemClick(object cmdParameter)
        {
            this.Folders.Remove(cmdParameter.ToString());
            if (this.Folders != null)
            {
                this.DialogDataView = CollectionViewSource.GetDefaultView(this.Folders);
                this.DialogDataView.Refresh();
                this.DialogDataView.MoveCurrentToFirst();

                LastSavedFolder.Remove(cmdParameter.ToString());
            }
        }

        private void OnSelectFolderButtonClick()
        {
            this.FolderAction = FileTargetFolderModus.SelectFolder;
            this.DialogResult = true;
            this.Close();

            if (string.IsNullOrEmpty(this.Settings.InitialFile) == false)
            {
                string initFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                FileFilter fileFilter = new FileFilter();
                foreach (string item in Settings.FileFilter)
                {
                    fileFilter.AddFilter(item.Split("|")[0], item.Split("|")[1], false);
                }

                if (this.Settings.FileFilter != null && this.Settings.FileFilter.Length > 0)
                {
                    fileFilter.SetDefaultFilter(this.Settings.FileFilter.FirstOrDefault().Split("|")[0]);
                }

                if (this.Settings.FolderAction == FileTargetFolderModus.OpenFile)
                {
                    this.SelectFolderValue = this.OpenFile(this.Settings.InitialFile, this.Settings.HeaderText, fileFilter, initFolder);
                }
                else
                {
                    this.SelectFolderValue = this.SaveFile(this.Settings.InitialFile, this.Settings.HeaderText, fileFilter, initFolder);
                }

                LastSavedFolder.GetOrSet(this.Settings.FolderTyp, Path.GetDirectoryName(this.SelectFolderValue));
                LastSavedFolder.Save();
                LastSavedFolder.Load();
                this.Folders = LastSavedFolder.GetFolders();

                if (this.Folders != null)
                {
                    this.DialogDataView = CollectionViewSource.GetDefaultView(this.Folders);
                    this.DialogDataView.MoveCurrentToFirst();
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.Settings.Folder) == true)
                {
                    string initFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    this.SelectFolderValue = this.BrowseFolder(this.Settings.HeaderText, initFolder);
                }
                else
                {
                    this.SelectFolderValue = this.BrowseFolder(this.Settings.HeaderText, this.Settings.Folder);
                }
            }
        }

        private void OnUsedFolderHandle(object p1)
        {
            this.FolderAction = FileTargetFolderModus.UsedFolder;
            this.DialogResult = true;
            this.Close();

            if (string.IsNullOrEmpty(this.Settings.InitialFile) == false)
            {
                FileFilter fileFilter = new FileFilter();
                foreach (string item in Settings.FileFilter)
                {
                    fileFilter.AddFilter(item.Split("|")[0], item.Split("|")[1], false);
                }

                if (this.Settings.FileFilter != null && this.Settings.FileFilter.Length > 0)
                {
                    fileFilter.SetDefaultFilter(this.Settings.FileFilter.FirstOrDefault().Split("|")[0]);
                }

                if (this.Settings.FolderAction == FileTargetFolderModus.OpenFile)
                {
                    this.SelectFolderValue = this.OpenFile(this.Settings.InitialFile, this.Settings.HeaderText, fileFilter, this.SelectFolderValue);
                }
                else
                {
                    this.SelectFolderValue = this.SaveFile(this.Settings.InitialFile, this.Settings.HeaderText, fileFilter, this.SelectFolderValue);
                }
            }
        }

        #endregion Command Methode Handler

        private string SaveFile(string saveName, string description, FileFilter fileFilter, string initialDirectory = "")
        {
            string currentFileName = string.Empty;
            initialDirectory = string.IsNullOrEmpty(initialDirectory) == true ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : initialDirectory;

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.CreatePrompt = false;
            dlg.OverwritePrompt = true;
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = initialDirectory;
            dlg.FileName = saveName;
            dlg.DefaultExt = System.IO.Path.GetExtension(saveName);
            dlg.AddExtension = true;
            dlg.Filter = fileFilter.GetFileFilter();
            dlg.FilterIndex = fileFilter.DefaultFilterIndex;
            dlg.Title = string.IsNullOrEmpty(description) == true ? "Save as ..." : description;
            if (dlg.ShowDialog() == true)
            {
                currentFileName = dlg.FileName;
            }

            return currentFileName;
        }

        private string OpenFile(string saveName, string description, FileFilter fileFilter, string initialDirectory = "")
        {
            string currentFileName = string.Empty;
            initialDirectory = string.IsNullOrEmpty(initialDirectory) == true ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : initialDirectory;

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.RestoreDirectory = true;
            dlg.InitialDirectory = initialDirectory;
            dlg.FileName = saveName;
            dlg.DefaultExt = System.IO.Path.GetExtension(saveName);
            dlg.AddExtension = true;
            dlg.Filter = fileFilter.GetFileFilter();
            dlg.FilterIndex = fileFilter.DefaultFilterIndex;
            dlg.Title = string.IsNullOrEmpty(description) == true ? "Open as ..." : description;
            if (dlg.ShowDialog() == true)
            {
                currentFileName = dlg.FileName;
            }

            return currentFileName;
        }

        private string BrowseFolder(string description, string initialDirectory = "")
        {
            string currentFileName = string.Empty;
            initialDirectory = string.IsNullOrEmpty(initialDirectory) == true ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : initialDirectory;

            using (FolderBrowserDialogEx dlgFolder = new FolderBrowserDialogEx())
            {
                dlgFolder.Title = description;
                dlgFolder.ShowNewFolderButton = false;
                dlgFolder.RootFolder = Environment.SpecialFolder.MyComputer;
                dlgFolder.OpenDialog();
                if (string.IsNullOrEmpty(dlgFolder.SelectedPath) == false)
                {
                    currentFileName = dlgFolder.SelectedPath;
                }
            }

            return currentFileName;
        }

        public static void GetFolderType(string folder)
        {
        }

        public static void Save()
        {
        }
    }
}
