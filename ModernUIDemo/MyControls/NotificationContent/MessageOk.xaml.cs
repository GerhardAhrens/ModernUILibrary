﻿namespace ModernUIDemo
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für MessageOk.xaml
    /// </summary>
    public partial class MessageOk : UserControl, INotificationServiceMessage
    {
        public MessageOk()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
        }

        public int CountDown { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (string InfoText, string CustomText, double FontSize) textOption = ((string InfoText, string CustomText, double FontSize))this.Tag;

            this.TbHeader.Text = textOption.Item1;
            this.ControlContent.NavigateToString(textOption.Item2);
        }
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Window window = this.Parent as Window;
            window.Tag = new Tuple<NotificationBoxButton, object>(NotificationBoxButton.Ok, null);
            window.DialogResult = true;
            window.Close();
        }
    }
}
