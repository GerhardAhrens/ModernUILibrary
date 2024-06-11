//-----------------------------------------------------------------------
// <copyright file="RelayTCommand.cs" company="Lifeprojects.de">
//     Class: RelayCommand
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>18.09.2018</date>
//
// <summary>Class with RelayCommand(TArgs) Definition</summary>
//-----------------------------------------------------------------------

namespace ModernUIDemo.Core
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;

    [DebuggerStepThrough]

    public class RelayCommand<TArgs> : ICommand
    {
        /// <summary>
        /// The execute delegate.
        /// </summary>
        protected Action<TArgs> executeDelegate;

        /// <summary>
        /// The can execute delegate.
        /// </summary>
        protected Predicate<TArgs> canExecuteDelegate;

        /// <summary>
        /// The pre action delegate.
        /// </summary>
        protected Action preActionDelegate;

        /// <summary>
        /// The post action delegate.
        /// </summary>
        public Action postActionDelegate;
        
        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action<TArgs> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<TArgs> execute, Predicate<TArgs> canExecute)
        {
            if (execute == null)
            {
                this.executeDelegate = new Action<TArgs>(p1 => { });
            }

            this.executeDelegate = execute;
            this.canExecuteDelegate = canExecute;
        }
       
        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <param name="preAction">The pre action.</param>
        /// <param name="postAction">The post action.</param>
        public RelayCommand(Action<TArgs> execute, Predicate<TArgs> canExecute, Action preAction, Action postAction)
        {
            if (execute == null)
            {
                this.executeDelegate = new Action<TArgs>(p1 => { });
            }

            this.executeDelegate = execute;
            this.canExecuteDelegate = canExecute;
            this.preActionDelegate = preAction;
            this.postActionDelegate = postAction;
        }
       
        /// <summary>
        /// The CanExecute method. Calls the given CanExecute delegate.
        /// </summary>
        /// <param name="parameter">The CanExecute parameter value.</param>
        /// <returns>
        /// True, if command can be executed; false otheriwse.
        /// </returns>
        public virtual bool CanExecute(object parameter)
        {
            if (parameter != null)
            {
                return this.canExecuteDelegate == null ? true : this.canExecuteDelegate((TArgs)parameter);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Registration of the CanExecute. Listening for changes using the CommandManager.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// The Execute method. Calls the given Execute delegate.
        /// </summary>
        /// <param name="parameter">The Execute parameter value.</param>
        public virtual void Execute(object parameter)
        {
            preActionDelegate?.Invoke();
            executeDelegate?.Invoke((TArgs)parameter);
            postActionDelegate?.Invoke();
        }

        /// <summary>
        /// Overrides the pre action delegate.
        /// </summary>
        /// <param name="preAction">The pre action.</param>
        public void OverridePreActionDelegate(Action preAction)
        {
            this.preActionDelegate = preAction;
        }

        /// <summary>
        /// Overrides the post action delegate.
        /// </summary>
        /// <param name="postAction">The post action.</param>
        public void OverridePostActionDelegate(Action postAction)
        {
            this.postActionDelegate = postAction;
        }
    }
}