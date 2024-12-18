namespace ModernUI.MVVM.Base
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Versioning;
    using System.Windows.Input;

    [DebuggerStepThrough]
    [SupportedOSPlatform("windows")]
    public class ButtonCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public event EventHandler CanExecuteChanged;

        public ButtonCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public ButtonCommand(Action<object> execute) : this(execute, null) { }

        public virtual bool CanExecute(object parameter)
        {
            if (this._canExecute == null)
            {
                return true;
            }

            return this._canExecute(parameter);
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
