//-----------------------------------------------------------------------
// <copyright file="SelectFolderEventArgs.cs" company="lifeprojects.de">
//     Class: SelectFolderEventArgs
//     Copyright © lifeprojects.de GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>06.10.2021</date>
//
// <summary>
//  Class with SelectFolderEventArgs Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;

    public class SelectFolderEventArgs : CancelEventArgs
    {
        public FileTargetFolderModus FolderAction { get; set; }

        public object Result { get; set; }

        public Exception Error { get; set; }
    }
}
