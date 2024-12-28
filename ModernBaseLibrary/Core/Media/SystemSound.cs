//-----------------------------------------------------------------------
// <copyright file="SystemSound.cs" company="Lifeprojects.de">
//     Class: SystemSound
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>10.07.2017</date>
//
// <summary>Definition of SystemSound Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Media
{
    using System;
    using System.IO;
    using System.Media;
    using System.Runtime.Versioning;

    using Microsoft.Win32;

    [SupportedOSPlatform("windows")]
    public sealed class SystemSound
    {
        private readonly string category = ".Default";
        private readonly string name = string.Empty;

        private SystemSound(string name)
        {
            this.name = name;
        }

        private SystemSound(string name, string category)
        {
            this.name = name;
            this.category = category;
        }

        public static SystemSound Default
        {
            get
            {
                return new SystemSound(".Default");
            }
        }

        public static SystemSound Beep
        {
            get
            {
                return new SystemSound("Beep");
            }
        }

        public static SystemSound AppFault
        {
            get
            {
                return new SystemSound("AppGPFault");
            }
        }

        public static SystemSound SystemStart
        {
            get
            {
                return new SystemSound("SystemStart");
            }
        }

        public static SystemSound SystemExit
        {
            get
            {
                return new SystemSound("SystemExit");
            }
        }

        public static SystemSound UserLogon
        {
            get
            {
                return new SystemSound("WindowsLogon");
            }
        }

        public static SystemSound UserLogoff
        {
            get
            {
                return new SystemSound("WindowsLogoff");
            }
        }

        public static SystemSound AppOpen
        {
            get
            {
                return new SystemSound("Open");
            }
        }

        public static SystemSound AppClose
        {
            get
            {
                return new SystemSound("Close");
            }
        }

        public static SystemSound WindowMaximise
        {
            get
            {
                return new SystemSound("Maximize");
            }
        }

        public static SystemSound WindowMinimise
        {
            get
            {
                return new SystemSound("Minimize");
            }
        }

        public static SystemSound WindowRestoreUp
        {
            get
            {
                return new SystemSound("RestoreUp");
            }
        }

        public static SystemSound WindowRestoreDown
        {
            get
            {
                return new SystemSound("RestoreDown");
            }
        }

        public static SystemSound MenuCommand
        {
            get
            {
                return new SystemSound("MenuCommand");
            }
        }

        public static SystemSound MenuPopup
        {
            get
            {
                return new SystemSound("MenuPopup");
            }
        }

        public static SystemSound PrintComplete
        {
            get
            {
                return new SystemSound("PrintComplete");
            }
        }

        public static SystemSound LowBattery
        {
            get
            {
                return new SystemSound("LowBatteryAlarm");
            }
        }

        public static SystemSound CriticalBattery
        {
            get
            {
                return new SystemSound("CriticalBatteryAlarm");
            }
        }

        public static SystemSound DeviceConnect
        {
            get
            {
                return new SystemSound("DeviceConnect");
            }
        }

        public static SystemSound DeviceDisconnect
        {
            get
            {
                return new SystemSound("DeviceDisconnect");
            }
        }

        public static SystemSound DeviceFail
        {
            get
            {
                return new SystemSound("DeviceFail");
            }
        }

        public static SystemSound InternetAlert
        {
            get
            {
                return new SystemSound("InternetAlert");
            }
        }

        public static SystemSound Critical
        {
            get
            {
                return new SystemSound("SystemHand");
            }
        }

        public static SystemSound Exclamation
        {
            get
            {
                return new SystemSound("SystemExclamation");
            }
        }

        public static SystemSound Notification
        {
            get
            {
                return new SystemSound("SystemNotification");
            }
        }

        public static SystemSound Question
        {
            get
            {
                return new SystemSound("SystemQuestion");
            }
        }

        public static SystemSound Mail
        {
            get
            {
                return new SystemSound("MailBeep");
            }
        }

        public static SystemSound BlockedPopup
        {
            get
            {
                return new SystemSound("BlockedPopup", "Explorer");
            }
        }

        public static SystemSound EmptyRecycleBin
        {
            get
            {
                return new SystemSound("EmptyRecycleBin", "Explorer");
            }
        }

        public static SystemSound Navigating
        {
            get
            {
                return new SystemSound("Navigating", "Explorer");
            }
        }

        /// <summary>
        /// Gets a value indicating whether a sound has been set for this event.
        /// </summary>
        public bool HasSound
        {
            get
            {
                try
                {
                    RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"AppEvents\Schemes\Apps\" + this.category + @"\" + this.name + @"\.Current");
                    if (regKey != null)
                    {
                        object value = regKey.GetValue(string.Empty);
                        if (value is string && (value as string) != string.Empty)
                        {
                            string path = value.ToString();
                            if (Path.IsPathRooted(path) == false)
                            {
                                path = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.System)) + "\\Media\\" + path;
                            }

                            return File.Exists(path);
                        }
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Find a custom sound that is not available through this class' static members.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="name"></param>
        /// <returns>SystemSound object that you can call <c>Play()</c> on</returns>
        public static SystemSound Find(string category, string name)
        {
            if (category.Contains("\\"))
            {
                throw new ArgumentException("category argument must not contain '\\' character");
            }

            if (name.Contains("\\"))
            {
                throw new ArgumentException("name argument must not contain '\\' character");
            }

            return new SystemSound(name, category);
        }

        /// <summary>
        /// Play the sound associated with this event, if any.
        /// In case of failure, a default PC speaker beep is played.
        /// </summary>
        public void Play()
        {
            try
            {
                if (this.name == "Beep")
                {
                    Console.Beep();
                }
                else
                {
                    RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"AppEvents\Schemes\Apps\" + this.category + @"\" + this.name + @"\.Current");
                    if (regKey != null)
                    {
                        object value = regKey.GetValue(string.Empty);
                        if (value is string && (value as string) != string.Empty)
                        {
                            string path = value.ToString();
                            if (Path.IsPathRooted(path) == false)
                            {
                                path = Path.GetDirectoryName(Environment.GetFolderPath(Environment.SpecialFolder.System)) + "\\Media\\" + path;
                            }

                            SoundPlayer sp = new SoundPlayer(path);
                            sp.Play();
                        }
                    }
                }
            }
            catch
            {
                Console.Beep();
            }
        }
    }
}