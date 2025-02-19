namespace ModernBaseLibrary.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Windows;

    /// <summary>
    /// 
    /// </summary>
    public class ResxTranslationProvider : ITranslationProvider
    {
        private readonly ResourceManager _resourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResxTranslationProvider"/> class.
        /// </summary>
        /// <param name="baseName">Name of the base.</param>
        /// <param name="assembly">The assembly.</param>
        public ResxTranslationProvider(string baseName, Assembly assembly)
        {
            this._resourceManager = new ResourceManager(baseName, assembly);
        }

        public bool Exist()
        {
            return default;
        }

        /// <summary>
        /// See <see cref="ITranslationProvider.Translate" />
        /// </summary>
        public object Translate(string key)
        {
            try
            {
                return this._resourceManager?.GetString(key);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                return $"#{key}#";
            }
        }

        /// <summary>
        /// See <see cref="ITranslationProvider.AvailableLanguages" />
        /// </summary>
        public IEnumerable<CultureInfo> Languages
        {
            get
            {
                yield return new CultureInfo("de");
                yield return new CultureInfo("en");
            }
        }
    }
}
