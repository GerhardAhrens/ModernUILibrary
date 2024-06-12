namespace ModernUIDemo.Messaging
{
    using System.ComponentModel;

    using ModernIU.Messaging;

    public class ControlBViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ControlBViewModel()
        {
            this.ContentA = "Kein Button gedrückt";
            Messenger.Default.Register<MessageName>(this, this.UpdateContent);
        }

        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private string _contentA;

        public string ContentA
        {
            get => _contentA;
            set
            {
                this._contentA = value;
                this.OnPropertyChanged(nameof(ContentA));
            }
        }

        public void UpdateContent(MessageName message)
        {
            this.ContentA = $"{message.Name} Pressed";
        }
    }
}

