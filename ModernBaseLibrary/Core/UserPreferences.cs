//-----------------------------------------------------------------------
// <copyright file="UserPreferences.cs" company="Lifeprojects.de">
//     Class: UserPreferences
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>07.03.2017</date>
//
// <summary>
// Lesen und schreiben der Bildschirmposition
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Xml.Linq;

    using ModernBaseLibrary.CoreBase;

    [SupportedOSPlatform("windows")]
    public class UserPreferences : DisposableCoreBase
    {
        public UserPreferences(SettingsLocation settingsLocation = SettingsLocation.ProgramData)
        {
            this.SettingsLocation = settingsLocation;
            this.FilePath = this.Filename;
        }

        public UserPreferences(Window @this, UserOutputTyp userTyp = UserOutputTyp.Windows)
        {
            this.SettingsLocation = SettingsLocation.ProgramData;
            this.FilePath = this.Filename;

            this.CurrentWindow = @this;
            if (userTyp == UserOutputTyp.Windows)
            {
                this.SizeToFit();
                this.MoveIntoView();
            }
            else if (userTyp == UserOutputTyp.Console)
            {
                Console.SetWindowPosition(0, 0);
                Console.SetWindowSize(0, 0);

                Console.WindowHeight = 0;
                Console.WindowWidth = 0;
                Console.WindowTop = 0;
                Console.WindowLeft = 0;
            }
            else
            {
                this.SizeToFit();
                this.MoveIntoView();
            }
        }

        public UserPreferences(Window @this, string assemblyPath, UserOutputTyp userTyp = UserOutputTyp.Windows)
        {
            if (string.IsNullOrEmpty(assemblyPath) == true)
            {
                this.SettingsLocation = SettingsLocation.ProgramData;
                this.FilePath = this.Filename;
            }
            else
            {
                this.SettingsLocation = SettingsLocation.CustomLocation;
                this.FilePath = assemblyPath;
            }

            this.CurrentWindow = @this;
            if (userTyp == UserOutputTyp.Windows)
            {
                this.SizeToFit();
                this.MoveIntoView();
            }
            else if (userTyp == UserOutputTyp.Console)
            {
                Console.SetWindowPosition(0, 0);
                Console.SetWindowSize(0, 0);

                Console.WindowHeight = 0;
                Console.WindowWidth = 0;
                Console.WindowTop = 0;
                Console.WindowLeft = 0;
            }
            else
            {
                this.SizeToFit();
                this.MoveIntoView();
            }
        }

        public string Filename
        {
            get
            {
                string settingsPath = this.CurrentSettingsPath();
                string settingsName = this.UserSettingsName();
                string settingsFile = Path.Combine(settingsPath, settingsName);
                return settingsFile;
            }
        }

        public string Pathname
        {
            get
            {
                return $"{this.CurrentSettingsPath()}\\";
            }
        }

        private SettingsLocation SettingsLocation { get; set; }

        private string FilePath { get; }

        private Window CurrentWindow { get; set; }

        /// <summary>
        /// If the saved window dimensions are larger than the current screen shrink the
        /// window to fit.
        /// </summary>
        public void SizeToFit()
        {
            if (this.CurrentWindow.Height > SystemParameters.VirtualScreenHeight)
            {
                this.CurrentWindow.Height = SystemParameters.VirtualScreenHeight;
            }

            if (this.CurrentWindow.Width > SystemParameters.VirtualScreenWidth)
            {
                this.CurrentWindow.Width = SystemParameters.VirtualScreenWidth;
            }
        }

        /// <summary>
        /// If the window is more than half off of the screen move it up and to the left 
        /// so half the height and half the width are visible.
        /// </summary>
        public void MoveIntoView()
        {
            if (((this.CurrentWindow.Top + this.CurrentWindow.Height) / 2) > SystemParameters.VirtualScreenHeight)
            {
                this.CurrentWindow.Top = SystemParameters.VirtualScreenHeight - this.CurrentWindow.Height;
            }

            if (((this.CurrentWindow.Left + this.CurrentWindow.Width) / 2) > SystemParameters.VirtualScreenWidth)
            {
                this.CurrentWindow.Left = SystemParameters.VirtualScreenWidth - this.CurrentWindow.Width;
            }

            if (this.CurrentWindow.Top < 0)
            {
                this.CurrentWindow.Top = 50;
            }

            if (this.CurrentWindow.Left < 0)
            {
                this.CurrentWindow.Left = 50;
            }
        }

        public string ProgramDataPath()
        {
            string result = string.Empty;

            string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            result = $"{rootPath}\\{this.ApplicationName()}\\";

            return result;
        }


        /// <summary>
        /// Speichern der aktuellen Applikation Window Position in einem JSON Datei
        /// </summary>
        /// <param name="userSave">True - Position wird gespeichert</param>
        public void Save(bool userSave = false)
        {
            if (userSave == true)
            {
                if (this.CurrentWindow.WindowState != System.Windows.WindowState.Minimized)
                {
                    UserScreenPosition pos = new UserScreenPosition(this.CurrentWindow);
                    SerializeHelper<UserScreenPosition>.Serialize(pos, SerializeFormatter.Json, this.FilePath);
                }
            }
        }

        /// <summary>
        /// Laden und wiederherstellen der zuletzt gespeicherten Applikation Window Position aus einer JSON Datei
        /// </summary>
        /// <param name="userLoad">True - Zuletzt gespeicherte Poistion wird gelesen, die letzte Windowposition wird wiederhergestellt</param>
        public void Load(bool userLoad = false)
        {
            UserScreenPosition screenPos = null;

            if (userLoad == false)
            {
                return;
            }

            try
            {
                if (File.Exists(this.FilePath) == false)
                {
                    return;
                }

                System.Windows.Forms.Screen[] screens = System.Windows.Forms.Screen.AllScreens;
                screenPos = SerializeHelper<UserScreenPosition>.DeSerialize(SerializeFormatter.Json, this.FilePath);

                if (screens.Length == 0)
                {
                    return;
                }

                if (screenPos != null)
                {
                    this.CurrentWindow.Height = screenPos.Height;
                    if (screenPos.Height < 400)
                    {
                        this.CurrentWindow.Height = 400;
                    }
                    else
                    {
                        this.CurrentWindow.Height = screenPos.Height;
                    }

                    if (screenPos.Width < 700)
                    {
                        this.CurrentWindow.Width = 700;
                    }
                    else
                    {
                        this.CurrentWindow.Width = screenPos.Width;
                    }

                    this.CurrentWindow.Top = screenPos.Top;

                    if (screens.Length == 1)
                    {
                        if (screens[0].WorkingArea.Width <= screenPos.Left)
                        {
                            this.CurrentWindow.Left = 100;
                        }
                        else
                        {
                            this.CurrentWindow.Left = screenPos.Left;
                        }
                    }
                    else
                    {
                        this.CurrentWindow.Left = screenPos.Left;
                    }

                    this.CurrentWindow.WindowState = screenPos.WindowState;
                }
            }
            catch (System.Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        private string CurrentSettingsPath()
        {
            string settingsPath = string.Empty;

            if (this.SettingsLocation == SettingsLocation.ProgramData)
            {
                string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                settingsPath = $"{rootPath}\\{this.ApplicationName()}\\Settings";
            }
            else if (this.SettingsLocation == SettingsLocation.AssemblyLocation)
            {
                string rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                settingsPath = $"{rootPath}\\{this.ApplicationName()}\\Settings";
            }

            if (string.IsNullOrEmpty(settingsPath) == false)
            {
                try
                {
                    if (Directory.Exists(settingsPath) == false)
                    {
                        Directory.CreateDirectory(settingsPath);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return settingsPath;
        }

        private string ApplicationName()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }

        private string UserSettingsName()
        {
            return $"{Environment.UserName}-LastWinPos.Setting";
        }
    }

    [Serializable]
    public class UserScreenPosition 
    {
        public UserScreenPosition()
        {
        }
        public UserScreenPosition(Window @this)
        {
            this.WindowState = @this.WindowState;
            this.Top = @this.Top;
            this.Left = @this.Left;
            this.Width = @this.Width;
            this.Height = @this.Height;
        }

        public WindowState WindowState { get; set; }

        public double Top { get; set; }

        public double Left { get;  set; }

        public double Height { get; set; }

        public double Width { get; set; }
    }

    public enum UserOutputTyp : int
    {
        [Description("Keine Ausgabe")]
        None = 0,
        Windows = 1,
        Console = 2
    }
}
