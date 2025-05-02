namespace ModernTemplate
{
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Threading;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Core.IO;
    using ModernBaseLibrary.Core.Logger;

    using ModernTemplate.Core;

    using ModernUI.MVVM.Enums;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string DEFAULTLANGUAGE = "de-DE";
        public const string SHORTNAME = "ModernTemplate";
        private static readonly string MessageBoxTitle = "ModernTemplate Application";
        private static readonly string UnexpectedError = "An unexpected error occured.";
        private string exePath = string.Empty;
        private string exeName = string.Empty;

        public App()
        {
            try
            {
                /* Name der EXE Datei*/
                exeName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
                /* Pfad der EXE-Datei*/
                exePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

                /* Synchronisieren einer Textenigabe mit dem primären Windows (wegen Validierung von Eingaben)*/
                FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;

                /* Alle nicht explicit abgefangene Exception spätesten hier abfangen und anzeigen */
                this.DispatcherUnhandledException += this.OnDispatcherUnhandledException;
            }
            catch (Exception ex)
            {
                ex.Data.Add("UserDomainName", Environment.UserDomainName);
                ex.Data.Add("UserName", Environment.UserName);
                ex.Data.Add("exePath", exePath);
                ErrorMessage(ex, "General Error: ");
                ApplicationExit();
            }
        }

        /// <summary>
        /// Festlegung für Abfrage des Programmendedialog
        /// </summary>
        public static double CurrentDialogHeight { get; set; }

        /// <summary>
        /// Festlegung für Abfrage des Programmendedialog
        /// </summary>
        public static bool ExitApplicationQuestion { get; set; }

        /// <summary>
        /// Festlegung für das Speichern der Position des Main-Windows
        /// </summary>
        public static bool SaveLastWindowsPosition { get; set; }

        /// <summary>
        /// Festlegung für die aktuelle Laufzeitumgebung der Applikation
        /// </summary>
        public static RunEnvironments RunEnvironment { get; set; }

        /// <summary>
        /// Festlegen, ob in der Applikation das Logging aktiviert werden soll
        /// </summary>
        public static bool IsLogging { get; set; }

        public static ILogger Logger { get { return Logging.Instance.GetLogger(SHORTNAME); } }

        public static string ProgramDataPath { get { return new UserPreferences().ProgramDataPath(); } }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                IsLogging = false;
                ExitApplicationQuestion = true;
                SaveLastWindowsPosition = false;
                RunEnvironment = RunEnvironments.Development;

#if DEBUG
                PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
                PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical | SourceLevels.Error;
                //PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical | SourceLevels.Error | SourceLevels.Warning;
                //PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.All;
                PresentationTraceSources.RoutedEventSource.Listeners.Add(new ConsoleTraceListener());
                PresentationTraceSources.RoutedEventSource.Switch.Level = SourceLevels.All;
                PresentationTraceSources.ResourceDictionarySource.Listeners.Add(new ConsoleTraceListener());
                PresentationTraceSources.ResourceDictionarySource.Switch.Level = SourceLevels.All;
#endif

                /* Initalisierung Spracheinstellung */
                InitializeCultures(DEFAULTLANGUAGE);

                /* Initiale Benutzer Einstellungen speichern */
                InitializeSettings();

                /* Initalisierung Logging */
                InitializeLogger();
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                ErrorMessage(ex, "General Error: ");
                ApplicationExit();
            }
        }

        public static void ErrorMessage(Exception ex, string message = "")
        {
            string expMsg = ex.Message;
            var aex = ex as AggregateException;

            if (aex != null && aex.InnerExceptions.Count == 1)
            {
                expMsg = aex.InnerExceptions[0].Message;
            }

            if (string.IsNullOrEmpty(message) == true)
            {
                message = UnexpectedError;
            }

            StringBuilder errorText = new StringBuilder();
            if (ex.Data != null && ex.Data.Count > 0)
            {
                foreach (DictionaryEntry item in ex.Data)
                {
                    errorText.AppendLine($"{item.Key} : {item.Value}");
                }
            }

            MessageBox.Show(
                message + $"{expMsg}\n{ex.Message}\n{errorText.ToString()}",
                MessageBoxTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public static void InfoMessage(string message)
        {
            MessageBox.Show(
                message,
                MessageBoxTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private void InitializeLogger()
        {
            LogFileOutHandler handler = new LogFileOutHandler(Path.Combine(ProgramDataPath,"Log"));
            Logger.AddHandler(handler);
            if (IsLogging == false)
            {
                Logger.SetLevel(LogLevel.NOTSET);
            }
            else
            {
                Logger.SetLevel(LogLevel.INFO);
            }

            Logger.Info($"Start '{SHORTNAME}'");
            Logger.Info("InitializeLogger");
            Logger.Flush();
        }

        private void InitializeSettings()
        {
            using (ApplicationSettings settings = new ApplicationSettings())
            {
                settings.Load();
                settings.ExitApplicationQuestion = App.ExitApplicationQuestion;
                settings.SaveLastWindowsPosition = App.SaveLastWindowsPosition;
                settings.IsLogging = App.IsLogging;
                settings.RunEnvironment = App.RunEnvironment;
                settings.Save();
            }
        }

        private void InitializeCultures(string language)
        {
            if (string.IsNullOrEmpty(language) == false)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(DEFAULTLANGUAGE);
            }

            if (string.IsNullOrEmpty(language) == false)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }
            else
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(DEFAULTLANGUAGE);
            }

            FrameworkPropertyMetadata frameworkMetadata = new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(new CultureInfo(language).IetfLanguageTag));
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), frameworkMetadata);
        }

        /// <summary>
        /// Screen zum aktualisieren zwingen, Globale Funktion
        /// </summary>
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine($"{exeName}-{(e.Exception as Exception).Message}");
        }

        /// <summary>
        /// Programmende erzwingen
        /// </summary>
        public static void ApplicationExit()
        {
            Application.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }
    }
}
