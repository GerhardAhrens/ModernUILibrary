//-----------------------------------------------------------------------
// <copyright file="FolderBrowserDialogEx.cs" company="Lifeprojects.de">
//     Class: FolderBrowserDialogEx
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.03.2020</date>
//
// <summary>
// Die Klasse stellt einen FolderBrowserDialog mit möglichen Optionen zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Forms;

    /// <summary>
    /// Die Klasse stellt einen FolderBrowserDialog mit möglichen Optionen zur Verfügung
    /// </summary>
    public class FolderBrowserDialogEx : DisposableCoreBase
    {
        private readonly string myDocuments = string.Empty;
        private readonly Window currentWindow = null;

        public FolderBrowserDialogEx()
        {
            myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            currentWindow = System.Windows.Application.Current.Windows.OfType<Window>().LastOrDefault(p => p.IsActive == true);
        }

        public string Title { get; set; }

        public string SelectedPath { get; set; }

        public string InitialDirectory { get; set; }

        public Environment.SpecialFolder RootFolder { get; set; } = Environment.SpecialFolder.UserProfile;

        public bool ShowNewFolderButton { get; set; } = false;

        public string OpenDialog()
        {
            string result = string.Empty;

            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.ShowNewFolderButton = this.ShowNewFolderButton;
                dlg.RootFolder = this.RootFolder;
                dlg.Description = this.Title;
                dlg.SelectedPath = this.SelectedPath;
                Wpf32Window win = new Wpf32Window(currentWindow);
                DialogResult resultDlg = dlg.ShowDialog(win);
                if (resultDlg == DialogResult.OK)
                {
                    this.SelectedPath = dlg.SelectedPath;
                    result = dlg.SelectedPath;
                }
            }

            return result;
        }
    }
}
