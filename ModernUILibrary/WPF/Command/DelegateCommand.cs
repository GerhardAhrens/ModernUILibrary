//-----------------------------------------------------------------------
// <copyright file="DelegateCommand.cs" company="www.pta.de">
//     Class: DelegateCommand
//     Copyright © www.pta.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - www.pta.de</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>27.05.2024 10:33:22</date>
//
// <summary>
// Klasse zur Erstellung von Commands
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.WPF.Base
{
    using System;
    using System.Windows.Input;

    public class DelegateCommand : ICommand
    {
        Action<object> _execute;
        Func<bool> _canExecute;

        public DelegateCommand(Action<object> execute, Func<bool> canExecute = null)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (this._canExecute == null)
            {
                return true;
            }

            return this._canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            this._execute?.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
