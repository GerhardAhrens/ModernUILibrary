namespace ModernUI.MVVM.Base
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Versioning;
    using System.Windows.Input;

    /// <summary>
    /// DelegateCommand borrowed from
    /// </summary>
    [DebuggerStepThrough]
    [SupportedOSPlatform("windows")]
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<bool> _canExecute;

        public DelegateCommand(Action<object> execute, Func<bool> canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (this._canExecute == null)
            {
                return true;
            }

            return this._canExecute();
        }

        public void Execute(object parameter)
        {
            this._execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}