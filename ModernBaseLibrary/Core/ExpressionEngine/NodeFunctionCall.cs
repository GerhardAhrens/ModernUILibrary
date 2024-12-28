/*
 * <copyright file="NodeFunctionCall.cs" company="Lifeprojects.de">
 *     Class: NodeFunctionCall
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>17.10.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Klasse für den aufruf von Funktionen aus einem  Ausdruck
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
    internal sealed class NodeFunctionCall : Node
    {
        private string _functionName;
        private Node[] _arguments;
        public NodeFunctionCall(string functionName, Node[] arguments)
        {
            this._functionName = functionName;
            this._arguments = arguments;
        }


        public override decimal Eval(ICalculationContext ctx)
        {
            // Evaluate all arguments
            var argVals = new decimal[this._arguments.Length];
            for (int i=0; i<_arguments.Length; i++)
            {
                argVals[i] = this._arguments[i].Eval(ctx);
            }

            // Call the function
            return ctx.CallFunction(this._functionName, argVals);
        }
    }
}
