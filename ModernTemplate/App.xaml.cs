//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="company">
//     Class: MainWindow
//     Copyright © company 2025
// </copyright>
//
// <author>Gerhard Ahrens - company</author>
// <email>gerhard.ahrens@company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Die Klasse beinhaltet die Funktionaliät die beim Start geprüft bzw. erstellt werden sollen.
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate
{
    using System.Cmdl;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Threading;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Core.Logger;

    using ModernTemplate.Core;

    using ModernUI.MVVM.Enums;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const int MAX_INSTANCE = 2;
        private const string DEFAULTLANGUAGE = "de-DE";
        public const string SHORTAPPNAME = "ModernTemplate";
        public const string LONGAPPNAME = "Modern Template - Projekt";
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

        public static double CurrentDialogWidth { get; set; }

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
        public static int SetLoggingLevel { get; set; }

        public static ILogger Logger { get { return Logging.Instance.GetLogger(App.SHORTAPPNAME); } }

        public static string ProgramDataPath { get { return new UserPreferences().ProgramDataPath(); } }

        public static string StartApplicationUser { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SingleInstanceApplication();

            CommandLineParser();

            try
            {
                SetLoggingLevel = 0;
                ExitApplicationQuestion = true;
                SaveLastWindowsPosition = false;
                RunEnvironment = RunEnvironments.Development;

#if DEBUG
                PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
                PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical;
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

            Logger.SetLevel(App.SetLoggingLevel);

            Logger.Info($"Start '{App.SHORTAPPNAME}'");
            Logger.Info("InitializeLogger");
            Logger.Flush();
        }

        private void InitializeSettings()
        {
            using (ApplicationSettings settings = new ApplicationSettings())
            {
                settings.Load();
                if (settings.IsExitSettings() == false)
                {
                    settings.ExitApplicationQuestion = App.ExitApplicationQuestion;
                    settings.SaveLastWindowsPosition = App.SaveLastWindowsPosition;
                    settings.SetLoggingLevel = App.SetLoggingLevel;
                    settings.Save();
                }
                else
                {
                    App.ExitApplicationQuestion = settings.ExitApplicationQuestion;
                    App.SaveLastWindowsPosition = settings.SaveLastWindowsPosition;
                    App.SetLoggingLevel = settings.SetLoggingLevel;
                }
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

        private static void SingleInstanceApplication()
        {
            Process proc = Process.GetCurrentProcess();
            int count = Process.GetProcesses().Where(p => p.ProcessName == proc.ProcessName).Count();

            if (count > MAX_INSTANCE)
            {
                InfoMessage($"Die Anwendung {proc.ProcessName} wird bereits ausgeführt ({count}x) und daher wieder beendet.");
                ApplicationExit();
            }
        }

        private static void CommandLineParser()
        {
            string[] cmdArgs = CommandManager.CommandLineToArgs(Environment.CommandLine);
            CommandParser parser = new CommandParser(cmdArgs);
            ApplicationCmdl commandLineInfo = parser.Parse<ApplicationCmdl>();
            string[] helpText = parser.GetHelpInfo<ApplicationCmdl>()?.Split('\n');

            if (helpText?.Length > 0)
            {
                InfoMessage(string.Join('\n', helpText));
                ApplicationExit();
            }
            else
            {
                if (commandLineInfo != null)
                {
                    if (string.IsNullOrEmpty(commandLineInfo.Username) == false)
                    {
                        StartApplicationUser = commandLineInfo.Username;
                        StartApplicationUser = $"{Environment.UserDomainName}\\{commandLineInfo.Username}";
                    }
                    else
                    {
                        StartApplicationUser = $"{Environment.UserDomainName}\\{Environment.UserName}";
                    }

                    StatusbarMain.Statusbar.SetCurrentUser(StartApplicationUser);
                }
                else
                {
                    StartApplicationUser = $"{Environment.UserDomainName}\\{Environment.UserName}";
                    StatusbarMain.Statusbar.SetCurrentUser(StartApplicationUser);
                }
            }

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
