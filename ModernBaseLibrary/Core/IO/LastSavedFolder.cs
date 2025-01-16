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
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Text.Json;
    using System.Xml;
    using Microsoft.VisualBasic.ApplicationServices;

    using ModernBaseLibrary.CoreBase;
    using ModernBaseLibrary.Extension;
    using ModernBaseLibrary.XML;
    using static System.Windows.Forms.Design.AxImporter;

    [SupportedOSPlatform("windows")]
    public sealed class LastSavedFolder
    {
        private const string XmlRootPath = "/configuration/LastFolder";

        private static List<LastTagetFolder> SavedFolder = null;

        static LastSavedFolder()
        {
            if (SavedFolder == null)
            {
                SavedFolder = new List<LastTagetFolder>();
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

            if (SavedFolder.Any(w =>w.Typ == typ) == true)
            {
                result = SavedFolder.SingleOrDefault(w => w.Typ == typ).Folder;
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

            if (SavedFolder.Any(w => w.Typ == typName) == true)
            {
                result = SavedFolder.SingleOrDefault(w => w.Folder == typName).Folder;
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
            if (SavedFolder.Any(w => w.Typ != null && w.Typ == typ) == true)
            {
                LastTagetFolder lastFolder = SavedFolder.SingleOrDefault(w => w.Typ == typ);
                int pos = SavedFolder.IndexOf(lastFolder);
                result = SavedFolder[pos].Folder = folder;
            }
            else
            {
                LastTagetFolder lastFolder = new LastTagetFolder();
                lastFolder.Typ =typ;
                lastFolder.Folder = folder;
                lastFolder.User = $"{Environment.UserDomainName}\\{Environment.UserName}";

                SavedFolder.Add(lastFolder);
                result = lastFolder.Folder;
            }

            return result;
        }

        public static string GetOrSet(Type typ, string folder = "")
        {
            string result = string.Empty;
            string typName = typ.GetFriendlyName();

            if (SavedFolder.Any(w => w.Typ != null && w.Typ == typName) == true)
            {
                LastTagetFolder lastFolder = SavedFolder.SingleOrDefault(w => w.Typ == typName);
                int pos = SavedFolder.IndexOf(lastFolder);
                result = SavedFolder[pos].Folder = folder;
            }
            else
            {
                LastTagetFolder lastFolder = new LastTagetFolder();
                lastFolder.Typ = typName;
                lastFolder.Folder = folder;
                lastFolder.User = $"{Environment.UserDomainName}\\{Environment.UserName}";

                SavedFolder.Add(lastFolder);
                result = lastFolder.Folder;
            }

            return result;
        }

        public static Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> export = new Dictionary<string, string>();
            foreach (LastTagetFolder item in SavedFolder)
            {
                export.Add(item.Typ, item.Folder);
            }

            return export;
        }

        public static List<string> GetFolders()
        {
            return SavedFolder.Select(s => s.Folder).ToList();
        }

        public static void Remove(string typ)
        {
            if (SavedFolder.Count > 0)
            {
                if (SavedFolder.Any(w => w.Typ == typ) == true)
                {
                    LastTagetFolder lastFolder = SavedFolder.SingleOrDefault(w => w.Typ == typ);
                    bool isRemoved = SavedFolder.Remove(lastFolder);
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
                if (SavedFolder.Any(w => w.Typ == typName) == true)
                {
                    LastTagetFolder lastFolder = SavedFolder.SingleOrDefault(w => w.Typ == typName);
                    bool isRemoved = SavedFolder.Remove(lastFolder);
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
                string jsonText = File.ReadAllText(settingsFile);
                SavedFolder = JsonSerializer.Deserialize<List<LastTagetFolder>>(jsonText);
            }
        }

        public static void Save()
        {
            string settingsPath = CurrentSettingsPath();
            string settingsName = UserSettingsName();
            string settingsFile = Path.Combine(settingsPath, settingsName);

            if (Directory.Exists(settingsPath) == false)
            {
                Directory.CreateDirectory(settingsPath);
            }

            if (SavedFolder.Count > 0)
            {
                string jsonText = JsonSerializer.Serialize(SavedFolder);
                File.WriteAllText(settingsFile, jsonText);
            }
        }

        public static void Clear()
        {
            string settingsPath = CurrentSettingsPath();
            string settingsName = UserSettingsName();
            string settingsFile = Path.Combine(settingsPath, settingsName);

            if (Directory.Exists(settingsPath) == false)
            {
                Directory.CreateDirectory(settingsPath);
            }

            if (SavedFolder.Count > 0)
            {
                SavedFolder.Clear();
                File.Delete(settingsFile);
            }
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

    internal enum LastFolderModus : int
    {
        [Description("Keine Funktion")]
        none = 0,
        [Description("Auswählen oder öffenen")]
        Open = 1,
        [Description("Speichern")]
        Save = 2
    }

    internal class LastTagetFolder
    {
        public LastFolderModus Modus { get; set; }
        public string Typ { get; set; }
        public string Folder { get; set; }
        public string User { get; set; }
    }
}
