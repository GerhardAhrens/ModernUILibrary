//-----------------------------------------------------------------------
// <copyright file="HomeRibbonUC.xaml.cs" company="Lifeprojects.de">
//     Class: HomeRibbonUC
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>02.04.2025</date>
//
// <summary>
// UI Control für den Home Dialog, als Ausgangspunkt für weitere Aktivitäten
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
    /// Interaktionslogik für HomeRibbonUC.xaml
    /// </summary>
    public partial class HomeRibbonUC : UserControlBase
    {
        public HomeRibbonUC() : base(typeof(HomeRibbonUC))
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
            this.CmdAgg.AddOrSetCommand(CommandButtons.CloseApp, new RelayCommand(this.CloseAppHandler));
            this.CmdAgg.AddOrSetCommand(CommandButtons.CustomA, new RelayCommand(this.CustomAHandler));
            this.CmdAgg.AddOrSetCommand(CommandButtons.CustomB, new RelayCommand(this.CustomBHandler));
            this.CmdAgg.AddOrSetCommand(CommandButtons.CatKatalogA, new RelayCommand(this.CatKatalogAHandler));
            this.CmdAgg.AddOrSetCommand(CommandButtons.CatKatalogB, new RelayCommand(this.CatKatalogBHandler));
            this.CmdAgg.AddOrSetCommand(CommandButtons.CatKatalogC, new RelayCommand(this.CatKatalogCHandler));
        }

        #region WindowEventHandler
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ContentPresenter titlePanel = this.HomeRibbonMenu.Template.FindName("PART_TitleHost", this.HomeRibbonMenu) as ContentPresenter;
            if (titlePanel != null)
            {
                titlePanel.Height = 0;
                titlePanel.Content = null;
                titlePanel.Visibility = Visibility.Collapsed;
            }

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
        private void CloseAppHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.CloseApp,
            });
        }

        private void CustomAHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.CustomA,
            });
        }

        private void CustomBHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.CustomB,
            });
        }

        private void CatKatalogAHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.CatKatalogA,
            });
        }

        private void CatKatalogBHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.CatKatalogB,
            });
        }

        private void CatKatalogCHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.CatKatalogC,
            });
        }
        #endregion CommandHandler
    }
}
