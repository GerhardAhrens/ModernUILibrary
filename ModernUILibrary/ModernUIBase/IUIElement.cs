namespace ModernIU.Base
{
    public interface IUIElement : IDisposable
    {
        void EventsRegistion();

        void EventDeregistration();
    }
}
