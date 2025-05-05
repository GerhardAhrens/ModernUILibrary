namespace ModernTemplate.Views.ContentControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernBaseLibrary.Core.Logger;
    using ModernTemplate.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für TabAppSettingsAllgemein.xaml
    /// </summary>
    public partial class TabAppSettingsAllgemein : UserControlBase
    {
        public TabAppSettingsAllgemein() : base(typeof(TabAppSettingsAllgemein))
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Unloaded", this.OnUnloaded);

            this.InitCommands();
            this.DataContext = this;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            using (ApplicationSettings settings = new ApplicationSettings())
            {
                if (settings.IsExitSettings() == true)
                {
                    settings.Load();
                    settings.SetLoggingLevel = this.LogLevelSelectionChanged;
                    settings.ExitApplicationQuestion = this.ExitQuestion;
                    settings.SaveLastWindowsPosition = this.ApplicationPosition;
                    settings.Save();
                }

                App.SetLoggingLevel = settings.SetLoggingLevel;
                App.ExitApplicationQuestion = settings.ExitApplicationQuestion;
                App.SaveLastWindowsPosition = settings.SaveLastWindowsPosition;
            }
        }

        #region Properties
        public bool ExitQuestion
        {
            get => base.GetValue<bool>();
            set => base.SetValue(value);
        }

        public bool ApplicationPosition
        {
            get => base.GetValue<bool>();
            set => base.SetValue(value);
        }

        public Dictionary<int, string> LogLevelSource
        {
            get => base.GetValue<Dictionary<int, string>>();
            set => base.SetValue(value);
        }

        public int LogLevelSelectionChanged
        {
            get => base.GetValue<int>();
            set => base.SetValue(value);
        }
        #endregion Properties

        public override void InitCommands()
        {
            /* Eventuelle Behandlung von Commands */
        }


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.LogLevelSource = LogLevelHelper.LogLevelToSource();

            using (ApplicationSettings settings = new ApplicationSettings())
            {
                if (settings.IsExitSettings() == true)
                {
                    settings.Load();
                    this.LogLevelSelectionChanged = settings.SetLoggingLevel;
                    this.ExitQuestion = settings.ExitApplicationQuestion;
                    this.ApplicationPosition = settings.SaveLastWindowsPosition;
                }

                this.IsUCLoaded = true;
            }
        }
    }
}
