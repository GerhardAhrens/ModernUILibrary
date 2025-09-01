//-----------------------------------------------------------------------
// <copyright file="TemplateOverviewUC.xaml.cs" company="company">
//     Class: TemplateOverviewUC.xaml
//     Copyright © company yyyy
// </copyright>
//
// <author>Autor - company</author>
// <email>autor@company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Beispiel UI Dialog zur Darstellung von Daten in einer Übersicht
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Views.ContentControls
{
    using System.ComponentModel;
    using System.Data;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Extension;

    using ModernIU.Controls;

    using ModernTemplate.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für TemplateOverviewUC.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
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
        public ICollectionView DialogDataView
        {
            get => base.GetValue<ICollectionView>();
            set => base.SetValue(value);
        }

        public DataRow CurrentSelectedItem
        {
            get => base.GetValue<DataRow>();
            set => base.SetValue(value);
        }

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
            this.CmdAgg.AddOrSetCommand("RecordDeleteCommand", new RelayCommand(this.RecordDeleteHandler, this.CanRecordDeleteHandler));
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
        /// <summary>
        /// Lesen der Daten aus einer Datenquelle zum Aufbau der Übersicht
        /// </summary>
        /// <param name="isRefresh">Es wird nicht Positioniert, der erste Eintrag ist als Aktuell markiert</param>
        private void LoadDataHandler(bool isRefresh = false)
        {
            try
            {
                using (ObjectRuntime objectRuntime = new ObjectRuntime())
                {
                    /* Lesen aller Daten zum Aufbau der Übersicht */

                    this.IsFilterContentFound = this.DisplayRowCount > 0 ? true : false;
                    StatusbarMain.Statusbar.SetNotification($"Bereit: {objectRuntime.ResultMilliseconds()}ms; Anzahl: {this.DisplayRowCount}");
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Diese Funktion wird zum Filtern der Daten aus dem Listview aufgerufen.
        /// </summary>
        /// <param name="rowItem">Aktuelles DataRow</param>
        /// <returns></returns>
        private bool DataDefaultFilter(DataRow rowItem)
        {
            bool found = false;

            if (rowItem == null)
            {
                return false;
            }

            return found;

        }

        /// <summary>
        /// Wird bei jeder Eingabe zum Suchen/Filtern aufgerufen
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        private void RefreshDefaultFilter(string value, string propertyName)
        {
            if (value != null)
            {

                this.DialogDataView.Refresh();
                this.DisplayRowCount = this.DialogDataView.Count<DataRow>();
                this.DialogDataView.MoveCurrentToFirst();

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
                RowNextAction = RowNextAction.AddRow,
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
            if (this.CurrentSelectedItem == null)
            {
                this.CurrentSelectedItem = ((DataRow)obj);
            }

            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.None,
                FromPage = CommandButtons.None,
                RowPosition = 0, /* aktuelle Position im Grid */
                RowNextAction = RowNextAction.Refresh,
                EntityId = Guid.Empty, /* Guid in der Auswahl vom Edit-Item */
            });
        }

        private bool CanRecordDeleteHandler(object commandParam)
        {
            if (this.CurrentSelectedItem == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void RecordDeleteHandler(object obj)
        {
            if (this.CurrentSelectedItem == null)
            {
                this.CurrentSelectedItem = ((DataRow)obj);
            }

            /* Logik zum löschen eines Datensatz */
            /* Refresh auf der Übersicht erzwingen */
        }
        #endregion CommandHandler
    }
}
