//-----------------------------------------------------------------------
// <copyright file="ActionDialog.xaml.cs" company="lifeprojects.de">
//     Class: ActionDialog
//     Copyright © lifeprojects.de GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>20.07.2021</date>
//
// <summary>
//  Class with ActionDialog Definition for XAML Window
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Versioning;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Threading;

    [SupportedOSPlatform("windows")]
    public partial class ActionDialog : Window
    {
        public static ActionDialogContext Current { get; set; }

        private BackgroundWorker worker;
        private bool isBusy;

        public ActionDialogResult Result { get; private set; }

        public ActionDialog(ActionDialogType settings)
        {
            this.InitializeComponent();
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Window, CancelEventArgs>.AddHandler(this, "Closing", this.OnClosing);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnCancelButton, "Click", this.OnCancelButtonClick);
            this.isBusy = false;

            if (settings == null)
            {
                settings = ActionDialogType.AllTextWihoutCancel;
            }

            if (settings.ShowInstructionText == true)
            {
                this.txtInstructionText.Visibility = Visibility.Visible;
            }
            else
            {
                this.txtInstructionText.Visibility = Visibility.Collapsed;
            }

            this.BtnCancelButton.Visibility = settings.ShowCancelButton ? Visibility.Visible : Visibility.Hidden;
        }

        public string HeaderText { get; set; }

        public string InstructionText
        {
            get { return this.txtInstructionText.Text; }
            set { this.txtInstructionText.Text = value; }
        }

        public string ActionText
        {
            get { return this.txtActionText.Text; }
            set { this.txtActionText.Text = value; }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        public void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = isBusy;
        }

        public void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.BtnCancelButton.IsEnabled == true)
            {
                if (this.worker != null && this.worker.WorkerSupportsCancellation)
                {
                    this.InstructionText = "Verarbeitung wird abgebrochen...";
                    this.BtnCancelButton.IsEnabled = false;
                    this.worker.CancelAsync();
                }
            }
        }

        public ActionDialogResult Execute(object operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException("operation");
            }

            ActionDialogResult result = null;
            this.isBusy = true;

            this.worker = new BackgroundWorker();
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;

            this.worker.DoWork +=
                (s, e) => {

                    try
                    {
                        ActionDialog.Current = new ActionDialogContext(s as BackgroundWorker, e as DoWorkEventArgs);

                        if (operation is Action)
                        {
                            ((Action)operation)();
                        }
                        else if (operation is Func<object>)
                        {
                            e.Result = ((Func<object>)operation)();
                        }
                        else
                        {
                            throw new InvalidOperationException("Operation type is not supoorted");
                        }

                        ActionDialog.Current.CheckCancellationPending();
                    }
                    catch (ProgressBarDialogCancellationExcpetion)
                    { }
                    catch (Exception ex)
                    {
                        string errorText = ex.Message;
                        if (!ActionDialog.Current.CheckCancellationPending())
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        ActionDialog.Current = null;
                    }

                };

            this.worker.RunWorkerCompleted +=
                (s, e) => {

                    result = new ActionDialogResult(e);

                    Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                    {
                        isBusy = false;
                        this.Close();
                    }, null);

                };

            this.worker.ProgressChanged +=
                (s, e) => {

                    if (!worker.CancellationPending)
                    {
                        if (e.UserState is string)
                        {
                            this.ActionText = (e.UserState as string) ?? string.Empty;
                        }
                        else if (e.UserState is Tuple<string,string>)
                        {
                            Tuple<string, string> message = (Tuple<string, string>)e.UserState;
                            this.InstructionText = message.Item1;
                            this.ActionText = message.Item2;
                        }
                    }

                };

            this.worker.RunWorkerAsync();
            this.ShowDialog();

            return result;
        }

        public static ActionDialogResult Execute(Window owner, string headerText, Action operation)
        {
            return ExecuteInternal(owner, headerText, (object)operation, null);
        }

        public static ActionDialogResult Execute(Window owner, string headerText, Action operation, ActionDialogType settings)
        {
            return ExecuteInternal(owner, headerText, (object)operation, settings);
        }

        public static ActionDialogResult Execute(Window owner, string headerText, Func<object> operationWithResult, ActionDialogType settings)
        {
            return ExecuteInternal(owner, headerText, (object)operationWithResult, settings);
        }

        internal static ActionDialogResult ExecuteInternal(Window owner, string headerText, object operation, ActionDialogType settings)
        {
            ActionDialog dialog = new ActionDialog(settings);
            dialog.Owner = owner;

            if (string.IsNullOrEmpty(headerText) == false)
            {
                dialog.HeaderText = headerText;
            }

            return dialog.Execute(operation);
        }
    }
}
