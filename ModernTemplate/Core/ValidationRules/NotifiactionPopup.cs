//-----------------------------------------------------------------------
// <copyright file="NotifiactionPopup.cs" company="company">
//     Class: NotifiactionPopup
//     Copyright © company 2025
// </copyright>
//
// <author>Gerhard Ahrens - company</author>
// <email>gerhard.ahrens@company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Die Klasse erstellt ein Popup um einen Einghabefehler darzustellen. 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Media;
    using System.Windows.Threading;


    /// <summary>
    /// Die Klasse erstellt ein Popup um einen Einghabefehler darzustellen. 
    /// Durch einen Klick auf das Popup wird ein Event ausgelöst. Hiermit kann z.B. der Fokus auf das Control mit dem Fehler gesetzt werden.
    /// </summary>
    /// <remarks>
    /// Das Control wird direkt im C# Source erstellt
    /// </remarks>
    public class NotifiactionPopup
    {
        /// <summary>
        /// Event das beim Klick auf ein Popup ausgelöst wird
        /// </summary>
        public static event EventHandler<PopupResultArgs> PopupClick;

        private static int popNumber = 0;

        /// <summary>
        /// Element/Control an dem das erste Popup plaziert ist. Alle weiteren werden direkt darunter gezeigt.
        /// </summary>
        public static UIElement PlacementTarget { get; set; }

        /// <summary>
        /// Zeit in Sekunden, bis sich das Popup automatisch schließt. Bei -1 bleibt das Popup solang stehen, bis es angeklickt wird.
        /// </summary>
        public static int Delay { get; set; } = 5;

        public static PlacementMode PlacementMode { get; set; } = PlacementMode.Right;

        public static PopupAnimation PopupAnimation { get; set; } = PopupAnimation.Slide;

        public static double PopupWidth { get; set; } = 200;

        public static double PopupHeight { get; set; } = 100;


        public static double HorizontalOffset { get; set; }

        public static double VerticalOffset { get; set; } = 35;

        /// <summary>
        /// Erstelle Popup
        /// </summary>
        /// <param name="field">Eingabefeld</param>
        /// <param name="msgText">Fehlerbeschreibung</param>
        /// <returns>Popup Object</returns>
        public static Popup CreatePopup(string field, string msgText)
        {
            popNumber++;
            Popup popup = new Popup();
            popup.Tag = $"{popNumber}";
            popup.AllowsTransparency = true;
            popup.Width = NotifiactionPopup.PopupWidth;
            popup.Height = NotifiactionPopup.PopupHeight;
            popup.Placement = NotifiactionPopup.PlacementMode;
            popup.PlacementTarget = NotifiactionPopup.PlacementTarget;
            popup.PopupAnimation = NotifiactionPopup.PopupAnimation;
            popup.HorizontalOffset = -110;
            popup.VerticalOffset = VerticalOffset;
            if (popNumber > 1)
            {
                popup.VerticalOffset = -70;
                popup.VerticalOffset = (popup.Height * popNumber) + popup.VerticalOffset;
            }

            Border border = new Border();
            border.Background = (Brush)(new BrushConverter().ConvertFrom("#FF6600"));
            border.BorderBrush = Brushes.Blue;
            border.BorderThickness = new Thickness(1);
            border.CornerRadius = new CornerRadius(5);
            StackPanel grid = new StackPanel();
            grid.Orientation = Orientation.Vertical;
            grid.Margin = new Thickness(5, 5, 0, 0);

            TextBlock textBlockTitle = new TextBlock();
            textBlockTitle.Foreground = Brushes.White;
            textBlockTitle.Height = 18;
            textBlockTitle.FontWeight = FontWeights.Bold;
            textBlockTitle.FontSize = 14;
            textBlockTitle.Inlines.Add(new Underline(new Run("Eingabefehler")));

            TextBlock textBlockMsgA = new TextBlock();
            textBlockMsgA.Foreground = Brushes.White;
            textBlockMsgA.Height = 18;
            textBlockMsgA.Margin = new Thickness(0, 4, 0, 0);
            textBlockMsgA.FontWeight = FontWeights.Medium;
            textBlockMsgA.Inlines.Add(new Run("Feld:"));
            textBlockMsgA.Inlines.Add(new Run(field));

            TextBlock textBlockMsgB = new TextBlock();
            textBlockMsgB.Foreground = Brushes.White;
            textBlockMsgB.Width = popup.Width - 10;
            textBlockMsgB.Height = popup.Height - (textBlockTitle.Height + textBlockMsgA.Height);
            textBlockMsgB.TextWrapping = TextWrapping.Wrap;
            textBlockMsgB.Margin = new Thickness(0, 4, 0, 0);
            textBlockMsgB.FontWeight = FontWeights.Medium;
            textBlockMsgB.Inlines.Add(new Run(msgText));

            grid.Children.Add(textBlockTitle);
            grid.Children.Add(textBlockMsgA);
            grid.Children.Add(textBlockMsgB);

            border.Child = grid;
            popup.Child = border;
            popup.StaysOpen = true;
            popup.IsOpen = false;
            popup.PreviewMouseLeftButtonDown += (s, e) =>
            {
                Popup internalPopup = (Popup)s;
                if (internalPopup != null)
                {
                    internalPopup.IsOpen = false;
                    e.Handled = true;
                    RaisePopupResult(internalPopup.Tag, field);
                }
            };

            if (Delay > 0)
            {
                popup.Opened += (s, e) =>
                {
                    Popup internalPopup = (Popup)s;
                    if (internalPopup != null)
                    {
                        DispatcherTimer timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromSeconds(Delay + (0.25 * popNumber));
                        timer.Start();
                        timer.Tick += (s, e) =>
                        {
                            internalPopup.IsOpen = false;
                            timer.Stop();
                            timer = null;
                        };
                    }
                };
            }

            popup.UpdateLayout();

            return popup;
        }

        public static void Remove()
        {
            if (popNumber > 0)
            {
                popNumber--;
            }
        }

        public static void Reset()
        {
            popNumber = 0;
            VerticalOffset = 35;
        }

        private static void RaisePopupResult(object tag, string sourceName)
        {
            var handler = PopupClick;
            if (handler != null)
            {
                var args = new PopupResultArgs();
                args.Tag = tag;
                args.SourceName = sourceName;
                handler(typeof(NotifiactionPopup), args);
            }
        }
    }

    public class PopupResultArgs : EventArgs
    {
        public object Tag { get; set; }

        public string SourceName { get; set; }
    }
}
