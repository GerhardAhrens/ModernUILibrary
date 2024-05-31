namespace ModernUIDemo
{
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Markup;

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


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
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
