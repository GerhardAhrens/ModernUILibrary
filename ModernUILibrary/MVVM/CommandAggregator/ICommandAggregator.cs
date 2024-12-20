//-----------------------------------------------------------------------
// <copyright file="ICommandAggregator.cs" company="Lifeprojects.de">
//     Class: ICommandAggregator
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>18.09.2018</date>
//
// <summary>Interface Class with ICommandAggregator Definition</summary>
//-----------------------------------------------------------------------

namespace ModernUI.MVVM.Base
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public interface ICommandAggregator
    {
        ICommand this[string key] { get; }

        void AddOrSetCommand<TEnum>(TEnum key, ICommand command);

        void AddOrSetCommand(string key, ICommand command);

        void AddOrSetCommand<TEnum>(TEnum key, Action<object> executeDelegate, Predicate<object> canExecuteDelegate);

        void AddOrSetCommand(string key, Action<object> executeDelegate, Predicate<object> canExecuteDelegate);

        void AddOrSetCommand(string key, Action<object> executeDelegate);

        int Count();
        
        Task ExecuteAsync(string key, object parameter = null);
        
        bool Exists(string key);

        ICommand GetCommand(string key);
        
        bool HasNullCommand(string key);
        
        void Remove(string key);
        
        void RemoveAll();
    }
}