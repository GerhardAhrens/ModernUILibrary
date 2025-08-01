//-----------------------------------------------------------------------
// <copyright file="TemplateDetailUC.xaml.cs" company="Lifeprojects.de">
//     Class: TemplateDetailUC.xaml
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using ModernBaseLibrary.Collection;
    using ModernBaseLibrary.Extension;

    using ModernTemplate.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für TemplateDetailUC.xaml
    /// </summary>
    public partial class TemplateDetailUC : UserControlBase
    {
        public TemplateDetailUC(ChangeViewEventArgs args) : base(typeof(TemplateDetailUC))
        {
            this.InitializeComponent();

            this.CtorArgs = args;

            this.InitCommands();

            this.ValidationErrors = new ObservableDictionary<string, string>();

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        #region Properties
        public string DemoText
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        #region InputValidation List
        public ObservableDictionary<string, string> ValidationErrors
        {
            get => base.GetValue<ObservableDictionary<string, string>>();
            set => base.SetValue(value);
        }

        public string ValidationErrorsSelected
        {
            get => base.GetValue<string>();
            set => base.SetValue(value, this.InputNavigationProperty);
        }
        #endregion InputValidation List

        private ChangeViewEventArgs CtorArgs { get; set; }

        #endregion Properties

        public override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(CommandButtons.DialogBack, new RelayCommand(this.DialogBackHandler));
            this.CmdAgg.AddOrSetCommand("RecordSaveCommand", new RelayCommand(this.RecordSaveHandler));
        }

        #region WindowEventHandler
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

        #endregion WindowEventHandler

        #region Daten landen und Filtern
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
        #endregion Daten landen und Filtern

        #region Register Validations
        private void RegisterValidations()
        {
            /* Validierungsregeln für 
            this.ValidationRules.Add(nameof(this.Titel), () =>
            {
                return ValidationRule<TemplateUC>.This(this).NotEmpty(x => x.Titel, "Titel");
            });
            */

        }
        #endregion Register Validations

        #region InputValidation Handler
        private void InputNavigationProperty<T>(T value, string propertyName)
        {
            /* Zu prüfende Eingaben, Background auf Transparent setzten */

            if (value == null)
            {
                return;
            }

            foreach (var ctrl in this.GetChildren())
            {
                /* Gegebenenfalls Eingabe anpassen bzw. erweitern, z.B. CheckBox usw. */
                if (ctrl is TextBox textBox)
                {
                    Binding binding = BindingOperations.GetBinding(textBox, TextBox.TextProperty);
                    if (value != null)
                    {
                        if (binding != null && binding.Path.Path.EndsWith(value.ToString()))
                        {
                            this.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
                            {
                                textBox.Focus();
                                textBox.Background = Brushes.Coral;
                            }));

                            break;
                        }
                    }
                }
            }
        }
        #endregion InputValidation Handler

        #region CommandHandler
        private void DialogBackHandler(object p1)
        {
            if (ValidationErrors?.Count > 0)
            {
                /* 
                 * Eventuelle Prüfung, ob Validation Errors vorhanden sind 
                 * Meldung ausgeben, oder andere Behandlung
                 */
            }

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
