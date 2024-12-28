//-----------------------------------------------------------------------
// <copyright file="OpenFileDialogEx.cs" company="Lifeprojects.de">
//     Class: OpenFileDialogEx
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.03.2020</date>
//
// <summary>
// Die Klasse stellt einen OpenFileDialog mit möglichen Optionen zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Win32;

    /// <summary>
    /// Die Klasse stellt einen OpenFileDialog mit möglichen Optionen zur Verfügung
    /// </summary>
    public class OpenFileDialogEx : DisposableCoreBase
    {
        readonly string myDocuments = string.Empty;

        public OpenFileDialogEx()
        {
            myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public string Title { get; set; }

        public string FileName { get; set; }

        public IEnumerable<string> FileNames { get; set; }

        public string InitialDirectory { get; set; }

        public FileFilter FileFilter { get; set; }

        public bool RestoreDirectory { get; set; } = true;

        public bool Multiselect { get; set; } = false;

        public string OpenDialog()
        {
            string result = string.Empty;
            string initialDirectory = string.IsNullOrEmpty(this.InitialDirectory) == true ? myDocuments : this.InitialDirectory;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = initialDirectory;
            dlg.RestoreDirectory = this.RestoreDirectory;
            dlg.Multiselect = this.Multiselect;
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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

            dlg.Title = string.IsNullOrEmpty(this.Title) == true ? "Öffnen von ..." : this.Title;
            if (dlg.ShowDialog() == true)
            {
                if (this.Multiselect == true)
                {
                    this.FileNames = dlg.FileNames;
                }
                else
                {
                    this.FileName = dlg.FileName;
                    result = dlg.FileName;
                }
            }

            return result;
        }
    }
}
