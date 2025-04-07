namespace ModernInsideVM.Views.ContentControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernInsideVM.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für DialogB.xaml
    /// </summary>
    public partial class DialogB : UserControlBase
    {
        public DialogB(ChangeViewEventArgs args) : base(typeof(DialogB))
        {
            this.InitializeComponent();
            this.InitCommands();
            this.CtorArgs = args;

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.Id = this.CtorArgs.EntityId.ToString();

            this.DataContext = this;
        }

        #region Properties
        public string Id
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        private ChangeViewEventArgs CtorArgs { get; set; }
        #endregion Properties

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            Keyboard.Focus(this);
            this.IsUCLoaded = true;
        }

        public override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand("DialogBackCommand", new RelayCommand(this.DialogBackHandler));
        }

        private void DialogBackHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.DialogA,
                FromPage = CommandButtons.DialogB,
                RowPosition = this.CtorArgs.RowPosition,
                IsRefresh = true,
            });
        }
    }
}
