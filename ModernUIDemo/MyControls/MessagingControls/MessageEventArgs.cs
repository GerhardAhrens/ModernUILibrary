namespace ModernUIDemo.Messaging
{
    using System;

    public class MessageEventArgs : EventArgs
    {
        public Type Sender { get; set; }

        public string Text { get; set; }
    }
}
