/*
 * <copyright file="NodeBinary.cs" company="Lifeprojects.de">
 *     Class: NodeBinary
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>17.10.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Klasse NodeBinary für Operationen Add, Sub, usw.
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

    // NodeBinary for binary operations such as Add, Subtract etc...
    internal sealed class NodeBinary : Node
    {
        private Node _lhs;                              // Left hand side of the operation
        private Node _rhs;                              // Right hand side of the operation
        private Func<decimal, decimal, decimal> _op;    // The callback operator

        // Constructor accepts the two nodes to be operated on and function
        // that performs the actual operation
        public NodeBinary(Node lhs, Node rhs, Func<decimal, decimal, decimal> op)
        {
            this._lhs = lhs;
            this._rhs = rhs;
            this._op = op;
        }

        public override decimal Eval(ICalculationContext ctx)
        {
            // Evaluate both sides
            var lhsVal = this._lhs.Eval(ctx);
            var rhsVal = this._rhs.Eval(ctx);

            // Evaluate and return
            var result = this._op(lhsVal, rhsVal);
            return result;
        }
    }
}
