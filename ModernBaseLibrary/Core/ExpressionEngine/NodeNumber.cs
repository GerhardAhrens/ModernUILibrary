/*
 * <copyright file="NodeNumber.cs" company="Lifeprojects.de">
 *     Class: NodeNumber
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>17.10.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Klasse ermittelt in einem Ausdruck die nummerischen Bestandteile
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
    /// <summary>
    /// NodeNumber represents a literal number in the expression
    /// </summary>
    internal sealed class NodeNumber : Node
    {
        private decimal _number;

        public NodeNumber(decimal number)
        {
            this._number = number;
        }


        public override decimal Eval(ICalculationContext ctx)
        {
            return this._number;
        }
    }
}
