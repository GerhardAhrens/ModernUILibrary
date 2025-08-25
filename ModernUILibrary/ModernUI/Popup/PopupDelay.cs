namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Threading;

    public class PopupDelay : Popup
    {
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(nameof(Delay), typeof(int), typeof(PopupDelay), new PropertyMetadata(2));

        public PopupDelay()
        {
            this.StaysOpen = false;
            this.PopupAnimation = PopupAnimation.Slide;
            this.AllowsTransparency = true;
            WeakEventManager<Popup, EventArgs>.AddHandler(this, "Opened", this.OnPopupOpened);
        }

        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            if (this.IsOpen == false)
            {
                return;
            }

            DispatcherTimer time = new DispatcherTimer();
            time.Interval = TimeSpan.FromSeconds(this.Delay);
            time.Start();
            time.Tick += delegate
            {
                this.IsOpen = false;
                time.Stop();
            };
        }

    }
}
