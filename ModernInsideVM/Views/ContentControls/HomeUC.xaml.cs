namespace ModernInsideVM.Views.ContentControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernInsideVM.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für HomeUC.xaml
    /// </summary>
    public partial class HomeUC : UserControlBase
    {
        public HomeUC() : base(typeof(HomeUC))
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            Keyboard.Focus(this);
            this.InitCommands();
            this.IsUCLoaded = true;
            this.DataContext = this;
        }
        public override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand("CloseAppCommand", new RelayCommand(this.CloseAppHandler));
            this.CmdAgg.AddOrSetCommand("DialogACommand", new RelayCommand(this.DialogAHandler));
        }

        private void CloseAppHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.CloseApp,
            });
        }

        private void DialogAHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.DialogA,
            });
        }
    }
}
