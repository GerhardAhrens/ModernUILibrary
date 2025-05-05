// -----------------------------------------------------------------------
// <copyright file="TabAboutAllgemein.xaml.cs" company="PTA GmbH Mannheim">
//     Class: TabAboutAllgemein
//     Copyright © PTA GmbH Mannheim 2025
// </copyright>
//
// <author>DeveloperName - PTA GmbH Mannheim</author>
// <email>DeveloperName@pta.de</email>
// <date>05.05.2025 16:12:35</date>
//
// <TemplateVersion>Templateversion</TemplateVersion>
//
// <summary>
// Beispiel UserControl mit Basisfunktionen des ModernUI Framework
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Views.ContentControls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernTemplate.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für TabAboutAllgemein.xaml
    /// </summary>
    public partial class TabAboutAllgemein : UserControlBase
    {
        public TabAboutAllgemein() : base(typeof(TabAboutAllgemein))
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Unloaded", this.OnUnloaded);

            this.InitCommands();
            this.DataContext = this;
        }

        #region Properties
        #endregion Properties

        public override void InitCommands()
        {
            /* Eventuelle Behandlung von Commands */
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            /* Aktion wenn das UserControl geladen wird */
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            /* Aktion wenn das UserControl verlassen wird */
        }
    }
}
