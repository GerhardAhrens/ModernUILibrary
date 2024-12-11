//-----------------------------------------------------------------------
// <copyright file="ActionDialogContext.cs" company="lifeprojects.de">
//     Class: ActionDialogContext
//     Copyright © lifeprojects.de GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>20.07.2021</date>
//
// <summary>
//  Class with ActionDialogContext Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public class ActionDialogContext
    {
        public BackgroundWorker Worker { get; private set; }

        public DoWorkEventArgs Arguments { get; private set; }

        public ActionDialogContext(BackgroundWorker worker, DoWorkEventArgs arguments)
        {
            if (worker == null)
            {
                throw new ArgumentNullException("worker darf nicht null sein");
            }

            if (arguments == null)
            {
                throw new ArgumentNullException("arguments darf nicht null sein");
            }

            this.Worker = worker;
            this.Arguments = arguments;
        }

        public bool CheckCancellationPending()
        {
            if(this.Worker.WorkerSupportsCancellation && this.Worker.CancellationPending)
            {
                this.Arguments.Cancel = true;
            }

            return this.Arguments.Cancel;
        }

        public void ThrowIfCancellationPending()
        {
            if(this.CheckCancellationPending() == true)
            {
                throw new ActionDialogCancellationExcpetion();
            }
        }

        public void Report(string message)
        {
            if(this.Worker.WorkerReportsProgress)
            {
                this.Worker.ReportProgress(0, message);
            }
        }

        public void Report(string format, params object[] arg)
        {
            if(this.Worker.WorkerReportsProgress)
            {
                this.Worker.ReportProgress(0, string.Format(format, arg));
            }
        }

        public void ReportWithCancellation(string message)
        {
            this.ThrowIfCancellationPending();

            if(this.Worker.WorkerReportsProgress)
            {
                this.Worker.ReportProgress(0, message);
            }
        }

        public void ReportWithCancellation(string instructionText, string actionText)
        {
            this.ThrowIfCancellationPending();

            if (this.Worker.WorkerReportsProgress)
            {
                Tuple<string, string> message = new Tuple<string, string>(instructionText, actionText);
                this.Worker.ReportProgress(0, message);
            }
        }

        public void ReportWithCancellation(string format, params object[] arg)
        {
            this.ThrowIfCancellationPending();

            if(this.Worker.WorkerReportsProgress)
            {
                this.Worker.ReportProgress(0, string.Format(format, arg));
            }
        }
    }
}
