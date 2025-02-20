namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using ModernBaseLibrary.Localization;

    /// <summary>
    /// Interaktionslogik für LocalizationUC.xaml
    /// </summary>
    public partial class LocalizationUC : UserControl, INotifyPropertyChanged
    {
        public LocalizationUC()
        {
            this.InitializeComponent();

            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private ICollectionView languages;

        public ICollectionView Languages
        {
            get { return this.languages; }
            set
            { 
                this.languages = value;
                this.OnPropertyChanged();
            }
        }

        private string selectedLanguages;

        public string SelectedLanguages
        {
            get { return this.selectedLanguages; }
            set
            { 
                this.selectedLanguages  = value;
                if (string.IsNullOrEmpty(this.selectedLanguages) == false)
                {
                    TranslationManager.Instance.CurrentLanguage = new CultureInfo(this.selectedLanguages);
                }

                this.OnPropertyChanged();
            }
        }

        private string stringA;

        public string StringA
        {
            get { return this.stringA; }
            set
            {
                this.stringA = value;
                this.OnPropertyChanged();
            }
        }

        private string stringB;

        public string StringB
        {
            get { return this.stringB; }
            set
            {
                this.stringB = value;
                this.OnPropertyChanged();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TranslationManager.Instance.TranslationProvider = new ResxTranslationProvider("ModernUIDemo.Language.Resources", Assembly.GetExecutingAssembly());
            /*TranslationManager.Instance.CurrentLanguage = new CultureInfo("en-US");*/
            this.Languages = CollectionViewSource.GetDefaultView(TranslationManager.Instance.Languages);

            int countResult = TranslationManager.Instance.TranslateAs<int>("Count");
            string[] valueTranslate = TranslationManager.Instance.TranslateAs<string[]>("OnViewIsClosing");
            this.StringA = $"{valueTranslate[0]}|{valueTranslate[1]}";

            string msgText = string.Format(valueTranslate[1], countResult);
            this.StringB = msgText;

            /* Text aus TextString.xaml lesen*/
            ResourcesText.SetResources("Resources/Localization/TextString.xaml");
            this.tbString_1.Text = ResourcesText.Get("tbSelectedLanguage");
            this.tbString_2.Text = ResourcesText.Get("tbName");
            this.tbString_3.Text = ResourcesText.Get("tbCompany");
            this.tbString_4.Text = ResourcesText.Get("tbError");
        }

        #region PropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion PropertyChanged Implementierung
    }
}
