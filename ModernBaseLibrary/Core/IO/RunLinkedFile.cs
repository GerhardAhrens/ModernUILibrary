//-----------------------------------------------------------------------
// <copyright file="RunLinkedFile.cs" company="Lifeprojects.de">
//     Class: RunLinkedFile
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>19.03.2021</date>
//
// <summary>
// Über die Klasse wird der Datei mit dem Default-Applikation aufgerufen.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Über die Klasse wird der Datei mit dem Default-Applikation aufgerufen.
    /// </summary>
    public class RunLinkedFile : DisposableCoreBase
    {
        private readonly Window currentWindow = Application.Current.Windows.OfType<Window>().First(f => f.IsActive == true);
        private ProcessStartInfo startInfo = null;

        public void Run(string filename)
        {
            if (File.Exists(filename) == true)
            {
                this.startInfo = new ProcessStartInfo();
                this.startInfo.FileName = filename;
                using (Process process = new Process())
                {
                    process.StartInfo = this.startInfo;
                    try
                    {
                        process.Start();
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("Filename", filename);
                        throw;
                    }
                }

                this.startInfo = null;
            }
            else
            {
                MessageBox.Show(currentWindow, $"Die Datei {filename} wurde nicht gefunden.",
                    "Datei nicht gefunden",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}

