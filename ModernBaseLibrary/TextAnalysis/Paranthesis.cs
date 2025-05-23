/*
 * <copyright file="Paranthesis.cs" company="Lifeprojects.de">
 *     Class: Paranthesis
 *     Copyright � Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>23.02.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Die Klasse untersucht Klammerpaare 
 * </summary>
 *
 * <WebSite>
 * </WebSite>
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Text
{
    using System.Collections.Generic;

    using ModernBaseLibrary.Core;

    public sealed class Paranthesis : DisposableCoreBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Paranthesis"/> class.
        /// </summary>
        public Paranthesis()
        {
        }

        public bool IsValid(string s)
        {
            Stack<char> paranthesisStack = new Stack<char>();

            foreach (char item in s)
            {
                switch (item)
                {
                    case '{':
                        paranthesisStack.Push('}');
                        break;
                    case '[':
                        paranthesisStack.Push(']');
                        break;
                    case '(':
                        paranthesisStack.Push(')');
                        break;
                    case '}':
                    case ']':
                    case ')':
                        if (paranthesisStack.Count == 0)
                        {
                            return false;
                        }

                        if (paranthesisStack.Pop() != item)
                        {
                            return false;
                        }

                        break;
                    default:
                        break;
                }
            }

            return paranthesisStack.Count == 0;
        }
    }
}
