namespace ModernMVVMDemo.View
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für ContentA_UC.xaml
    /// </summary>
    public partial class ContentA_UC : UserControlBase
    {
        public ContentA_UC() : base(typeof(ContentA_UC))
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Unloaded", this.OnUnloaded);

            this.InitCommands();
            this.DataContext = this;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        public string ClickContentText
        {
            get { return base.GetValue<string>(); }
            set { base.SetValue(value); }
        }

        public override void InitCommands()
        {
            base.CmdAgg.AddOrSetCommand("CallACommand", new RelayCommand(p1 => this.CallAHandler(p1), p2 => true));
        }

        private void CallAHandler(object p1)
        {
            this.ClickContentText = "Button geklickt";
        }
    }
}
