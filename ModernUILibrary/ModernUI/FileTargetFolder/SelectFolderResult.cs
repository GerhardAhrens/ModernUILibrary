//-----------------------------------------------------------------------
// <copyright file="SelectFolderResult.cs" company="lifeprojects.de">
//     Class: SelectFolderResult
//     Copyright © lifeprojects.de GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>06.10.2021</date>
//
// <summary>
//  Class with SelectFolderResult Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;

    public class SelectFolderResult
    {
        public SelectFolderResult(SelectFolderEventArgs e)
        {
            if (e.Cancel == true)
            {
                this.Cancelled = true;
            }
            else if (e.Error != null)
            {
                this.Error = e.Error;
            }
            else
            {
                this.SelectFolder = e.Result.ToString();
                this.FolderAction = e.FolderAction;
            }
        }

        public SelectFolderAction FolderAction { get; private set; }

        public string SelectFolder { get; private set; }

        public bool Cancelled { get; private set; }

        public Exception Error { get; private set; }

        public bool OperationFailed
        {
            get { return this.Error != null; }
        }
    }
}
