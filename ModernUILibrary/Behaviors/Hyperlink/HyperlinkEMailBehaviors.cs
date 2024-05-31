//-----------------------------------------------------------------------
// <copyright file="HyperlinkEMailBehaviors.cs" company="Lifeprojects.de">
//     Class: HyperlinkEMailBehaviors
//     Copyright © Gerhard Ahrens, 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>06.05.2019</date>
//
// <summary>
// 
// <Hyperlink>
//  localCore:HyperlinkEMailBehaviors.IsExternal="True"
// </Hyperlink>
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Behaviors
{
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Navigation;

    public static class HyperlinkEMailBehaviors
    {
        public static readonly DependencyProperty IsExternalProperty =
            DependencyProperty.RegisterAttached("IsExternal", typeof(bool), typeof(HyperlinkEMailBehaviors), new UIPropertyMetadata(false, OnIsExternalChanged));

        public static bool GetIsExternal(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsExternalProperty);
        }

        public static void SetIsExternal(DependencyObject obj, bool value)
        {
            obj.SetValue(IsExternalProperty, value);
        }

        private static void OnIsExternalChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var hyperlink = sender as Hyperlink;

            if ((bool)args.NewValue)
            {
                hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
            }
            else
            {
                hyperlink.RequestNavigate -= Hyperlink_RequestNavigate;
            }
        }

        private static void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            if (hyperlink == null)
            {
                return;
            }

            if (Regex.IsMatch(hyperlink.NavigateUri.ToString(), RegExPatternEMailAddress(), RegexOptions.IgnoreCase))
            {
                string address = string.Concat("mailto:", hyperlink.NavigateUri.ToString());
                string tagValue = hyperlink.Tag.ToString();
                try
                {
                    Process.Start(address);
                    /*MessageBox.Show($"Send EMail to '{address}'.", "Send EMail");*/
                }
                catch
                {
                    MessageBox.Show("That e-mail address is invalid.", "E-mail error");
                }
            }
            else
            {

            }

            e.Handled = true;
        }

        private static string RegExPatternEMailAddress()
        {
            return @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        }
    }
}
