/*
 * <copyright file="SingletonBase.cs" company="Lifeprojects.de">
 *     Class: SingletonBase
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>27.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Basisklasse für die Erstellung eines Singelton<T> Objects
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

namespace ModernIU.Controls
{
    using System;
    using System.Reflection;
    using System.Threading;

    public abstract class SingletonCoreBase<T> where T : class
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(() =>
        {
            // Get non-public constructors for T.
            ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public );

            // If we can't find the right type of construcor, throw an exception.
            if (!Array.Exists(ctors, (ci) => ci.GetParameters().Length == 0))
            {
                throw new ConstructorNotFoundException($"Non-public ctor() note found. => {typeof(T).Name}");
            }

            // Get reference to default non-public constructor.
            ConstructorInfo ctor = Array.Find(ctors, (ci) => ci.GetParameters().Length == 0);

            // Invoke constructor and return resulting object.
            return ctor.Invoke(new object[] { }) as T;
        }, LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Singleton instance access property.
        /// </summary>
        public static T Instance
        {
            get { return _instance.Value; }
        }
    }

    /// <summary>
    /// Exception thrown by Singleton&lt;T&gt; when derived type does not contain a non-public default constructor.
    /// </summary>
    public class ConstructorNotFoundException : Exception
    {
        private const string ConstructorNotFoundMessage = "Singleton<T> derived types require a non-public default constructor.";

        public ConstructorNotFoundException() : base(ConstructorNotFoundMessage) { }

        public ConstructorNotFoundException(string auxMessage) : base($"{ConstructorNotFoundMessage} - {auxMessage}") { }

        public ConstructorNotFoundException(string auxMessage, Exception inner) : base($"{ConstructorNotFoundMessage} - {auxMessage}", inner) { }
    }
}
