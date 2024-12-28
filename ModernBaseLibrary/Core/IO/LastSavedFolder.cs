//-----------------------------------------------------------------------
// <copyright file="LastSavedFolder.cs" company="Lifeprojects.de">
//     Class: LastSavedFolder
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>07.12.2020</date>
//
// <summary>
// Die Klasse speichert zur Laufzeit das aktuelle verwendetet 
// Verzeichnis und bietet das bei erneuten Verwendung wieder an.
// Pro Typ ist ein Verzeichnis möglich.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Xml;

    using ModernBaseLibrary.CoreBase;
    using ModernBaseLibrary.Extension;
    using ModernBaseLibrary.XML;

    [SupportedOSPlatform("windows")]
    public sealed class LastSavedFolder
    {
        private const string XmlRootPath = "/configuration/LastFolder";

        private static readonly ConcurrentDictionary<string, string> SavedFolder = null;

        static LastSavedFolder()
        {
            if (SavedFolder == null)
            {
                SavedFolder = new ConcurrentDictionary<string, string>();
            }

            Load();
        }

        public static SettingsLocation SettingsLocation { get; set; } = SettingsLocation.ProgramData;

        public static int Count
        {
            get
            {
                return SavedFolder.Count;
            }
        }

        public static string Filename
        {
            get
            {
                string settingsPath = CurrentSettingsPath();
                string settingsName = UserSettingsName();
                string settingsFile = Path.Combine(settingsPath, settingsName);
                return settingsFile;
            }
        }

        public static string Get(string typ)
        {
            string result = string.Empty;

            if (SavedFolder.ContainsKey(typ) == true)
            {
                result = SavedFolder[typ];
                if (string.IsNullOrEmpty(result) == true)
                {
                    result = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
            }
            else
            {
                result = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            return result;
        }

        public static string Get(Type typ)
        {
            string result = string.Empty;
            string typName = typ.GetFriendlyName();

            if (SavedFolder.ContainsKey(typName) == true)
            {
                result = SavedFolder[typName];
                if (string.IsNullOrEmpty(result) == true)
                {
                    result = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
            }
            else
            {
                result = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            return result;
        }

        public static string GetOrSet(string typ, string folder = "")
        {
            string result = string.Empty;

            if (SavedFolder.ContainsKey(typ) == true)
            {
                result = SavedFolder[typ] = folder;
            }
            else
            {
                SavedFolder.TryAdd(typ, folder);
                result = SavedFolder[typ] = folder;
            }

            return result;
        }

        public static string GetOrSet(Type typ, string folder = "")
        {
            string result = string.Empty;
            string typName = typ.GetFriendlyName();

            if (SavedFolder.ContainsKey(typName) == true)
            {
                result = SavedFolder[typName] = folder;
            }
            else
            {
                if (SavedFolder.TryAdd(typName, folder) == true)
                {
                    result = SavedFolder[typName] = folder;
                }
                else
                {
                    result = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
            }

            return result;
        }

        public static Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> export = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in SavedFolder)
            {
                export.Add(item.Key, item.Value);
            }

            return export;
        }

        public static Dictionary<string, string> ToDictionary(Func<KeyValuePair<string, string>, bool> predicate)
        {
            Dictionary<string, string> export = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in SavedFolder)
            {
                export.Add(item.Key, item.Value);
            }

            return SavedFolder.Where(predicate).ToDictionary(x => x.Key, x => x.Value);
        }

        public static List<string> GetFolders()
        {
            List<string> folders = new List<string>();
            IEnumerable<IGrouping<string,KeyValuePair<string,string>>> groupFolders = SavedFolder.GroupBy(g => g.Value);
            foreach (IGrouping<string, KeyValuePair<string, string>> folder in groupFolders)
            {
                folders.Add(folder.Key);
            }

            return folders;
        }

        public static void Remove(string typ)
        {
            if (SavedFolder.Count > 0)
            {
                if (SavedFolder.ContainsKey(typ) == true)
                {
                    string outValue;
                    bool isRemoved = SavedFolder.TryRemove(typ, out outValue);
                    if (isRemoved == true)
                    {
                        Save();
                    }
                }
            }
        }

        public static void Remove(Type typ)
        {
            if (SavedFolder.Count > 0)
            {
                string typName = typ.GetFriendlyName();
                if (SavedFolder.ContainsKey(typName) == true)
                {
                    string outValue;
                    bool isRemoved = SavedFolder.TryRemove(typName, out outValue);
                    if (isRemoved == true)
                    {
                        Save();
                    }
                }
            }
        }

        public static void Load()
        {
            string settingsPath = CurrentSettingsPath();
            string settingsName = UserSettingsName();
            string settingsFile = Path.Combine(settingsPath, settingsName);

            if (File.Exists(settingsFile) == true)
            {
                string xmlResult = File.ReadAllText(settingsFile);

                SavedFolder.Clear();

                using (DynamicXml xmlRead = new DynamicXml(xmlResult))
                {
                    XmlNodeList lastFolders = xmlRead.XmlDocument.SelectNodes(XmlRootPath);
                    XmlNode childNotes = lastFolders[0];
                    if (((XmlElement)childNotes).HasAttributes == true)
                    {
                        foreach (XmlNode item in ((XmlElement)childNotes).Attributes)
                        {
                            string typName = item.Name;
                            string typValue = item.Value;
                        }
                    }

                    if (childNotes.HasChildNodes == true)
                    {
                        foreach (XmlNode item in childNotes.ChildNodes)
                        {
                            if (SavedFolder.ContainsKey(item.Name) == false)
                            {
                                SavedFolder.TryAdd(item.Name, item.InnerText);
                            }
                        }
                    }
                }
            }
        }

        public static void Save()
        {
            string xmlResult = string.Empty;

            string settingsPath = CurrentSettingsPath();
            string settingsName = UserSettingsName();
            string settingsFile = Path.Combine(settingsPath, settingsName);

            if (Directory.Exists(settingsPath) == false)
            {
                Directory.CreateDirectory(settingsPath);
            }

            using (DynamicXml xmlWrite = new DynamicXml())
            {
                XmlNode application = xmlWrite.CreateNew(XmlRootPath);
                xmlWrite.Write($"{XmlRootPath }/@Typ", "String");
                foreach (KeyValuePair<string,string> item in SavedFolder)
                {
                    xmlWrite.Write(application, item.Key, item.Value);
                }

                xmlResult = xmlWrite.Xml;
            }

            File.WriteAllText(settingsFile, xmlResult);
        }

        public static bool Exist()
        {
            bool result = false;

            try
            {
                if (File.Exists(Filename) == true)
                {
                    result = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        private static string CurrentSettingsPath()
        {
            string result = string.Empty;

            if (SettingsLocation == SettingsLocation.ProgramData)
            {
                string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                result = $"{rootPath}\\{ApplicationName()}\\Settings";
            }
            else
            {
                result = $"{CurrentAssemblyPath()}\\Settings";
            }

            return result;
        }

        private static string ApplicationName()
        {
            string result = string.Empty;

            if (UnitTestDetector.IsInUnitTest == true)
            {
                result = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
            else
            {
                Assembly assm = Assembly.GetEntryAssembly();
                result = assm.GetName().Name;
            }


            return result;
        }

        private static string CurrentAssemblyPath()
        {
            string result = string.Empty;

            if (UnitTestDetector.IsInUnitTest == true)
            {
                result = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
            else
            {
                Assembly assm = Assembly.GetEntryAssembly();
                result = Path.GetDirectoryName(assm.Location);
            }

            return result;
        }

        private static string UserSettingsName()
        {
            string result = string.Empty;
            string username = Environment.UserName;

            result = $"LastFolders_{username}.Settings";

            return result;
        }

        private static class UnitTestDetector
        {
            static UnitTestDetector()
            {
                string testAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
                UnitTestDetector.IsInUnitTest = AppDomain.CurrentDomain.GetAssemblies()
                    .Any(a => a.FullName.StartsWith(testAssemblyName));
            }

            public static bool IsInUnitTest { get; private set; }
        }
    }
}
