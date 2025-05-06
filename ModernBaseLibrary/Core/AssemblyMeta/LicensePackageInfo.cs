//-----------------------------------------------------------------------
// <copyright file="LicensePackageInfo.cs" company="Lifeprojects.de">
//     Class: LicensePackageInfo
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>06.05.2025 08:49:16</date>
//
// <summary>
// Klasse zur Verwaltung von NuGet-Paket Informationen
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.AssemblyMeta
{
    using System;
    using System.Collections.Generic;

    public class LicensePackageInfo
    {
        private List<LicensePackageItem> licensePackageSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="LicensePackageInfo"/> class.
        /// </summary>
        public LicensePackageInfo()
        {
            this.licensePackageSource = new List<LicensePackageItem>();
        }

        public int Count 
        { 
            get 
            { 
                return this.licensePackageSource.Count; 
            } 
        }

        public void Add(LicensePackageItem licensePackageItem)
        {
            if (this.licensePackageSource!= null && this.licensePackageSource.Contains(licensePackageItem) == false)
            {
                this.licensePackageSource.Add(licensePackageItem);
            }
        }

        public IEnumerable<LicensePackageItem> GetSource()
        {
            return this.licensePackageSource;
        }
    }

    public class LicensePackageItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LicensePackageInfo"/> class.
        /// </summary>
        public LicensePackageItem()
        {
        }

        public string Name { get; set; }

        public Version Version { get; set; }

        public DateTime VersionDate { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public override string ToString()
        {
            return $"{this.Name} - {this.Version}";
        }

        public override bool Equals(object obj)
        {
            var item = obj as LicensePackageItem;
            if (item == null)
            {
                return false;
            }

            return this.Name.Equals(item.Name) && this.Version.Equals(item.Version);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();
            hashCode.Add(this.Name);
            hashCode.Add(this.Version);
            return hashCode.ToHashCode();
        }
    }
}
