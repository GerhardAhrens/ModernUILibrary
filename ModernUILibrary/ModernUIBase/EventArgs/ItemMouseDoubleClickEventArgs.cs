namespace ModernIU.Base
{
    using System;

    public class ItemMouseDoubleClickEventArgs<T> : EventArgs
    {
        public ItemMouseDoubleClickEventArgs() { }

        public T NewValue { get; private set; }

        public static ItemMouseDoubleClickEventArgs<T> ItemDoubleClick(T newValue)
        {
            return new ItemMouseDoubleClickEventArgs<T>() { NewValue = newValue };
        }
    }
}
