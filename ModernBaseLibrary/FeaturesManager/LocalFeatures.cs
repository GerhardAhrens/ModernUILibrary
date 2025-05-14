/*
 * <copyright file="FeaturesManager.cs" company="Lifeprojects.de">
 *     Class: FeaturesManager
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>16.02.2023 20:12:06</date>
 * <Project>EasyPrototypingNET</Project>
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

namespace ModernBaseLibrary.Features
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public sealed class LocalFeatures : DisposableCoreBase
    {
        private List<FeaturesContent> features;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalFeatures"/> class.
        /// </summary>
        public LocalFeatures()
        {
            this.features = new List<FeaturesContent>();
        }

        ~LocalFeatures()
        {
            this.features?.Clear();
        }

        public string Filename
        {
            get
            {
                string featuresPath = this.CurrentSettingsPath();
                string featuresName = this.ApplicationName();
                string featuresFile = Path.Combine(featuresPath, $"{featuresName}.features");
                return featuresFile;
            }
        }

        public FeaturesResult Get(Guid key)
        {
            FeaturesResult result = null;
            if (this.Exists(key) == true)
            {
                int keyIndex = this.features.FindIndex(x => x.Key.Equals(key));
                if (keyIndex >= 0)
                {
                    FeaturesContent content = this.features[keyIndex];
                    result = new FeaturesResult(content.Key, content.Description);
                    return result;
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }

        public bool Exists(Guid key)
        {
            lock (this.features)
            {
                return this.features.FindIndex(x => x.Key.Equals(key)) != -1;

            }
        }

        public int Count()
        {
            int count = -1;
            if (this.features != null)
            {
                count = this.features.Count;
            }

            return count;
        }

        public bool IsExitFeatures()
        {
            if (File.Exists(this.Filename) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddOrSet(Guid key, string description)
        {
            if (this.Exists(key) == true)
            {
                int keyIndex = this.features.FindIndex(x => x.Key.Equals(key));
                this.features[keyIndex].Description = description;
            }
            else
            {
                this.features.Add(new FeaturesContent() { Key = key, Description = description });
            }
        }

        public void Save()
        {
            try
            {
                SerializeHelper<List<FeaturesContent>>.Serialize(this.features, SerializeFormatter.Xml, this.Filename);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void Load()
        {
            try
            {
                if (this.features != null)
                {
                    this.features.Clear();
                    this.features = SerializeHelper<List<FeaturesContent>>.DeSerialize(SerializeFormatter.Xml, this.Filename);
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        private string CurrentSettingsPath()
        {
            string settingsPath = string.Empty;

            string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            settingsPath = $"{rootPath}\\{this.ApplicationName()}\\Settings";

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
            string result = string.Empty;

            Assembly assm = Assembly.GetEntryAssembly();
            result = assm.GetName().Name;
            return result;
        }
    }
}
