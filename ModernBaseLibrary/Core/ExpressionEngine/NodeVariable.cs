/*
 * <copyright file="NodeVariable.cs" company="Lifeprojects.de">
 *     Class: NodeVariable
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>17.10.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Klasse ermittelt in einem Ausdruck vorhadenen Konstanten und Variabeln
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
    /// Represents a variable (or a constant) in an expression.  eg: "2 * pi"
    /// </summary>
    internal sealed class NodeVariable : Node
    {
        private string _variableName;

        public NodeVariable(string variableName)
        {
            this._variableName = variableName;
        }

        public override decimal Eval(ICalculationContext ctx)
        {
            return ctx.ResolveVariable(this._variableName);
        }
    }
}
