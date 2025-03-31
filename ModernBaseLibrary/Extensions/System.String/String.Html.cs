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
    using System.Text;
    using System.Text.RegularExpressions;

    public static partial class StringExtension
    {
        private static Dictionary<char, string> umlauteMap = null;

        static StringExtension()
        {
            umlauteMap = new Dictionary<char, string>() { { 'Ä', "&Auml;" }, { 'Ö', "&Ouml;" }, { 'Ü', "&Uuml;" }, { 'ä', "&auml;" }, { 'ö', "&ouml;" }, { 'ü', "&uuml;" }, { 'ß', "&szlig;" } };
        }

        public static bool IsHtmlTag(this string text, string tag)
        {
            var pattern = @"<\s*" + tag + @"\s*\/?>";
            return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
        }

        public static bool IsHtmlTags(this string text, string tags)
        {
            var ba = tags.Split('|').Select(x => new { tag = x, hastag = text.IsHtmlTag(x) }).Where(x => x.hastag);

            return ba.Count() > 0;
        }

        public static bool IsHtmlTags(this string text)
        {
            return
                text.IsHtmlTags(
                    "a|abbr|acronym|address|area|b|base|bdo|big|blockquote|body|br|button|caption|cite|code|col|colgroup|dd|del|dfn|div|dl|DOCTYPE|dt|em|fieldset|form|h1|h2|h3|h4|h5|h6|head|html|hr|i|img|input|ins|kbd|label|legend|li|link|map|meta|noscript|object|ol|optgroup|option|p|param|pre|q|samp|script|select|small|span|strong|style|sub|sup|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|ul|var");
        }

        public static string ReplaceHtmlDiacriticals(this string @this)
        {
            string result = string.Empty;

            result = @this.Aggregate(
                          new StringBuilder(),
                          (sb, c) => umlauteMap.TryGetValue(c, out var r) ? sb.Append(r) : sb.Append(c)).ToString();

            return result;
        }
    }
}
