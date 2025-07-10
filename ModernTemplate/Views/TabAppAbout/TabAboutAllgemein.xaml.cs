// -----------------------------------------------------------------------
// <copyright file="TabAboutAllgemein.xaml.cs" company="Lifeprojects.de Mannheim">
//     Class: TabAboutAllgemein
//     Copyright © Lifeprojects.de Mannheim 2025
// </copyright>
//
// <author>DeveloperName - Lifeprojects.de Mannheim</author>
// <email>DeveloperName@lifeprojects.de</email>
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

    using ModernIU.Controls;

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
        public string Product
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string ProductVersion
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public DateTime ProductDate
        {
            get => base.GetValue<DateTime>();
            set => base.SetValue(value);
        }

        public string Description
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string Copyright
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string GitRepository
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string FrameworkVersion
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string OSEnvironment
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }
        #endregion Properties

        public override void InitCommands()
        {
            /* Variante, wenn RequestNavigate; IsExtren auf True gesetzt ist.*/
            this.CmdAgg.AddOrSetCommand("RequestNavigateCommand", new RelayCommand<UriEventArgs>(this.OnRequestNavigateHandler));
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            this.IsUCLoaded = true;

            AssemblyMetaInfo ami = new AssemblyMetaInfo();
            this.Product = ami.AssemblyName;
            this.ProductVersion = ami.AssemblyVersion.ToString();
            this.ProductDate = ami.AssemblyDate;
            this.Description = ami.Description;
            this.Copyright = ami.Copyright;
            this.GitRepository = ami.GitRepository;
            this.FrameworkVersion = ami.FrameworkVersion;
            this.OSEnvironment = $"{ami.RuntimeIdentifier} / {ami.OSPlatform}";
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            /* Aktion wenn das UserControl verlassen wird */
        }

        private void OnRequestNavigateHandler(UriEventArgs item)
        {
            MessageBox.Show(item.TextNavigate, "URL anzeigen");
        }
    }
}
