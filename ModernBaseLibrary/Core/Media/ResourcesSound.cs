/*
 * <copyright file="ResourcesSound.cs" company="Lifeprojects.de">
 *     Class: ResourcesSound
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>26.01.2023 19:54:51</date>
 * <Project>CurrentProject</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Core.Media
{
    using System;
    using System.IO;
    using System.Media;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public class ResourcesSound
    {
        private readonly string name = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesSound"/> class.
        /// </summary>
        public ResourcesSound(string name)
        {
            this.name = name;
        }

        public static ResourcesSound SoundCapture
        {
            get
            {
                return new ResourcesSound("SoundCapture");
            }
        }

        public static ResourcesSound SoundFailure
        {
            get
            {
                return new ResourcesSound("SoundFailure");
            }
        }

        public static ResourcesSound SoundSuccess
        {
            get
            {
                return new ResourcesSound("SoundSuccess");
            }
        }

        public static ResourcesSound AccessAllowedTone
        {
            get
            {
                return new ResourcesSound("AccessAllowedTone");
            }
        }

        public static ResourcesSound ArabianMysteryNotification
        {
            get
            {
                return new ResourcesSound("ArabianMysteryNotification");
            }
        }

        public static ResourcesSound ArcadeMagicNotification
        {
            get
            {
                return new ResourcesSound("ArcadeMagicNotification");
            }
        }

        public static ResourcesSound BellNotification
        {
            get
            {
                return new ResourcesSound("BellNotification");
            }
        }

        public static ResourcesSound CashMachineKeyPress
        {
            get
            {
                return new ResourcesSound("CashMachineKeyPress");
            }
        }

        public static ResourcesSound ClearAnnounceTones
        {
            get
            {
                return new ResourcesSound("ClearAnnounceTones");
            }
        }

        public static ResourcesSound ConfirmationTone
        {
            get
            {
                return new ResourcesSound("ConfirmationTone");
            }
        }

        public static ResourcesSound GuitarNotificationAlert
        {
            get
            {
                return new ResourcesSound("GuitarNotificationAlert");
            }
        }

        public static ResourcesSound HappyBellsNotification
        {
            get
            {
                return new ResourcesSound("HappyBellsNotification");
            }
        }

        public static ResourcesSound MagicMarimba
        {
            get
            {
                return new ResourcesSound("MagicMarimba");
            }
        }

        public static ResourcesSound MelodicalFluteMusicNotification
        {
            get
            {
                return new ResourcesSound("MelodicalFluteMusicNotification");
            }
        }

        public static ResourcesSound MusicalAlertNotification
        {
            get
            {
                return new ResourcesSound("MusicalAlertNotification");
            }
        }

        public static ResourcesSound MusicalReveal
        {
            get
            {
                return new ResourcesSound("MusicalReveal");
            }
        }

        public static ResourcesSound OrchestralEmergencyAlarm
        {
            get
            {
                return new ResourcesSound("OrchestralEmergencyAlarm");
            }
        }

        public static ResourcesSound PositiveNotification
        {
            get
            {
                return new ResourcesSound("PositiveNotification");
            }
        }

        public static ResourcesSound SoftwareInterfaceBack
        {
            get
            {
                return new ResourcesSound("SoftwareInterfaceBack");
            }
        }

        public static ResourcesSound SuccessTone
        {
            get
            {
                return new ResourcesSound("SuccessTone");
            }
        }

        public static ResourcesSound UpliftingFluteNotification
        {
            get
            {
                return new ResourcesSound("UpliftingFluteNotification");
            }
        }

        public bool HasSound
        {
            get
            {
                try
                {
                    AssemblyResource.AssemblyResourceLocation = AssemblyLocation.ExecutingAssembly;
                    bool soundFound = AssemblyResource.HasResource($"{this.name}.wav");
                    return soundFound;
                }
                catch
                {
                    return false;
                }
            }
        }

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
                    AssemblyResource.AssemblyResourceLocation = AssemblyLocation.ExecutingAssembly;
                    Stream resourceStream = AssemblyResource.GetResourceStream($"Sound.{this.name}.wav");
                    SoundPlayer player = new SoundPlayer(resourceStream);
                    player.Play();
                }
            }
            catch
            {
                Console.Beep();
            }
        }
    }
}
