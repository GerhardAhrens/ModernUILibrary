//-----------------------------------------------------------------------
// <copyright file="RunFileExplorer.cs" company="Lifeprojects.de">
//     Class: RunFileExplorer
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.09.2019</date>
//
// <summary>
// Über die Klasse wird der Datei-Explorer gestartet und die gewählte Datei/Verzeichis selektiert angezeigt.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public class RunFileExplorer : DisposableCoreBase
    {
        private ProcessStartInfo startInfo = null;

        public void SelectFile(string filename)
        {
            if (File.Exists(filename) == true)
            {
                this.startInfo = new ProcessStartInfo();
                this.startInfo.FileName = "explorer.exe";
                this.startInfo.Arguments = $"/e, /select, \"{filename}\"";
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
        }

        public void SelectFolder(string folderName)
        {
            if (Directory.Exists(folderName) == true)
            {
                this.startInfo = new ProcessStartInfo();
                this.startInfo.FileName = "explorer.exe";
                this.startInfo.Arguments = $"\"{folderName}\"";
                using (Process process = new Process())
                {
                    process.StartInfo = this.startInfo;
                    try
                    {
                        process.Start();
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("Foldername", folderName);
                        throw;
                    }
                }

                this.startInfo = null;
            }
        }
    }
}
