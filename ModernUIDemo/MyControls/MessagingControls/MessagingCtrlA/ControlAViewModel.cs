namespace ModernUIDemo.Messaging
{
    using System.ComponentModel;
    using System.Windows.Input;

    public class ControlAViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private ICommand actionCommand;
        public ICommand ActionCommand
        {
            get => this.actionCommand = this.actionCommand ?? new RelayCommand(this);
        }

    }
}
