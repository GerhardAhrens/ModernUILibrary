/*
 * <copyright file="ExpressionEvaluatorParentheses.cs" company="Lifeprojects.de">
 *     Class: ExpressionEvaluatorParentheses
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
    internal class ExpressionEvaluatorParentheses : ExpressionEvaluatorSymbol
    {
        public static readonly ExpressionEvaluatorParentheses Left = new ExpressionEvaluatorParentheses();
        public static readonly ExpressionEvaluatorParentheses Right = new ExpressionEvaluatorParentheses();

        private ExpressionEvaluatorParentheses()
        {

        }
    }

    internal class ExpressionEvaluatorSymbol
    {
    }

}