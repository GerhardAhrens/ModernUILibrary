//-----------------------------------------------------------------------
// <copyright file="ActionDialogResult.cs" company="lifeprojects.de">
//     Class: ActionDialogResult
//     Copyright © lifeprojects.de GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>20.07.2021</date>
//
// <summary>
//  Class with ActionDialogResult Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;

    public class ActionDialogResult
    {
        public object Result { get; private set; }

        public bool Cancelled { get; private set; }

        public Exception Error { get; private set; }

        public bool OperationFailed
        {
            get { return Error != null; }
        }

        public ActionDialogResult(RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.Cancelled = true;
            }
            else if (e.Error != null)
            {
                this.Error = e.Error;
            }
            else
            {
                this.Result = e.Result;
            }
        }
    }
}
