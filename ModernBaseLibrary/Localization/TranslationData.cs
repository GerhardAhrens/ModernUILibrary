namespace ModernBaseLibrary.Localization
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    public class TranslationData : IWeakEventListener, INotifyPropertyChanged
    {
        private string _key;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationData"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public TranslationData( string key)
        {
            this._key = key;
            LanguageChangedEventManager.AddListener(TranslationManager.Instance, this);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="TranslationData"/> is reclaimed by garbage collection.
        /// </summary>
        ~TranslationData()
        {
            LanguageChangedEventManager.RemoveListener(TranslationManager.Instance, this);
        }

        public object Value
        {
            get
            {
                return TranslationManager.Instance.Translate(this._key);
            }
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(LanguageChangedEventManager))
            {
                this.OnLanguageChanged(sender, e);
                return true;
            }
            return false;
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            if( this.PropertyChanged != null )
            {
                this.PropertyChanged( this, new PropertyChangedEventArgs("Value"));
            }
        }

    }
}
