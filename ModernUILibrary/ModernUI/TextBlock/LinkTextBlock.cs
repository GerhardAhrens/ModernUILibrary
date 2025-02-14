//-----------------------------------------------------------------------
// <copyright file="LinkTextBlock.cs" company="Lifeprojects.de">
//     Class: LinkTextBlock
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>13.02.2025 12:40:49</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Diagnostics;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Navigation;

    using ModernIU.Base;

    public class LinkTextBlock : TextBlock
    {
        public static readonly DependencyProperty LinkTextProperty;
        public static readonly DependencyProperty IsExternProperty;

        #region Public DependencyICommand
        public static readonly DependencyProperty RequestNavigateCommandProperty;

        #endregion Public DependencyICommand

        static LinkTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkTextBlock), new FrameworkPropertyMetadata(typeof(LinkTextBlock)));
            LinkTextProperty = DependencyProperty.Register(nameof(LinkText), typeof(string), typeof(LinkTextBlock),new PropertyMetadata(null, OnSetLinkTextChanged));
            IsExternProperty = DependencyProperty.Register(nameof(IsExtern), typeof(bool), typeof(LinkTextBlock), new PropertyMetadata(false));
            RequestNavigateCommandProperty = DependencyProperty.Register(nameof(RequestNavigateCommand), typeof(ICommand), typeof(LinkTextBlock), new PropertyMetadata(null));
        }

        #region Public DependencyICommand
        public ICommand RequestNavigateCommand
        {
            get { return (ICommand)GetValue(RequestNavigateCommandProperty); }
            set { SetValue(RequestNavigateCommandProperty, value); }
        }

        public string LinkText
        {
            get { return (string)GetValue(LinkTextProperty); }
            set {SetValue(LinkTextProperty, value); }
        }

        public bool IsExtern
        {
            get { return (bool)GetValue(IsExternProperty); }
            set { SetValue(IsExternProperty, value); }
        }
        #endregion Public DependencyProperty

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkTextBlock"/> class.
        /// </summary>
        [SupportedOSPlatform("windows")]
        public LinkTextBlock()
        {
            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 23;
            this.Focusable = true;
            this.Background = Brushes.LightYellow;
            this.Foreground = Brushes.Blue;
            this.SetUnderline(Brushes.Blue);
            this.Inlines.Clear();
            this.Inlines.Add(this.Text);

            /* Trigger an Style übergeben */
            this.Style = this.SetTriggerFunction();
            this.ContextMenu = this.BuildContextMenu();
        }

        private void SetUnderline(Brush underlineColor)
        {
            TextDecoration myUnderline = new TextDecoration();

            myUnderline.Pen = new Pen(underlineColor, 1);
            myUnderline.PenThicknessUnit = TextDecorationUnit.FontRecommended;

            TextDecorationCollection myCollection = new TextDecorationCollection();
            myCollection.Add(myUnderline);
            this.TextDecorations = myCollection;
        }

        private static void OnSetLinkTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                LinkTextBlock @this = (LinkTextBlock)d;
                if (e.NewValue.GetType() == typeof(string))
                {
                    if (string.IsNullOrEmpty((string)e.NewValue) == false)
                    {
                        string uriFormat = string.Empty;
                        if (((string)e.NewValue).StartsWith("http://", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            uriFormat = $"{((string)e.NewValue)}";
                        }
                        else if (((string)e.NewValue).StartsWith("https://", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            uriFormat = $"{((string)e.NewValue)}";
                        }
                        else
                        {
                            uriFormat = $"https://{((string)e.NewValue)}";
                        }

                        Hyperlink hyperLink = new Hyperlink()
                        {
                            NavigateUri = new Uri(uriFormat)
                        };

                        Hyperlink link = new Hyperlink();
                        link.IsEnabled = true;
                        link.Inlines.Add(uriFormat);
                        link.NavigateUri = new Uri(uriFormat);
                        link.RequestNavigate += (sender, args) =>
                        {
                            MouseButtonEventArgs e = new MouseButtonEventArgs(Mouse.PrimaryDevice,0,MouseButton.Left);
                            OnMouseLeftButtonDown(sender, e);
                        };

                        hyperLink.Inlines.Add(uriFormat);
                        @this.Inlines.Add(link);

                        WeakEventManager<LinkTextBlock, MouseButtonEventArgs>.AddHandler(@this, "MouseLeftButtonDown", OnMouseLeftButtonDown);
                    }
                }
            }
        }

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LinkTextBlock @this = sender as LinkTextBlock;
            if (@this != null)
            {
                string uriFormat = string.Empty;
                if (@this.LinkText.StartsWith("http://", StringComparison.OrdinalIgnoreCase) == true)
                {
                    uriFormat = $"{@this.LinkText}";
                }
                else if (@this.LinkText.StartsWith("https://", StringComparison.OrdinalIgnoreCase) == true)
                {
                    uriFormat = $"{@this.LinkText}";
                }
                else
                {
                    uriFormat = $"https://{@this.LinkText}";
                }

                if (@this.IsExtern == false)
                {
                    var sInfo = new System.Diagnostics.ProcessStartInfo(uriFormat)
                    {
                        UseShellExecute = true,
                    };
                    System.Diagnostics.Process.Start(sInfo);
                }
                else
                {
                    if (@this.RequestNavigateCommand != null)
                    {
                        Uri uri = new Uri(uriFormat);
                        UriEventArgs uriArgs = new UriEventArgs();
                        uriArgs.Sender = @this.GetType();
                        uriArgs.TextNavigate = uriFormat;
                        uriArgs.UriNavigate = uri;

                        @this.RequestNavigateCommand.Execute(uriArgs);
                    }
                }
            }
        }

        private Style SetTriggerFunction()
        {
            Style inputControlStyle = new Style();

            /* Trigger für IsMouseOver = True */
            Trigger triggerIsMouseOver = new Trigger();
            triggerIsMouseOver.Property = TextBlock.IsMouseOverProperty;
            triggerIsMouseOver.Value = true;
            triggerIsMouseOver.Setters.Add(new Setter() { Property = TextBlock.CursorProperty, Value = Cursors.Hand });
            inputControlStyle.Triggers.Add(triggerIsMouseOver);

            /* Trigger für IsFocused = True */
            Trigger triggerIsFocused = new Trigger();
            triggerIsFocused.Property = TextBlock.IsFocusedProperty;
            triggerIsFocused.Value = true;
            triggerIsFocused.Setters.Add(new Setter() { Property = TextBlock.CursorProperty, Value = Cursors.Hand });
            inputControlStyle.Triggers.Add(triggerIsFocused);

            return inputControlStyle;
        }

        /// <summary>
        /// Spezifisches Kontextmenü erstellen
        /// </summary>
        /// <returns></returns>
        private ContextMenu BuildContextMenu()
        {
            ContextMenu textBoxContextMenu = new ContextMenu();
            MenuItem copyMenu = new MenuItem();
            copyMenu.Header = "Kopiere";
            copyMenu.Icon = IconsDevs.GetPathGeometry(IconsDevs.IconCopy);
            WeakEventManager<MenuItem, RoutedEventArgs>.AddHandler(copyMenu, "Click", this.OnCopyMenu);
            textBoxContextMenu.Items.Add(copyMenu);

            return textBoxContextMenu;
        }

        private void OnCopyMenu(object sender, RoutedEventArgs e)
        {
            if (this.LinkText.Contains("http://") == false)
            {
                this.LinkText = $"http://{this.LinkText}";
            }

            Clipboard.SetText(this.LinkText);
        }
    }

    public class UriEventArgs : EventArgs
    {
        public Type Sender { get; set; }
        public string TextNavigate { get; set; }
        public Uri UriNavigate { get; set; }
    }
}
