namespace ModernUIDemo
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Markup;

    using ModernBaseLibrary.Core;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string DEFAULTLANGUAGE = "de-DE";

        public App()
        {
            InitializeCultures();
        }

        public static string DatePattern { get; private set; }

        public record Person(string Name, int Age);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            /*
            Person scooby = new("Scooby Doo", 7);
            Person charlie = scooby with { Name = "Charlie", Age = 3 };
            */

            /*
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Add("NewName", "Daniel Doe");

            if (config.AppSettings.Settings["NewName"].Value.ToString() == "Daniel Doe")
            {
            }

            config.Save(ConfigurationSaveMode.Modified);
            */
        }

        private static void InitializeCultures(string language = DEFAULTLANGUAGE)
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

            DatePattern = $"{DateTimeFormatInfo.CurrentInfo.ShortDatePattern}";

            FrameworkPropertyMetadata frameworkMetadata = new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(new CultureInfo(language).IetfLanguageTag));
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), frameworkMetadata);
        }
    }
}
