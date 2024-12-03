namespace ModernUIDemo
{
    using System.Globalization;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Markup;

    using ModernUILibrary.Core;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string DEFAULTLANGUAGE = "de-DE";

        public App()
        {
            InitializeCultures();

            /*
            var aa = new CipherService().Encrypt("Gerhard", "2024202420242024");
            var bb = new CipherService().Decrypt(aa,"2024202420242024");
            */

            /*
            Type ti = typeof(IAssemblyInfo);
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (ti.IsAssignableFrom(type))
                    {
                        if (type != null)
                        {
                            var name = type.FullName;
                            IAssemblyInfo assemblyInfoObject = (IAssemblyInfo)Activator.CreateInstance(type);
                        }
                    }
                }
            }
            */
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
