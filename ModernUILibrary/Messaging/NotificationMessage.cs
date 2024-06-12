namespace ModernIU.Messaging
{
    public class NotificationMessage : MessageBase
    {
        public NotificationMessage(string notification)
        {
            this.Notification = notification;
        }

        public NotificationMessage(object sender, string notification) : base(sender)
        {
            this.Notification = notification;
        }

        public NotificationMessage(object sender, object target, string notification) : base(sender, target)
        {
            this.Notification = notification;
        }

        public string Notification  {get; private set; }
    }
}