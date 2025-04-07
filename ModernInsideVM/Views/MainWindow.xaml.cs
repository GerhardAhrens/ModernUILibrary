namespace ModernInsideVM.Views
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Extension;

    using ModernInsideVM.Core;
    using ModernInsideVM.Views.ContentControls;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase, IDialogClosing
    {
        private const string DateFormat = "dd.MM.yyyy HH:mm";
        private DispatcherTimer statusBarDate = null;

        public MainWindow()
        {
            this.InitializeComponent();
            this.InitTimer();
            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            this.InitTimer();
            Keyboard.Focus(this);

            this.DialogDescription = "ModernInsideVM";
            base.EventAgg.Subscribe<ChangeViewEventArgs>(this.ChangeControl);

            ChangeViewEventArgs arg = new ChangeViewEventArgs();
            arg.MenuButton = CommandButtons.Home;
            this.ChangeControl(arg);

            StatusbarMain.Statusbar.SetDatabaeInfo();
        }

        #region Properties
        public string DialogDescription
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public UserControl WorkContent
        {
            get { return base.GetValue<UserControl>(); }
            set { base.SetValue(value); }
        }

        private CommandButtons CurrentUCName { get; set; }
        #endregion Properties

        public override void OnViewIsClosing(CancelEventArgs e)
        {
            Window window = Application.Current.MainWindow;
            if (window != null)
            {
                e.Cancel = false;
                this.statusBarDate.Stop();
                Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                Application.Current.Shutdown();
            }

        }

        private void ChangeControl(ChangeViewEventArgs e)
        {
            if (e.MenuButton == CommandButtons.CloseApp)
            {
                this.Close();
                return;
            }

            FactoryResult view = DialogFactory.Get(e.MenuButton, e);
            if (view is FactoryResult menuWorkArea)
            {
                using (ObjectRuntime objectRuntime = new ObjectRuntime())
                {
                    StatusbarMain.Statusbar.SetNotification();
                    this.CurrentUCName = e.MenuButton;
                    string titelUC = e.MenuButton.ToDescription();
                    this.WorkContent = menuWorkArea.WorkContent;
                    this.WorkContent.VerticalAlignment = VerticalAlignment.Stretch;
                    this.WorkContent.Focusable = true;

                    TextBlock textBlock = UIHelper.FindByName<TextBlock>(this.WorkContent, "TbTitelUC");
                    if (textBlock != null)
                    {
                        textBlock.Text = titelUC;
                    }

                    StatusbarMain.Statusbar.SetNotification($"Bereit: {objectRuntime.ResultMilliseconds()}ms");
                }
            }
            else
            {
                throw new NotSupportedException($"Der Dialog '{e.MenuButton}|{e.MenuButton.ToString()}' kann nicht gefunden werden.");
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (this.WorkContent != null)
            {
                if (this.WorkContent.GetType() == typeof(HomeUC))
                {
                    this.WorkContent.Width = this.Width;
                }
            }
        }

        private void InitTimer()
        {
            this.statusBarDate = new DispatcherTimer();
            this.statusBarDate.Interval = new TimeSpan(0, 0, 1);
            this.statusBarDate.Start();
            this.statusBarDate.Tick += new EventHandler(
                delegate (object s, EventArgs a)
                {
                    this.dtStatusBarDate.Text = DateTime.Now.ToString(DateFormat);
                });
        }
    }
}