namespace ModernInsideVM.Views.ContentControls
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernInsideVM.Core;

    using ModernUI.MVVM;
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
            WeakEventManager<UserControl, MouseWheelEventArgs>.AddHandler(this, "PreviewMouseWheel", this.OnPreviewMouseWheel);

            this.Id = this.CtorArgs.EntityId.ToString();

            DoSomething = x => Messages.Add("Action executed: " + x.ToString());
            Messages = new ObservableCollection<string>();
            Events = new[]
            {
                "PreviewMouseDown",
                "PreviewMouseUp",
                "PreviewMouseLeftButtonDown",
                "PreviewMouseLeftButtonUp",
                "PreviewMouseRightButtonDown",
                "PreviewMouseRightButtonUp",
                "MouseEnter",
                "MouseLeave"
            };

            this.SomeCommand = new SimpleCommand
            {
                //this will set the Message property to the value of the CommandParameter
                ExecuteDelegate = x => Messages.Add(x.ToString())
            };

            this.ClearMessagesCommand = new SimpleCommand
            {
                ExecuteDelegate = x => Messages.Clear(),
                CanExecuteDelegate = x => Messages.Count > 0
            };

            this.DoSomething = x => Messages.Add("Action executed: " + x.ToString());

            this.DataContext = this;
        }

        #region Properties
        public string Id
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        /// <summary>
        /// Gets the list of events to bind to
        /// </summary>
        public IList<string> Events { get; private set; }

        /// <summary>
        /// Gets the list of Messages populated (The messages are the names of the events that execute the commands)
        /// </summary>
        public IList<string> Messages { get; private set; }

        /// <summary>
        /// Gets an action that adds a message
        /// </summary>
        public Action<object> DoSomething { get; private set; }

        /// <summary>
        /// Command that clears the list of messages
        /// </summary>
        public ICommand ClearMessagesCommand { get; private set; }

        /// <summary>
        /// Command that write the event name that executed the command
        /// </summary>
        public ICommand SomeCommand { get; private set; }

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
