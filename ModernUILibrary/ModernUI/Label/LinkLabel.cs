//-----------------------------------------------------------------------
// <copyright file="LinkLabel.cs" company="Lifeprojects.de">
//     Class: LinkLabel
//     Copyright © Gerhard Ahrens, 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>29.04.2023</date>
//
// <summary>Class for UI Control LinkLabel</summary>
// <website>
// https://www.cnblogs.com/lazycoding/archive/2012/10/11/2719778.html
// </website>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class LinkLabel : Label
    {
        private const string _linkLabel = "LinkLabel";

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkLabel"/> class.
        /// </summary>
        static LinkLabel()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkLabel), new FrameworkPropertyMetadata(typeof(LinkLabel)));
        }

        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register("Url", typeof(Uri), typeof(LinkLabel));

        [Category("Common Properties"), Bindable(true)]
        public Uri Url
        {
            get { return GetValue(UrlProperty) as Uri; }
            set { SetValue(UrlProperty, value); }
        }

        public static readonly DependencyProperty HyperlinkStyleProperty = DependencyProperty.Register("HyperlinkStyle", typeof(Style), typeof(LinkLabel));

        public Style HyperlinkStyle
        {
            get { return GetValue(HyperlinkStyleProperty) as Style; }
            set { SetValue(HyperlinkStyleProperty, value); }
        }

        public static readonly DependencyProperty HoverForegroundProperty = DependencyProperty.Register("HoverForeground", typeof(Brush), typeof(LinkLabel));

        [Category("Brushes"), Bindable(true)]
        public Brush HoverForeground
        {
            get { return GetValue(HoverForegroundProperty) as Brush; }
            set { SetValue(HoverForegroundProperty, value); }
        }
    }
}
