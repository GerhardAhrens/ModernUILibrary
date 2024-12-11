//-----------------------------------------------------------------------
// <copyright file="ProgressBarDialogResult.cs" company="lifeprojects.de">
//     Class: ProgressBarDialogResult
//     Copyright © lifeprojects.de GmbH 2019
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>08.02.2019</date>
//
// <summary>
//  Class with ProgressBarDialogResult Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;


    public class ProgressBarDialogResult
    {
        public object Result { get; private set; }

        public bool Cancelled { get; private set; }

        public Exception Error { get; private set; }

        public bool OperationFailed
        {
            get { return Error != null; }
        }

        public ProgressBarDialogResult(RunWorkerCompletedEventArgs e)
        {
            if(e.Cancelled)
            {
                this.Cancelled = true;
            }
            else if(e.Error != null)
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
