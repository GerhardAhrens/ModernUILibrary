﻿//-----------------------------------------------------------------------
// <copyright file="StringBuilderExtractExtensions.cs" company="Lifeprojects.de">
//     Class: StringBuilderExtractExtensions
//     Copyright © Lifeprojects.de 2016
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>
// Extension Class for StringBuilder
// </summary>
// <WebLink>
// https://jonlabelle.com/snippets/tag/extensions/2
// </WebLink>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Text;

    public static class StringBuilderExtractExtensions
    {
        #region ExtractChar
        /// <summary>A StringBuilder extension method that extracts the character described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted character.</returns>
        public static char ExtractChar(this StringBuilder @this)
        {
            return @this.ExtractChar(0);
        }

        /// <summary>A StringBuilder extension method that extracts the character described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted character.</returns>
        public static char ExtractChar(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractChar(0, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the character described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted character.</returns>
        public static char ExtractChar(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractChar(startIndex, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the character described by @this.</summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted character.</returns>
        public static char ExtractChar(this StringBuilder @this, int startIndex, out int endIndex)
        {
            if (@this.Length > startIndex + 1)
            {
                var ch1 = @this[startIndex];
                var ch2 = @this[startIndex + 1];
                var ch3 = @this[startIndex + 2];

                if (ch1 == '\'' && ch3 == '\'')
                {
                    endIndex = startIndex + 2;
                    return ch2;
                }
            }

            throw new Exception("Invalid char at position: " + startIndex);
        }
        #endregion ExtractChar

        #region ExtractComment
        /// <summary>A StringBuilder extension method that extracts the comment described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted comment.</returns>
        public static StringBuilder ExtractComment(this StringBuilder @this)
        {
            return @this.ExtractComment(0);
        }

        /// <summary>A StringBuilder extension method that extracts the comment described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted comment.</returns>
        public static StringBuilder ExtractComment(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractComment(0, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the comment described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted comment.</returns>
        public static StringBuilder ExtractComment(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractComment(startIndex, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the comment described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted comment.</returns>
        public static StringBuilder ExtractComment(this StringBuilder @this, int startIndex, out int endIndex)
        {
            if (@this.Length > startIndex + 1)
            {
                var ch1 = @this[startIndex];
                var ch2 = @this[startIndex + 1];

                if (ch1 == '/' && ch2 == '/')
                {
                    // Single line comment

                    return @this.ExtractCommentSingleLine(startIndex, out endIndex);
                }

                if (ch1 == '/' && ch2 == '*')
                {
                    /*
                     * Multi-line comment
                     */

                    return @this.ExtractCommentMultiLine(startIndex, out endIndex);
                }
            }

            endIndex = -1;
            return null;
        }
        #endregion ExtractComment

        #region ExtractCommentMultiLine

        /// <summary>
        ///     A StringBuilder extension method that extracts the comment multi line described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted comment multi line.</returns>
        public static StringBuilder ExtractCommentMultiLine(this StringBuilder @this)
        {
            return @this.ExtractCommentMultiLine(0);
        }

        /// <summary>
        ///     A StringBuilder extension method that extracts the comment multi line described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted comment multi line.</returns>
        public static StringBuilder ExtractCommentMultiLine(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractCommentMultiLine(0, out endIndex);
        }

        /// <summary>
        ///     A StringBuilder extension method that extracts the comment multi line described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted comment multi line.</returns>
        public static StringBuilder ExtractCommentMultiLine(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractCommentMultiLine(startIndex, out endIndex);
        }

        /// <summary>
        ///     A StringBuilder extension method that extracts the comment multi line described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted comment multi line.</returns>
        public static StringBuilder ExtractCommentMultiLine(this StringBuilder @this, int startIndex, out int endIndex)
        {
            var sb = new StringBuilder();

            if (@this.Length > startIndex + 1)
            {
                var ch1 = @this[startIndex];
                var ch2 = @this[startIndex + 1];

                if (ch1 == '/' && ch2 == '*')
                {
                    /*
                     * Multi-line comment
                     */

                    sb.Append(ch1);
                    sb.Append(ch2);
                    var pos = startIndex + 2;

                    while (pos < @this.Length)
                    {
                        var ch = @this[pos];
                        pos++;

                        if (ch == '*' && pos < @this.Length && @this[pos] == '/')
                        {
                            sb.Append(ch);
                            sb.Append(@this[pos]);
                            endIndex = pos;
                            return sb;
                        }

                        sb.Append(ch);
                    }

                    endIndex = pos;
                    return sb;
                }
            }

            endIndex = -1;
            return null;
        }
        #endregion ExtractCommentMultiLine

        #region ExtractCommentSingleLine

        /// <summary>
        ///     A StringBuilder extension method that extracts the comment single line described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted comment single line.</returns>
        public static StringBuilder ExtractCommentSingleLine(this StringBuilder @this)
        {
            return @this.ExtractCommentSingleLine(0);
        }

        /// <summary>
        ///     A StringBuilder extension method that extracts the comment single line described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted comment single line.</returns>
        public static StringBuilder ExtractCommentSingleLine(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractCommentSingleLine(0, out endIndex);
        }

        /// <summary>
        ///     A StringBuilder extension method that extracts the comment single line described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted comment single line.</returns>
        public static StringBuilder ExtractCommentSingleLine(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractCommentSingleLine(startIndex, out endIndex);
        }

        /// <summary>
        ///     A StringBuilder extension method that extracts the comment single line described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted comment single line.</returns>
        public static StringBuilder ExtractCommentSingleLine(this StringBuilder @this, int startIndex, out int endIndex)
        {
            var sb = new StringBuilder();

            if (@this.Length > startIndex + 1)
            {
                var ch1 = @this[startIndex];
                var ch2 = @this[startIndex + 1];

                if (ch1 == '/' && ch2 == '/')
                {
                    // Single line comment

                    sb.Append(ch1);
                    sb.Append(ch2);
                    var pos = startIndex + 2;

                    while (pos < @this.Length)
                    {
                        var ch = @this[pos];
                        pos++;

                        if (ch == '\r' && pos < @this.Length && @this[pos] == '\n')
                        {
                            endIndex = pos - 1;
                            return sb;
                        }

                        sb.Append(ch);
                    }

                    endIndex = pos;
                    return sb;
                }
            }

            endIndex = -1;
            return null;
        }
        #endregion ExtractCommentSingleLine

        #region ExtractHexadecimal
        /// <summary>A StringBuilder extension method that extracts the hexadecimal described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted hexadecimal.</returns>
        public static StringBuilder ExtractHexadecimal(this StringBuilder @this)
        {
            return @this.ExtractHexadecimal(0);
        }

        /// <summary>A StringBuilder extension method that extracts the hexadecimal described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted hexadecimal.</returns>
        public static StringBuilder ExtractHexadecimal(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractHexadecimal(0, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the hexadecimal described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted hexadecimal.</returns>
        public static StringBuilder ExtractHexadecimal(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractHexadecimal(startIndex, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the hexadecimal described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted hexadecimal.</returns>
        public static StringBuilder ExtractHexadecimal(this StringBuilder @this, int startIndex, out int endIndex)
        {
            // WARNING: This method support all kind of suffix for .NET Runtime Compiler
            // An operator can be any sequence of supported operator character

            if (startIndex + 1 < @this.Length && @this[startIndex] == '0'
                && (@this[startIndex + 1] == 'x' || @this[startIndex + 1] == 'X'))
            {
                var sb = new StringBuilder();

                var hasNumber = false;
                var hasSuffix = false;

                sb.Append(@this[startIndex]);
                sb.Append(@this[startIndex + 1]);

                var pos = startIndex + 2;

                while (pos < @this.Length)
                {
                    var ch = @this[pos];
                    pos++;

                    if (((ch >= '0' && ch <= '9')
                         || (ch >= 'a' && ch <= 'f')
                         || (ch >= 'A' && ch <= 'F'))
                        && !hasSuffix)
                    {
                        hasNumber = true;
                        sb.Append(ch);
                    }
                    else if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
                    {
                        hasSuffix = true;
                        sb.Append(ch);
                    }
                    else
                    {
                        pos -= 2;
                        break;
                    }
                }

                if (hasNumber)
                {
                    endIndex = pos;
                    return sb;
                }
            }

            endIndex = -1;
            return null;
        }
        #endregion ExtractHexadecimal

        #region ExtractKeyword
        /// <summary>A StringBuilder extension method that extracts the keyword described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted keyword.</returns>
        public static StringBuilder ExtractKeyword(this StringBuilder @this)
        {
            return @this.ExtractKeyword(0);
        }

        /// <summary>A StringBuilder extension method that extracts the keyword described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted keyword.</returns>
        public static StringBuilder ExtractKeyword(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractKeyword(0, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the keyword described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted keyword.</returns>
        public static StringBuilder ExtractKeyword(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractKeyword(startIndex, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the keyword described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted keyword.</returns>
        public static StringBuilder ExtractKeyword(this StringBuilder @this, int startIndex, out int endIndex)
        {
            // WARNING: This method support custom operator for .NET Runtime Compiler
            // An operator can be any sequence of supported operator character
            var sb = new StringBuilder();

            var pos = startIndex;
            var hasCharacter = false;

            while (pos < @this.Length)
            {
                var ch = @this[pos];
                pos++;

                if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
                {
                    hasCharacter = true;
                    sb.Append(ch);
                }
                else if (ch == '@')
                {
                    sb.Append(ch);
                }
                else if (ch >= '0' && ch <= '9' && hasCharacter)
                {
                    sb.Append(ch);
                }
                else
                {
                    pos -= 2;
                    break;
                }
            }

            if (hasCharacter)
            {
                endIndex = pos;
                return sb;
            }

            endIndex = -1;
            return null;
        }
        #endregion ExtractKeyword

        #region ExtractNumber
        /// <summary>A StringBuilder extension method that extracts the number described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted number.</returns>
        public static StringBuilder ExtractNumber(this StringBuilder @this)
        {
            return @this.ExtractNumber(0);
        }

        /// <summary>A StringBuilder extension method that extracts the number described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted number.</returns>
        public static StringBuilder ExtractNumber(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractNumber(0, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the number described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted number.</returns>
        public static StringBuilder ExtractNumber(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractNumber(startIndex, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the number described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted number.</returns>
        public static StringBuilder ExtractNumber(this StringBuilder @this, int startIndex, out int endIndex)
        {
            // WARNING: This method support all kind of suffix for .NET Runtime Compiler
            // An operator can be any sequence of supported operator character
            var sb = new StringBuilder();

            var hasNumber = false;
            var hasDot = false;
            var hasSuffix = false;

            var pos = startIndex;

            while (pos < @this.Length)
            {
                var ch = @this[pos];
                pos++;

                if (ch >= '0' && ch <= '9' && !hasSuffix)
                {
                    hasNumber = true;
                    sb.Append(ch);
                }
                else if (ch == '.' && !hasSuffix && !hasDot)
                {
                    hasDot = true;
                    sb.Append(ch);
                }
                else if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
                {
                    hasSuffix = true;
                    sb.Append(ch);
                }
                else
                {
                    pos -= 2;
                    break;
                }
            }

            if (hasNumber)
            {
                endIndex = pos;
                return sb;
            }

            endIndex = -1;
            return null;
        }
        #endregion ExtractNumber

        #region ExtractOperator
        // <summary>A StringBuilder extension method that extracts the operator described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted operator.</returns>
        public static StringBuilder ExtractOperator(this StringBuilder @this)
        {
            return @this.ExtractOperator(0);
        }

        /// <summary>A StringBuilder extension method that extracts the operator described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted operator.</returns>
        public static StringBuilder ExtractOperator(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractOperator(0, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the operator described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted operator.</returns>
        public static StringBuilder ExtractOperator(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractOperator(startIndex, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the operator described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted operator.</returns>
        public static StringBuilder ExtractOperator(this StringBuilder @this, int startIndex, out int endIndex)
        {
            // WARNING: This method support custom operator for .NET Runtime Compiler
            // An operator can be any sequence of supported operator character
            var sb = new StringBuilder();

            var pos = startIndex;

            while (pos < @this.Length)
            {
                var ch = @this[pos];
                pos++;

                switch (ch)
                {
                    case '`':
                    case '~':
                    case '!':
                    case '#':
                    case '$':
                    case '%':
                    case '^':
                    case '&':
                    case '*':
                    case '(':
                    case ')':
                    case '-':
                    case '_':
                    case '=':
                    case '+':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                    case '|':
                    case ':':
                    case ';':
                    case ',':
                    case '.':
                    case '<':
                    case '>':
                    case '?':
                    case '/':
                        sb.Append(ch);
                        break;
                    default:
                        if (sb.Length > 0)
                        {
                            endIndex = pos - 2;
                            return sb;
                        }

                        endIndex = -1;
                        return null;
                }
            }

            if (sb.Length > 0)
            {
                endIndex = pos;
                return sb;
            }

            endIndex = -1;
            return null;
        }
        #endregion ExtractOperator

        #region ExtractString
        /// <summary>A StringBuilder extension method that extracts the string described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted string.</returns>
        public static StringBuilder ExtractString(this StringBuilder @this)
        {
            return @this.ExtractString(0);
        }

        /// <summary>A StringBuilder extension method that extracts the string described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted string.</returns>
        public static StringBuilder ExtractString(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractString(0, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the string described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted string.</returns>
        public static StringBuilder ExtractString(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractString(startIndex, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the string described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted string.</returns>
        public static StringBuilder ExtractString(this StringBuilder @this, int startIndex, out int endIndex)
        {
            if (@this.Length > startIndex + 1)
            {
                var ch1 = @this[startIndex];
                var ch2 = @this[startIndex + 1];

                if (ch1 == '@' && ch2 == '"')
                {
                    // @"my string"

                    return @this.ExtractStringArobasDoubleQuote(startIndex, out endIndex);
                }

                if (ch1 == '@' && ch2 == '\'')
                {
                    // WARNING: This is not a valid string, however single quote is often used to make it more readable in text templating
                    // @'my string'

                    return @this.ExtractStringArobasSingleQuote(startIndex, out endIndex);
                }

                if (ch1 == '"')
                {
                    // "my string"

                    return @this.ExtractStringDoubleQuote(startIndex, out endIndex);
                }

                if (ch1 == '\'')
                {
                    // WARNING: This is not a valid string, however single quote is often used to make it more readable in text templating
                    // 'my string'

                    return @this.ExtractStringSingleQuote(startIndex, out endIndex);
                }
            }

            endIndex = -1;
            return null;
        }
        #endregion ExtractString

        #region ExtractStringArobasDoubleQuote
        /// <summary>
        ///     A StringBuilder extension method that extracts the string arobas double quote
        ///     described by @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted string arobas double quote.</returns>
        public static StringBuilder ExtractStringArobasDoubleQuote(this StringBuilder @this)
        {
            return @this.ExtractStringArobasDoubleQuote(0);
        }

        /// <summary>
        ///     A StringBuilder extension method that extracts the string arobas double quote
        ///     described by @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted string arobas double quote.</returns>
        public static StringBuilder ExtractStringArobasDoubleQuote(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractStringArobasDoubleQuote(0, out endIndex);
        }

        /// <summary>
        ///     A StringBuilder extension method that extracts the string arobas double quote
        ///     described by @this.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted string arobas double quote.</returns>
        public static StringBuilder ExtractStringArobasDoubleQuote(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractStringArobasDoubleQuote(startIndex, out endIndex);
        }

        /// <summary>
        ///     A StringBuilder extension method that extracts the string arobas double quote
        ///     described by @this.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted string arobas double quote.</returns>
        public static StringBuilder ExtractStringArobasDoubleQuote(this StringBuilder @this, int startIndex, out int endIndex)
        {
            var sb = new StringBuilder();

            if (@this.Length > startIndex + 1)
            {
                var ch1 = @this[startIndex];
                var ch2 = @this[startIndex + 1];

                if (ch1 == '@' && ch2 == '"')
                {
                    // @"my string"

                    var pos = startIndex + 2;

                    while (pos < @this.Length)
                    {
                        var ch = @this[pos];
                        pos++;

                        if (ch == '"' && pos < @this.Length && @this[pos] == '"')
                        {
                            sb.Append(ch);
                            pos++; // Treat as escape character for @"abc""def"
                        }
                        else if (ch == '"')
                        {
                            endIndex = pos;
                            return sb;
                        }
                        else
                        {
                            sb.Append(ch);
                        }
                    }

                    throw new Exception("Unclosed string starting at position: " + startIndex);
                }
            }

            endIndex = -1;
            return null;
        }
        #endregion ExtractStringArobasDoubleQuote

        #region ExtractStringArobasSingleQuote
        /// <summary>
        ///     A StringBuilder extension method that extracts the string arobas single quote
        ///     described by @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted string arobas single quote.</returns>
        public static StringBuilder ExtractStringArobasSingleQuote(this StringBuilder @this)
        {
            return @this.ExtractStringArobasSingleQuote(0);
        }
        /// <summary>A StringBuilder extension method that extracts the string arobas single quote
        /// described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted string arobas single quote.</returns>
        public static StringBuilder ExtractStringArobasSingleQuote(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractStringArobasSingleQuote(0, out endIndex);
        }

        /// <summary>
        ///     A StringBuilder extension method that extracts the string arobas single quote
        ///     described by @this.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted string arobas single quote.</returns>
        public static StringBuilder ExtractStringArobasSingleQuote(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractStringArobasSingleQuote(startIndex, out endIndex);
        }
        /// <summary>A StringBuilder extension method that extracts the string arobas single quote
        /// described by @this.</summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted string arobas single quote.</returns>
        public static StringBuilder ExtractStringArobasSingleQuote(this StringBuilder @this, int startIndex, out int endIndex)
        {
            var sb = new StringBuilder();

            if (@this.Length > startIndex + 1)
            {
                var ch1 = @this[startIndex];
                var ch2 = @this[startIndex + 1];

                if (ch1 == '@' && ch2 == '\'')
                {
                    // WARNING: This is not a valid string, however single quote is often used to make it more readable in text templating
                    // @'my string'

                    var pos = startIndex + 2;

                    while (pos < @this.Length)
                    {
                        var ch = @this[pos];
                        pos++;

                        if (ch == '\'' && pos < @this.Length && @this[pos] == '\'')
                        {
                            sb.Append(ch);
                            pos++; // Treat as escape character for @'abc''def'
                        }
                        else if (ch == '\'')
                        {
                            endIndex = pos;
                            return sb;
                        }
                        else
                        {
                            sb.Append(ch);
                        }
                    }

                    throw new Exception("Unclosed string starting at position: " + startIndex);
                }
            }

            endIndex = -1;
            return null;
        }
        #endregion

        #region ExtractStringDoubleQuote
        /// <summary>
        ///     A StringBuilder extension method that extracts the string double quote described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted string double quote.</returns>
        public static StringBuilder ExtractStringDoubleQuote(this StringBuilder @this)
        {
            return @this.ExtractStringDoubleQuote(0);
        }
        /// <summary>A StringBuilder extension method that extracts the string double quote described by
        /// @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted string double quote.</returns>
        public static StringBuilder ExtractStringDoubleQuote(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractStringDoubleQuote(0, out endIndex);
        }

        /// <summary>
        ///     A StringBuilder extension method that extracts the string double quote described by
        ///     @this.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted string double quote.</returns>
        public static StringBuilder ExtractStringDoubleQuote(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractStringDoubleQuote(startIndex, out endIndex);
        }
        /// <summary>A StringBuilder extension method that extracts the string double quote described by
        /// @this.</summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted string double quote.</returns>
        public static StringBuilder ExtractStringDoubleQuote(this StringBuilder @this, int startIndex, out int endIndex)
        {
            var sb = new StringBuilder();

            if (@this.Length > startIndex + 1)
            {
                var ch1 = @this[startIndex];

                if (ch1 == '"')
                {
                    // "my string"

                    var pos = startIndex + 1;

                    while (pos < @this.Length)
                    {
                        var ch = @this[pos];
                        pos++;

                        char nextChar;
                        if (ch == '\\' && pos < @this.Length && ((nextChar = @this[pos]) == '\\' || nextChar == '"'))
                        {
                            sb.Append(nextChar);
                            pos++; // Treat as escape character for \\ or \"
                        }
                        else if (ch == '"')
                        {
                            endIndex = pos;
                            return sb;
                        }
                        else
                        {
                            sb.Append(ch);
                        }
                    }

                    throw new Exception("Unclosed string starting at position: " + startIndex);
                }
            }

            endIndex = -1;
            return null;
        }
        #endregion

        #region ExtractStringSingleQuote
        /// <summary>
        ///     A StringBuilder extension method that extracts the string single quote described by
        ///     @this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted string single quote.</returns>
        public static StringBuilder ExtractStringSingleQuote(this StringBuilder @this)
        {
            return @this.ExtractStringSingleQuote(0);
        }
        /// <summary>A StringBuilder extension method that extracts the string single quote described by
        /// @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted string single quote.</returns>
        public static StringBuilder ExtractStringSingleQuote(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractStringSingleQuote(0, out endIndex);
        }


        /// <summary>
        ///     A StringBuilder extension method that extracts the string single quote described by
        ///     @this.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted string single quote.</returns>
        public static StringBuilder ExtractStringSingleQuote(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractStringSingleQuote(startIndex, out endIndex);
        }
        /// <summary>A StringBuilder extension method that extracts the string single quote described by
        /// @this.</summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted string single quote.</returns>
        public static StringBuilder ExtractStringSingleQuote(this StringBuilder @this, int startIndex, out int endIndex)
        {
            var sb = new StringBuilder();

            if (@this.Length > startIndex + 1)
            {
                var ch1 = @this[startIndex];

                if (ch1 == '\'')
                {
                    // WARNING: This is not a valid string, however single quote is often used to make it more readable in text templating
                    // 'my string'

                    var pos = startIndex + 1;

                    while (pos < @this.Length)
                    {
                        var ch = @this[pos];
                        pos++;

                        char nextChar;
                        if (ch == '\\' && pos < @this.Length && ((nextChar = @this[pos]) == '\\' || nextChar == '\''))
                        {
                            sb.Append(nextChar);
                            pos++; // Treat as escape character for \\ or \"
                        }
                        else if (ch == '\'')
                        {
                            endIndex = pos;
                            return sb;
                        }
                        else
                        {
                            sb.Append(ch);
                        }
                    }

                    throw new Exception("Unclosed string starting at position: " + startIndex);
                }
            }

            endIndex = -1;
            return null;
        }
        #endregion ExtractStringSingleQuote

        #region ExtractToken
        /// <summary>A StringBuilder extension method that extracts the directive described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>The extracted directive.</returns>
        public static StringBuilder ExtractToken(this StringBuilder @this)
        {
            return @this.ExtractToken(0);
        }

        /// <summary>A StringBuilder extension method that extracts the directive described by @this.</summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted directive.</returns>
        public static StringBuilder ExtractToken(this StringBuilder @this, out int endIndex)
        {
            return @this.ExtractToken(0, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the directive described by @this.</summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The extracted directive.</returns>
        public static StringBuilder ExtractToken(this StringBuilder @this, int startIndex)
        {
            int endIndex;
            return @this.ExtractToken(startIndex, out endIndex);
        }

        /// <summary>A StringBuilder extension method that extracts the directive described by @this.</summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <param name="this">The @this to act on.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">[out] The end index.</param>
        /// <returns>The extracted directive.</returns>
        public static StringBuilder ExtractToken(this StringBuilder @this, int startIndex, out int endIndex)
        {
            /* A token can be:
             * - Keyword / Literal
             * - Operator
             * - String
             * - Integer
             * - Real
             */

            // CHECK first which type is the token
            var ch1 = @this[startIndex];
            var pos = startIndex + 1;

            switch (ch1)
            {
                case '@':
                    if (pos < @this.Length && @this[pos] == '"')
                    {
                        return @this.ExtractStringArobasDoubleQuote(startIndex, out endIndex);
                    }
                    if (pos < @this.Length && @this[pos] == '\'')
                    {
                        return @this.ExtractStringArobasSingleQuote(startIndex, out endIndex);
                    }

                    break;
                case '"':
                    return @this.ExtractStringDoubleQuote(startIndex, out endIndex);
                case '\'':
                    return @this.ExtractStringSingleQuote(startIndex, out endIndex);
                case '`':
                case '~':
                case '!':
                case '#':
                case '$':
                case '%':
                case '^':
                case '&':
                case '*':
                case '(':
                case ')':
                case '-':
                case '_':
                case '=':
                case '+':
                case '[':
                case ']':
                case '{':
                case '}':
                case '|':
                case ':':
                case ';':
                case ',':
                case '.':
                case '<':
                case '>':
                case '?':
                case '/':
                    return @this.ExtractOperator(startIndex, out endIndex);
                case '0':
                    if (pos < @this.Length && (@this[pos] == 'x' || @this[pos] == 'X'))
                    {
                        return @this.ExtractHexadecimal(startIndex, out endIndex);
                    }

                    return @this.ExtractNumber(startIndex, out endIndex);
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return @this.ExtractNumber(startIndex, out endIndex);
                default:
                    if ((ch1 >= 'a' && ch1 <= 'z') || (ch1 >= 'A' && ch1 <= 'Z'))
                    {
                        return @this.ExtractKeyword(startIndex, out endIndex);
                    }

                    endIndex = -1;
                    return null;
            }

            throw new Exception("Invalid token");
        }
        #endregion ExtractToken
    }
}