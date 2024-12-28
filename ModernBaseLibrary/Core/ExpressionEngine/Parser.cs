/*
 * <copyright file="Parser.cs" company="Lifeprojects.de">
 *     Class: Parser
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>17.10.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Die Klasse überprüft den übergeben String. Über einen Tokenizer wird der String
 * in einzelne Terme zerlegt.
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
    using System;
    using System.Collections.Generic;
    using System.IO;

    public sealed class Parser
    {
        private Tokenizer _tokenizer;

        public Parser(Tokenizer tokenizer)
        {
            this._tokenizer = tokenizer;
        }


        // Parse an entire expression and check EOF was reached
        public Node ParseExpression()
        {
            // For the moment, all we understand is add and subtract
            var expr = ParseAddSubtract();

            // Check everything was consumed
            if (this._tokenizer.Token != Token.EOF)
            {
                throw new SyntaxException("Unexpected characters at end of expression");
            }

            return expr;
        }

        // Parse an sequence of add/subtract operators
        private Node ParseAddSubtract()
        {
            // Parse the left hand side
            var lhs = ParseMultiplyDivide();

            while (true)
            {
                // Work out the operator
                Func<decimal, decimal, decimal> op = null;
                if (this._tokenizer.Token == Token.Add)
                {
                    op = (a, b) => a + b;
                }
                else if (this._tokenizer.Token == Token.Subtract)
                {
                    op = (a, b) => a - b;
                }
                else if (this._tokenizer.Token == Token.Exponent)
                {
                    op = (a, b) => (int)a ^ (int)b;
                }

                // Binary operator found?
                if (op == null)
                {
                    return lhs;             // no
                }

                // Skip the operator
                this._tokenizer.NextToken();

                // Parse the right hand side of the expression
                var rhs = ParseMultiplyDivide();

                // Create a binary node and use it as the left-hand side from now on
                lhs = new NodeBinary(lhs, rhs, op);
            }
        }

        // Parse an sequence of add/subtract operators
        Node ParseMultiplyDivide()
        {
            // Parse the left hand side
            var lhs = ParseUnary();

            while (true)
            {
                // Work out the operator
                Func<decimal, decimal, decimal> op = null;
                if (this._tokenizer.Token == Token.Multiply)
                {
                    op = (a, b) => a * b;
                }
                else if (this._tokenizer.Token == Token.Divide)
                {
                    op = (a, b) => a / b;
                }

                // Binary operator found?
                if (op == null)
                {
                    return lhs;             // no
                }

                // Skip the operator
                this._tokenizer.NextToken();

                // Parse the right hand side of the expression
                var rhs = ParseUnary();

                // Create a binary node and use it as the left-hand side from now on
                lhs = new NodeBinary(lhs, rhs, op);
            }
        }


        // Parse a unary operator (eg: negative/positive)
        private Node ParseUnary()
        {
            while (true)
            {
                // Positive operator is a no-op so just skip it
                if (this._tokenizer.Token == Token.Add)
                {
                    // Skip
                    this._tokenizer.NextToken();
                    continue;
                }

                // Negative operator
                if (this._tokenizer.Token == Token.Subtract)
                {
                    // Skip
                    this._tokenizer.NextToken();

                    // Parse RHS 
                    // Note this recurses to self to support negative of a negative
                    var rhs = ParseUnary();

                    // Create unary node
                    return new NodeUnary(rhs, (a) => -a);
                }

                // No positive/negative operator so parse a leaf node
                return ParseLeaf();
            }
        }

        // Parse a leaf node
        // (For the moment this is just a number)
        private Node ParseLeaf()
        {
            // Is it a number?
            if (this._tokenizer.Token == Token.Number)
            {
                var node = new NodeNumber(_tokenizer.Number);
                this._tokenizer.NextToken();
                return node;
            }

            // Parenthesis?
            if (this._tokenizer.Token == Token.OpenParens)
            {
                // Skip '('
                this._tokenizer.NextToken();

                // Parse a top-level expression
                var node = ParseAddSubtract();

                // Check and skip ')'
                if (this._tokenizer.Token != Token.CloseParens)
                {
                    throw new SyntaxException("Missing close parenthesis");
                }

                this._tokenizer.NextToken();

                // Return
                return node;
            }

            // Variable
            if (this._tokenizer.Token == Token.Identifier)
            {
                // Capture the name and skip it
                var name = this._tokenizer.Identifier;
                this._tokenizer.NextToken();

                // Parens indicate a function call, otherwise just a variable
                if (this._tokenizer.Token != Token.OpenParens)
                {
                    // Variable
                    return new NodeVariable(name);
                }
                else
                {
                    // Function call

                    // Skip parens
                    this._tokenizer.NextToken();

                    // Parse arguments
                    var arguments = new List<Node>();
                    while (true)
                    {
                        // Parse argument and add to list
                        arguments.Add(ParseAddSubtract());

                        // Is there another argument?
                        if (this._tokenizer.Token == Token.Comma)
                        {
                            _tokenizer.NextToken();
                            continue;
                        }

                        // Get out
                        break;
                    }

                    // Check and skip ')'
                    if (this._tokenizer.Token != Token.CloseParens)
                    {
                        throw new SyntaxException("Missing close parenthesis");
                    }

                    this._tokenizer.NextToken();

                    // Create the function call node
                    return new NodeFunctionCall(name, arguments.ToArray());
                }
            }

            // Don't Understand
            throw new SyntaxException($"Unexpect token: {_tokenizer.Token}");
        }


        #region Convenience Helpers
        
        // Static helper to parse a string
        public static Node Parse(string str)
        {
            return Parse(new Tokenizer(new StringReader(str)));
        }

        // Static helper to parse from a tokenizer
        public static Node Parse(Tokenizer tokenizer)
        {
            var parser = new Parser(tokenizer);
            return parser.ParseExpression();
        }

        #endregion
    }
}
