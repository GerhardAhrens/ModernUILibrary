//-----------------------------------------------------------------------
// <copyright file="HomeUC.xaml.cs" company="Lifeprojects.de">
//     Class: HomeUC
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
    /// Interaktionslogik für HomeUC.xaml
    /// </summary>
    public partial class HomeUC : UserControlBase
    {
        public HomeUC() : base(typeof(HomeUC))
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, MouseWheelEventArgs>.AddHandler(this, "PreviewMouseWheel", this.OnPreviewMouseWheel);
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
            this.CmdAgg.AddOrSetCommand(CommandButtons.Custom, new RelayCommand(this.CustomHandler));
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

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) == true)
            {
                if (e.Delta > 0)
                {
                    if (this.Scalefactor.ScaleX <= 2.0)
                    {
                        this.Scalefactor.ScaleX = this.Scalefactor.ScaleX + 0.25;
                        this.Scalefactor.ScaleY = this.Scalefactor.ScaleY + 0.25;
                    }
                }

                if (e.Delta < 0)
                {
                    if (this.Scalefactor.ScaleX > 1.0)
                    {
                        this.Scalefactor.ScaleX = this.Scalefactor.ScaleX - 0.25;
                        this.Scalefactor.ScaleY = this.Scalefactor.ScaleY - 0.25;
                    }
                }
            }
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

        private void CustomHandler(object p1)
        {
            base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
            {
                Sender = this.GetType().Name,
                MenuButton = CommandButtons.Custom,
            });
        }
        #endregion CommandHandler
    }
}
