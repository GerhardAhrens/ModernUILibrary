namespace ModernInsideVM.Views.ContentControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernInsideVM.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für DialogA.xaml
    /// </summary>
    public partial class DialogA : UserControlBase
    {
        public DialogA(ChangeViewEventArgs args) : base(typeof(DialogA))
        {
            this.InitializeComponent();
            this.InitCommands();
            this.CtorArgs = args;

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.Id = this.CtorArgs.EntityId.ToString();
            this.RowPos = this.CtorArgs.RowPosition;
            this.IsRefresh = this.CtorArgs.IsRefresh;

            this.DataContext = this;
        }

        #region Properties
        public string Id
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public int RowPos
        {
            get => base.GetValue<int>();
            set => base.SetValue(value);
        }

        public bool IsRefresh
        {
            get => base.GetValue<bool>();
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
            this.CmdAgg.AddOrSetCommand("DialogBCommand", new RelayCommand(this.DialogBHandler));
        }

        private void DialogBackHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.Home,
            });
        }

        private void DialogBHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.DialogB,
                FromPage = CommandButtons.DialogA,
                EntityId = Guid.NewGuid(),
                IsNew = false,
                RowPosition = 1,
                IsRefresh = false,
            });
        }
    }
}
