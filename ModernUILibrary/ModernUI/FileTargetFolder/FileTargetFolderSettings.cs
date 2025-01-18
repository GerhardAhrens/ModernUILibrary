//-----------------------------------------------------------------------
// <copyright file="SelectFolderSettings.cs" company="lifeprojects.de">
//     Class: SelectFolderSettings
//     Copyright © lifeprojects.de GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>07.10.2021</date>
//
// <summary>
//  Class with SelectFolderSettings Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;

    public class FileTargetFolderSettings
    {
        public FileTargetFolderSettings()
        {
        }
        public FileTargetFolderSettings(Type folderTyp)
        {
            this.FolderTyp = folderTyp.Name;
        }

        public FileTargetFolderSettings(Type folderTyp, string folder)
        {
            this.FolderTyp = folderTyp.Name;
            this.Folder = folder;
        }

        public FileTargetFolderSettings(string typ)
        {
            this.FolderTyp = typ;
        }

        public FileTargetFolderSettings(string folderTyp, string folder)
        {
            this.FolderTyp = folderTyp;
            this.Folder = folder;
        }

        public Window Owner { get; set; }

        public string HeaderText { get; set; }

        public string InstructionText { get; set; }

        public string DescriptionText { get; set; }

        public string SelectFolderText { get; set; }

        public string[] FileFilter { get; set; }

        public string FolderTyp { get; set; }

        public string Folder { get; set; }

        public FileTargetFolderModus FolderAction { get; set; }
    }
}
