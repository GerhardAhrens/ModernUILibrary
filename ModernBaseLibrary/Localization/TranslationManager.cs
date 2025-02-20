namespace ModernBaseLibrary.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    public class TranslationManager
    {
        private static TranslationManager _translationManager;

        public event EventHandler LanguageChanged;

        public CultureInfo CurrentLanguage
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set
            {
                if (value != Thread.CurrentThread.CurrentUICulture)
                {
                    Thread.CurrentThread.CurrentUICulture = value;
                    this.OnLanguageChanged();
                }
            }
        }

        public IEnumerable<CultureInfo> Languages
        {
            get
            {
                if (TranslationProvider != null)
                {
                    return TranslationProvider.Languages;
                }

                return Enumerable.Empty<CultureInfo>();
            }
        }

        public static TranslationManager Instance
        {
            get
            {
                if (_translationManager == null)
                {
                    _translationManager = new TranslationManager();
                }

                return _translationManager;
            }
        }

        public ITranslationProvider TranslationProvider { get; set; }

        private void OnLanguageChanged()
        {
            if (this.LanguageChanged != null)
            {
                this.LanguageChanged(this, EventArgs.Empty);
            }
        }

        public object Translate(string key)
        {
            if (this.TranslationProvider != null)
            {
                object translatedValue = this.TranslationProvider.Translate(key);
                if (translatedValue != null)
                {
                    return translatedValue;
                }
            }

            return $"#{key}#";
        }

        public TResult TranslateAs<TResult>(string key, char separator = '|')
        {
            string CurrentLanguage = string.Empty;

            if (this.TranslationProvider != null)
            {
                CurrentLanguage = this.CurrentLanguage.Name;
                object translatedValue = this.TranslationProvider.Translate(key);
                if (translatedValue != null)
                {
                    if (translatedValue.ToString().StartsWith("#") == true && translatedValue.ToString().EndsWith("#") == true)
                    {
                        return default(TResult);
                    }

                    if (typeof(TResult) == typeof(string))
                    {
                        return (TResult)Convert.ChangeType(translatedValue, typeof(TResult));
                    }
                    else if (typeof(TResult) == typeof(int))
                    {
                        return (TResult)Convert.ChangeType(translatedValue, typeof(TResult));
                    }
                    else if (typeof(TResult) == typeof(long))
                    {
                        return (TResult)Convert.ChangeType(translatedValue, typeof(TResult));
                    }
                    else if (typeof(TResult) == typeof(string[]))
                    {
                        var tmpText = (TResult)Convert.ChangeType(translatedValue.ToString().Split(separator), typeof(TResult));
                        return (TResult)Convert.ChangeType(tmpText, typeof(TResult));
                    }
                    else
                    {
                        return (TResult)Convert.ChangeType(translatedValue, typeof(TResult));
                    }
                }
            }

            return default(TResult);
        }
    }
}
