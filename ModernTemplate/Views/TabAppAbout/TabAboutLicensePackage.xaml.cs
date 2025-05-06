//-----------------------------------------------------------------------
// <copyright file="TabAboutLicensePackage.xaml.cs" company="PTA GmbH Mannheim">
//     Class: TabAboutLicensePackage
//     Copyright © PTA GmbH Mannheim 2025
// </copyright>
//
// <author>DeveloperName - PTA GmbH Mannheim</author>
// <email>DeveloperName@pta.de</email>
// <date>06.05.2025 10:55:57</date>
//
// <TemplateVersion>2.0</TemplateVersion>
//
// <summary>
// Beispiel UserControl mit Basisfunktionen des ModernUI Framework
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Views.ContentControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernBaseLibrary.Core.AssemblyMeta;

    using ModernTemplate.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für TabAboutLicensePackage.xaml
    /// </summary>
    public partial class TabAboutLicensePackage : UserControlBase
    {
        public TabAboutLicensePackage() : base(typeof(TabAboutLicensePackage))
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Unloaded", this.OnUnloaded);

            this.InitCommands();
            this.DataContext = this;
        }

        #region Properties
        public IEnumerable<LicensePackageItem> LicensePackageSource
        {
            get => base.GetValue<IEnumerable<LicensePackageItem>>();
            set => base.SetValue(value);
        }
        #endregion Properties

        public override void InitCommands()
        {
            /* Eventuelle Behandlung von Commands */
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            LicensePackageInfo lpInfo = new LicensePackageInfo();

            LicensePackageItem licensePackageItem = new LicensePackageItem();
            licensePackageItem.Name = "ModernBaseLibrary";
            licensePackageItem.Version = new Version(1, 0, 2025, 15);
            licensePackageItem.Description = "Basis Bibliothek";
            licensePackageItem.Link = "https://github.com/GerhardAhrens/ModernUILibrary";
            lpInfo.Add(licensePackageItem);

            licensePackageItem = new LicensePackageItem();
            licensePackageItem.Name = "ModernUILibrary";
            licensePackageItem.Version = new Version(1, 0, 2025, 15);
            licensePackageItem.Description = "UI Basis Bibliothek";
            licensePackageItem.Link = "https://github.com/GerhardAhrens/ModernUILibrary";
            lpInfo.Add(licensePackageItem);

            this.LicensePackageSource = lpInfo.GetSource();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.LicensePackageSource = null;
        }
    }
}
