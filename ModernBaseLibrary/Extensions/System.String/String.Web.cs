//-----------------------------------------------------------------------
// <copyright file="String.cs" company="Lifeprojects.de">
//     Class: String
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>31.03.2025 09:42:36</date>
//
// <summary>
// Extension Class for 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static partial class StringExtension
    {
        /// <summary>
        /// Prüft die übergebene EMail Adresse auf Gültigkeit
        /// </summary>
        /// <param name="this"></param>
        /// <returns>True = wenn EMail Adresse gültig</returns>
        public static bool IsValidEmail(this string @this)
        {
            try
            {
                // Normalize the domain
                @this = Regex.Replace(@this, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(@this,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Prüft die übergebene URL auf Gültigkeit
        /// </summary>
        /// <param name="this"></param>
        /// <returns>True = wenn URL gültig</returns>
        public static bool IsValidUrl(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                return false;
            }

            Regex rx = new Regex(@"^(((ht|f)tp(s?)\://)|(www))?[^.](.)[^.](([-.\w]*[0-9a-zA-Z])+(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*))[^.](.)[^.]([a-zA-Z0-9]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rx.IsMatch(@this);
        }

        /// <summary>
        /// Prüft die übergebene URL auf Gültigkeit
        /// </summary>
        /// <param name="this"></param>
        /// <returns>True = wenn URL gültig</returns>
        public static bool IsUrl(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                return false;
            }

            Regex rx = new Regex(@"^(((ht|f)tp(s?)\://)|(www))?[^.](.)[^.](([-.\w]*[0-9a-zA-Z])+(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*))[^.](.)[^.]([a-zA-Z0-9]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return rx.IsMatch(@this);
        }
    }
}
