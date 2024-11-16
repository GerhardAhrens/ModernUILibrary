namespace ModernIU.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ItemMouseRightButtonDownEventArgs<T> : EventArgs
    {
        public ItemMouseRightButtonDownEventArgs() { }

        public T NewValue { get; private set; }

        public static ItemMouseRightButtonDownEventArgs<T> ItemRightMouseButtonDown(T newValue)
        {
            return new ItemMouseRightButtonDownEventArgs<T>() { NewValue = newValue };
        }

        public static ItemMouseRightButtonDownEventArgs<T> ShowContextMenu()
        {
            return new ItemMouseRightButtonDownEventArgs<T>() {  };
        }
    }
}
