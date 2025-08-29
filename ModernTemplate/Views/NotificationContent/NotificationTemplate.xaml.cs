//-----------------------------------------------------------------------
// <copyright file="QuestionYesNo.xaml.cs" company="company">
//     Class: QuestionYesNo
//     Copyright © company 2025
// </copyright>
//
// <author>Gerhard Ahrens - company</author>
// <email>gerhard.ahrens@company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Deklaration eines Message Dialog (als Template Vorlage) für eine Meldung die mit 'Ok' bestädigt werden soll.
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für NotificationTemplate.xaml
    /// </summary>
    public partial class NotificationTemplate : UserControl, INotificationServiceMessage
    {
        public NotificationTemplate()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        public int CountDown { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (string InfoText, string CustomText, double FontSize) textOption = ((string InfoText, string CustomText, double FontSize))this.Tag;

            this.TbHeader.Text = textOption.Item1;
            this.TbFull.Text = textOption.Item2;
            this.TbFull.FontSize = textOption.Item3;
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton>(NotificationBoxButton.Ok);
            window.DialogResult = true;
            window.Close();
        }
    }
}
