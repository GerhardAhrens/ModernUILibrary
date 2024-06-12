namespace ModernIU.Messaging
{
    public class PropertyChangedMessage<T> : PropertyChangedMessageBase
    {
        public PropertyChangedMessage(object sender, T oldValue, T newValue, string propertyName) : base(sender, propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public PropertyChangedMessage(T oldValue, T newValue, string propertyName) : base(propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public PropertyChangedMessage(object sender, object target, T oldValue, T newValue, string propertyName) : base(sender, target, propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public T NewValue { get; private set; }

        public T OldValue { get; private set; }
    }
}