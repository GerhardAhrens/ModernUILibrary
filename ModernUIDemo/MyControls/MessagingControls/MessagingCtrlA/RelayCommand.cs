namespace ModernUIDemo.Messaging
{
    using System.Windows.Input;

    using ModernIU.Messaging;

    public class RelayCommand : ICommand
    {
        private ControlAViewModel _controlAViewModel;

        public RelayCommand(ControlAViewModel controlAViewModel, object canExecute = null)
        {
            this._controlAViewModel = controlAViewModel;
            this.CanExecuteChanged = (EventHandler)canExecute;
        }

        public event EventHandler CanExecuteChanged = null;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (parameter is null)
            {
                return;
            }

            MessageName messageName = new MessageName
            {
                Name = parameter.ToString()
            };

            Messenger.Default.Send(messageName);

        }
    }
}