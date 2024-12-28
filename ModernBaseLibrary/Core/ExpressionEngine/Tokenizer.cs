/*
 * <copyright file="Tokenizer.cs" company="Lifeprojects.de">
 *     Class: Tokenizer
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>17.10.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Die Klasse wertet einen String von Ausdrücken aus und gibt diese als Token zurück.
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.ExpressionEngine
{
    using System.Globalization;
    using System.IO;
    using System.Text;

    public sealed class Tokenizer
    {
        private TextReader _reader;
        private char _currentChar;
        private Token _currentToken;
        private decimal _number;
        private string _identifier;

        public Tokenizer(TextReader reader)
        {
            this._reader = reader;
            this.NextChar();
            this.NextToken();
        }

        public Token Token
        {
            get { return _currentToken; }
        }

        public decimal Number
        {
            get { return _number; }
        }

        public string Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Read the next character from the input strem and store it
        /// in _currentChar, or load '\0' if EOF
        /// </summary>
        public void NextChar()
        {
            int ch = this._reader.Read();
            this._currentChar = ch < 0 ? '\0' : (char)ch;
        }

        // Read the next token from the input stream
        public void NextToken()
        {
            // Skip whitespace
            while (char.IsWhiteSpace(this._currentChar))
            {
                this.NextChar();
            }

            // Special characters
            switch (this._currentChar)
            {
                case '\0':
                    this._currentToken = Token.EOF;
                    return;

                case '+':
                    this.NextChar();
                    this._currentToken = Token.Add;
                    return;

                case '-':
                    this.NextChar();
                    this._currentToken = Token.Subtract;
                    return;

                case '*':
                    this.NextChar();
                    this._currentToken = Token.Multiply;
                    return;

                case '/':
                    this.NextChar();
                    this._currentToken = Token.Divide;
                    return;

                case '(':
                    NextChar();
                    _currentToken = Token.OpenParens;
                    return;

                case ')':
                    this.NextChar();
                    this._currentToken = Token.CloseParens;
                    return;

                case ',':
                    this.NextChar();
                    this._currentToken = Token.Comma;
                    return;

                case '^':
                    this.NextChar();
                    this._currentToken = Token.Exponent;
                    return;
            }

            // Number?
            if (char.IsDigit(this._currentChar) || this._currentChar =='.')
            {
                // Capture digits/decimal point
                var sb = new StringBuilder();
                bool haveDecimalPoint = false;
                while (char.IsDigit(this._currentChar) || (!haveDecimalPoint && this._currentChar == '.'))
                {
                    sb.Append(this._currentChar);
                    haveDecimalPoint = this._currentChar == '.';
                    this.NextChar();
                }

                // Parse it
                this._number = decimal.Parse(sb.ToString(), CultureInfo.InvariantCulture);
                this._currentToken = Token.Number;
                return;
            }

            // Identifier - starts with letter or underscore
            if (char.IsLetter(this._currentChar) || this._currentChar == '_')
            {
                var sb = new StringBuilder();

                // Accept letter, digit or underscore
                while (char.IsLetterOrDigit(this._currentChar) || this._currentChar == '_')
                {
                    sb.Append(this._currentChar);
                    this.NextChar();
                }

                // Setup token
                this._identifier = sb.ToString();
                this._currentToken = Token.Identifier;
                return;
            }
        }
    }
}
