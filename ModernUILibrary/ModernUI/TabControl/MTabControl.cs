﻿namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    using ModernIU.Base;

    public class MTabControl : TabControl
    {
        public static readonly DependencyProperty TypeProperty;
        public EnumTabControlType Type
        {
            get { return (EnumTabControlType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public object HeaderContent
        {
            get { return (object)GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register("HeaderContent", typeof(object), typeof(MTabControl));

        #region Constructors
        static MTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MTabControl), new FrameworkPropertyMetadata(typeof(MTabControl)));
            MTabControl.TypeProperty = DependencyProperty.Register("Type", typeof(EnumTabControlType), typeof(MTabControl), new PropertyMetadata(EnumTabControlType.Line));
        }
        #endregion

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabItem();
        }
    }
}
