//-----------------------------------------------------------------------
// <copyright file="CommandKeyReference.cs" company="Lifeprojects">
//     Class: CommandKeyReference
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>
//      Class with CommandKeyReference Definition
//
//      <localCore:CommandKeyReference x:Key="ExitAppCmdKey" Command="{Binding Path=CmdAgg[ExitCommand]}" />
//      <Window.InputBindings>
//          <KeyBinding Key = "x" Command="{StaticResource ExitAppCmdKey}" Modifiers="Alt" />
//      </Window.InputBindings>
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Base
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class CommandKeyReference : Freezable, ICommand
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandKeyReference), new PropertyMetadata(new PropertyChangedCallback(OnCommandChanged)));

        public CommandKeyReference()
        {
        }

        public event EventHandler CanExecuteChanged;

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        public bool CanExecute(object parameter)
        {
            if (this.Command != null)
            {
                return this.Command.CanExecute(parameter);
            }

            return false;
        }

        public void Execute(object parameter)
        {
            this.Command.Execute(parameter);
        }

        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var commandReference = d as CommandKeyReference;
            var oldCommand = e.OldValue as ICommand;
            var newCommand = e.NewValue as ICommand;
            if (commandReference != null)
            {
                if (oldCommand != null)
                {
                    oldCommand.CanExecuteChanged -= commandReference.CanExecuteChanged;
                }

                if (newCommand != null)
                {
                    newCommand.CanExecuteChanged += commandReference.CanExecuteChanged;
                }
            }
        }
    }
}