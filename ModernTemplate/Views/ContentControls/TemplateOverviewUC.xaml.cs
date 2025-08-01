//-----------------------------------------------------------------------
// <copyright file="TemplateOverviewUC.xaml.cs" company="Lifeprojects.de">
//     Class: TemplateOverviewUC.xaml
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>16.04.2025</date>
//
// <summary>
// Beispiel UI Dialog mit einem 'Back'-Button
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Views.ContentControls
{
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernIU.Controls;

    using ModernTemplate.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für TemplateOverviewUC.xaml
    /// </summary>
    public partial class TemplateOverviewUC : UserControlBase
    {
        private INotificationService notificationService = new NotificationService();

        public TemplateOverviewUC(ChangeViewEventArgs args) : base(typeof(TemplateOverviewUC))
        {
            this.InitializeComponent();

            this.CtorArgs = args;

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        #region Properties
        public string FilterDefaultSearch
        {
            get => base.GetValue<string>();
            set => base.SetValue(value, this.RefreshDefaultFilter);
        }

        public bool IsFilterContentFound
        {
            get => base.GetValue<bool>();
            set => base.SetValue(value);
        }

        private ChangeViewEventArgs CtorArgs { get; set; }

        #endregion Properties

        public override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(CommandButtons.DialogBack, new RelayCommand(this.DialogBackHandler));
            this.CmdAgg.AddOrSetCommand("RecordAddCommand", new RelayCommand(this.RecordAddHandler));
            this.CmdAgg.AddOrSetCommand("RecordEditCommand", new RelayCommand(this.RecordEditHandler, this.CanRecordEditHandler));
        }

        #region WindowEventHandler
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            Keyboard.Focus(this);
            this.InitCommands();

            this.DataContext = this;

            this.LoadDataHandler();

            this.IsUCLoaded = true;
        }

        #endregion WindowEventHandler

        #region Daten landen und Filtern
        private void LoadDataHandler(bool isRefresh = false)
        {
            try
            {

            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        private bool DataDefaultFilter(DataRow rowItem)
        {
            bool found = false;

            if (rowItem == null)
            {
                return false;
            }

            return found;

        }

        private void RefreshDefaultFilter(string value, string propertyName)
        {
            if (value != null)
            {

                this.IsFilterContentFound = this.DisplayRowCount > 0 ? true : false;

                StatusbarMain.Statusbar.SetNotification($"Bereit: Anzahl: {this.DisplayRowCount}");
            }
        }

        #endregion Daten landen und Filtern

        #region CommandHandler
        private void DialogBackHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.Home,
                FromPage = CommandButtons.CustomA
            });
        }

        private void RecordAddHandler(object obj)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.None,
                FromPage = CommandButtons.None,
                IsNew = true,
                IsRefresh = true,
            });
        }

        private bool CanRecordEditHandler(object commandParam)
        {
            /* Regel wann der Button inaktiv sein soll */
            return true;
        }

        private void RecordEditHandler(object obj)
        {
            /*
            if (this.CurrentSelectedItem == null)
            {
                this.CurrentSelectedItem = ((DataRow)obj);
            }
            */

            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.None,
                FromPage = CommandButtons.None,
                RowPosition = 0, /* aktuelle Position im Grid */
                IsNew = false,
                IsRefresh = true,
                EntityId = Guid.Empty, /* Guid in der Auswahl vom Edit-Item */
            });
        }
        #endregion CommandHandler
    }
}
