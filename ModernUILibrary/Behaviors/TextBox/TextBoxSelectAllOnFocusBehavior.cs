// <copyright file="TextBoxSelectAllOnFocusBehavior.cs" company="Lifeprojects.de">
//     Class: TextBoxSelectAllOnFocusBehavior
//     Copyright © Gerhard Ahrens, 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.10.2022</date>
//
// <summary>
// Inhalt einer TextBox selektieren, wenn diese den Fokus erhält.
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Behaviors
{
    using System.Runtime.Versioning;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Microsoft.Xaml.Behaviors;

    [SupportedOSPlatform("windows")]
    public class TextBoxSelectAllOnFocusBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotKeyboardFocus += AssociatedObject_GotKeyboardFocus;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotKeyboardFocus -= AssociatedObject_GotKeyboardFocus;
        }

        private void AssociatedObject_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            AssociatedObject.SelectAll();
        }
    }
}
