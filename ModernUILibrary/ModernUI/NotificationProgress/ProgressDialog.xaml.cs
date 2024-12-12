//-----------------------------------------------------------------------
// <copyright file="ProgressDialog.xaml.cs" company="lifeprojects.de">
//     Class: ProgressDialog
//     Copyright © lifeprojects.de GmbH 2019
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>08.02.2019</date>
//
// <summary>
//  Class with ProgressDialog Definition for XAML Window
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;

    public partial class ProgressDialog : Window
    {
        public static ProgressBarDialogContext Current { get; set; }

        private volatile bool isBusy;
        private BackgroundWorker worker;

        public ProgressBarDialogResult Result { get; private set; }

        public ProgressDialog(ProgressBarDialogType settings)
        {
            this.InitializeComponent();

            if(settings == null)
            {
                settings = ProgressBarDialogType.WithLabelOnly;
            }

            if (settings.ShowSubLabel)
            {
                this.Height = 140;
                this.MinHeight = 140;
                this.SubTextLabel.Visibility = Visibility.Visible;
            }
            else
            {
                this.Height = 110;
                this.MinHeight = 110;
                this.SubTextLabel.Visibility = Visibility.Collapsed;
            }

            this.CancelButton.Visibility = settings.ShowCancelButton ? Visibility.Visible : Visibility.Collapsed;

            this.ProgressBar.IsIndeterminate = settings.ShowProgressBarIndeterminate;
        }

        public string Label
        {
            get { return TextLabel.Text; }
            set { TextLabel.Text = value; }
        }

        public string SubLabel
        {
            get { return SubTextLabel.Text; }
            set { SubTextLabel.Text = value; }
        }

        public ProgressBarDialogResult Execute(object operation)
        {
            if(operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            ProgressBarDialogResult result = null;

            this.isBusy = true;

            this.worker = new BackgroundWorker();
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;

            this.worker.DoWork +=
                (s, e) => {

                    try
                    {
                        ProgressDialog.Current = new ProgressBarDialogContext(s as BackgroundWorker, e as DoWorkEventArgs);

                        if(operation is Action)
                        {
                            ((Action)operation)();
                        }
                        else if(operation is Func<object>)
                        {
                            e.Result = ((Func<object>)operation)();
                        }
                        else
                        {
                            throw new InvalidOperationException("Operation type is not supoorted");
                        }

                        ProgressDialog.Current.CheckCancellationPending();
                    }
                    catch(ProgressBarDialogCancellationExcpetion)
                    { }
                    catch(Exception ex)
                    {
                        string errorText = ex.Message;
                        if(!ProgressDialog.Current.CheckCancellationPending())
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        ProgressDialog.Current = null;
                    }

                };

            this.worker.RunWorkerCompleted +=
                (s, e) => {

                    result = new ProgressBarDialogResult(e);

                    Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate {
                        isBusy = false;
                        Close();
                    }, null);
                };

            this.worker.ProgressChanged +=
                (s, e) => {

                    if(!worker.CancellationPending)
                    {
                        this.SubLabel = (e.UserState as string) ?? string.Empty;
                        ProgressBar.Value = e.ProgressPercentage;
                    }

                };

            this.worker.RunWorkerAsync();

            this.ShowDialog();

            return result;
        }

        public void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            if(this.worker != null && this.worker.WorkerSupportsCancellation)
            {
                this.SubLabel = "Please wait while process will be cancelled...";
                this.CancelButton.IsEnabled = false;
                this.worker.CancelAsync();
            }
        }

        public void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = isBusy;
        }

        public static ProgressBarDialogResult Execute(Window owner, string label, Action operation)
        {
            return ExecuteInternal(owner, label, (object)operation, null);
        }

        public static ProgressBarDialogResult Execute(Window owner, string label, Action operation, ProgressBarDialogType settings)
        {
            return ExecuteInternal(owner, label, (object)operation, settings);
        }

        public static ProgressBarDialogResult Execute(Window owner, string label, Func<object> operationWithResult)
        {
            return ExecuteInternal(owner, label, (object)operationWithResult, null);
        }

        public static ProgressBarDialogResult Execute(Window owner, string label, Func<object> operationWithResult, ProgressBarDialogType settings)
        {
            return ExecuteInternal(owner, label, (object)operationWithResult, settings);
        }

        public static void Execute(Window owner, string label, Action operation, Action<ProgressBarDialogResult> successOperation, Action<ProgressBarDialogResult> failureOperation = null, Action<ProgressBarDialogResult> cancelledOperation = null)
        {
            ProgressBarDialogResult result = ExecuteInternal(owner, label, operation, null);

            if(result.Cancelled && cancelledOperation != null)
            {
                cancelledOperation(result);
            }
            else if(result.OperationFailed && failureOperation != null)
            {
                failureOperation(result);
            }
            else if(successOperation != null)
            {
                successOperation(result);
            }
        }

        internal static ProgressBarDialogResult ExecuteInternal(Window owner, string label, object operation, ProgressBarDialogType settings)
        {
            ProgressDialog dialog = new ProgressDialog(settings);
            dialog.Owner = owner;

            if(string.IsNullOrEmpty(label) == false)
            {
                dialog.Label = label;
            }

            return dialog.Execute(operation);
        }
    }
}
