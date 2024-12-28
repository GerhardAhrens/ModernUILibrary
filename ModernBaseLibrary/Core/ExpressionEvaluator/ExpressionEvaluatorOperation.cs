/*
 * <copyright file="ExpressionEvaluatorOperation.cs" company="Lifeprojects.de">
 *     Class: ExpressionEvaluatorOperation
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>17.10.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Klasse mit für die Auswertung von String zum ermitteln von Ausdrücken
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
    using System.Linq.Expressions;

    internal sealed class ExpressionEvaluatorOperation : ExpressionEvaluatorSymbol
    {
        private readonly Func<Expression, Expression, Expression> operation;
        private readonly Func<Expression, Expression> unaryOperation;

        public static readonly ExpressionEvaluatorOperation Addition = new ExpressionEvaluatorOperation(1, Expression.Add, "Addition");
        public static readonly ExpressionEvaluatorOperation Subtraction = new ExpressionEvaluatorOperation(1, Expression.Subtract, "Subtraction");
        public static readonly ExpressionEvaluatorOperation Multiplication = new ExpressionEvaluatorOperation(2, Expression.Multiply, "Multiplication");
        public static readonly ExpressionEvaluatorOperation Division = new ExpressionEvaluatorOperation(2, Expression.Divide, "Division");
        public static readonly ExpressionEvaluatorOperation UnaryMinus = new ExpressionEvaluatorOperation(2, Expression.Negate, "Negation");

        private static readonly Dictionary<char, ExpressionEvaluatorOperation> Operations = new Dictionary<char, ExpressionEvaluatorOperation>
        {
            { '+', Addition },
            { '-', Subtraction },
            { '*', Multiplication},
            { '/', Division }
        };

        private ExpressionEvaluatorOperation(int precedence, string name)
        {
            Name = name;
            Precedence = precedence;
        }

        private ExpressionEvaluatorOperation(int precedence, Func<Expression, Expression> unaryOperation, string name) : this(precedence, name)
        {
            this.unaryOperation = unaryOperation;
            NumberOfOperands = 1;
        }

        private ExpressionEvaluatorOperation(int precedence, Func<Expression, Expression, Expression> operation, string name) : this(precedence, name)
        {
            this.operation = operation;
            NumberOfOperands = 2;
        }

        public string Name { get; private set; }

        public int NumberOfOperands { get; private set; }

        public int Precedence { get; private set; }

        public static explicit operator ExpressionEvaluatorOperation(char operation)
        {
            ExpressionEvaluatorOperation result;

            if (Operations.TryGetValue(operation, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        private Expression Apply(Expression expression)
        {
            return unaryOperation(expression);
        }

        private Expression Apply(Expression left, Expression right)
        {
            return operation(left, right);
        }

        public static bool IsDefined(char operation)
        {
            return Operations.ContainsKey(operation);
        }

        public Expression Apply(params Expression[] expressions)
        {
            if (expressions.Length == 1)
            {
                return Apply(expressions[0]);
            }

            if (expressions.Length == 2)
            {
                return Apply(expressions[0], expressions[1]);
            }

            throw new NotImplementedException();
        }
    }
}