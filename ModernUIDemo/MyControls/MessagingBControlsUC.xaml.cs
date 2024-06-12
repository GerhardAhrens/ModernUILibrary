namespace ModernUIDemo.MyControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Messaging;

    using ModernUIDemo.Messaging;

    /// <summary>
    /// Interaktionslogik für MessagingBControlsUC.xaml
    /// </summary>
    public partial class MessagingBControlsUC : UserControl
    {
        public MessagingBControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.StartButton, "Click", this.OnStartButtonClick);
            Messenger.Default.Register<MessageEventArgs>(this, this.UpdateContent);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            MessageEventArgs messageName = new MessageEventArgs
            {
                Sender = typeof(MessagingBControlsUC),
                Text = ((Button)sender).Content.ToString()
            };

            Messenger.Default.Send(messageName);
        }

        private void UpdateContent(MessageEventArgs args)
        {
            MessageBox.Show(args.Text);
        }
    }
}
