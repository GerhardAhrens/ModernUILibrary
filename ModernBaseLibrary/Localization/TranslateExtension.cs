﻿//-----------------------------------------------------------------------
// <copyright file="TranslateExtension.cs" company="Lifeprojects.de">
//     Class: TranslateExtension
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>19.01.2023</date>
//
// <summary>
// The Translate Markup extension returns a binding to a TranslationData that provides a translated resource of the specified key
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Localization
{
    using System;
    using System.Windows.Data;
    using System.Windows.Markup;

    public class TranslateExtension : MarkupExtension
    {
        private string _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateExtension"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public TranslateExtension(string key)
        {
            this._key = key;
        }

        [ConstructorArgument("key")]
        public string Key
        {
            get { return this._key; }
            set { this._key = value;}
        }

        /// <summary>
        /// See <see cref="MarkupExtension.ProvideValue" />
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding("Value")
                  {
                      Source = new TranslationData(this._key)
                  };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
