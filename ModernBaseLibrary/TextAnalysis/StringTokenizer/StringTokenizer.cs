/*
 * <copyright file="StringTokenizer.cs" company="Lifeprojects.de">
 *     Class: StringTokenizer
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>14.05.2020</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * StringTokenizer class. You can use this class in any way you want as long as this header remains in this file.
 * </summary>
 *
 * <WebSite>
 * http://www.adersoftware.com
 * </WebSite>
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Text
{
    using System;
    using System.IO;

    /// <summary>
    /// StringTokenizer tokenized string (or stream) into tokens.
    /// </summary>
    public class StringTokenizer
    {
        const char EOF = (char)0;
        private readonly string data;

        private int line;
        private int column;
        private int pos;
        private bool ignoreWhiteSpace;
        private char[] symbolChars;

        private int saveLine;
        private int saveCol;
        private int savePos;

        public StringTokenizer(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            data = reader.ReadToEnd();

            this.Reset();
        }

        public StringTokenizer(string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.data = data;

            this.Reset();
        }

        /// <summary>
        /// gets or sets which characters are part of TokenKind.Symbol
        /// </summary>
        public char[] SymbolChars
        {
            get { return this.symbolChars; }
            set { this.symbolChars = value; }
        }

        /// <summary>
        /// if set to true, white space characters will be ignored,
        /// but EOL and whitespace inside of string will still be tokenized
        /// </summary>
        public bool IgnoreWhiteSpace
        {
            get { return this.ignoreWhiteSpace; }
            set { this.ignoreWhiteSpace = value; }
        }

        private void Reset()
        {
            this.ignoreWhiteSpace = false;
            this.symbolChars = new char[] { '=', '+', '-', '/', ',', '.', '*', '~', '!', '@', '#', '$', '%', '^', '&', '(', ')', '{', '}', '[', ']', ':', ';', '<', '>', '?', '|', '\\' };

            this.line = 1;
            this.column = 1;
            this.pos = 0;
        }

        protected char LA(int count)
        {
            if (this.pos + count >= this.data.Length)
            {
                return EOF;
            }
            else
            {
                return this.data[this.pos + count];
            }
        }

        protected char Consume()
        {
            char ret = this.data[pos];
            this.pos++;
            this.column++;

            return ret;
        }

        protected Token CreateToken(TokenKind kind, string value)
        {
            return new Token(kind, value, line, this.column);
        }

        protected Token CreateToken(TokenKind kind)
        {
            string tokenData = data.Substring(this.savePos, this.pos - this.savePos);
            return new Token(kind, tokenData, this.saveLine, this.saveCol);
        }

        public Token Next()
        {
        ReadToken:

            char ch = LA(0);
            switch (ch)
            {
                case EOF:
                    return this.CreateToken(TokenKind.EOF, string.Empty);

                case ' ':
                case '\t':
                    {
                        if (this.ignoreWhiteSpace)
                        {
                            this.Consume();
                            goto ReadToken;
                        }
                        else
                            return this.ReadWhitespace();
                    }
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return this.ReadNumber();

                case '\r':
                    {
                        this.StartRead();
                        this.Consume();
                        if (this.LA(0) == '\n')
                        {
                            this.Consume();
                        }

                        this.line++;
                        this.column = 1;

                        return this.CreateToken(TokenKind.EOL);
                    }
                case '\n':
                    {
                        this.StartRead();
                        this.Consume();
                        this.line++;
                        this.column = 1;

                        return this.CreateToken(TokenKind.EOL);
                    }

                case '"':
                    {
                        return this.ReadString();
                    }

                default:
                    {
                        if (char.IsLetter(ch) || ch == '_')
                        {
                            return ReadWord();
                        }
                        else if (this.IsSymbol(ch))
                        {
                            this.StartRead();
                            this.Consume();
                            return this.CreateToken(TokenKind.Symbol);
                        }
                        else
                        {
                            this.StartRead();
                            this.Consume();
                            return this.CreateToken(TokenKind.Unknown);
                        }
                    }

            }
        }

        /// <summary>
        /// save read point positions so that CreateToken can use those
        /// </summary>
        private void StartRead()
        {
            this.saveLine = this.line;
            this.saveCol = this.column;
            this.savePos = this.pos;
        }

        /// <summary>
        /// reads all whitespace characters (does not include newline)
        /// </summary>
        /// <returns></returns>
        protected Token ReadWhitespace()
        {
            this.StartRead();

            this.Consume();

            while (true)
            {
                char ch = this.LA(0);
                if (ch == '\t' || ch == ' ')
                {
                    Consume();
                }
                else
                {
                    break;
                }
            }

            return this.CreateToken(TokenKind.WhiteSpace);

        }

        /// <summary>
        /// reads number. Number is: DIGIT+ ("." DIGIT*)?
        /// </summary>
        /// <returns></returns>
        protected Token ReadNumber()
        {
            this.StartRead();

            bool hadDot = false;

            Consume(); // read first digit

            while (true)
            {
                char ch = this.LA(0);
                if (char.IsDigit(ch))
                {
                    this.Consume();
                }
                else if (ch == '.' && !hadDot)
                {
                    hadDot = true;
                    this.Consume();
                }
                else
                {
                    break;
                }
            }

            return this.CreateToken(TokenKind.Number);
        }

        /// <summary>
        /// reads word. Word contains any alpha character or _
        /// </summary>
        protected Token ReadWord()
        {
            StartRead();

            this.Consume();

            while (true)
            {
                char ch = LA(0);
                if (Char.IsLetter(ch) || ch == '_')
                    this.Consume();
                else
                    break;
            }

            return this.CreateToken(TokenKind.Word);
        }

        /// <summary>
        /// reads all characters until next " is found.
        /// If "" (2 quotes) are found, then they are consumed as
        /// part of the string
        /// </summary>
        /// <returns></returns>
        protected Token ReadString()
        {
            this.StartRead();

            this.Consume();

            while (true)
            {
                char ch = this.LA(0);
                if (ch == EOF)
                    break;
                else if (ch == '\r')
                {
                    this.Consume();
                    if (LA(0) == '\n')
                    {
                        this.Consume();
                    }

                    this.line++;
                    this.column = 1;
                }
                else if (ch == '\n')	// new line in quoted string
                {
                    this.Consume();

                    this.line++;
                    this.column = 1;
                }
                else if (ch == '"')
                {
                    this.Consume();
                    if (LA(0) != '"')
                    {
                        break;
                    }
                    else
                    {
                        this.Consume();
                    }
                }
                else
                {
                    this.Consume();
                }
            }

            return this.CreateToken(TokenKind.QuotedString);
        }

        /// <summary>
        /// checks whether c is a symbol character.
        /// </summary>
        protected bool IsSymbol(char c)
        {
            for (int i = 0; i < symbolChars.Length; i++)
            {
                if (symbolChars[i] == c)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
