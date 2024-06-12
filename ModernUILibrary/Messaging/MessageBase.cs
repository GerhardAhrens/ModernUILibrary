namespace ModernIU.Messaging
{
    public class MessageBase
    {
        /// <summary>
        /// Initializes a new instance of the MessageBase class.
        /// </summary>
        public MessageBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MessageBase class.
        /// </summary>
        /// <param name="sender">The message's original sender.</param>
        public MessageBase(object sender)
        {
            this.Sender = sender;
        }

        public MessageBase(object sender, object target) : this(sender)
        {
            this.Target = target;
        }

        public object Sender { get; protected set; }

        public object Target  { get;  protected set; }
    }
}