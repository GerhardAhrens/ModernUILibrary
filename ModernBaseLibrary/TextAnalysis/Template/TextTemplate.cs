//-----------------------------------------------------------------------
// <copyright file="TextTemplate.cs" company="Lifeprojects.de">
//     Class: TextTemplate
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// < author > Gerhard Ahrens - Lifeprojects.de </ author >
// < email > developer@lifeprojects.de </ email >
// <date>17.05.2023 11:41:20</date>
//
// <summary>
// Klasse zum erstellen von Texten aus Templates
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Text
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class TextTemplate
    {
        private static readonly Dictionary<char, string> EscapeChars = new Dictionary<char, string>
        {
            ['r'] = "\r",
            ['n'] = "\n",
            ['\\'] = "\\",
            ['{'] = "{",
        };

        private static readonly Regex RenderExpr = new Regex(@"\\.|{([a-z0-9_.\-]+)}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Initializes a new instance of the <see cref="TextTemplate"/> class.
        /// </summary>
        public TextTemplate(string templateString)
        {
            if (templateString == null)
            {
                throw new ArgumentNullException(nameof(templateString));
            }

            this.TemplateString = templateString;
        }

        public string TemplateString { get; }

        public string Render(Dictionary<string, object> variables)
        {
            return Render(this.TemplateString, variables);
        }

        public static string Render(string templateString, Dictionary<string, object> variables)
        {
            if (templateString == null)
            {
                throw new ArgumentNullException(nameof(TemplateString));
            }

            return RenderExpr.Replace(templateString, Match => {
                switch (Match.Value[0])
                {
                    case '\\':
                        if (EscapeChars.ContainsKey(Match.Value[1]))
                        {
                            return EscapeChars[Match.Value[1]];
                        }
                        break;

                    case '{':
                        if (variables.ContainsKey(Match.Groups[1].Value))
                        {
                            return variables[Match.Groups[1].Value].ToString();
                        }
                        break;
                }

                return string.Empty;
            });
        }
    }
}
