//-----------------------------------------------------------------------
// <copyright file="AppAboutUC.xaml.cs" company="company">
//     Class: AppAboutUC
//     Copyright © company 2025
// </copyright>
//
// <author>Gerhard Ahrens - company</author>
// <email>gerhard.ahrens@company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Die Klasse dient als Container für TabControl Item um verschiedene Inhalte für den About-Dialog darstellen zuu können
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Views.ContentControls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using ModernBaseLibrary.Cryptography;

    using ModernTemplate.Core;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Interaktionslogik für AppAboutUC.xaml
    /// </summary>
    public partial class AppAboutUC : UserControlBase
    {
        public AppAboutUC() : base(typeof(AppAboutUC))
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        #region Properties
        public string DemoText
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }
        #endregion Properties

        public override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand(CommandButtons.DialogBack, new RelayCommand(this.DialogBackHandler));
        }

        #region WindowEventHandler
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            Keyboard.Focus(this);
            this.InitCommands();
            this.IsUCLoaded = true;
            this.DataContext = this;
        }

        #endregion WindowEventHandler

        #region CommandHandler
        private void DialogBackHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.Home,
                FromPage = CommandButtons.AppAbout
            });
        }
        #endregion CommandHandler
    }
}
