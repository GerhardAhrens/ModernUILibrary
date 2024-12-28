//-----------------------------------------------------------------------
// <copyright file="FileAssociation.cs" company="Lifeprojects.de">
//     Class: FileAssociation
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>21.12.2022 12:49:08</date>
//
// <summary>
// Umleiten einer Ausgabe über den TextWriter
// </summary>
// <Website>
// </Website>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Security.AccessControl;

    using Microsoft.Win32;

    [SupportedOSPlatform("windows")]
    public class FileAssociation : DisposableCoreBase
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        public FileAssociation(string fileExtension, string openWith, string executableName)
        {
            this.FileExtension = fileExtension;
            this.OpenWith = openWith;
            this.ExecutableName = executableName;
        }

        public string FileExtension { get; private set; }

        public string OpenWith { get; private set; }

        public string ExecutableName { get; private set; }

        public void CreateAssociation()
        {
            if (File.Exists(this.OpenWith) == false)
            {
                throw new FileNotFoundException($"Executable File {this.OpenWith} not found!");
            }

            try
            {
                using (RegistryKey User_Classes = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\", true))
                using (RegistryKey User_Ext = User_Classes.CreateSubKey($".{this.FileExtension}"))
                using (RegistryKey User_AutoFile = User_Classes.CreateSubKey($"{this.FileExtension}_auto_file"))
                using (RegistryKey User_AutoFile_Command = User_AutoFile.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command"))
                using (RegistryKey ApplicationAssociationToasts = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\ApplicationAssociationToasts\\", true))
                using (RegistryKey User_Classes_Applications = User_Classes.CreateSubKey("Applications"))
                using (RegistryKey User_Classes_Applications_Exe = User_Classes_Applications.CreateSubKey(ExecutableName))
                using (RegistryKey User_Application_Command = User_Classes_Applications_Exe.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command"))
                using (RegistryKey User_Explorer = Registry.CurrentUser.CreateSubKey($"Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.{this.FileExtension}"))
                using (RegistryKey User_Choice = User_Explorer.OpenSubKey("UserChoice"))
                {
                    User_Ext.SetValue("", $"{this.FileExtension}_auto_file", RegistryValueKind.String);
                    User_Classes.SetValue("", $"{this.FileExtension}_auto_file", RegistryValueKind.String);
                    User_Classes.CreateSubKey($"{this.FileExtension}_auto_file");
                    User_AutoFile_Command.SetValue("", $"\"{this.OpenWith}\"" + " \"%1\"");
                    ApplicationAssociationToasts.SetValue($"{this.FileExtension}_auto_file_.{this.FileExtension}", 0);
                    ApplicationAssociationToasts.SetValue(@"Applications\" + this.ExecutableName + "_.{this.FileExtension}", 0);
                    User_Application_Command.SetValue("", "\"" + this.OpenWith + "\"" + " \"%1\"");
                    User_Explorer.CreateSubKey("OpenWithList").SetValue("a", this.ExecutableName);
                    User_Explorer.CreateSubKey("OpenWithProgids").SetValue($"{this.FileExtension}_auto_file", "0");
                    if (User_Choice != null)
                    {
                        User_Explorer.DeleteSubKey("UserChoice");
                    }

                    User_Explorer.CreateSubKey("UserChoice").SetValue("ProgId", $"Applications\\{this.ExecutableName}");
                }

                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
            }
        }

        public bool ExtensionExist()
        {
            bool extKeyExists = false;
            bool progIDkeyExists = false;

            if (Registry.ClassesRoot.OpenSubKey("."+this.FileExtension) != null)
            {
                extKeyExists = true;

                if (string.IsNullOrEmpty(this.ExecutableName) == false)
                {
                    if (Registry.CurrentUser.OpenSubKey($"SOFTWARE\\Classes\\Applications\\{this.ExecutableName}") != null)
                    {
                        progIDkeyExists = true;
                    }
                }
            }

            if (extKeyExists && progIDkeyExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ExtensionDelete()
        {
            if (this.ExtensionExist() == true)
            {
                if (Registry.ClassesRoot.OpenSubKey("."+this.FileExtension, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl) != null)
                {
                    try
                    {
                        Registry.CurrentUser.DeleteSubKeyTree($"SOFTWARE\\Classes\\Applications\\{this.ExecutableName}");
                        Registry.ClassesRoot.DeleteSubKeyTree("."+this.FileExtension);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Failed to delete all keys used in the '{this.FileExtension}' file association, error: {ex.Message}");
                    }
                }
            }
            else
            {
                throw new Exception("One of your association keys don't exist, use the create method to get started...");
            }
        }
    }
}
