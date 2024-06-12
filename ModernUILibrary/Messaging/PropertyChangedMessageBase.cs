namespace ModernIU.Messaging
{
    public abstract class PropertyChangedMessageBase : MessageBase
    {
        protected PropertyChangedMessageBase(object sender, string propertyName) : base(sender)
        {
            this.PropertyName = propertyName;
        }

        protected PropertyChangedMessageBase(object sender, object target, string propertyName) : base(sender, target)
        {
            this.PropertyName = propertyName;
        }

        protected PropertyChangedMessageBase(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public string PropertyName { get; protected set; }
    }
}