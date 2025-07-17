//-----------------------------------------------------------------------
// <copyright file="TemplateOverviewUC.xaml.cs" company="Lifeprojects.de">
//     Class: TemplateOverviewUC.xaml
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>16.04.2025</date>
//
// <summary>
// Beispiel UI Dialog mit einem 'Back'-Button
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
    /// Interaktionslogik für TemplateOverviewUC.xaml
    /// </summary>
    public partial class TemplateOverviewUC : UserControlBase
    {
        public TemplateOverviewUC() : base(typeof(TemplateOverviewUC))
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

            string words = string.Empty;
            using (LoremIpsumBuilder lb = new LoremIpsumBuilder())
            {
                words = lb.GetParagraphs(10,15);
            }

            this.DemoText = words;
        }

        #endregion WindowEventHandler

        #region CommandHandler
        private void DialogBackHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.Home,
                FromPage = CommandButtons.CustomA
            });
        }
        #endregion CommandHandler
    }
}
