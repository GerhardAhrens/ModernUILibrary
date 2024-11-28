//-----------------------------------------------------------------------
// <copyright file="CommandShortcut.cs" company="Lifeprojects.de">
//     Class: CommandShortcut
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>Class of CommandShortcut Implemation</summary>
// <example>easyLibCore:CommandShortcut.Hotkey="Control+X"</example>
//-----------------------------------------------------------------------

namespace ModernIU.Base
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Class provides attached property to shorten Button hotkey declaration via KeyBinding
    /// </summary>
    public static class CommandKeyShortcut
    {
        public static readonly DependencyProperty HotkeyProperty =
        DependencyProperty.RegisterAttached("Hotkey", typeof(string), typeof(CommandKeyShortcut), new PropertyMetadata(null, HotkeyChangedCallback));

        private static readonly char CmdJoinChar = '+';
        private static readonly char CmdNameChar = '_';

        public static string GetHotkey(DependencyObject obj)
        {
            return (string)obj.GetValue(HotkeyProperty);
        }

        public static void SetHotkey(DependencyObject obj, string value)
        {
            obj.SetValue(HotkeyProperty, value);
        }

        private static string NormalizeName(string name)
        {
            // + symbol in names is prohibited by NameScope (throws exception)
            return name.Replace(CmdJoinChar, CmdNameChar);
        }

        private static void HotkeyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var btn = obj as Button;
            if (btn == null)
            {
                return;
            }

            Window parentWindow = Window.GetWindow(btn);
            if (parentWindow == null)
            {
                return;
            }

            KeyBinding kb = null;

            string hotkeyOld = (string)e.OldValue;

            // find and remove key binding with old hotkey
            if (false == string.IsNullOrWhiteSpace(hotkeyOld))
            {
                hotkeyOld = NormalizeName(hotkeyOld);
                kb = parentWindow.InputBindings
                    .OfType<KeyBinding>()
                    .FirstOrDefault(k => hotkeyOld == (string)k.GetValue(FrameworkElement.NameProperty));

                if (kb != null)
                {
                    parentWindow.InputBindings.Remove(kb);
                }
            }

            string hotkeyNew = (string)e.NewValue;

            if (string.IsNullOrWhiteSpace(hotkeyNew))
            {
                return;
            }

            var keys = hotkeyNew.Split(new[] { CmdJoinChar }, StringSplitOptions.RemoveEmptyEntries);

            ModifierKeys modifier = ModifierKeys.None;
            ModifierKeys m;

            // parse hotkey string and extract modifiers and main key
            string strKey = null;
            foreach (string k in keys)
            {
                if (Enum.TryParse(k, out m))
                {
                    modifier = modifier | m;
                }
                else
                {
                    if (strKey != null)
                    {
                        return;
                    }

                    strKey = k;
                }
            }

            Key key;
            if (false == Enum.TryParse(strKey, out key))
            {
                return;
            }

            // Key + Modifier
            kb = new KeyBinding { Key = key, Modifiers = modifier };

            // x:Name
            kb.SetValue(FrameworkElement.NameProperty, NormalizeName(hotkeyNew));

            // Command
            var cmdBinding = new Binding("Command") { Source = btn };
            BindingOperations.SetBinding(kb, InputBinding.CommandProperty, cmdBinding);

            // Command Parameter
            var paramBinding = new Binding("CommandParameter") { Source = btn };
            BindingOperations.SetBinding(kb, InputBinding.CommandParameterProperty, paramBinding);

            // Adding hotkey to Window
            parentWindow.InputBindings.Add(kb);
        }
    }
}