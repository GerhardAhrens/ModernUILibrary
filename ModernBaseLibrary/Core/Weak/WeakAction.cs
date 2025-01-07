/*
 * <copyright file="WeakAction.cs" company="Lifeprojects.de">
 *     Class: WeakAction
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>09.01.2020</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Implementation of the WeakAction Pattern
 * </summary>
 *
 * <WebLink>
 * https://thomaslevesque.com/2010/05/17/c-a-simple-implementation-of-the-weakevent-pattern/
 * </WebLink>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Represents an action delegate which contains a so the GarbageCollector can remove the reference if its not needed anymore.
    /// </summary>
    public class WeakAction<T>
    {
        private readonly MethodInfo _method;
        private readonly WeakReference _targetReference;

        /// <summary>
        /// Initializes a new instance of the DW.SharpTools.WeakAction{T} class.
        /// </summary>
        /// <param name="action">The action which target will be hold in the <see cref="System.WeakReference"/>.</param>
        public WeakAction(Action<T> action)
        {
            _targetReference = new WeakReference(action.Target);
            _method = action.Method;
        }

        /// <summary>
        /// Checks if the current WeakAction contains a specific action delegate.
        /// </summary>
        /// <param name="action">The action delegate to check.</param>
        /// <returns>True if the current WeakAction contains the passed action delegate; otherwise false.</returns>
        public bool HasAction(Action<T> action)
        {
            return _method.Equals(action.Method);
        }

        /// <summary>
        /// Gets the information if the action delegate target object is still there or removed by the GarbageCollector.
        /// </summary>
        public bool IsAlive
        {
            get { return _targetReference.IsAlive; }
        }

        /// <summary>
        /// Returns the in the WeakAction stored action delegate if its still alive.
        /// </summary>
        /// <returns>The in the WeakAction stored action delegate if its still alive; otherwise false</returns>
        /// <remarks>If the stored action delegate cannot be created to a real delegate an exception will be catched and this method returns null</remarks>
        public Action<T> GetAction()
        {
            if (this.IsAlive == false)
            {
                return null;
            }

            try
            {
                return Delegate.CreateDelegate(typeof(Action<T>), _targetReference.Target, _method.Name) as Action<T>;
            }
            catch
            {
                return null;
            }
        }
    }
}
