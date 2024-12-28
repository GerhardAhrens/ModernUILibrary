/*
 * <copyright file="ReflectionContext.cs" company="Lifeprojects.de">
 *     Class: ReflectionContext
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>17.10.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Die Klasse wertet Ausdrücke per Reflection aus
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
    using System.IO;
    using System.Linq;

    public sealed class ReflectionContext : ICalculationContext
    {
        private object _targetObject;

        public ReflectionContext(object targetObject)
        {
            this._targetObject = targetObject;
        }

        public decimal ResolveVariable(string name)
        {
            // Find property
            var methode = this._targetObject.GetType()
                .GetProperties().Select(s => s.Name)
                .FirstOrDefault(a => a.ToLower().Contains(name.ToLower()) == true);
            if (methode != null)
            {
                var pi = this._targetObject.GetType().GetProperty(name);
                if (pi == null)
                {
                    throw new InvalidDataException($"Unknown variable: '{name}'");
                }

                // Call the property
                return (decimal)pi.GetValue(this._targetObject);
            }

            return 0;
        }

        public decimal CallFunction(string name, decimal[] arguments)
        {
            // Find method
            var methode = this._targetObject.GetType()
                .GetMethods().Select(s => s.Name)
                .FirstOrDefault(a => a.ToLower().Contains(name.ToLower()) == true);
            if (methode != null)
            {
                var mi = this._targetObject.GetType().GetMethod(methode);
                if (mi == null)
                {
                    throw new InvalidDataException($"Unknown function: '{name}'");
                }

                // Convert double array to object array
                var argObjs = arguments.Select(x => (object)x).ToArray();

                // Call the method
                return (decimal)mi.Invoke(this._targetObject, argObjs);
            }

            return 0;
        }
    }
}
