/*
 * <copyright file="IEventObserver.cs" company="Lifeprojects.de">
 *     Class: IEventObserver
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>15.10.2022</date>
 * <Project>Mainova.Tools</Project>
 *
 * <summary>
 * Interface Klasse zu IEventObserver (EventObserver)
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

    public interface IEventAggregator
    {
        void Publish<TPayload>(TPayload payload) where TPayload : IEventAggregatorArgs;

        ISubscription<TPayload> Subscribe<TPayload>(Action<TPayload> action) where TPayload : IEventAggregatorArgs;

        void UnSubscribe<TPayload>(ISubscription<TPayload> subscription) where TPayload : IEventAggregatorArgs;

        void ClearAllSubscriptions();

        void ClearAllSubscriptions(Type[] exceptMessages);
    }
}
