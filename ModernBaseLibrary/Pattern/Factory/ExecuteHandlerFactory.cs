/*
 * <copyright file="ExecuteHandlerFactory.cs" company="Lifeprojects.de">
 *     Class: ExecuteHandlerFactory
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>29.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Factory Class of ExecuteHandlerFactory Implemation
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

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ExecuteHandlerFactory
    {
        private static readonly Dictionary<string, MethodInfo> commandMethods = new Dictionary<string, MethodInfo>();

        public static MethodInfo ExecuteMethod<Tch>(string commandText)
        {
            MethodInfo methodInfo;
            var commands = Activator.CreateInstance(typeof(Tch));

            try
            {
                if (commandMethods.Count == 0)
                {
                    var methodNames = typeof(Tch).GetMethods(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                    var commandAttributeMethods = methodNames.Where(y => y.GetCustomAttributes().OfType<ExecuteMethodeHandlerAttribute>().Any());
                    foreach (var commandAttributeMethod in commandAttributeMethods)
                    {
                        foreach (var attribute in commandAttributeMethod.GetCustomAttributes(true))
                        {
                            string key = ((ExecuteMethodeHandlerAttribute)attribute).CommandHandler;
                            if (commandMethods.ContainsKey(key) == false)
                            {
                                commandMethods.Add(((ExecuteMethodeHandlerAttribute)attribute).CommandHandler, commandAttributeMethod);
                            }
                        }
                    }

                    if (commandMethods.Count(p => p.Key == commandText) == 0)
                    {
                        throw new ArgumentException(string.Format("The Command Key '{0}' not found", commandText));
                    }

                    methodInfo = commandMethods[commandText];
                }
                else
                {
                    if (commandMethods.Count(p => p.Key == commandText) == 0)
                    {
                        throw new ArgumentException(string.Format("The Command Key '{0}' not found", commandText));
                    }

                    methodInfo = commandMethods[commandText];
                }
            }
            catch (Exception)
            {
                throw;
            }

            return methodInfo;
        }

        public static object Call()
        {
            return null;
        }
    }
}