//-----------------------------------------------------------------------
// <copyright file="ResourcesText.cs" company="Lifeprojects.de">
//     Class: ResourcesText
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <Framework>4.8</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.01.2023 09:01:02</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Localization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Windows;

    [SupportedOSPlatform("windows")]
    public class ResourcesText
    {
        private const string DICTIONARYNAME = "Resources\\Localization\\TextString.xaml";
        private static ResourceDictionary resourceDict = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesText"/> class.
        /// </summary>
        static ResourcesText()
        {
            resourceDict = Application.Current.Resources.MergedDictionaries.Where(md => md.Source.OriginalString.Equals(DICTIONARYNAME)).FirstOrDefault();
        }

        public ResourcesText()
        {
        }

        public static int Count { get { return resourceDict.Count; } }

        public static IEnumerable<string> Keys { get { return resourceDict.Keys.Cast<string>().Select(s => s); } }

        public static Dictionary<string,string> KeyValue {
            get
            {
                return resourceDict.Keys.Cast<string>().ToDictionary
                    (x => x,                             // Key selector
                     x => (string)resourceDict[x]        // Value selector
                     );
            } 
        }

        public static string Get(string key)
        {
            key.IsArgumentNullOrEmpty(nameof(key));

            if (resourceDict == null)
            {
                return $"ResourceDictionary 'TextString.xaml' nicht gefunden.";
            }

            bool keyFound = resourceDict.Cast<DictionaryEntry>().Any(f => f.Key.ToString().ToLower() == key.ToLower());
            if (keyFound == false)
            {
                return $"ResourceKey '{key}' nicht gefunden.";
            }

            string value = resourceDict.Cast<DictionaryEntry>().FirstOrDefault(f => f.Key.ToString().ToLower() == key.ToLower()).Value.ToString();

            return value;
        }
    }
}
