namespace ModernInsideVM.Views.ContentControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernBaseLibrary.Extension;

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
            WeakEventManager<UserControl, MouseWheelEventArgs>.AddHandler(this, "PreviewMouseWheel", this.OnPreviewMouseWheel);

            this.Id = this.CtorArgs.EntityId.ToString();
            if (this.Id.IsGuidEmpty() == true)
            {
                this.Id = Guid.NewGuid().ToString();
            }

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
            this.CmdAgg.AddOrSetCommand(CommandButtons.DialogBack, new RelayCommand(this.DialogBackHandler));
            this.CmdAgg.AddOrSetCommand(CommandButtons.DialogB, new RelayCommand(this.DialogBHandler));
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
                EntityId = this.Id.ToGuid(),
                IsNew = false,
                RowPosition = 1,
                IsRefresh = false,
            });
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) == true)
            {
                if (e.Delta > 0)
                {
                    if (this.Scalefactor.ScaleX <= 2.0)
                    {
                        this.Scalefactor.ScaleX = this.Scalefactor.ScaleX + 0.25;
                        this.Scalefactor.ScaleY = this.Scalefactor.ScaleY + 0.25;
                    }
                }

                if (e.Delta < 0)
                {
                    if (this.Scalefactor.ScaleX > 1.0)
                    {
                        this.Scalefactor.ScaleX = this.Scalefactor.ScaleX - 0.25;
                        this.Scalefactor.ScaleY = this.Scalefactor.ScaleY - 0.25;
                    }
                }
            }
        }
    }
}
