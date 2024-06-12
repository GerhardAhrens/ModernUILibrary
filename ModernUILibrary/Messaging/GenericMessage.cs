namespace ModernIU.Messaging
{
    /// <summary>
    /// Passes a generic value (Content) to a recipient.
    /// </summary>
    public class GenericMessage<T> : MessageBase
    {
        public GenericMessage(T content)
        {
            this.Content = content;
        }

        public GenericMessage(object sender, T content) : base(sender)
        {
            this.Content = content;
        }

        public GenericMessage(object sender, object target, T content) : base(sender, target)
        {
            this.Content = content;
        }

        public T Content { get; protected set; }
    }
}