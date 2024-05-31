//-----------------------------------------------------------------------
// <copyright file="HyperlinkURLBehaviors.cs" company="Lifeprojects.de">
//     Class: HyperlinkURLBehaviors
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
//  localCore:HyperlinkURLBehaviors.IsExternal="True"
// </Hyperlink>
// </summary>

namespace ModernIU.Behaviors
{
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Navigation;

    public static class HyperlinkURLBehaviors
    {
        public static readonly DependencyProperty IsExternalProperty =
            DependencyProperty.RegisterAttached("IsExternal", typeof(bool), typeof(HyperlinkURLBehaviors), new UIPropertyMetadata(false, OnIsExternalChanged));

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

            if (Regex.IsMatch(hyperlink.NavigateUri.ToString(), RegExPatternURLAddress(), RegexOptions.IgnoreCase))
            {
                string address = hyperlink.NavigateUri.ToString();
                string tagValue = hyperlink.Tag.ToString();
                try
                {
                    Process.Start(address);
                }
                catch
                {
                    MessageBox.Show("That web address is invalid.", "web address error");
                }
            }
            else
            {

            }

            e.Handled = true;
        }

        private static string RegExPatternURLAddress()
        {
            var pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            return pattern;
        }
    }
}
