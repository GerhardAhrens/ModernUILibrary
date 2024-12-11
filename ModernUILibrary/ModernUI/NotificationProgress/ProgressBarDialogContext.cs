//-----------------------------------------------------------------------
// <copyright file="ProgressBarDialogContext.cs" company="lifeprojects.de">
//     Class: ProgressBarDialogContext
//     Copyright © lifeprojects.de GmbH 2019
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>08.02.2019</date>
//
// <summary>
//  Class with ProgressBarDialogContext Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;

    public class ProgressBarDialogContext
    {
        public BackgroundWorker Worker { get; private set; }

        public DoWorkEventArgs Arguments { get; private set; }

        public ProgressBarDialogContext(BackgroundWorker worker, DoWorkEventArgs arguments)
        {
            if(worker == null)
            {
                throw new ArgumentNullException("worker");
            }

            if (arguments == null)
            {
                throw new ArgumentNullException("arguments");
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
                throw new ProgressBarDialogCancellationExcpetion();
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

        public void Report(int percentProgress, string message)
        {
            if(this.Worker.WorkerReportsProgress)
            {
                this.Worker.ReportProgress(percentProgress, message);
            }
        }

        public void Report(int percentProgress, string format, params object[] arg)
        {
            if(this.Worker.WorkerReportsProgress)
            {
                this.Worker.ReportProgress(percentProgress, string.Format(format, arg));
            }
        }

        public void ReportWithCancellationCheck(string message)
        {
            this.ThrowIfCancellationPending();

            if(this.Worker.WorkerReportsProgress)
            {
                this.Worker.ReportProgress(0, message);
            }
        }

        public void ReportWithCancellationCheck(string format, params object[] arg)
        {
            this.ThrowIfCancellationPending();

            if(this.Worker.WorkerReportsProgress)
            {
                this.Worker.ReportProgress(0, string.Format(format, arg));
            }
        }

        public void ReportWithCancellationCheck(int percentProgress, string message)
        {
            this.ThrowIfCancellationPending();

            if(this.Worker.WorkerReportsProgress)
            {
                this.Worker.ReportProgress(percentProgress, message);
            }
        }

        public void ReportWithCancellationCheck(int percentProgress, string format, params object[] arg)
        {
            this.ThrowIfCancellationPending();

            if(this.Worker.WorkerReportsProgress)
            {
                this.Worker.ReportProgress(percentProgress, string.Format(format, arg));
            }
        }
    }
}
