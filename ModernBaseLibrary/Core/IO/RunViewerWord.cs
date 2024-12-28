//-----------------------------------------------------------------------
// <copyright file="RunViewerWord.cs" company="Lifeprojects.de">
//     Class: RunViewerWord
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.09.2019</date>
//
// <summary>
//      Über die Klasse wird Word mit der gewählte Datei aufgerufen.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;

    public class RunViewerWord : DisposableCoreBase
    {
        private ProcessStartInfo startInfo = null;
        private readonly Window currentWindow = Application.Current.Windows.OfType<Window>().First(f => f.IsActive == true);

        public bool Run(string fileOrDocument)
        {
            bool retVal = false;

            try
            {
                if (File.Exists(fileOrDocument) == true)
                {
                    this.startInfo = new ProcessStartInfo();
                    this.startInfo.FileName = "WORD.EXE";
                    this.startInfo.Arguments = string.Format(@"""{0}""", fileOrDocument);
                    Process.Start(this.startInfo);

                    retVal = true;
                }
                else
                {
                    MessageBox.Show(currentWindow, $"Die Datei {fileOrDocument} wurde nicht gefunden\nWord wird nicht aufgerufen.",
                        "Datei nicht gefunden", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return retVal;
        }
    }
}
