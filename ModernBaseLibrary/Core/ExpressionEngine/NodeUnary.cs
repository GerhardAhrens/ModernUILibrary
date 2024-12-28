/*
 * <copyright file="NodeUnary.cs" company="Lifeprojects.de">
 *     Class: NodeUnary
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>17.10.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Klasse NodeUnary wertet einen Ausdruck mit negativen nummerischen Werten aus
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

    // NodeUnary for unary operations such as Negate
    internal sealed class NodeUnary : Node
    {
        private Node _rhs;                              // Right hand side of the operation
        private Func<decimal, decimal> _op;               // The callback operator

        /// <summary>
        /// Constructor accepts the two nodes to be operated on and function 
        /// that performs the actual operation
        /// </summary>
        /// <param name="rhs">Right hand side of the operation</param>
        /// <param name="op">The callback operator</param>
        public NodeUnary(Node rhs, Func<decimal, decimal> op)
        {
            this._rhs = rhs;
            this._op = op;
        }

        public override decimal Eval(ICalculationContext ctx)
        {
            // Evaluate RHS
            var rhsVal = this._rhs.Eval(ctx);

            // Evaluate and return
            var result = this._op(rhsVal);
            return result;
        }
    }
}
