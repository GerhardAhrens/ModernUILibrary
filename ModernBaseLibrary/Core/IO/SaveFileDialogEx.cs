//-----------------------------------------------------------------------
// <copyright file="SaveFileDialogEx.cs" company="Lifeprojects.de">
//     Class: SaveFileDialogEx
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.03.2020</date>
//
// <summary>
// Die Klasse stellt einen SaveFileDialog mit möglichen Optionen zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.IO;

    /// <summary>
    /// Die Klasse stellt einen SaveFileDialog mit möglichen Optionen zur Verfügung
    /// </summary>
    public class SaveFileDialogEx : DisposableCoreBase
    {
        readonly string myDocuments = string.Empty;

        public SaveFileDialogEx()
        {
            myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public string Title { get; set; }

        public string FileName { get; set; }

        public string InitialDirectory { get; set; }

        public string DefaultExtension { get; set; }

        public FileFilter FileFilter { get; set; }

        public bool RestoreDirectory { get; set; } = true;

        public bool CreatePrompt { get; set; } = false;

        public bool OverwritePrompt { get; set; } = true;

        public string OpenDialog()
        {
            string result = string.Empty;
            string initialDirectory = string.IsNullOrEmpty(this.InitialDirectory) == true ? myDocuments : this.InitialDirectory;

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.CreatePrompt = this.CreatePrompt;
            dlg.OverwritePrompt = this.OverwritePrompt;
            dlg.RestoreDirectory = this.RestoreDirectory;
            dlg.InitialDirectory = initialDirectory;
            dlg.FileName = this.FileName;
            if (string.IsNullOrEmpty(this.FileName) == false)
            {
                dlg.DefaultExt = System.IO.Path.GetExtension(this.FileName);
            }
            else
            {
                dlg.DefaultExt = this.DefaultExtension;
            }

            if (this.FileFilter != null)
            {
                dlg.Filter = this.FileFilter.GetFileFilter();
                dlg.FilterIndex = this.FileFilter.DefaultFilterIndex;
            }
            else
            {
                using (FileFilter fileFilter = new FileFilter())
                {
                    fileFilter.AddFilter("Alle Dateien", "*", true);
                    dlg.Filter = fileFilter.GetFileFilter();
                    dlg.FilterIndex = fileFilter.DefaultFilterIndex;
                }
            }

            dlg.Title = string.IsNullOrEmpty(this.Title) == true ? "Speichern von ..." : this.Title;
            if (dlg.ShowDialog() == true)
            {
                result = dlg.FileName;
            }

            return result;
        }
    }
}
