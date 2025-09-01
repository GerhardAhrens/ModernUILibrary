//-----------------------------------------------------------------------
// <copyright file="TemplateDetailUC.xaml.cs" company="company">
//     Class: TemplateDetailUC.xaml
//     Copyright © company yyyy
// </copyright>
//
// <author>Autor - company</author>
// <email>autor@lifeprojects.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Beispiel UI Dialog zur Detail Bearbeitung
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Views.ContentControls
{
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    using ModernBaseLibrary.Collection;
    using ModernBaseLibrary.Core;

    using ModernIU.Controls;

    using ModernTemplate.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für TemplateDetailUC.xaml
    /// </summary>
    public partial class TemplateDetailUC : UserControlBase
    {
        /// <summary>
        /// Konstruktor der Klasse mit einem Konfigurationsparameter 'ChangeViewEventArgs'
        /// </summary>
        /// <param name="args"></param>
        public TemplateDetailUC(ChangeViewEventArgs args) : base(typeof(TemplateDetailUC))
        {
            this.InitializeComponent();

            this.CtorArgs = args;

            this.InitCommands();

            this.ValidationErrors = new ObservableDictionary<string, Popup>();

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Unloaded", this.OnUnLoaded);
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Unloaded", this.OnUnLoaded);
            WeakEventManager<Button, RoutedEventArgs>.AddHandler(this.BtnShowErrors, "Click", this.ShowInputValidation);
            NotifiactionPopup.PopupClick += this.OnNotifiactionPopupClick;
        }

        #region Properties
        public string DemoText
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public DataRow OriginalRow
        {
            get => base.GetValue<DataRow>();
            set => base.SetValue(value);
        }

        public DataRow CurrentRow
        {
            get => base.GetValue<DataRow>();
            set => base.SetValue(value);
        }

        #region InputValidation List
        public ObservableDictionary<string, Popup> ValidationErrors
        {
            get => base.GetValue<ObservableDictionary<string, Popup>>();
            set => base.SetValue(value);
        }
        #endregion InputValidation List

        private INotificationService NotificationService { get; set; } = new NotificationService();
        private bool IsColumnModified { get; set; } = false;
        private ChangeViewEventArgs CtorArgs { get; set; }

        #endregion Properties

        public override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(CommandButtons.DialogBack, new RelayCommand(this.DialogBackHandler));
            this.CmdAgg.AddOrSetCommand("RecordSaveCommand", new RelayCommand(this.RecordSaveHandler));
            this.CmdAgg.AddOrSetCommand("DialogCancelCommand", new RelayCommand(this.DialogCancelHandler));
        }

        #region WindowEventHandler
        /// <summary>
        /// Ereignis wenn das UserControl erstellt wurde und sichtbar ist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            Keyboard.Focus(this);

            this.RegisterValidations();
            this.LoadDataHandler();

            this.DataContext = this;
            this.IsUCLoaded = true;

            this.DemoText = string.Empty;
        }

        /// <summary>
        /// Ereignis, wenn das UserControl verlassen wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            /* Funktionalität beim verlassen des UserControls */
            NotifiactionPopup.Reset();
            NotifiactionPopup.PopupClick -= this.OnNotifiactionPopupClick;
        }

        #endregion WindowEventHandler

        #region Daten landen und Filtern
        /// <summary>
        /// In der Methode werden die notwendigen Daten für die GUI geladen
        /// </summary>
        private void LoadDataHandler()
        {
            /* lesen und vorbereiten von Daten */
            try
            {

            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Löst ein Event aus, wenn sich ein DataColumn auf einer DataRow ändert. Intern wird die Methode 'CheckInputControls' ausgelöst.
        /// </summary>
        /// <param name="sender">DataRow</param>
        /// <param name="e">DataColumn</param>
        private void OnColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            string fieldName = e.Column.ColumnName;
            this.IsColumnModified = true;
            this.CheckInputControls(fieldName);
            StatusbarMain.Statusbar.SetNotification("Geändert:");
        }


        #endregion Daten landen und Filtern

        #region Register und Prüfen Validations
        /// <summary>
        /// Registrierung der Validierungsregeln für die Eingabe. Zu den vorhandenen Regeln können weitere erstellt werden. 
        /// </summary>
        private void RegisterValidations()
        {
            /* Validierungsregeln für 
            this.ValidationRules.Add("Word", () =>
            {
                return InputValidation<DataRow>.This(this.CurrentRow).NotEmpty("Word", "Stichwort");
            });
            */

        }

        /// <summary>
        /// Prüfung der Eingaben mit den für das Column regisitriere Regelset.
        /// </summary>
        /// <param name="fieldNames">Column</param>
        /// <returns>True ? Eingabe OK</returns>
        private bool CheckInputControls(params string[] fieldNames)
        {
            bool result = false;

            try
            {
                foreach (string fieldName in fieldNames)
                {
                    Func<Result<string>> function = null;
                    if (this.ValidationRules.TryGetValue(fieldName, out function) == true)
                    {
                        Result<string> ruleText = this.DoValidation(function, fieldName);
                        if (string.IsNullOrEmpty(ruleText.Value) == false)
                        {
                            if (this.ValidationErrors.ContainsKey(fieldName) == false)
                            {
                                NotifiactionPopup.PlacementTarget = this.BtnShowErrors;
                                NotifiactionPopup.Delay = 5;
                                Popup po = NotifiactionPopup.CreatePopup(fieldName, ruleText.Value);
                                po.IsOpen = true;
                                this.ValidationErrors.Add(fieldName, po);
                            }
                        }
                        else
                        {
                            if (this.ValidationErrors.ContainsKey(fieldName) == true)
                            {
                                NotifiactionPopup.Remove();
                                this.ValidationErrors.Remove(fieldName);
                            }
                        }
                    }
                }

                if (this.ValidationErrors != null && this.ValidationErrors.Count > 0)
                {
                    result = true;
                }
                else if (this.ValidationErrors != null && this.ValidationErrors.Count == 0)
                {
                    NotifiactionPopup.Reset();
                    result = false;
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return result;
        }

        /// <summary>
        /// Bei einem Fehler in der Eingabe wird ein Popup mit einem entsprechendem Hinweis dargestellt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowInputValidation(object sender, RoutedEventArgs e)
        {
            foreach (Popup item in this.ValidationErrors.Values)
            {
                item.IsOpen = true;
            }
        }

        /// <summary>
        /// Bei einem Fehler in der Eingabe wird ein Popup dargestellt. Durch einen Klick auf das Popup wird auf das entsprechende Eingabe-Control gesprungen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNotifiactionPopupClick(object sender, PopupResultArgs e)
        {
            if (e != null)
            {
                /* Zu überwachendes Control
                if (e.SourceName == "Word")
                {
                    this.txtWord.Focus();
                }
                */
            }
        }
        #endregion Register und Prüfen Validations

        #region CommandHandler
        private void DialogBackHandler(object p1)
        {
            if (ValidationErrors?.Count > 0 || this.IsColumnModified == true)
            {
                /* 
                 * Eventuelle Prüfung, ob Validation Errors vorhanden sind 
                 * Meldung ausgeben, oder andere Behandlung
                 */
                _ = this.NotificationService.InputErrorsFound();
            }

            if (this.IsColumnModified == true)
            {
                NotificationBoxButton result = this.NotificationService.ExistLastChanges();
                if (result == NotificationBoxButton.Yes)
                {
                    base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
                    {
                        Sender = this.GetType().Name,
                        MenuButton = CommandButtons.Home,
                        FromPage = CommandButtons.TemplateDetailUC
                    });
                }
                else
                {
                    return;
                }
            }
            else
            {
                base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
                {
                    Sender = this.GetType().Name,
                    MenuButton = CommandButtons.Home,
                    FromPage = CommandButtons.TemplateDetailUC
                });
            }
        }

        private void DialogCancelHandler(object obj)
        {
            /* Dialog sofort ohne weitere Prüfung abbrechen*/
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.Home,
                FromPage = CommandButtons.TemplateDetailUC
            });
        }

        private void RecordSaveHandler(object obj)
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

        #endregion CommandHandler
    }
}
