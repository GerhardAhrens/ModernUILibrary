//-----------------------------------------------------------------------
// <copyright file="StringReplacer.cs" company="Lifeprojects.de">
//     Class: StringReplacer
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.08.2017</date>
//
// <summary>
// Klasse zum Ersetzten von String in einem Text
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// StringReplacer is a utility class similar to StringBuilder, with fast Replace function.
    /// StringReplacer is limited to replacing only properly formatted tokens.
    /// Use ToString() function to get the final text.
    /// </summary>
    public class StringReplacer : DisposableCoreBase
    {
        private StringReplacerHelper rootSnippet = new StringReplacerHelper(string.Empty);
        private Dictionary<string, List<TokenOccurrence>> occurrencesOfToken;
        private readonly string TokenOpen;
        private readonly string TokenClose;

        /// <summary>
        /// All tokens that will be replaced must have same opening and closing delimiters, such as "{" and "}".
        /// </summary>
        /// <param name="tokenOpen">Opening delimiter for tokens.</param>
        /// <param name="tokenClose">Closing delimiter for tokens.</param>
        /// <param name="caseSensitive">Set caseSensitive to false to use case-insensitive search when replacing tokens.</param>
        public StringReplacer(string tokenOpen, string tokenClose, bool caseSensitive = true)
        {
            if (string.IsNullOrEmpty(tokenOpen) || string.IsNullOrEmpty(tokenClose))
            {
                throw new ArgumentException("Token must have opening and closing delimiters, such as \"{\" and \"}\".");
            }

            this.TokenOpen = tokenOpen;
            this.TokenClose = tokenClose;

            var stringComparer = caseSensitive ? StringComparer.Ordinal : StringComparer.InvariantCultureIgnoreCase;
            this.occurrencesOfToken = new Dictionary<string, List<TokenOccurrence>>(stringComparer);
        }

        public StringReplacer(char tokenOpen, char tokenClose, bool caseSensitive = true)
        {
            if (string.IsNullOrEmpty(tokenOpen.ToString()) || string.IsNullOrEmpty(tokenClose.ToString()))
            {
                throw new ArgumentException("Token must have opening and closing delimiters, such as \"{\" and \"}\".");
            }

            this.TokenOpen = tokenOpen.ToString();
            this.TokenClose = tokenClose.ToString();

            var stringComparer = caseSensitive ? StringComparer.Ordinal : StringComparer.InvariantCultureIgnoreCase;
            this.occurrencesOfToken = new Dictionary<string, List<TokenOccurrence>>(stringComparer);
        }

        public void Append(string text)
        {
            var snippet = new StringReplacerHelper(text);
            this.rootSnippet.Append(snippet);
            this.ExtractTokens(snippet);
        }

        /// <returns>Returns true if the token was found, false if nothing was replaced.</returns>
        public bool Replace(string token, string text)
        {
            ValidateToken(token, text, false);
            List<TokenOccurrence> occurrences;
            if (this.occurrencesOfToken.TryGetValue(token, out occurrences) && occurrences.Count > 0)
            {
                this.occurrencesOfToken.Remove(token);
                var snippet = new StringReplacerHelper(text);
                foreach (var occurrence in occurrences)
                {
                    occurrence.Snippet.Replace(occurrence.Start, occurrence.End, snippet);
                }

                this.ExtractTokens(snippet);
                return true;
            }

            return false;
        }

        /// <returns>Returns true if the token was found, false if nothing was replaced.</returns>
        public bool InsertBefore(string token, string text)
        {
            ValidateToken(token, text, false);
            List<TokenOccurrence> occurrences;
            if (this.occurrencesOfToken.TryGetValue(token, out occurrences) && occurrences.Count > 0)
            {
                var snippet = new StringReplacerHelper(text);
                foreach (var occurrence in occurrences)
                {
                    occurrence.Snippet.InsertBefore(occurrence.Start, snippet);
                }

                this.ExtractTokens(snippet);
                return true;
            }

            return false;
        }

        /// <returns>Returns true if the token was found, false if nothing was replaced.</returns>
        public bool InsertAfter(string token, string text)
        {
            ValidateToken(token, text, false);
            List<TokenOccurrence> occurrences;
            if (this.occurrencesOfToken.TryGetValue(token, out occurrences) && occurrences.Count > 0)
            {
                var snippet = new StringReplacerHelper(text);
                foreach (var occurrence in occurrences)
                {
                    occurrence.Snippet.InsertAfter(occurrence.End, snippet);
                }

                this.ExtractTokens(snippet);
                return true;
            }

            return false;
        }

        public bool Contains(string token)
        {
            ValidateToken(token, token, false);
            List<TokenOccurrence> occurrences;
            if (this.occurrencesOfToken.TryGetValue(token, out occurrences))
            {
                return occurrences.Count > 0;
            }

            return false;
        }

        private void ExtractTokens(StringReplacerHelper snippet)
        {
            int last = 0;
            while (last < snippet.Text.Length)
            {
                int start = snippet.Text.IndexOf(this.TokenOpen, last, StringComparison.InvariantCultureIgnoreCase);
                if (start == -1)
                {
                    return;
                }

                int end = snippet.Text.IndexOf(this.TokenClose, start + this.TokenOpen.Length, StringComparison.InvariantCultureIgnoreCase);
                if (end == -1)
                {
                    throw new ArgumentException(string.Format("Token is opened but not closed in text \"{0}\".", snippet.Text));
                }

                int eol = snippet.Text.IndexOf('\n', start + this.TokenOpen.Length);
                if (eol != -1 && eol < end)
                {
                    last = eol + 1;
                    continue;
                }

                end += this.TokenClose.Length;
                string token = snippet.Text.Substring(start, end - start);
                string context = snippet.Text;
                this.ValidateToken(token, context, true);

                var tokenOccurrence = new TokenOccurrence { Snippet = snippet, Start = start, End = end };
                List<TokenOccurrence> occurrences;
                if (this.occurrencesOfToken.TryGetValue(token, out occurrences))
                {
                    occurrences.Add(tokenOccurrence);
                }
                else
                {
                    this.occurrencesOfToken.Add(token, new List<TokenOccurrence> { tokenOccurrence });
                }

                last = end;
            }
        }

        private void ValidateToken(string token, string context, bool alreadyValidatedStartAndEnd)
        {
            if (!alreadyValidatedStartAndEnd)
            {
                if (!token.StartsWith(this.TokenOpen, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new ArgumentException(string.Format("Token \"{0}\" shoud start with \"{1}\". Used with text \"{2}\".", token, this.TokenOpen, context));
                }

                int closePosition = token.IndexOf(this.TokenClose, StringComparison.InvariantCultureIgnoreCase);
                if (closePosition == -1)
                {
                    throw new ArgumentException(string.Format("Token \"{0}\" should end with \"{1}\". Used with text \"{2}\".", token, this.TokenClose, context));
                }

                if (closePosition != token.Length - this.TokenClose.Length)
                {
                    throw new ArgumentException(string.Format("Token \"{0}\" is closed before the end of the token. Used with text \"{1}\".", token, context));
                }
            }

            if (token.Length == this.TokenOpen.Length + this.TokenClose.Length)
            {
                throw new ArgumentException(string.Format("Token has no body. Used with text \"{0}\".", context));
            }

            if (token.Contains("\n"))
            {
                throw new ArgumentException(string.Format("Unexpected end-of-line within a token. Used with text \"{0}\".", context));
            }

            if (token.IndexOf(this.TokenOpen, this.TokenOpen.Length, StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                throw new ArgumentException(string.Format("Next token is opened before a previous token was closed in token \"{0}\". Used with text \"{1}\".", token, context));
            }
        }

        public override string ToString()
        {
            int totalTextLength = this.rootSnippet.GetLength();
            var sb = new StringBuilder(totalTextLength);
            this.rootSnippet.ToString(sb);
            if (sb.Length != totalTextLength)
            {
                throw new InvalidOperationException($"Internal error: Calculated total text length ({totalTextLength}) is different from actual ({sb.Length}).");
            }

            return sb.ToString();
        }

        public override void DisposeManagedResources()
        {
            this.rootSnippet = null;
            this.occurrencesOfToken = null;
        }

        private class TokenOccurrence
        {
            public StringReplacerHelper Snippet;
            public int Start; /* Position of a token in the snippet.*/
            public int End;  /* Position of a token in the snippet. */
        }
    }
}
